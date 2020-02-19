using FlashFileProcessor.Service.Interfaces;
using FlashFileProcessor.Service.Models;
using FlashFileProcessor.Service.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FlashFileProcessor.Service.Helpers
{
   /// <summary>
   /// Validator class
   /// </summary>
   /// <seealso cref="FlashFileProcessor.Service.Interfaces.IValidator" />
   public class Validator : IValidator
   {
      /// <summary>
      /// The rules list
      /// </summary>
      private List<Rule> RulesList = new List<Rule>();

      /// <summary>
      /// The logger
      /// </summary>
      private readonly ILogger<Validator> _logger;

      /// <summary>
      /// The customers options
      /// </summary>
      private readonly List<CustomerOptions> customersOptions;

      public IRuleProcessor _ruleProcessor { get; set; }

      /// <summary>
      /// Initializes a new instance of the <see cref="Validator"/> class.
      /// </summary>
      /// <param name="customers">The files.</param>
      /// <param name="ruleProcessor">The rule processor.</param>
      public Validator(IOptions<CustomersOptions> customers, IRuleProcessor ruleProcessor, ILogger<Validator> logger)
      {
         customersOptions = customers.Value.CustomerArray;
         _ruleProcessor = ruleProcessor;
         _logger = logger;
      }

      /// <summary>
      /// Processes the line items.
      /// </summary>
      /// <param name="lineItems">This method will process each field entry agains the rule being configured.</param>
      /// <returns>
      /// ValidatedResult
      /// </returns>
      public async Task<ValidatedResult> ProcessLineItems(string line, string[] fieldArray, string[] validators)
      {
         bool isValid = false;
         List<string> rejectReasons = new List<string>();
         ValidatedResult validatedResult = new ValidatedResult();
         string[] lineItems = line.Split(',');
         RulesList = _ruleProcessor.GetRules(fieldArray, validators);

         try
         {
            for (int i = 0; i < lineItems.Length; i++)
            {
               if (RulesList.Count > i)
               {
                  isValid = await ValidateAsync(RulesList[i].ExpressonToUse, lineItems[i]);
               }
               else
               {
                  isValid = true;
               }

               if (!isValid)
               {
                  rejectReasons.Add(RulesList[i].RejectReason);
               }
            }

            if (rejectReasons.Count == 0)
            {
               validatedResult.Content = line;
               validatedResult.IsValid = true;
            }
            else
            {
               validatedResult.Content = string.Concat(line, ",", string.Join(",", rejectReasons));
               validatedResult.IsValid = false;
            }
         }
         catch (Exception ex)
         {
            _logger.LogInformation($"Error occurred in ProcessLineItems : {ex.Message}");
         }

         return validatedResult;
      }

      /// <summary>
      /// Validates the specific input with appropriate regular expression.
      /// </summary>
      /// <param name="regExpression">The reg expression.</param>
      /// <param name="input">The input.</param>
      /// <returns>
      /// boolean
      /// </returns>
      public Task<bool> ValidateAsync(string regExpression, string input)
      {
         Regex rx = new Regex(regExpression);

         return Task.FromResult<bool>(rx.Match(input).Success);
      }
   }
}
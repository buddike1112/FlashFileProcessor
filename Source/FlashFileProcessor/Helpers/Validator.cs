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
      private readonly List<Rule> RulesList = new List<Rule>();

      /// <summary>
      /// The logger
      /// </summary>
      private readonly ILogger<Validator> _logger;

      /// <summary>
      /// The customers options
      /// </summary>
      private readonly CustomersOptions customersOptions;

      /// <summary>
      /// Gets or sets the columns list.
      /// </summary>
      /// <value>
      /// The columns list according to the file specifications.
      /// </value>
      private string[] ColumnsList { get; set; }

      /// <summary>
      /// Initializes a new instance of the <see cref="Validator"/> class.
      /// </summary>
      /// <param name="files">The files.</param>
      /// <param name="ruleProcessor">The rule processor.</param>
      public Validator(IOptionsMonitor<CustomersOptions> files, IRuleProcessor ruleProcessor, ILogger<Validator> logger)
      {
         customersOptions = files.CurrentValue;
         ColumnsList = customersOptions.Columns;
         RulesList = ruleProcessor.GetRules();
         _logger = logger;
      }

      /// <summary>
      /// Processes the line items.
      /// </summary>
      /// <param name="lineItems">This method will process each field entry agains the rule being configured.</param>
      /// <returns>
      /// ValidatedResult
      /// </returns>
      public async Task<ValidatedResult> ProcessLineItems(string line)
      {
         bool isValid = false;
         List<string> rejectReasons = new List<string>();
         ValidatedResult validatedResult = new ValidatedResult();
         string[] lineItems = line.Split(',');

         try
         {
            for (int i = 0; i < lineItems.Length; i++)
            {
               isValid = await ValidateAsync(RulesList[i].ExpressonToUse, lineItems[i]);

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
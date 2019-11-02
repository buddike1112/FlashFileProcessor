using FlashFileProcessor.Service.Interfaces;
using FlashFileProcessor.Service.Models;
using FlashFileProcessor.Service.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
      private List<Rule> rulesList = new List<Rule>();

      /// <summary>
      /// The files options
      /// </summary>
      private FilesOptions filesOptions;

      /// <summary>
      /// Gets or sets the columns list.
      /// </summary>
      /// <value>
      /// The columns list according to the file specifications.
      /// </value>
      private string[] columnsList { get; set; }

      /// <summary>
      /// Initializes a new instance of the <see cref="Validator"/> class.
      /// </summary>
      /// <param name="files">The files.</param>
      /// <param name="ruleProcessor">The rule processor.</param>
      public Validator(IOptionsMonitor<FilesOptions> files, IRuleProcessor ruleProcessor)
      {
         filesOptions = files.CurrentValue;
         columnsList = filesOptions.Columns;
         rulesList = ruleProcessor.GetRules();
      }

      /// <summary>
      /// Processes the line items.
      /// </summary>
      /// <param name="lineItems">This method will process each field entry agains the rule being configured.</param>
      /// <returns>
      /// ValidatedResult
      /// </returns>
      public async Task<ValidatedResult> ProcessLineItems(string[] lineItems)
      {
         bool isValid = false;
         List<string> rejectReasons = new List<string>();
         ValidatedResult validatedResult = new ValidatedResult();

         try
         {
            for (int i = 0; i < lineItems.Length; i++)
            {
               isValid = await ValidateAsync(rulesList[i].ExpressonToUse, lineItems[i]);

               if (!isValid)
               {
                  rejectReasons.Add(rulesList[i].RejectReason);
               }
            }

            if (rejectReasons.Count == 0)
            {
               validatedResult.Content = string.Join(",", lineItems);
               validatedResult.IsValid = true;
            }
            else
            {
               validatedResult.Content = string.Concat(string.Join(",", lineItems), ",", string.Join(",", rejectReasons));
               validatedResult.IsValid = false;
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine($"Error occurred in ProcessLineItems : {ex.Message}");
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

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
   public class Validator : IValidator
   {
      private List<Rule> rulesList = new List<Rule>();
      private FilesOptions filesOptions;
      private string[] columnsList { get; set; }

      public Validator(IOptionsMonitor<FilesOptions> files, IRuleProcessor ruleProcessor)
      {
         filesOptions = files.CurrentValue;
         columnsList = filesOptions.Columns;
         rulesList = ruleProcessor.GetRules();
      }

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

      public Task<bool> ValidateAsync(string regExpression, string input)
      {
         Regex rx = new Regex(regExpression);

         return Task.FromResult<bool>(rx.Match(input).Success);
      }
   }
}

using FlashFileProcessor.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlashFileProcessor.Service.Interfaces
{
   /// <summary>
   /// Validator interface
   /// </summary>
   public interface IValidator
   {
      /// <summary>
      /// Processes the line items.
      /// </summary>
      /// <param name="lineItems">This method will process each field entry agains the rule being configured.</param>
      /// <returns>ValidatedResult</returns>
      Task<ValidatedResult> ProcessLineItems(string line);

      /// <summary>
      /// Validates the specific input with appropriate regular expression.
      /// </summary>
      /// <param name="regExpression">The reg expression.</param>
      /// <param name="input">The input.</param>
      /// <returns>boolean</returns>
      Task<bool> ValidateAsync(string regExpression, string input);
   }
}
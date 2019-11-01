using FlashFileProcessor.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlashFileProcessor.Service.Interfaces
{
   public interface IValidator
   {
      Task<ValidatedResult> ProcessLineItems(string[] lineItems);
      Task<bool> ValidateAsync(string regExpression, string input);
   }
}

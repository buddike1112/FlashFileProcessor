using FlashFileProcessor.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlashFileProcessor.Service.Interfaces
{
   /// <summary>
   /// Rule Processor interface
   /// </summary>
   public interface IRuleProcessor
   {
      /// <summary>
      /// Gets the rules as a collection.
      /// </summary>
      /// <param name="fieldArray">The field array.</param>
      /// <param name="validators">The validators.</param>
      /// <returns>
      /// Rule object
      /// </returns>
      List<Rule> GetRules(string[] fieldArray, string[] validators);
   }
}
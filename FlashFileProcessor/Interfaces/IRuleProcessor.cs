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
      /// Gets the rule for a feld.
      /// </summary>
      /// <param name="fieldName">Name of the field.</param>
      /// <returns>Rule object</returns>
      Rule GetRule(string fieldName);

      /// <summary>
      /// Gets the rules as a collection.
      /// </summary>
      /// <returns>Rule object</returns>
      List<Rule> GetRules();
   }
}

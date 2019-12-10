using FlashFileProcessor.Service.Interfaces;
using FlashFileProcessor.Service.Models;
using FlashFileProcessor.Service.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlashFileProcessor.Service.Helpers
{
   /// <summary>
   /// Rule Processor class
   /// </summary>
   /// <seealso cref="FlashFileProcessor.Service.Interfaces.IRuleProcessor" />
   public class RuleProcessor : IRuleProcessor
   {
      /// <summary>
      /// The files options to use
      /// </summary>
      private FilesOptions filesOptions;

      /// <summary>
      /// The fields array
      /// </summary>
      string[] fieldsArray = new string[] { };

      /// <summary>
      /// Initializes a new instance of the <see cref="RuleProcessor"/> class.
      /// </summary>
      /// <param name="files">The files.</param>
      public RuleProcessor(IOptionsMonitor<FilesOptions> files)
      {
         filesOptions = files.CurrentValue;
         fieldsArray = filesOptions.Columns;
      }

      /// <summary>
      /// Gets the rule for a feld.
      /// </summary>
      /// <param name="fieldName">Name of the field.</param>
      /// <returns>
      /// Rule object
      /// </returns>
      public Rule GetRule(string fieldName)
      {
         return fieldsArray.Select(x => new Rule() { Field = x.ToString().Split(";")[0], ExpressonToUse = x.ToString().Split(";")[1], RejectReason = x.ToString().Split(";")[2] })
            .FirstOrDefault(x => string.Equals(x.Field, fieldName));

      }

      /// <summary>
      /// Gets the rules as a collection.
      /// </summary>
      /// <returns>
      /// Rule object
      /// </returns>
      public List<Rule> GetRules()
      {
         return fieldsArray.Select(x =>
            new Rule() { Field = x.ToString().Split(";")[0], ExpressonToUse = x.ToString().Split(";")[1], RejectReason = x.ToString().Split(";")[2] }).ToList();
      }
   }
}

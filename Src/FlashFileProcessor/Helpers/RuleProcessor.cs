using FlashFileProcessor.Service.Interfaces;
using FlashFileProcessor.Service.Models;
using FlashFileProcessor.Service.Options;
using Microsoft.Extensions.Logging;
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
      /// The logger
      /// </summary>
      private readonly ILogger<RuleProcessor> _logger;

      /// <summary>
      /// Initializes a new instance of the <see cref="RuleProcessor"/> class.
      /// </summary>
      /// <param name="files">The files.</param>
      public RuleProcessor(IOptionsMonitor<FilesOptions> files, ILogger<RuleProcessor> logger)
      {
         filesOptions = files.CurrentValue;
         fieldsArray = filesOptions.Columns;
         _logger = logger;
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
         _logger.LogInformation($"Getting Rule for field name : {fieldName}");
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
         _logger.LogInformation("Getting all available rules from configurations");
         return fieldsArray.Select(x =>
            new Rule() { Field = x.ToString().Split(";")[0], ExpressonToUse = x.ToString().Split(";")[1], RejectReason = x.ToString().Split(";")[2] }).ToList();
      }
   }
}

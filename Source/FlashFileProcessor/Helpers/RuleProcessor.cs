using FlashFileProcessor.Service.Interfaces;
using FlashFileProcessor.Service.Models;
using FlashFileProcessor.Service.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
      private List<CustomerOptions> customersOptions;

      /// <summary>
      /// The logger
      /// </summary>
      private readonly ILogger<RuleProcessor> _logger;

      /// <summary>
      /// Initializes a new instance of the <see cref="RuleProcessor"/> class.
      /// </summary>
      /// <param name="customers">The customers.</param>
      public RuleProcessor(IOptions<CustomersOptions> customers, ILogger<RuleProcessor> logger)
      {
         customersOptions = customers.Value.CustomerArray;
         _logger = logger;
      }

      /// <summary>
      /// Gets the rules as a collection.
      /// </summary>
      /// <returns>
      /// Rule object
      /// </returns>
      public List<Rule> GetRules(string[] fieldArray, string[] validators)
      {
         List<Rule> originalItems = fieldArray.Select(x =>
            new Rule() { Field = x.ToString().Split(";")[0], ExpressonToUse = x.ToString().Split(";")[1], RejectReason = x.ToString().Split(";")[2] }).ToList();

         List<Rule> qualifiedRules = new List<Rule>();

         for (int i = 0; i < originalItems.Count; i++)
         {
            if (i < validators.Length)
            {
               if (originalItems[i].Field.Equals(validators[i]))
               {
                  qualifiedRules.Add(originalItems[i]);
               }
            }
            else
            {
               break;
            }
         }

         return qualifiedRules;
      }
   }
}
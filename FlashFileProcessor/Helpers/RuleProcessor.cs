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
   public class RuleProcessor : IRuleProcessor
   {
      private FilesOptions filesOptions;

      string[] fieldsArray = new string[] { };

      public RuleProcessor(IOptionsMonitor<FilesOptions> files)
      {
         filesOptions = files.CurrentValue;
         fieldsArray = filesOptions.Columns;
      }

      public Rule GetRule(string fieldName)
      {
         return fieldsArray.Select(x =>
         string.Equals(x.ToString().Split(";")[0], fieldName) ?
         new Rule() { Field = x.ToString().Split(";")[0], ExpressonToUse = x.ToString().Split(";")[1], RejectReason = x.ToString().Split(";")[2] } : null).FirstOrDefault();
      }

      public List<Rule> GetRules()
      {
         return fieldsArray.Select(x =>
            new Rule() { Field = x.ToString().Split(";")[0], ExpressonToUse = x.ToString().Split(";")[1], RejectReason = x.ToString().Split(";")[2] }).ToList();
      }
   }
}

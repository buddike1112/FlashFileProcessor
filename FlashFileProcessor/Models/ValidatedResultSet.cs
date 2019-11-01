using System;
using System.Collections.Generic;
using System.Text;

namespace FlashFileProcessor.Service.Models
{
   public class ValidatedResultSet
   {
      public List<string> SuccessItemsList { get; set; }

      public List<string> FailureItemsList { get; set; }
   }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FlashFileProcessor.Service.Models
{
   public class Rule
   {
      public string Field { get; set; }

      public string ExpressonToUse { get; set; }

      public string RejectReason { get; set; }
   }
}

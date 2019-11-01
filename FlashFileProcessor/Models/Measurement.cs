using System;
using System.Collections.Generic;
using System.Text;

namespace FlashFileProcessor.Service.Models
{
   public class Measurement
   {
      public int MeasurementID { get; set; }

      public long ElapsedTime { get; set; }

      public string MethodName { get; set; }
   }
}

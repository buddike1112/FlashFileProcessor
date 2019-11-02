using System;
using System.Collections.Generic;
using System.Text;

namespace FlashFileProcessor.Service.Models
{
   /// <summary>
   /// This class will hold the time measurements taken while processing the file
   /// </summary>
   public class Measurement
   {
      /// <summary>
      /// Gets or sets the measurement identifier.
      /// </summary>
      /// <value>
      /// The measurement identifier.
      /// </value>
      public int MeasurementID { get; set; }

      /// <summary>
      /// Gets or sets the elapsed time.
      /// </summary>
      /// <value>
      /// The elapsed time spent for the specific process.
      /// </value>
      public long ElapsedTime { get; set; }

      /// <summary>
      /// Gets or sets the name of the method.
      /// </summary>
      /// <value>
      /// The name of the method that measurement data capture against.
      /// </value>
      public string MethodName { get; set; }
   }
}

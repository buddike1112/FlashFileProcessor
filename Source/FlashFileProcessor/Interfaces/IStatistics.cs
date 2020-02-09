using FlashFileProcessor.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlashFileProcessor.Service.Interfaces
{
   /// <summary>
   /// Statistics interface
   /// </summary>
   public interface IStatistics
   {
      /// <summary>
      /// Gets the measurement.
      /// </summary>
      /// <returns>Measurement object</returns>
      public Measurement GetMeasurement();
   }
}

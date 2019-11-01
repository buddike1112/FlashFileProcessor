using FlashFileProcessor.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlashFileProcessor.Service.Interfaces
{
   public interface IStatistics
   {
      public Measurement GetMeasurement();
   }
}

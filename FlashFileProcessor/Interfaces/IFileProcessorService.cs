using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlashFileProcessor.Service.Interfaces
{
   public interface IFileProcessorService
   {
      Task ProcessFilesAsync();
   }
}

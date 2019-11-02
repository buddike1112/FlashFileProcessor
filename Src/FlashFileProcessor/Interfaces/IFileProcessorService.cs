using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlashFileProcessor.Service.Interfaces
{
   /// <summary>
   /// File Processor Service interface
   /// </summary>
   public interface IFileProcessorService
   {
      /// <summary>
      /// Processes the files asynchronous.
      /// </summary>
      /// <returns>Task</returns>
      Task ProcessFilesAsync();
   }
}

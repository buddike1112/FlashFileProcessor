using FlashFileProcessor.Service.Interfaces;
using FlashFileProcessor.Service.Models;
using FlashFileProcessor.Service.Options;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FlashFileProcessor.Service.Helpers
{
   /// <summary>
   /// File Processor Service
   /// </summary>
   /// <seealso cref="FlashFileProcessor.Service.Interfaces.IFileProcessorService" />
   public class FileProcessorService : IFileProcessorService
   {
      /// <summary>
      /// The files options
      /// </summary>
      private FilesOptions filesOptions;

      /// <summary>
      /// The file helper instance
      /// </summary>
      private IFileHelper fileHelperInstance;

      /// <summary>
      /// Initializes a new instance of the <see cref="FileProcessorService"/> class.
      /// </summary>
      /// <param name="files">The files.</param>
      /// <param name="fileHelper">The file helper instance.</param>
      public FileProcessorService(IOptionsMonitor<FilesOptions> files, IFileHelper fileHelper)
      {
         filesOptions = files.CurrentValue;
         fileHelperInstance = fileHelper;
      }

      /// <summary>
      /// Processes the files asynchronous.
      /// </summary>
      public async Task ProcessFilesAsync()
      {
         try
         {
            string importFile = string.Concat(filesOptions.ImportFileLocation, string.Concat(filesOptions.ImportFileNamePattern, DateTime.Now.ToString("yyyyMMdd"), filesOptions.Extension));
            string processedFile = string.Concat(filesOptions.DestinationProcessedLocation, string.Concat(filesOptions.ImportFileNamePattern, "Processed_", DateTime.Now.ToString("yyyyMMdd"), filesOptions.Extension));
            string rejectedFile = string.Concat(filesOptions.DestinationRejectLocation, string.Concat(filesOptions.ImportFileNamePattern, "Rejected_", DateTime.Now.ToString("yyyyMMdd"), filesOptions.Extension));
            string destinationFile = string.Concat(filesOptions.DestinationArchiveLocation, string.Concat(filesOptions.ImportFileNamePattern, DateTime.Now.ToString("yyyyMMdd"), filesOptions.Extension));
            bool isRejectedFileCreated = false;
            bool isProcessedFileCreated = false;

            if (File.Exists(importFile))
            {
               Console.WriteLine($"Reading File : {importFile}");
               ValidatedResultSet resultSetToWrite = await fileHelperInstance.ReadFile(importFile);

               if (resultSetToWrite.SuccessItemsList.Count > 0)
               {
                  Console.WriteLine($"Writing successful Items to file : {processedFile} \n");
                  isProcessedFileCreated = await fileHelperInstance.CreateFileAsync(processedFile, resultSetToWrite.SuccessItemsList);
               }
               else
               {
                  Console.WriteLine("No succesful records to process");
               }

               if (resultSetToWrite.FailureItemsList.Count > 0)
               {
                  Console.WriteLine($"\n Writing rejected Items to file : {rejectedFile} \n");
                  isRejectedFileCreated = await fileHelperInstance.CreateFileAsync(rejectedFile, resultSetToWrite.FailureItemsList);
               }
               else
               {
                  Console.WriteLine("No failure records to process");
               }

               if (isProcessedFileCreated && isRejectedFileCreated)
               {
                  Console.WriteLine("Success and Failure records files generated moving original file to Archive.");

                  await fileHelperInstance.MoveFileAsync(importFile, destinationFile);
               }
            }
            else
            {
               Console.WriteLine("Waiting for Campaign File for today!");
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine($"Error occurred in ProcessFilesAsync : {ex.Message}");
         }
      }
   }
}

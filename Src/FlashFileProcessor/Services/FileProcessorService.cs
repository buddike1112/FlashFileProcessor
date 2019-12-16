using FlashFileProcessor.Service.Interfaces;
using FlashFileProcessor.Service.Models;
using FlashFileProcessor.Service.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FlashFileProcessor.Service.Services
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
      /// The logger
      /// </summary>
      private readonly ILogger _logger;

      /// <summary>
      /// The file helper instance
      /// </summary>
      private IFileHelper fileHelperInstance;

      /// <summary>
      /// Initializes a new instance of the <see cref="FileProcessorService"/> class.
      /// </summary>
      /// <param name="files">The files.</param>
      /// <param name="fileHelper">The file helper instance.</param>
      public FileProcessorService(IOptionsMonitor<FilesOptions> files, IFileHelper fileHelper, ILogger<FileProcessorService> logger)
      {
         filesOptions = files.CurrentValue;
         fileHelperInstance = fileHelper;
         _logger = logger;
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
               _logger.LogInformation($"Reading File : {importFile}");
               ValidatedResultSet resultSetToWrite = await fileHelperInstance.ReadFile(importFile);

               if (resultSetToWrite.SuccessItemsList.Count > 0)
               {
                  _logger.LogInformation($"Writing successful Items to file : {processedFile} \n");
                  isProcessedFileCreated = await fileHelperInstance.CreateFileAsync(processedFile, resultSetToWrite.SuccessItemsList);
               }
               else
               {
                  _logger.LogInformation("No succesful records to process");
               }

               if (resultSetToWrite.FailureItemsList.Count > 0)
               {
                  _logger.LogInformation($"\n Writing rejected Items to file : {rejectedFile} \n");
                  isRejectedFileCreated = await fileHelperInstance.CreateFileAsync(rejectedFile, resultSetToWrite.FailureItemsList);
               }
               else
               {
                  _logger.LogInformation("No failure records to process");
               }

               if (isProcessedFileCreated && isRejectedFileCreated)
               {
                  _logger.LogInformation("Success and Failure records files generated moving original file to Archive.");

                  await fileHelperInstance.MoveFileAsync(importFile, destinationFile);
               }
            }
            else
            {
               _logger.LogInformation("Waiting for Campaign File for today!");
            }
         }
         catch (Exception ex)
         {
            _logger.LogInformation($"Error occurred in ProcessFilesAsync : {ex.Message}");
         }
      }
   }
}

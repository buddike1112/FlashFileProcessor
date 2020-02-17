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
      /// The customers options
      /// </summary>
      private CustomersOptions customersOptions;

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
      /// <param name="customers">The customers.</param>
      /// <param name="fileHelper">The file helper instance.</param>
      public FileProcessorService(IOptionsMonitor<CustomersOptions> customers, IFileHelper fileHelper, ILogger<FileProcessorService> logger)
      {
         customersOptions = customers.CurrentValue;
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
            string importFile = string.Concat(customersOptions.ImportFileLocation, string.Concat(customersOptions.ImportFileNamePattern, DateTime.Now.ToString("yyyyMMdd"), customersOptions.Extension));
            string processedFile = string.Concat(customersOptions.DestinationProcessedLocation, string.Concat(customersOptions.ImportFileNamePattern, "Processed_", DateTime.Now.ToString("yyyyMMdd"), customersOptions.Extension));
            string rejectedFile = string.Concat(customersOptions.DestinationRejectLocation, string.Concat(customersOptions.ImportFileNamePattern, "Rejected_", DateTime.Now.ToString("yyyyMMdd"), customersOptions.Extension));
            string destinationFile = string.Concat(customersOptions.DestinationArchiveLocation, string.Concat(customersOptions.ImportFileNamePattern, DateTime.Now.ToString("yyyyMMdd"), customersOptions.Extension));
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
               _logger.LogInformation("Waiting for Campaign Files for today!");
            }
         }
         catch (Exception ex)
         {
            _logger.LogInformation($"Error occurred in ProcessFilesAsync : {ex.Message}");
         }
      }
   }
}
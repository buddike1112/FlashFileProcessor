using FlashFileProcessor.Service.Interfaces;
using FlashFileProcessor.Service.Models;
using FlashFileProcessor.Service.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
      private List<CustomerOptions> customersOptions;

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
      public FileProcessorService(IOptions<CustomersOptions> customers, IFileHelper fileHelper, ILogger<FileProcessorService> logger)
      {
         customersOptions = customers.Value.CustomerArray;
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
            if (customersOptions != null && customersOptions.Count > 0)
            {
               foreach (var customer in customersOptions)
               {
                  List<ProfilesOptions> profiles = customer.Profiles;

                  if (profiles != null && profiles.Count > 0)
                  {
                     foreach (var profile in profiles)
                     {
                        string importFile = string.Concat(profile.ImportFileLocation, string.Concat(profile.ImportFileNamePattern, DateTime.Now.ToString("yyyyMMdd"), profile.Extension));
                        string processedFile = string.Concat(profile.DestinationProcessedLocation, string.Concat(profile.ImportFileNamePattern, "Processed_", DateTime.Now.ToString("yyyyMMdd"), profile.Extension));
                        string rejectedFile = string.Concat(profile.DestinationRejectLocation, string.Concat(profile.ImportFileNamePattern, "Rejected_", DateTime.Now.ToString("yyyyMMdd"), profile.Extension));
                        string destinationFile = string.Concat(profile.DestinationArchiveLocation, string.Concat(profile.ImportFileNamePattern, DateTime.Now.ToString("yyyyMMdd"), profile.Extension));
                        bool isRejectedFileCreated = false;
                        bool isProcessedFileCreated = false;

                        if (File.Exists(importFile))
                        {
                           _logger.LogInformation($"Reading File : {importFile}");
                           ValidatedResultSet resultSetToWrite = await fileHelperInstance.ReadFile(profile);

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

                           if (isProcessedFileCreated || isRejectedFileCreated)
                           {
                              _logger.LogInformation("Success and Failure records files generated. Moving original file to Archive.");

                              await fileHelperInstance.MoveFileAsync(importFile, destinationFile);
                           }
                        }
                        else
                        {
                           _logger.LogInformation("Waiting for Campaign Files for today!");
                        }
                     }
                  }
                  else
                  {
                     _logger.LogWarning($"Unable to find profile information for customer {customer.CustomerName} to proceed.");
                  }
               }
            }
            else
            {
               _logger.LogWarning("Unable to find customer information to proceed.");
            }
         }
         catch (Exception ex)
         {
            _logger.LogInformation($"Error occurred in ProcessFilesAsync : {ex.Message}");
         }
      }
   }
}
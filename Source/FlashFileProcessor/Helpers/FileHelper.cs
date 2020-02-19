using FlashFileProcessor.Service.Interfaces;
using FlashFileProcessor.Service.Models;
using FlashFileProcessor.Service.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FlashFileProcessor.Service.Helpers
{
   /// <summary>
   /// File Helper class
   /// </summary>
   /// <seealso cref="FlashFileProcessor.Service.Interfaces.IFileHelper" />
   public class FileHelper : IFileHelper
   {
      /// <summary>
      /// Gets or sets the validator.
      /// </summary>
      /// <value>
      /// The validator instance.
      /// </value>
      public IValidator _validator { get; set; }

      /// <summary>
      /// The customers options
      /// </summary>
      private CustomersOptions customersOptions;

      /// <summary>
      /// The logger
      /// </summary>
      private readonly ILogger<FileHelper> _logger;

      /// <summary>
      /// Gets or sets the watcher.
      /// </summary>
      /// <value>
      /// The watcher.
      /// </value>
      public Stopwatch watcher { get; set; }

      /// <summary>
      /// Initializes a new instance of the <see cref="FileHelper"/> class.
      /// </summary>
      /// <param name="customers">The customerss.</param>
      /// <param name="validator">The validator.</param>
      public FileHelper(IOptionsMonitor<CustomersOptions> customers, IValidator validator, ILogger<FileHelper> logger)
      {
         _validator = validator;
         customersOptions = customers.CurrentValue;
         _logger = logger;
      }

      /// <summary>
      /// Creates the file asynchronous.
      /// </summary>
      /// <param name="fileName">Name of the file.</param>
      /// <param name="fileContent">Contents to write.</param>
      /// <returns>
      /// boolean
      /// </returns>
      public async Task<bool> CreateFileAsync(string fileName, List<string> fileContent)
      {
         watcher = Stopwatch.StartNew();
         bool isFileCreated = false;

         try
         {
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
               foreach (string line in fileContent)
                  await outputFile.WriteLineAsync(line);

               isFileCreated = true;
            }
         }
         catch (Exception ex)
         {
            _logger.LogInformation($"CreateFileAsync -> Unable to create file : {ex.Message}");
         }

         watcher.Stop();

         _logger.LogInformation($"Time consumed for creating file : {fileName} : {watcher.ElapsedMilliseconds} milliseconds");

         return isFileCreated;
      }

      /// <summary>
      /// Moves the file asynchronous.
      /// </summary>
      /// <param name="sourceFile">The source file.</param>
      /// <param name="destinationFile">The destination file.</param>
      /// <returns>boolean</returns>
      public async Task<bool> MoveFileAsync(string sourceFile, string destinationFile)
      {
         watcher = Stopwatch.StartNew();
         bool isFileMoved = false;

         if (File.Exists(sourceFile))
         {
            try
            {
               File.Move(sourceFile, destinationFile);
               isFileMoved = true;
            }
            catch (Exception ex)
            {
               _logger.LogInformation($"MoveFileAsync -> Unable to move file : {ex.Message}");
            }
         }

         watcher.Stop();

         _logger.LogInformation($"Time consumed for moving file : {sourceFile} : {watcher.ElapsedMilliseconds} milliseconds");

         return isFileMoved;
      }

      /// <summary>
      /// Creates the folder.
      /// </summary>
      /// <param name="folderPath">The folder path.</param>
      /// <returns>boolean</returns>
      private bool CreateFolder(string folderPath)
      {
         watcher = Stopwatch.StartNew();

         if (!Directory.Exists(folderPath))
         {
            try
            {
               Directory.CreateDirectory(folderPath);
            }
            catch (Exception ex)
            {
               _logger.LogInformation($"CreateFolder -> Unable to create Folder : {ex.Message}");
               return false;
            }
         }

         watcher.Stop();

         _logger.LogInformation($"Time consumed for Creating directory : {folderPath} : {watcher.ElapsedMilliseconds} milliseconds");

         return true;
      }

      /// <summary>
      /// Reads the file.
      /// </summary>
      /// <param name="fileName">Name of the file.</param>
      /// <returns>ValidatedResultSet</returns>
      public async Task<ValidatedResultSet> ReadFile(ProfilesOptions profile)
      {
         string importFile = string.Concat(profile.ImportFileLocation, string.Concat(profile.ImportFileNamePattern, DateTime.Now.ToString("yyyyMMdd"), profile.Extension));
         ValidatedResultSet result = new ValidatedResultSet();
         result.SuccessItemsList = new List<string>();
         result.FailureItemsList = new List<string>();

         watcher = Stopwatch.StartNew();

         try
         {
            using (StreamReader sr = new StreamReader(importFile))
            {
               string line = string.Empty;

               while ((line = sr.ReadLine()) != null)
               {
                  ValidatedResult validateResult = await _validator.ProcessLineItems(line, profile.Columns, profile.Validations);

                  if (validateResult.IsValid)
                  {
                     result.SuccessItemsList.Add(validateResult.Content);
                  }
                  else
                  {
                     result.FailureItemsList.Add(validateResult.Content);
                  }
               }
            }
         }
         catch (Exception ex)
         {
            _logger.LogInformation($"Error occurred in ReadFile : {ex.Message}");
         }

         watcher.Stop();

         _logger.LogInformation($"Time consumed for Reading file : {importFile} : {watcher.ElapsedMilliseconds} milliseconds");

         return result;
      }
   }
}
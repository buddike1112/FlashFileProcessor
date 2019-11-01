using FlashFileProcessor.Service.Interfaces;
using FlashFileProcessor.Service.Models;
using FlashFileProcessor.Service.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FlashFileProcessor.Service.Helpers
{
   public class FileHelper : IFileHelper
   {
      public IValidator _validator { get; set; }
      private FilesOptions filesOptions;

      public FileHelper(IOptionsMonitor<FilesOptions> files, IValidator validator)
      {
         _validator = validator;
         filesOptions = files.CurrentValue;
      }

      public async Task<bool> CreateFileAsync(string fileName, List<string> fileContent)
      {
         bool isFileCreated = false;

         try
         {
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
               foreach (string line in fileContent)
                  await outputFile.WriteAsync(line);

               isFileCreated = true;
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine($"CreateFileAsync -> Unable to create file : {ex.Message}");
         }

         return isFileCreated;
      }

      public async Task<bool> MoveFileAsync(string sourceFile, string destinationFile)
      {
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
               Console.WriteLine($"MoveFileAsync -> Unable to move file : {ex.Message}");
            }
         }

         return isFileMoved;
      }

      private bool CreateFolder(string folderPath)
      {
         if (!Directory.Exists(folderPath))
         {
            try
            {
               Directory.CreateDirectory(folderPath);
            }
            catch (Exception ex)
            {
               Console.WriteLine($"CreateFolder -> Unable to create Folder : {ex.Message}");
               return false;
            }
         }

         return true;
      }

      public async Task<ValidatedResultSet> ReadFile(string fileName)
      {
         ValidatedResultSet result = new ValidatedResultSet();
         result.SuccessItemsList = new List<string>();
         result.FailureItemsList = new List<string>();
         Measurement measurement = new Measurement();
         Stopwatch stopwatch;
         stopwatch = Stopwatch.StartNew();

         try
         {
            using (StreamReader sr = new StreamReader(fileName))
            {
               String line = string.Empty;

               while ((line = sr.ReadLine()) != null)
               {
                  string[] parts = line.Split(',');

                  ValidatedResult validateResult = await _validator.ProcessLineItems(parts);

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
            Console.WriteLine($"Error occurred in ReadFile : {ex.Message}");
         }

         stopwatch.Stop();
         measurement.ElapsedTime = stopwatch.ElapsedMilliseconds;
         measurement.MethodName = "ReadFile";

         Console.WriteLine($"Time sonsumed : {measurement.ElapsedTime}");

         return result;
      }
   }
}

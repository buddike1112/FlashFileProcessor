using FlashFileProcessor.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlashFileProcessor.Service.Interfaces
{
   public interface IFileHelper
   {
      Task<ValidatedResultSet> ReadFile(string filePath);

      Task<bool> CreateFileAsync(string fileName, List<string> fileContent);

      Task<bool> MoveFileAsync(string sourcesourceFile, string destinationPath);
   }
}

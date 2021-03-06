﻿using FlashFileProcessor.Service.Models;
using FlashFileProcessor.Service.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlashFileProcessor.Service.Interfaces
{
   /// <summary>
   /// File Helper interface
   /// </summary>
   public interface IFileHelper
   {
      /// <summary>
      /// Reads the file.
      /// </summary>
      /// <param name="profilesOptions">The profiles options.</param>
      /// <returns>
      /// ValidatedResultSet
      /// </returns>
      Task<ValidatedResultSet> ReadFile(ProfilesOptions profilesOptions);

      /// <summary>
      /// Creates the file asynchronous.
      /// </summary>
      /// <param name="fileName">Name of the file.</param>
      /// <param name="fileContent">Content to write.</param>
      /// <returns>boolean</returns>
      Task<bool> CreateFileAsync(string fileName, List<string> fileContent);

      /// <summary>
      /// Moves the file asynchronous.
      /// </summary>
      /// <param name="sourcesourceFile">The sourcesource file.</param>
      /// <param name="destinationPath">The destination path.</param>
      /// <returns>boolean</returns>
      Task<bool> MoveFileAsync(string sourcesourceFile, string destinationPath);
   }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace FlashFileProcessor.Service.Options
{
   /// <summary>
   /// This class will be providing access to the file related configurations
   /// coming from appsettings.json file
   /// </summary>
   public class FilesOptions
   {
      public FilesOptions()
      {

      }
      /// <summary>
      /// Gets or sets the import file location.
      /// </summary>
      /// <value>
      /// The import file folder location to search the files.
      /// </value>
      public string ImportFileLocation { get; set; }

      /// <summary>
      /// Gets or sets the import file name pattern.
      /// </summary>
      /// <value>
      /// The import file name pattern as a regular expression.
      /// </value>
      public string ImportFileNamePattern { get; set; }

      /// <summary>
      /// Gets or sets the destination archive location.
      /// </summary>
      /// <value>
      /// The destination archive location folder path.
      /// </value>
      public string DestinationArchiveLocation { get; set; }

      /// <summary>
      /// Gets or sets the destination reject location.
      /// </summary>
      /// <value>
      /// The destination reject location folder path for rejected items.
      /// </value>
      public string DestinationRejectLocation { get; set; }

      /// <summary>
      /// Gets or sets the destination processed location.
      /// </summary>
      /// <value>
      /// The destination processed location folder path.
      /// </value>
      public string DestinationProcessedLocation { get; set; }

      /// <summary>
      /// Gets or sets the extension.
      /// </summary>
      /// <value>
      /// The extension of the file to be use.
      /// </value>
      public string Extension { get; set; }

      /// <summary>
      /// Gets or sets the customer.
      /// </summary>
      /// <value>
      /// The customer name to be use.
      /// </value>
      public string Customer { get; set; }

      /// <summary>
      /// Gets or sets the solution key.
      /// </summary>
      /// <value>
      /// The solution key if we have specific kind of solution.
      /// </value>
      public string SolutionKey { get; set; }

      /// <summary>
      /// Gets or sets the columns.
      /// </summary>
      /// <value>
      /// The columns that can be in the import file.
      /// </value>
      public string[] Columns { get; set; }

      /// <summary>
      /// Gets or sets the profiles.
      /// </summary>
      /// <value>
      /// The profiles of a specific customer.
      /// </value>
      public ProfilesOptions[] Profiles { get; set; }
   }
}

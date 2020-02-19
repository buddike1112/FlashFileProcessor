using System;
using System.Collections.Generic;
using System.Text;

namespace FlashFileProcessor.Service.Options
{
   /// <summary>
   /// This class will hold the configurations needed by the profiles of a customer
   /// </summary>
   public class ProfilesOptions
   {
      public ProfilesOptions()
      {
      }

      /// <summary>
      /// Gets or sets the name.
      /// </summary>
      /// <value>
      /// The name of the profile.
      /// </value>
      public string Name { get; set; }

      /// <summary>
      /// Gets or sets the validations.
      /// </summary>
      /// <value>
      /// The validations that will be use while using the profile.
      /// </value>
      public string[] Validations { get; set; }

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
      /// Gets or sets the extension.
      /// </summary>
      /// <value>
      /// The extension of the file to be use.
      /// </value>
      public string Extension { get; set; }

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
      /// Gets or sets the columns.
      /// </summary>
      /// <value>
      /// The columns.
      /// </value>
      public string[] Columns { get; set; }
   }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace FlashFileProcessor.Service.Options
{
   public class FilesOptions
   {
      public FilesOptions()
      {

      }
      public string ImportFileLocation { get; set; }
      public string ImportFileNamePattern { get; set; }
      public string DestinationArchiveLocation { get; set; }
      public string DestinationRejectLocation { get; set; }
      public string DestinationProcessedLocation { get; set; }
      public string Extension { get; set; }
      public string Customer { get; set; }
      public string SolutionKey { get; set; }
      public string[] Columns { get; set; }
      public ProfilesOptions[] Profiles { get; set; }
   }
}

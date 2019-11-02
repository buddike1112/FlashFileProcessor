using System;
using System.Collections.Generic;
using System.Text;

namespace FlashFileProcessor.Service.Models
{
   /// <summary>
   /// This class will hold the validated result for each row
   /// </summary>
   public class ValidatedResult
   {
      /// <summary>
      /// Gets or sets the content.
      /// </summary>
      /// <value>
      /// Validated content row.
      /// </value>
      public string Content { get; set; }

      /// <summary>
      /// Return true if a row of data is valid.
      /// </summary>
      /// <value>
      ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
      /// </value>
      public bool IsValid { get; set; }
   }
}

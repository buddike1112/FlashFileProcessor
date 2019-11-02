using System;
using System.Collections.Generic;
using System.Text;

namespace FlashFileProcessor.Service.Models
{
   /// <summary>
   /// This class will hold the Rule configurations from appsettings
   /// </summary>
   public class Rule
   {
      /// <summary>
      /// Gets or sets the field.
      /// </summary>
      /// <value>
      /// The field that use this Rule.
      /// </value>
      public string Field { get; set; }

      /// <summary>
      /// Gets or sets the expresson to use.
      /// </summary>
      /// <value>
      /// The regular expresson to use when validating.
      /// </value>
      public string ExpressonToUse { get; set; }

      /// <summary>
      /// Gets or sets the reject reason.
      /// </summary>
      /// <value>
      /// The reject reason to show after validation process.
      /// </value>
      public string RejectReason { get; set; }
   }
}

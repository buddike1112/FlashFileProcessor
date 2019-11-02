using System;
using System.Collections.Generic;
using System.Text;

namespace FlashFileProcessor.Service.Models
{
   /// <summary>
   /// This class will hold the list of success items and list of failure items
   /// </summary>
   public class ValidatedResultSet
   {
      /// <summary>
      /// Gets or sets the success items list.
      /// </summary>
      /// <value>
      /// The success items list.
      /// </value>
      public List<string> SuccessItemsList { get; set; }

      /// <summary>
      /// Gets or sets the failure items list.
      /// </summary>
      /// <value>
      /// The failure items list.
      /// </value>
      public List<string> FailureItemsList { get; set; }
   }
}

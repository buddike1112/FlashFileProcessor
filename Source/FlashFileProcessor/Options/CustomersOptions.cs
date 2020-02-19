using System;
using System.Collections.Generic;
using System.Text;

namespace FlashFileProcessor.Service.Options
{
   public class CustomersOptions
   {
      public CustomersOptions()
      {
      }

      /// <summary>
      /// Gets or sets the customer array.
      /// </summary>
      /// <value>
      /// The customer array.
      /// </value>
      public List<CustomerOptions> CustomerArray { get; set; }
   }
}
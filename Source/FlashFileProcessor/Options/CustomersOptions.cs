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
      /// Gets or sets the customer.
      /// </summary>
      /// <value>
      /// The customer name to be use.
      /// </value>
      public string CustomerName { get; set; }

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
      /// The columns.
      /// </value>
      public string[] Columns { get; set; }

      public ProfilesOptions[] Profiles { get; set; }
   }
}
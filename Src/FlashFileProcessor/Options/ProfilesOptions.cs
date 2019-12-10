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
   }
}

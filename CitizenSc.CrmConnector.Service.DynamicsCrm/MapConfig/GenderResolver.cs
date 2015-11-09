using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace CitizenSc.CrmConnector.Service.DynamicsCrm.MapConfig
{
   public class GenderResolver : ValueResolver<Contact, String>
   {
      protected override string ResolveCore(Contact source)
      {
         var genderString = String.Empty;

         if (source.GenderCode != null)
         {
            genderString = source.GenderCode.Value == 1 ? "Male" : "Female";
         }

         return genderString;
      }
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CitizenSc.CrmConnector.Model.DynamicsCrm;

namespace CitizenSc.CrmConnector.Service.DynamicsCrm.MapConfig
{
   public class PhoneNumberCollectionResolver : ValueResolver<Contact, ICollection<Model.IPhoneNumber>>
   {
      protected override ICollection<Model.IPhoneNumber> ResolveCore(Contact source)
      {
         var collection = new List<Model.IPhoneNumber>();

         if (!String.IsNullOrEmpty(source.MobilePhone))
            collection.Add(new PhoneNumber() { Value = source.MobilePhone, Preferred = true, Type = "Mobile" });

         if (!String.IsNullOrEmpty(source.Fax))
            collection.Add(new PhoneNumber() { Value = source.Fax, Preferred = false, Type = "Fax" });

         return collection;
      }
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CitizenSc.CrmConnector.Model.DynamicsCrm;

namespace CitizenSc.CrmConnector.Service.DynamicsCrm.MapConfig
{
   public class AddressCollectionResolver : ValueResolver<Contact, ICollection<Model.IAddress>>
   {
      protected override ICollection<Model.IAddress> ResolveCore(Contact source)
      {
         var collection = new List<Model.IAddress>();

         if (!String.IsNullOrEmpty(source.Address1_Line1))
         {
            var address = new Address();

            address.StreetLine1 = source.Address1_Line1;
            address.StreetLine2 = source.Address1_Line2 ?? String.Empty;
            address.StreetLine3 = source.Address1_Line3 ?? String.Empty;
            address.City = source.Address1_City ?? String.Empty;
            address.StateProvince = source.Address1_StateOrProvince ?? String.Empty;
            address.PostalCode = source.Address1_PostalCode ?? String.Empty;
            address.Country = source.Address1_Country ?? String.Empty;
            address.Preferred = true;

            collection.Add(address);
         }

         return collection;
      }
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CitizenSc.CrmConnector.Model.DynamicsCrm;

namespace CitizenSc.CrmConnector.Service.DynamicsCrm.MapConfig
{
   public class EmailCollectionResolver : ValueResolver<Contact, ICollection<Model.IEmailAddress>>
   {
      protected override ICollection<Model.IEmailAddress> ResolveCore(Contact source)
      {
         var collection = new List<Model.IEmailAddress>();

         if (!String.IsNullOrEmpty(source.EMailAddress1))
            collection.Add(new EmailAddress() { Value = source.EMailAddress1, Preferred = true });

         if (!String.IsNullOrEmpty(source.EMailAddress2))
            collection.Add(new EmailAddress() { Value = source.EMailAddress2, Preferred = false });

         if (!String.IsNullOrEmpty(source.EMailAddress3))
            collection.Add(new EmailAddress() { Value = source.EMailAddress3, Preferred = false });

         return collection;
      }
   }
}

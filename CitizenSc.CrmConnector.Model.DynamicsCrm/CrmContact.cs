using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenSc.CrmConnector.Model.DynamicsCrm
{
   public class CrmContact : ICrmContact
   {
      public IPersonalInformation PersonalInformation { get; set; }
      public ICollection<IEmailAddress> EmailAddresses { get; set; }
      public ICollection<IPhoneNumber> PhoneNumbers { get; set; }
      public ICollection<IAddress> Addresses { get; set; }

      public CrmContact()
      {
         PersonalInformation = new PersonalInformation();
         EmailAddresses = new List<IEmailAddress>();
         PhoneNumbers = new List<IPhoneNumber>();
         Addresses = new List<IAddress>();
      }
   }
}

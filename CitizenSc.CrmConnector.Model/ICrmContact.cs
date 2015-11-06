using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenSc.CrmConnector.Model
{
   public interface ICrmContact
   {
      IPersonalInformation PersonalInformation { get; set; }
      ICollection<IEmailAddress> EmailAddresses { get; set; }
      ICollection<IPhoneNumber> PhoneNumbers { get; set; }
      ICollection<IAddress> Addresses { get; set; }
   }
}

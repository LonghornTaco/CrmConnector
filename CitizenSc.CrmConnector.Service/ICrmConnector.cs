using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenSc.CrmConnector.Model;

namespace CitizenSc.CrmConnector.Service
{
   public interface ICrmConnector
   {
      bool DoesEntityExistInCrm(string uniqueIdentifier);
      ICrmContact GetInformationFromCrm(string uniqueIdentifier);
      void SyncCrmToXdb(ICrmContact crmContact);
      void RegisterProfileInCrm(string email, string profile);
   }
}

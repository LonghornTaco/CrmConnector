using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrmConnector.Model;

namespace CrmConnector.Service
{
   public interface ICrmContext
   {
      bool DoesPersonExistInCrm(string uniqueIdentifier);
      ICrmContact GetInformationFromCrm(string uniqueIdentifier);
      void SyncCrmToXdb(ICrmContact crmContact);
      void RegisterProfileInCrm(string email, string profile);
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CitizenSc.CrmConnector.Service;
using Sitecore.Analytics;
using Sitecore.Analytics.Model;
using Sitecore.Pipelines.SessionEnd;

namespace CitizenSc.CrmConnector.Web.Processors.SessionEnd
{
   public class SyncCrmToXdb
   {
      public ICrmConnector CrmConnector { get; set; }

      public void Process(SessionEndArgs args)
      {
         Sitecore.Diagnostics.Log.Info("Calling SyncCrmToXdb.Process", this);
         if (Tracker.Current != null)
         {
            var xdbContact = Tracker.Current.Contact;
            if (Tracker.Current.Contact.Identifiers.IdentificationLevel == ContactIdentificationLevel.Known)
            {
               Sitecore.Diagnostics.Log.Info("The contact is identified - calling CRM to get contact info...", this);
               var crmContact = CrmConnector.GetInformationFromCrm(xdbContact.Identifiers.Identifier);
               if (crmContact != null)
                  CrmConnector.SyncCrmToXdb(crmContact);
            }
         }
      }
   }
}
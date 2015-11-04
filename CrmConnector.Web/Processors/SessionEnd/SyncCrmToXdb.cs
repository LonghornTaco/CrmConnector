using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CrmConnector.Service;
using Sitecore.Analytics;
using Sitecore.Analytics.Model;
using Sitecore.Pipelines.SessionEnd;

namespace CrmConnector.Web.Processors.SessionEnd
{
   public class SyncCrmToXdb
   {
      public ICrmContext CrmContext { get; set; }

      public void Process(SessionEndArgs args)
      {
         Sitecore.Diagnostics.Log.Info("Calling SyncCrmToXdb.Process", this);
         if (Tracker.Current != null)
         {
            var xdbContact = Tracker.Current.Contact;
            if (Tracker.Current.Contact.Identifiers.IdentificationLevel == ContactIdentificationLevel.Known)
            {
               Sitecore.Diagnostics.Log.Info("The contact is identified - calling CRM to get contact info...", this);
               var crmContact = CrmContext.GetInformationFromCrm(xdbContact.Identifiers.Identifier);
               CrmContext.SyncCrmToXdb(crmContact);
            }
         }
      }
   }
}
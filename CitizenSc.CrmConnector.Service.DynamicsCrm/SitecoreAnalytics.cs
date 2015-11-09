using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Analytics;

namespace CitizenSc.CrmConnector.Service.DynamicsCrm
{
   public class SitecoreAnalytics : IAnalytics
   {
      public ITracker Tracker { get { return Sitecore.Analytics.Tracker.Current; } }

      public Sitecore.Analytics.Tracking.Contact GetCurrentContact()
      {
         Sitecore.Analytics.Tracking.Contact contact = null;

         if (Tracker != null && Tracker.IsActive)
         {
            contact = Tracker.Contact;
         }

         return contact;
      }
   }
}

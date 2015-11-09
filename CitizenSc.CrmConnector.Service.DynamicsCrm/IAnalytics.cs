using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Analytics;

namespace CitizenSc.CrmConnector.Service.DynamicsCrm
{
   public interface IAnalytics
   {
      ITracker Tracker { get; }
      Sitecore.Analytics.Tracking.Contact GetCurrentContact();
   }
}

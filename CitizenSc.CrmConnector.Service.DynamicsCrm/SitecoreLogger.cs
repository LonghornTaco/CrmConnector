using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenSc.CrmConnector.Service.DynamicsCrm
{
   public class SitecoreLogger : ILogger
   {
      public void Info(string message, object owner)
      {
         Sitecore.Diagnostics.Log.Info(message, owner);
      }

      public void Warning(string message, object owner)
      {
         Sitecore.Diagnostics.Log.Warn(message, owner);
      }

      public void Error(string message, Exception ex, object owner)
      {
         Sitecore.Diagnostics.Log.Error(message, ex, owner);
      }
   }
}

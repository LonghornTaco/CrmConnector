using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenSc.CrmConnector.Service.DynamicsCrm
{
   public interface ILogger
   {
      void Info(string message, object owner);
      void Warning(string message, object owner);
      void Error(string message, Exception ex, object owner);
   }
}

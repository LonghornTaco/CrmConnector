using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenSc.CrmConnector.Model
{
   public interface IListEntryType
   {
      string Type { get; set; }
      bool Preferred { get; set; }
   }
}

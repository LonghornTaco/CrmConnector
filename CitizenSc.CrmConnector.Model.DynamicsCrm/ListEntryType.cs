using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenSc.CrmConnector.Model.DynamicsCrm
{
   public class ListEntryType : IListEntryType
   {
      public string Type { get; set; }
      public bool Preferred { get; set; }
   }
}

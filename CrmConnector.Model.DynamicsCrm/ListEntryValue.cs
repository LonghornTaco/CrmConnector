using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmConnector.Model.DynamicsCrm
{
   public class ListEntryValue : ListEntryType, IListEntryValue
   {
      public string Value { get; set; }
   }
}

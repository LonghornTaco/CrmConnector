using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmConnector.Model.DynamicsCrm
{
   public class Address : ListEntryType, IAddress
   {
      public string StreetLine1 { get; set; }
      public string StreetLine2 { get; set; }
      public string StreetLine3 { get; set; }
      public string StreetLine4 { get; set; }
      public string City { get; set; }
      public string StateProvince { get; set; }
      public string PostalCode { get; set; }
      public string Country { get; set; }
   }
}

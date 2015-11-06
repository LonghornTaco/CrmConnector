using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenSc.CrmConnector.Model
{
   public interface IAddress : IListEntryType
   {
      string StreetLine1 { get; set; }
      string StreetLine2 { get; set; }
      string StreetLine3 { get; set; }
      string StreetLine4 { get; set; }
      string City { get; set; }
      string StateProvince { get; set; }
      string PostalCode { get; set; }
      string Country { get; set; }
   }
}

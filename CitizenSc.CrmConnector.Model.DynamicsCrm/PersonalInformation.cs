using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenSc.CrmConnector.Model.DynamicsCrm
{
   public class PersonalInformation : IPersonalInformation
   {
      public DateTime? BirthDate { get; set; }
      public string FirstName { get; set; }
      public string MiddleName { get; set; }
      public string LastName { get; set; }
      public string Suffix { get; set; }
      public string Nickname { get; set; }
      public string Title { get; set; }
      public string Gender { get; set; }
      public string JobTitle { get; set; }
   }
}

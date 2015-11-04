using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmConnector.Model
{
   public interface IPersonalInformation
   {
      DateTime? BirthDate { get; set; }
      string FirstName { get; set; }
      string MiddleName { get; set; }
      string LastName { get; set; }
      string Suffix { get; set; }
      string Nickname { get; set; }
      string Title { get; set; }
      string Gender { get; set; }
      string JobTitle { get; set; }
   }
}

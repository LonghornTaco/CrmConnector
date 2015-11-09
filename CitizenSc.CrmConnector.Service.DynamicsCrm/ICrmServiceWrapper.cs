using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CitizenSc.CrmConnector.Service.DynamicsCrm
{
   public interface ICrmServiceWrapper
   {
      Contact FindContact(Expression<Func<Contact, bool>> predicate);
      void UpdateCrm(Contact contact);
   }
}

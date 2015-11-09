using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;

namespace CitizenSc.CrmConnector.Service.DynamicsCrm
{
   public class DynamicsCrmServiceWrapper : ICrmServiceWrapper
   {
      private ILogger _logger;

      public DynamicsCrmServiceWrapper() : this(new SitecoreLogger())
      {
      }

      public DynamicsCrmServiceWrapper(ILogger logger)
      {
         _logger = logger;
      }

      public Contact FindContact(Expression<Func<Contact, bool>> predicate)
      {
         Contact returnValue = null;

         GetConnection(context =>
         {
            returnValue = context.ContactSet.First(predicate);
         });

         return returnValue;
      }

      public void UpdateCrm(Contact contact)
      {
         GetConnection(context =>
         {
            context.UpdateObject(contact);
            context.SaveChanges();
         });
      }

      private void GetConnection(Action<MyDynamicsServiceContext> action)
      {
         _logger.Info("Connecting to CRM...", this);
         var connection = new CrmConnection("CRMServiceConnection");
         using (var org = new OrganizationService(connection))
         {
            try
            {
               var context = new MyDynamicsServiceContext(org);
               action(context);
            }
            catch (Exception ex)
            {
               //Log the exception, then let the business layer handle the error scenario
               _logger.Error("There was a problem connecting to CRM", ex, this);
               throw ex;
            }
         }
      }
   }
}

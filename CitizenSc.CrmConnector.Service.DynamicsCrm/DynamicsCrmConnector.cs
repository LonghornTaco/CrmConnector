using System;
using System.Linq;
using AutoMapper;
using CitizenSc.CrmConnector.Model;
using CitizenSc.CrmConnector.Model.DynamicsCrm;
using CitizenSc.CrmConnector.Service.DynamicsCrm.MapConfig;
using Sitecore.Analytics.Model.Entities;
using Sitecore.Diagnostics;

namespace CitizenSc.CrmConnector.Service.DynamicsCrm
{
   public class DynamicsCrmConnector : ICrmConnector
   {
      private ICrmServiceWrapper _crmService;
      private ILogger _logger;
      private IAnalytics _analytics;

      public DynamicsCrmConnector() : this(new DynamicsCrmServiceWrapper(), new SitecoreLogger(), new SitecoreAnalytics())
      {
         
      }

      public DynamicsCrmConnector(ICrmServiceWrapper crmService, ILogger logger, IAnalytics analytics)
      {
         _crmService = crmService;
         _logger = logger;
         _analytics = analytics;

         DynamicsMapper.Configure();
      }

      public bool DoesEntityExistInCrm(string uniqueIdentifier)
      {
         var returnValue = false;

         try
         {
            returnValue = _crmService.FindContact(c => c.EMailAddress1 == uniqueIdentifier) != null;
         }
         catch (Exception ex)
         {
            _logger.Error("There was an error attempting to find an entity in CRM for " + uniqueIdentifier, ex, this);
         }

         return returnValue;
      }

      public ICrmContact GetInformationFromCrm(string uniqueIdentifier)
      {
         CrmContact crmContact = null;

         try
         {
            _logger.Info("Attempting to find a contact where EMailAddress1 = " + uniqueIdentifier, this);
            var result = _crmService.FindContact(c => c.EMailAddress1 == uniqueIdentifier);
            if (result != null)
            {
               _logger.Info("Found a contact in CRM.", this);
               crmContact = Mapper.Map<Contact, CrmContact>(result);
            }
            else
            {
               _logger.Warning("A contact could not be found for " + uniqueIdentifier, this);
            }
         }
         catch (Exception ex)
         {
            _logger.Error("There was an error getting contact information from CRM", ex, this);
         }

         return crmContact;
      }

      public void SyncCrmToXdb(ICrmContact crmContact)
      {
         try
         {
            _logger.Info("Syncing CRM Contact to Tracker.Current.Contact", this);
            
            var xdbContact = _analytics.GetCurrentContact();

            if (xdbContact != null)
            {
               var emailFacet = xdbContact.GetFacet<IContactEmailAddresses>("Emails");
               var addressFacet = xdbContact.GetFacet<IContactAddresses>("Addresses");
               var personalFacet = xdbContact.GetFacet<IContactPersonalInfo>("Personal");
               var phoneFacet = xdbContact.GetFacet<IContactPhoneNumbers>("Phone Numbers");
               var email = emailFacet.Entries.Contains("Work Email") ? emailFacet.Entries["Work Email"] : emailFacet.Entries.Create("Work Email");
               var address = addressFacet.Entries.Contains("Work Address") ? addressFacet.Entries["Work Address"] : addressFacet.Entries.Create("Work Address");
               var workPhone = phoneFacet.Entries.Contains("Work Phone") ? phoneFacet.Entries["Work Phone"] : phoneFacet.Entries.Create("Work Phone");

               if (crmContact.EmailAddresses.Any())
               {
                  email.SmtpAddress = crmContact.EmailAddresses.First().Value;
                  emailFacet.Preferred = "Work Email";
               }
               if (crmContact.Addresses.Any())
               {
                  address.StreetLine1 = crmContact.Addresses.First().StreetLine1;
                  address.StreetLine2 = crmContact.Addresses.First().StreetLine2;
                  address.StreetLine3 = crmContact.Addresses.First().StreetLine3;
                  address.City = crmContact.Addresses.First().City;
                  address.StateProvince = crmContact.Addresses.First().StateProvince;
                  address.PostalCode = crmContact.Addresses.First().PostalCode;
                  address.Country = crmContact.Addresses.First().Country;
               }
               if (crmContact.PhoneNumbers.Any())
               {
                  phoneFacet.Preferred = "Work Phone";
                  workPhone.Number = crmContact.PhoneNumbers.First().Value;
               }

               personalFacet.Title = crmContact.PersonalInformation.Title;
               personalFacet.JobTitle = crmContact.PersonalInformation.JobTitle;
               personalFacet.FirstName = crmContact.PersonalInformation.FirstName;
               personalFacet.MiddleName = crmContact.PersonalInformation.MiddleName;
               personalFacet.Surname = crmContact.PersonalInformation.LastName;
               personalFacet.Gender = crmContact.PersonalInformation.Gender;
               personalFacet.BirthDate = crmContact.PersonalInformation.BirthDate;
               _logger.Info("Finished syncing CRM Contact", this); 
            }
            else
            {
               _logger.Warning("The current Tracker.Contact was null", this);
            }
         }
         catch (Exception ex)
         {
            _logger.Error("There was a problem syncing the CRM Contact", ex, this);
         }
      }

      public void RegisterProfileInCrm(string email, string profile)
      {
         try
         {
            var contact = _crmService.FindContact(c => c.EMailAddress1 == email);
            if (contact != null)
            {
               contact.new_SitecoreProfile = profile;
               _crmService.UpdateCrm(contact);
            }
         }
         catch (Exception ex)
         {
            _logger.Error("Error registering the profile in CRM", ex, this);
         }
      }
      
   }
}

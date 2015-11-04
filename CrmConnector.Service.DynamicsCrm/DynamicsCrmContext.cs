using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using CrmConnector.Model;
using CrmConnector.Model.DynamicsCrm;
using Sitecore.Analytics;
using Sitecore.Analytics.Model.Entities;

namespace CrmConnector.Service.DynamicsCrm
{
   public class DynamicsCrmContext : ICrmContext
   {
      public DynamicsCrmContext()
      {
         ConfigureMaps();
      }

      public bool DoesPersonExistInCrm(string uniqueIdentifier)
      {
         var returnValue = false;

         try
         {
            var context = GetContext();
            returnValue = context.ContactSet.First(c => c.EMailAddress1 == uniqueIdentifier) != null;
         }
         catch (Exception ex)
         {

         }

         return returnValue;
      }

      public ICrmContact GetInformationFromCrm(string uniqueIdentifier)
      {
         CrmContact crmContact = null;

         try
         {
            Sitecore.Diagnostics.Log.Info("Connecting to CRM...", this);
            var context = GetContext();

            Sitecore.Diagnostics.Log.Info("Attempting to find a contact where EMailAddress1 = " + uniqueIdentifier, this);
            var result = context.ContactSet.FirstOrDefault(c => c.EMailAddress1 == uniqueIdentifier);
            if (result != null)
            {
               Sitecore.Diagnostics.Log.Info("Found a contact in CRM.", this);
               crmContact = Mapper.Map<Contact, CrmContact>(result);
            }
            else
            {
               Sitecore.Diagnostics.Log.Warn("A contact could not be found for " + uniqueIdentifier, this);
            }
         }
         catch (Exception ex)
         {
            //TODO: Determine what to do
            Sitecore.Diagnostics.Log.Error("There was an error getting contact information from CRM", ex, this);
         }

         return crmContact;
      }

      public void SyncCrmToXdb(ICrmContact crmContact)
      {
         try
         {
            Sitecore.Diagnostics.Log.Info("Syncing CRM Contact to Tracker.Current.Contact", this);
            
            var xdbContact = Tracker.Current.Contact;

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
            Sitecore.Diagnostics.Log.Info("Finished syncing CRM Contact", this);
         }
         catch (Exception ex)
         {
            Sitecore.Diagnostics.Log.Error("There was a problem syncing the CRM Contact", ex, this);
         }
      }

      public void RegisterProfileInCrm(string email, string profile)
      {
         try
         {
            var context = GetContext();
            var contact = context.ContactSet.First(c => c.EMailAddress1 == email);
            if (contact != null)
            {
               contact.new_SitecoreProfile = profile;
               context.UpdateObject(contact);
               context.SaveChanges();
            }
         }
         catch (Exception ex)
         {
            Sitecore.Diagnostics.Log.Error("Error registering the profile in CRM", ex, this);
         }
      }

      private RbaCrmContext GetContext()
      {
         var connection = new CrmConnection("CRMServiceConnection");
         var org = new OrganizationService(connection);
         var context = new RbaCrmContext(org);
         return context;
      }

      private void ConfigureMaps()
      {
         Mapper.CreateMap<Contact, PersonalInformation>()
            .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
            .ForMember(d => d.MiddleName, o => o.MapFrom(s => s.MiddleName))
            .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName))
            .ForMember(d => d.Title, o => o.MapFrom(s => s.JobTitle))
            .ForMember(d => d.Nickname, o => o.MapFrom(s => s.NickName))
            .ForMember(d => d.Gender, o => o.ResolveUsing<GenderResolver>())
            .ForMember(d => d.BirthDate, o => o.MapFrom(s => s.BirthDate))
            ;

         Mapper.CreateMap<Contact, CrmContact>()
            .ForMember(d => d.PersonalInformation, o => o.MapFrom(s => Mapper.Map<PersonalInformation>(s)))
            .ForMember(d => d.EmailAddresses, o => o.ResolveUsing<EmailCollectionResolver>())
            .ForMember(d => d.Addresses, o => o.ResolveUsing<AddressCollectionResolver>())
            .ForMember(d => d.PhoneNumbers, o => o.ResolveUsing<PhoneNumberCollectionResolver>())
            ;
      }
   }

   public class GenderResolver : ValueResolver<Contact, String>
   {
      protected override string ResolveCore(Contact source)
      {
         var genderString = String.Empty;

         if (source.GenderCode != null)
         {
            genderString = source.GenderCode.Value == 1 ? "Male" : "Female";
         }

         return genderString;
      }
   }

   public class EmailCollectionResolver : ValueResolver<Contact, ICollection<Model.IEmailAddress>>
   {
      protected override ICollection<Model.IEmailAddress> ResolveCore(Contact source)
      {
         var collection = new List<Model.IEmailAddress>();

         if (!String.IsNullOrEmpty(source.EMailAddress1))
            collection.Add(new EmailAddress() { Value = source.EMailAddress1, Preferred = true });

         if (!String.IsNullOrEmpty(source.EMailAddress2))
            collection.Add(new EmailAddress() { Value = source.EMailAddress2, Preferred = false });

         if (!String.IsNullOrEmpty(source.EMailAddress3))
            collection.Add(new EmailAddress() { Value = source.EMailAddress3, Preferred = false });

         return collection;
      }
   }

   public class AddressCollectionResolver : ValueResolver<Contact, ICollection<Model.IAddress>>
   {
      protected override ICollection<Model.IAddress> ResolveCore(Contact source)
      {
         var collection = new List<Model.IAddress>();

         if (!String.IsNullOrEmpty(source.Address1_Line1))
         {
            var address = new Address();

            address.StreetLine1 = source.Address1_Line1;
            address.StreetLine2 = source.Address1_Line2 ?? String.Empty;
            address.StreetLine3 = source.Address1_Line3 ?? String.Empty;
            address.City = source.Address1_City ?? String.Empty;
            address.StateProvince = source.Address1_StateOrProvince ?? String.Empty;
            address.PostalCode = source.Address1_PostalCode ?? String.Empty;
            address.Country = source.Address1_Country ?? String.Empty;
            address.Preferred = true;

            collection.Add(address);
         }

         return collection;
      }
   }

   public class PhoneNumberCollectionResolver : ValueResolver<Contact, ICollection<Model.IPhoneNumber>>
   {
      protected override ICollection<Model.IPhoneNumber> ResolveCore(Contact source)
      {
         var collection = new List<Model.IPhoneNumber>();

         if (!String.IsNullOrEmpty(source.MobilePhone))
            collection.Add(new PhoneNumber() { Value = source.MobilePhone, Preferred = true, Type = "Mobile" });

         if (!String.IsNullOrEmpty(source.Fax))
            collection.Add(new PhoneNumber() { Value = source.Fax, Preferred = false, Type = "Fax" });

         return collection;
      }
   }
}

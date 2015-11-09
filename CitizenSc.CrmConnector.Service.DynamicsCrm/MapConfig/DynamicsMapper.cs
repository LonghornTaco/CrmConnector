using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CitizenSc.CrmConnector.Model.DynamicsCrm;

namespace CitizenSc.CrmConnector.Service.DynamicsCrm.MapConfig
{
   public class DynamicsMapper
   {
      public static void Configure()
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
}

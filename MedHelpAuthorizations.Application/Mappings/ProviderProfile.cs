using AutoMapper;
using LazyCache.Providers;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Features.Providers.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Providers.GetByCriteria;
using MedHelpAuthorizations.Application.Features.Providers.Queries.GetAllProviders;
using MedHelpAuthorizations.Application.Features.Providers.Queries.GetProviderById;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ProviderProfile : Profile
    {
        public ProviderProfile()
        {
            CreateMap<Address, AddEditProviderCommand>()
                .ReverseMap();                

            CreateMap<Person, AddEditProviderCommand>()
                .ReverseMap();

            CreateMap<AddEditProviderCommand, ClientProvider>()
                .ForPath(x => x.Person.Address, map => map.MapFrom(y => y))
                .ForPath(x => x.Person.Id, map => map.MapFrom(y => y.PersonId))
                .ForMember(x => x.Person, map => map.MapFrom(y => y))
                .ReverseMap();
            

            CreateMap<Person, GetAllProvidersResponse>()    
               .ReverseMap();

            CreateMap<ClientProvider, GetAllProvidersResponse>()
                .ForMember(x => x.FirstName, map => map.MapFrom(y => y.Person.FirstName))
                .ForMember(x => x.LastName, map => map.MapFrom(y => y.Person.LastName))
                .ForMember(x => x.MiddleName, map => map.MapFrom(y => y.Person.LastName))
                .ForMember(x => x.Email, map => map.MapFrom(y => y.Person.Email))
                .ForMember(x => x.OfficePhoneNumber, map => map.MapFrom(y => y.Person.OfficePhoneNumber))
                .ForMember(x => x.FaxNumber, map => map.MapFrom(y => y.Person.FaxNumber))
                .ForMember(x => x.Address, map => map.MapFrom(y => y.Person.Address))
                .ForMember(x => x.DateOfBirth, map => map.MapFrom(y => y.Person.DateOfBirth))
                .ForMember(x => x.SpecialtyId, map => map.MapFrom(y => y.SpecialtyId))
                .ForMember(x => x.License, map => map.MapFrom(y => y.License))
                .ForMember(x => x.Npi, map => map.MapFrom(y => y.Npi))
                .ForMember(x => x.TaxId, map => map.MapFrom(y => y.TaxId))
                .ForMember(x => x.Upin, map => map.MapFrom(y => y.Upin))
                .ForMember(x => x.TaxonomyCode, map => map.MapFrom(y => y.TaxonomyCode))
                .ForMember(x => x.ExternalId, map => map.MapFrom(y => y.ExternalId))
                .ForMember(x => x.Id, map => map.MapFrom(y => y.Id))
                .ForMember(x => x.PersonId, map => map.MapFrom(y => y.PersonId))
                .ForMember(x => x.ClientId, map => map.MapFrom(y => y.ClientId));
                //.ReverseMap();

            CreateMap<GetAllProvidersResponse, ClientProvider>()
                .ForPath(x => x.Person.Address, map => map.MapFrom(y => y))
                .ForMember(x => x.Person, map => map.MapFrom(y => y))                
                .ForMember(x => x.ClientId, map => map.MapFrom(y => y.ClientId));

            //CreateMap<Person, GetByCritieriaProviderResponse>()
            //    .ReverseMap();

            //CreateMap<GetByCritieriaProviderResponse, Provider>()
            //    .ForMember(x => x.Person, map => map.MapFrom(y => y))
            //    .ReverseMap();

            CreateMap<Person, GetProviderByIdResponse>()
               .ReverseMap();

            CreateMap<GetProviderByIdResponse, ClientProvider>()
                .ForMember(x => x.Person, map => map.MapFrom(y => y))
                .ReverseMap();
            CreateMap<GetProvidersByCriteriaResponse, ClientProvider>().ReverseMap();
            CreateMap<ProviderComparisonQuery, ComparisonDashboardQuery>().ReverseMap(); //AA-142
            CreateMap<ProviderChargesTotals, ProviderDenialReasonTotal>().ReverseMap();
            CreateMap<ProviderChargesTotals, DenialReasonsTotalsByProvider>().ReverseMap();
        }
    }
}

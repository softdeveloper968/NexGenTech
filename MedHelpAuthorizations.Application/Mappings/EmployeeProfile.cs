using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.Employees;
using MedHelpAuthorizations.Application.Features.Administration.Employees.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.Employees.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Persons.Commands.UpsertPerson;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()  //AA-206 changed person mappings to user
        {
            CreateMap<EmployeeDto, Employee>().ReverseMap();
            CreateMap<EmployeeManagerDto, Employee>().ReverseMap();

            CreateMap<AddEditEmployeeCommand, Employee>().ReverseMap();
            CreateMap<AddEditEmployeeCommand, Person>().ReverseMap();
            CreateMap<GetAllPagedEmployeeResponse, AddEditEmployeeCommand>().ReverseMap();
            ////.ForPath(dest => dest.DateOfBirth, vm => vm.MapFrom(src => src.User.DateOfBirth))
            //.ForPath(dest => dest.User.PhoneNumber, vm => vm.MapFrom(src => long.Parse(src.User.PhoneNumber)))
            ////.ForPath(dest => dest.OfficePhoneNumber, vm => vm.MapFrom(src => src.Person.OfficePhoneNumber))
            //.ForPath(dest => dest.User.Email, vm => vm.MapFrom(src => src.User.Email))
            //.ForPath(dest => dest.User.LastName, vm => vm.MapFrom(src => src.User.LastName))
            //.ForPath(dest => dest.UserId, vm => vm.MapFrom(src => src.User.Id))
            //.ForPath(dest => dest.User.FirstName, vm => vm.MapFrom(src => src.User.FirstName)).ReverseMap();
            CreateMap<EmployeeDto, AddEditEmployeeCommand>().ReverseMap();
                ////.ForPath(dest => dest.DateOfBirth, vm => vm.MapFrom(src => src.Person.DateOfBirth))
                //.ForPath(dest => dest.User.PhoneNumber, vm => vm.MapFrom(src => long.Parse(src.User.PhoneNumber)))
                ////.ForPath(dest => dest.OfficePhoneNumber, vm => vm.MapFrom(src => src.Person.OfficePhoneNumber))
                //.ForPath(dest => dest.User.Email, vm => vm.MapFrom(src => src.User.Email))
                //.ForPath(dest => dest.User.LastName, vm => vm.MapFrom(src => src.User.LastName))
                //.ForPath(dest => dest.UserId, vm => vm.MapFrom(src => src.User.Id))
                //.ForPath(dest => dest.User.FirstName, vm => vm.MapFrom(src => src.User.FirstName)).ReverseMap();
            CreateMap<UpsertPersonCommand, AddEditEmployeeCommand>().ReverseMap();
            CreateMap<UpsertPersonCommand, Person>().ReverseMap();
        }
    }
}

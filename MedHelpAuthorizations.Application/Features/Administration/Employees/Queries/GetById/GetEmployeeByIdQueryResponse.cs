using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.Administration.Employees.Queries.GetById
{
    //public class GetEmployeeByIdQueryResponse 
    //{
    //    public int Id { get; set; }
    //    public string FirstName { get; set; }
    //    public string MiddleName { get; set; }
    //    public string LastName { get; set; }
    //    public string Email { get; set; }
    //    public long? MobilePhoneNumber { get; set; } 
    //    public DateTime? DateOfBirth { get; set; }
    //    public string EmployeeNumber { get; set; }
    //    public EmployeeDto Manager { get; set; }   
    //    public PersonDto Person { get; set; }
    //    public long? OfficePhoneNumber { get; set; }
    //    public EmployeeRoleDto EmployeeRole { get; set; }
    //    public int ClaimCountRequired { get; set; }
    //    public int RemainingClaimCount { get; set; }
    //    public virtual IList<EmployeeClientDto> EmployeeClients { get; set; }
    //}

    public class EmployeeSearchOptions
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public long? MobilePhoneNumber { get; set; }
        public IList<EmployeeDto> Employees { get; set; }
    }

}

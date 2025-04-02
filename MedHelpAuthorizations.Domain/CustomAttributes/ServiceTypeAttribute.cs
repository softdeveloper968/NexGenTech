using System;

namespace MedHelpAuthorizations.Domain.CustomAttributes
{
    //AA-298
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class ServiceTypeAttribute : Attribute
    {
        public string Name { get; }
        public string Description { get; }
        public string StartDate { get; }
        public string Code { get; }

        public ServiceTypeAttribute(string name, string description, string startDate, string code)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            Code = code;
        }
    }
}

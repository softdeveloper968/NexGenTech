using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Domain.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ExcelCustomAttribute : Attribute
    {
        public string PropertyName { get; }
        public CustomTypeCode CustomType { get; }
        public string Alias { get; }
        public object DefaultValue { get; }

        public ExcelCustomAttribute(string propertyName, CustomTypeCode customType, string alias, object defaultValue)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            CustomType = customType;
            Alias = alias;
            DefaultValue = defaultValue;
        }
    }
}

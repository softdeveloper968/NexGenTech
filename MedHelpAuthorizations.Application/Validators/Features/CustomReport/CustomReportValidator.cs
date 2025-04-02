using MedHelpAuthorizations.Domain.CustomAttributes;
using System.Collections.Generic;
using System.Linq;

namespace MedHelpAuthorizations.Application.Validators.Features.CustomReport
{
	public class CustomReportDictionaryModel : Dictionary<string, object>
    {
        public CustomReportDictionaryModel() { }
        public string PropertyValue { get; set; }
        public CustomTypeCode PropertyTypeCode { get; set; }
    }
    public class CustomReportValidator : AbstractValidator<CustomReportDictionaryModel>
    {
        public string PropertyValue { get; set; }
        public CustomTypeCode PropertyTypeCode { get; set; }
        public CustomReportValidator()
        {
            switch (PropertyTypeCode)
            {
                case CustomTypeCode.Boolean:
                    {
                        RuleFor(model => model[PropertyValue]).Must(x => x != null).WithMessage("Value must be greater than 0.");
                        break;
                    }
                case CustomTypeCode.Int32:
                case CustomTypeCode.Int64:
                    {
                        RuleFor(model => model[PropertyValue]).Must(x => x != null).WithMessage("Value must be greater than 0.");
                        break;
                    }
                case CustomTypeCode.Double:
                    {
                        RuleFor(model => model[PropertyValue]).Must(x => x != null).WithMessage("Value must be greater than 0.");
                        break;
                    }
                case CustomTypeCode.Decimal:
                    {
                        RuleFor(model => model[PropertyValue]).Must(x => x != null).WithMessage("Value must be greater than 0.0m.");
                        break;
                    }
                case CustomTypeCode.String:
                    {
                        RuleFor(model => model[PropertyValue]).Must(x => x != null).WithMessage("Value must be greater than 0.");
                        break;
                    }
                case CustomTypeCode.DateRangeType:
                case CustomTypeCode.DateTime:
                case CustomTypeCode.DateRangeTypeCombined:
                    {
                        RuleFor(model => model[PropertyValue]).Must(x => x != null).WithMessage("Value must be greater than 0.");
                        break;
                    }
                case CustomTypeCode.EnumType:
                    {
                        RuleFor(model => model[PropertyValue]).Must(x => x != null).WithMessage("Value must be Enum Type.");
                        break;
                    }
            }
        }

        private bool ValidateCustomReportProperty(object value)
        {
            // Check if the value is an integer and greater than 0.
            if (value is int intValue && intValue > 0)
            {
                return true;
            }

            return false;
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue
        {
            get
            {
                return async (model, propertyName) =>
                {
                    var result = await ValidateAsync(ValidationContext<CustomReportDictionaryModel>.CreateWithOptions((CustomReportDictionaryModel)model, x =>      x.IncludeProperties(propertyName)));
                    if (result.IsValid)
                        return Array.Empty<string>();
                    return result.Errors.Select(e => e.ErrorMessage);
                };
            }
        }

        //public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        //{
        //    var result = await ValidateAsync(ValidationContext<Dictionary<string, object>>.CreateWithOptions((Dictionary<string, object>)model, x => x.IncludeProperties(propertyName)));
        //    if (result.IsValid)
        //        return Array.Empty<string>();
        //    return result.Errors.Select(e => e.ErrorMessage);
        //};
    }
}

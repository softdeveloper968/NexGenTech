using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Mappings.Converters;

namespace MedHelpAuthorizations.Application.Models.CsvDeserialize
{
	public class X12ClaimCodeLineItemStatusCsvModel
	{
		public string Code { get; set; }

		public string Description { get; set; }

		public X12ClaimCodeTypeEnum X12ClaimCodeTypeId { get; set; }

		public ClaimLineItemStatusEnum? ClaimLineItemStatusId { get; set; } = null;

		public ClaimStatusExceptionReasonCategoryEnum? ClaimStatusExceptionReasonCategoryId { get; set; } = null;

		public sealed class X12ClaimCodeLineItemStatusCsvModelMap : ClassMap<X12ClaimCodeLineItemStatusCsvModel>
		{
			public X12ClaimCodeLineItemStatusCsvModelMap()
			{
				Map(m => m.Code).Name("Code");
				Map(m => m.Description).Name("Description");
				Map(m => m.X12ClaimCodeTypeId).TypeConverter<CsvHelperConverters.CustomEnumConverter<X12ClaimCodeTypeEnum>>().Name("X12ClaimCodeType");
				Map(m => m.ClaimLineItemStatusId).TypeConverter<CsvHelperConverters.CustomNullableEnumConverter<ClaimLineItemStatusEnum?>>().Name("ClaimLineItemStatus");
				Map(m => m.ClaimStatusExceptionReasonCategoryId).TypeConverter<CsvHelperConverters.CustomNullableEnumConverter<ClaimStatusExceptionReasonCategoryEnum?>>().Name("ClaimStatusExceptionReasonCategory");
			}
		}
	}
}

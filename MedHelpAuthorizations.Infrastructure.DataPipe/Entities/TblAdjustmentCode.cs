using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;
using MedHelpAuthorizations.Infrastructure.DataPipe.CustomAttributes;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Models;

[ImportOrderAttribute(1)]
public class TblAdjustmentCode : DfStagingAuditableEntity
{
    public int? Id { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public AdjustmentTypeEnum GetAdjustmentTypeId()
    {
		switch (Type?.Trim()?.Substring(0, 1)?.ToUpper())
        {
            case "D":
                return AdjustmentTypeEnum.Debit;
            case "C":
                return AdjustmentTypeEnum.Credit;
            default: 
                throw new Exception($"Cannot convert AdjustmentCode.Type to AdjustmentTypeEnum.Debit or AdjustmentTypeEnum.Credit. Value: {Type}");
        }
    }
}

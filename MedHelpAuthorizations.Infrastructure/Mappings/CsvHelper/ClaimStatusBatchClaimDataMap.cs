using CsvHelper.Configuration;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Mappings.CsvHelper
{
    public class ClaimStatusBatchClaimDataMap : ClassMap<ClaimStatusBatchClaim>
    {
        public ClaimStatusBatchClaimDataMap()
        {
            Map(m => m.PATIENT_NM).Name("PATIENT_NM");
            Map(m => m.PATIENT_BD).Name("PATIENT_BD");
            Map(m => m.PATIENT_MEDICAID_ID).Name("PATIENT_MEDICAID_ID");
            Map(m => m.DOS_FROM).Name("DOS_FROM");
            Map(m => m.DOS_TO).Name("DOS_TO");
            Map(m => m.PROC_CODE).Name("PROC_CODE");
            Map(m => m.BILLED_AMT).Name("BILLED_AMT");
        }
    }
}

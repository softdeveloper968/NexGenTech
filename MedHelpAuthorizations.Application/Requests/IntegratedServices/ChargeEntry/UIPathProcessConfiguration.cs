using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Text.Json;

namespace MedHelpAuthorizations.Application.Models.IntegratedServices.ChargeEntry
{
    public class UiPathProcessConfiguration
    {
        [JsonPropertyName("in_strSiteName")]
        public string LocationName { get; set; }


        [JsonPropertyName("in_strDateFrom")]
        public DateTime DateOfServiceFrom { get; set; }


        [JsonPropertyName("in_strDateTo")]
        public DateTime DateOfServiceTo { get; set; }


        [JsonPropertyName("in_intChargeEntryBatchId")]
        public int ChargeEntryBatchId { get; set; }


        [JsonPropertyName("in_intClientId")]
        public int ClientId { get; set; }


        [JsonPropertyName("in_strClientCode")]
        public string ClientCode { get; set; }


        [JsonPropertyName("in_strTransactionTypeName")]
        public string TransactionTypeName { get; set; }


        [JsonPropertyName("in_strRpaTypeDescription")]
        public string RpaTypeDescription { get; set; }


        [JsonPropertyName("in_strUserName")]
        public string Username { get; set; }


        [JsonPropertyName("in_strPassword")]
        public string Password { get; set; }


        [JsonPropertyName("in_strTargetUrl")]
        public string TargetUrl { get; set; }

        public TransactionTypeEnum TransactionTypeId { get; set; }

        public RpaTypeEnum RpaTypeId { get; set; }

        public bool IsMaxConsecutiveIssueResolved { get; set; }

        public int AuthTypeId { get; set; }

        public string AuthTypeName { get; set; }


        // When send this.. Only do the failed ones. 
        [JsonPropertyName("in_arrProcessFailedTransactions")]
        public List<string> ProcessFailedTransactions { get; set; }
    }

    public class StartInfo
    {
        public string ReleaseKey { get; set; }

        public string Strategy { get; set; }

        public int JobsCount { get; set; }
        
        [JsonIgnore]
        public UiPathProcessConfiguration InputArguments { get; set; }


        [JsonPropertyName("InputArguments")]
        public string InputArgumentsJsonString => JsonSerializer.Serialize(InputArguments);
    }
}

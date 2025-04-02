using MedHelpAuthorizations.Shared.Helpers;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MedHelpAuthorizations.Application.Options
{
    public class TenantSettings
    {
        public DatabaseSettings Defaults { get; set; }
        public List<Tenant> Tenants { get; set; }
    }
    public class Tenant
    {
        public string Name { get; set; }
        public string TID { get; set; }
        public string ConnectionString { get; set; }
    }
    public partial class DatabaseSettings
    {
        [JsonPropertyName(TenantHelper.DBProvider)]
        public string DBProvider { get; set; }
        [JsonPropertyName(TenantHelper.ConnectionString)]
        public string ConnectionString { get; set; }
        [JsonPropertyName(TenantHelper.DataPipeConnectionString)]
        public string DataPipeConnectionString { get; set; }

    }
    public class ApplicationSettings
    {
        [JsonPropertyName(TenantHelper.DefaultConnection)]
        public string DefaultConnection { get; set; }

        [JsonPropertyName(TenantHelper.DatabaseSettings)]
        public DatabaseSettings DatabaseSettings { get; set; }

        [JsonPropertyName(TenantHelper.AzureBlobStorageSettings)]
        public AzureBlobStorageSettings AzureBlobStorageSettings { get; set; }

        [JsonPropertyName(TenantHelper.AzureSignalIRPath)]
        public string AzureSignalIRPath { get; set; }
    }

    public partial class DatabaseSettings
    {
        [JsonPropertyName(TenantHelper.BetaConnectionString)]
        public string BetaConnectionString { get; set; }

        [JsonPropertyName(TenantHelper.ProdConnectionString)]
        public string ProdConnectionString { get; set; }

        [JsonPropertyName(TenantHelper.HangfireConnection)]
        public string HangfireConnection { get; set; }
    }

    public class AzureBlobStorageSettings
    {

        [JsonPropertyName(TenantHelper.AzureBlobStorage)]
        public string AzureBlobStorage { get; set; }
        [JsonPropertyName(TenantHelper.BetaContainerName)]
        public string BetaContainerName { get; set; }

        [JsonPropertyName(TenantHelper.ProdContainerName)]
        public string ProdContainerName { get; set; }
    }

}

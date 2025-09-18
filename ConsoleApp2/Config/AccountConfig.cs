using System.Text.Json.Serialization;

namespace ConsoleApp2.Config
{
    public class Account
    {
        [JsonPropertyName("environment")]
        public string? Environment { get; set; }

        [JsonPropertyName("IbizaUser")]
        public string? IbizaUser { get; set; }

        [JsonPropertyName("IbizaUserCode")]
        public string? IbizaUserCode { get; set; }

        [JsonPropertyName("GraphUser")]
        public string? GraphUser { get; set; }

        [JsonPropertyName("GraphUserCode")]
        public string? GraphUserCode { get; set; }

        [JsonPropertyName("tenantid")]
        public string? TenantId { get; set; }

        [JsonPropertyName("clientid")]
        public string? ClientId { get; set; }

        [JsonPropertyName("clientSecret")]
        public string? ClientSecret { get; set; }

        [JsonPropertyName("certName")]
        public string? CertName { get; set; }

        [JsonPropertyName("certCode")]
        public string? CertCode { get; set; }

        [JsonPropertyName("portalUrl")]
        public string? PortalUrl { get; set; }

        [JsonPropertyName("keepBrowserOpen")]
        public bool? KeepBrowserOpen { get; set; }
    }
}

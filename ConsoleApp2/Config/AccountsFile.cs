using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ConsoleApp2.Config
{
    public class AccountsFile
    {
        [JsonPropertyName("accounts")]
        public List<Account>? Accounts { get; set; }
    }
}

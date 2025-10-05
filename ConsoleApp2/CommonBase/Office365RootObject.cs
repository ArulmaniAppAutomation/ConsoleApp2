using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Account_Management.Framework
{
    public class Office365RootObject : RootObject
    {
        public Dictionary<string, bool> ExcludedApps { get; set; }

        public string OfficePlatformArchitecture { get; set; }

        public string DefaultFileFormat { get; set; }

        [JsonPropertyName("Update channel")]
        public string UpdateChannel { get; set; }

        public string VersionToInstall { get; set; }

        public string SpecificVersion { get; set; }

        public bool ShouldUninstallOlderVersionsOfOffice { get; set; }

        public List<string> ProductIds { get; set; }

        public bool AutoAcceptEula { get; set; }

        public bool UseSharedComputerActivation { get; set; }

        public List<string> LocalesToInstall { get; set; }

        public AppUpdateInfo AppUpdateInfo { get; set; }
    }

    public class AppUpdateInfo
    {
        [JsonPropertyName("Information URL")]
        public string InformationURL { get; set; }
    }
}

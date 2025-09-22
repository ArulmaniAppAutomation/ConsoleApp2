using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class RootObject
    {
        public string TestCaseName { get; set; }
        public int TestCaseID { get; set; }
        public string AppType { get; set; }
        public AppInfo AppInfo { get; set; }
        public AppUpdateInfo AppUpdateInfo { get; set; }
        public AppValidation AppValidation { get; set; }
        public List<AssignmentEntity> AssignmentEntities { get; set; }
    }

    public class AppInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        [JsonPropertyName("Appstore URL")]
        public string AppstoreURL { get; set; }
        [JsonPropertyName("Minimum operating system")]
        public string MinimumOperatingSystem { get; set; }
        public string Logo { get; set; }
        [JsonPropertyName("Information URL")]
        public string InformationURL { get; set; }
        public string Category { get; set; }
        [JsonPropertyName("Display this as a featured app in the Company Portal")]
        public string DisplayFeaturedApp { get; set; }
    }

    public class AppUpdateInfo
    {
        [JsonPropertyName("Information URL")]
        public string InformationURL { get; set; }
        public string Developer { get; set; }
    }

    public class AppValidation
    {
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string LastModifiedDateTime { get; set; }
        public string PrivacyInformationUrl { get; set; }
        public string InformationUrl { get; set; }
        public string Owner { get; set; }
        public string Developer { get; set; }
        public string Notes { get; set; }
        public int UploadState { get; set; }
        public string PublishingState { get; set; }
        public string AppAvailability { get; set; }
        public string AppStoreUrl { get; set; }
        public string MinimumSupportedOperatingSystem { get; set; }
        public string ApplicableDeviceType { get; set; }
        public string FileName { get; set; }
        public string Categories { get; set; }
        public string LargeIcon { get; set; }
    }

    public class AssignmentEntity
    {
        public string AssignmentType { get; set; }
        public string GroupSelectType { get; set; }
        public bool AssignAllUsers { get; set; }
        public bool AssignAllDevices { get; set; }
        public List<AssignGroup> AssignGroups { get; set; }
    }

    public class AssignGroup
    {
        public string GroupName { get; set; }
        public string InstallContext { get; set; }
    }

}

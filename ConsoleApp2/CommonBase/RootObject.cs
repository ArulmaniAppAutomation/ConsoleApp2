using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Account_Management.Framework
{
    public class RootObject
    {
        public string TestCaseName { get; set; }
        public int TestCaseID { get; set; }
        public List<string> AppType { get; set; }


        public string FilePath { get; set; }
        public AppInfo AppInfo { get; set; }
        public  RequirementsInfo RequirementsInfo { get; set; }

        public string RulesFormat { get; set; }

        public List<DetectionRule> DetectionRules { get; set; }
        public List<RequirementRule> RequirementRules { get; set; }
        public List<AssignmentEntity> AssignmentEntities { get; set; }
        public List<SupersedenceEntity> SupersedenceEntities { get; set; }
        public List<SupersedenceEntity> UpdatedSupersedenceEntities { get; set; }
        public List<DependencyEntity> DependencyEntities { get; set; }
        public List<DependencyEntity> UpdatedDependencyEntities { get; set; }
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
        [JsonPropertyName("Privacy URL")]
        public string  PrivacyURL { get; set; }

        public string Category { get; set; }

        [JsonPropertyName("Display this as a featured app in the Company Portal")]
        public string DisplayFeaturedApp { get; set; }
    }

    public class RequirementsInfo
    {
        [JsonPropertyName("Operating system architecture")]
        public string OperatingSystemArchitecture { get; set; }

        [JsonPropertyName("Allowed Architectures")]
        public string AllowedArchitectures { get; set; }

        [JsonPropertyName("Minimum operating system")]
        public string MinimumOperatingSystem { get; set; }

        [JsonPropertyName("Disk space required (MB)")]
        public string DiskSpaceRequiredMB { get; set; }

        [JsonPropertyName("Physical memory required (MB)")]
        public string PhysicalMemoryRequiredMB { get; set; }

        [JsonPropertyName("Minimum number of logical processors required")]
        public string MinimumLogicalProcessors { get; set; }

        [JsonPropertyName("Minimum CPU speed required (MHz)")]
        public string MinimumCPUSpeedMHz { get; set; }
    }

    public class DetectionRule
    {
        public string RuleType { get; set; }
        public Dictionary<string, string> RuleInfo { get; set; }
    }

    public class RequirementRule
    {
        public string RequirementType { get; set; }
        public Dictionary<string, string> RequirementInfo { get; set; }
    }

    public class AssignmentEntity
    {
        public string AssignmentType { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GroupSelectType GroupSelectType { get; set; }
        public bool AssignAllUsers { get; set; }
        public bool AssignAllDevices { get; set; }
        public List<AssignGroup> AssignGroups { get; set; }

        public AllDevicesAssignFilterSetting AllDevicesAssignFilterSetting { get; set; }
        public AllDevicesAssignFilterSetting AllUsersAssignFilterSetting { get; set; }

    }
    [Serializable]
    public enum GroupSelectType
    {
        IncludedGroups,
        ExcludedGroups
    }

    public class AllDevicesAssignFilterSetting
    {
        public string FilterBehave { get; set; }
        public string FilterName { get; set; }
    }

    public class AssignGroup
    {
        public string GroupName { get; set; }
        public string InstallContext { get; set; }
        public GroupAssignFilterSetting AssignFilters { get; set; }
    }
    [Serializable]
    public class GroupAssignFilterSetting
    {
        public string FilterBehave { get; set; }
        public string FilterName { get; set; }
    }

    public class SupersedenceEntity
    {
        public string Name { get; set; }
        public bool UninstallPreviousVersion { get; set; }
    }

    public class DependencyEntity
    {
        public string Name { get; set; }
        public bool AutomaticallyInstall { get; set; }
    }
}

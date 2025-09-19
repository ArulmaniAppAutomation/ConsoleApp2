
namespace PlaywrightTests.Common.Controller.VerifySection
{
    internal interface IVerifySection
    {
        public void SetNode(string node);

        // Configuration settings section
        public Task VerifyBasedOnLabelAsync(string labelName, string? Value = null, string? hasNotText = null, int nth = 0);
        public Task VerifyBasededOnValueAsync(string existText, string value);
        public Task VerifyBasedOnIdAsync(string id, string labelName, string? value = null);
        public Task VerifyBasedOnLabelAndValueAsync(string setting, string? value = null);
        // Assignments section
        public Task VerifyAssignmentsGroupsAsync(string groupName, string labelName = "Included groups");
        // Basics section
        public Task VerifyBasicsSectionAsync(string labelName, string value);
    }
}

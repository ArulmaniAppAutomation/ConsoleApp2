using PlaywrightTests.Common.Helper;

namespace PlaywrightTests.Common.Model
{
    public class CommonEntity : BaseEntity
    {
        public List<Scenario> Scenarios { get; set; }
        public List<string> Language { get; set; } = new List<string>() { "English" };
    }
    public class Scenario
    {
        public List<Steps> Steps { get; set; }
        public string UtilsName { get; set; }
    }
    public class Steps
    {
        public List<ControlInfo> Preparation { get; set; } = new List<ControlInfo>();
        public List<TabConfigInfo> TabConfigInfos { get; set; } = new List<TabConfigInfo>();
    }
    public class TabConfigInfo
    {
        public List<ControlInfo> ConfigInfo { get; set; } = new List<ControlInfo>();
    }
    public class ControlInfo
    {
        public string ControlType { get; set; }
        public string Text { get; set; }
        public List<string> Value { get; set; } = new List<string>();
        private string _operation = string.Empty;
        private string _operationValue = string.Empty;
        public string OperationValue
        {
            get
            {
                if (this.Value != null && this.Value.Any())
                {
                    _operationValue = this.Value[0];
                }
                return _operationValue;
            }
            set
            {
                _operationValue = value;
            }
        }
        public string Operation
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ControlType))
                {
                    return this.ControlType;
                }
                return _operation;
            }
            set
            {
                _operation = value;
            }
        }
        public Dictionary<string, string> Parameter { get; set; } = new Dictionary<string, string>();
    }
}
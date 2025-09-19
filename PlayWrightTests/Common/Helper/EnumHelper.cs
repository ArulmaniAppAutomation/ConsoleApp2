namespace PlaywrightTests.Common.Helper
{
    public class EnumHelper
    {
        public enum Environment
        {
            SH,
            PE,
            CTIP
        }
        public enum Language
        {
            English,
            Chinese,
            Deutsch
        }
        public enum StepResultStatus
        {
            Success,
            Failed
        }
        public enum AlertType
        {
            Failure,
            Summary,
            Info,
            Warning

        }
    }
}

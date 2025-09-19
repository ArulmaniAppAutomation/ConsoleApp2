using log4net.Layout;

namespace LogService
{
    public class LogLayout : PatternLayout
    {
        public override string Header
        {
            get
            {
                return string.Format("******Start Run PlaywrightTests Automation******\r\n");
            }
            set
            {
                base.Header = value;
            }
        }

        public override string Footer
        {
            get
            {
                return string.Format("******Finish Run PlaywrightTests Automation******\r\n\r\n");
            }
            set
            {
                base.Footer = value;
            }
        }
    }
}

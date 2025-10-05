using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.Helper
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


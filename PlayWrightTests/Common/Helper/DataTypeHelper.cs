using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Common.Helper
{
    public class DataTypeHelper
    {
        public static Type IdentityDataType(string? data)
        {
            if (data.Length >10 && DateTime.TryParse(data, out DateTime dateTimeResult))
            {
                return typeof(DateTime);
            }
            else
            {
                return typeof(string);
            }
        }
    }
}

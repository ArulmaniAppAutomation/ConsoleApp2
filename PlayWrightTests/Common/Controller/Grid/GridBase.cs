using LogService;
using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using System.Data;

namespace PlaywrightTests.Common.Controller.Grid
{
    public class GridBase : BaseController, IController
    {
        public List<string> UnableToSortColumnNameList = new List<string>() { "Scope tags"};
        public GridBase(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null,string noDataText=null) : base(page, frameName, language, parentLocator,noDataText)
        {

        }
        public List<string> BaseGetColumnsDataByColumnName(DataTable dataTableSource, string columnName)
        {
            var columnData = new List<string>();
            foreach (DataRow row in dataTableSource.Rows)
            {
                columnData.Add(row[columnName].ToString());
            }
            return columnData.Where(t => !string.IsNullOrEmpty(t)).ToList();
        }

        public string GetCurrentLanguageText(string key)
        {
            throw new NotImplementedException();
        }
        public bool SortValidation(List<string> targetColumnValues)
        {
            bool result = false;
            var distinctValues = targetColumnValues.Distinct().ToList();
            int loopTime = 0;
            int maxLoopTimes = targetColumnValues.Count + distinctValues.Count + 5;
            string? verifyValue = null;
            while (targetColumnValues.Count > 0 && distinctValues.Count > 0 && loopTime < maxLoopTimes)
            {
                loopTime++;
                if (verifyValue == null)// avoid "" value
                {
                    LogHelper.Info($"set value for verifyValue: {verifyValue}");
                    verifyValue = targetColumnValues.First();
                }
                if (verifyValue == targetColumnValues.First())
                {
                    LogHelper.Info($"remove: {verifyValue}");
                    targetColumnValues.RemoveAt(0);
                    if (targetColumnValues.Count == 0) //while targetColumnValues.Count equals 0,but the last value of distinctValues not handle
                    {
                        LogHelper.Info($"remove \"{verifyValue}\" from distinct list...");
                        distinctValues.Remove(verifyValue);
                    }
                }
                else
                {
                    LogHelper.Info($"remove \"{verifyValue}\" from distinct list...");
                    distinctValues.Remove(verifyValue);
                    verifyValue = null;
                }
            }

            if (distinctValues.Count == 0 && targetColumnValues.Count == 0)
            {
                result = true;
            }
            return result;
        }
        
    }
}

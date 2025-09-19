using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using System.IO;

namespace PlaywrightTests.Common.Controller.ToolBar
{
    public class ToolBarBase : BaseController, IController
    {

        private string ExportFilePath = $@"{AppDomain.CurrentDomain.BaseDirectory}Common\Export\ToolBar\";
        public ToolBarBase(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null) : base(page, frameName, language, parentLocator)
        {
        }
        public string GetExportFilePath(string folderName)
        {
            var folderPath= ExportFilePath+folderName+ "\\"+DateTime.Now.ToString("yyyyMMddHHmmssffff");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }
        public async Task<string> BaseExportFileAsync(string folderName,Func<Task> ClickExport)
        {
            var download = await CurrentIPage.RunAndWaitForDownloadAsync(async () =>
            {
                await ClickExport();
            }, new PageRunAndWaitForDownloadOptions() { Timeout = 100000 });
            var filePath = GetExportFilePath(folderName) + download.SuggestedFilename;
            await download.SaveAsAsync(filePath);
            Assert.True(new FileInfo(filePath).Exists);
            return filePath;
        }
        public string GetCurrentLanguageText(string key)
        {
            string result = string.Empty;
            switch (this.CurrentLanguage)
            {
                case EnumHelper.Language.English:
                    result = key;
                    break;
                case EnumHelper.Language.Chinese:
                    {
                        switch (key)
                        {
                            #region A
                            case "Add":
                                result = "添加";
                                break;
                            #endregion                            
                            #region B
                            case "Bulk device actions":
                                result = "批量设备操作";
                                break;
                            case "Bulk operations":
                                result = "批量操作";
                                break;
                            case "Bulk restore":
                                result = "批量还原";
                                break;
                            #endregion
                            #region C
                            case "Create":
                                result = "创建";
                                break;
                            case "Create policy":
                                result = "创建策略";
                                break;
                            case "Columns":
                                result = "列";
                                break;
                            #endregion
                            #region D
                            case "Delete":
                                result = "删除";
                                break;
                            case "Delete permanently":
                                result = "永久删除";
                                break;
                            case "Download users":
                                result = "下载用户";
                                break;
                            case "Download":
                                result = "下载";
                                break;
                            #endregion
                            #region E
                            case "Export":
                                result = "导出";
                                break;
                            case "Export Data Settings":
                                result = "导出数据设置";
                                break;
                            #endregion
                            #region F
                            case "Filter":
                                result = "筛选器";
                                break;
                            #endregion
                            #region H
                            case "Help":
                                result = "帮助";
                                break;
                            #endregion
                            #region M
                            case "Manage view":
                                result = "管理视图";
                                break;
                            #endregion
                            #region N
                            case "New Support Request":
                                result = "新建支持请求";
                                break;
                            case "New user":
                                result = "新用户";
                                break;
                            #endregion
                            #region P
                            case "Per-user MFA":
                                result = "每用户MFA";
                                break;
                            case "Preview features":
                                result = "预览版功能";
                                break;
                            #endregion
                            #region R
                            case "Refresh":
                                result = "刷新";
                                break;
                            case "Restore users":
                                result = "还原用户";
                                break;
                            #endregion
                            #region S
                            case "Start Over":
                                result = "重新开始";
                                break;
                            #endregion
                            #region T
                            case "Troubleshoot":
                                result = "疑难解答";
                                break;
                                #endregion
                        }
                    }
                    break;

            }
            return result;
        }
    }
}

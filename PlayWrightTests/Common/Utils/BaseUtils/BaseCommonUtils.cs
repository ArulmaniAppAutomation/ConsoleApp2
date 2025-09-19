//using LogService;
//using LogService.Extension;
using Microsoft.Playwright;
using Microsoft.VisualStudio.Services.Common;
using Newtonsoft.Json;
using PlaywrightTests.Common.Controller.SiteBar;
using PlaywrightTests.Common.Controller.SiteBarMenu;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.Utils.BaseUtils.PopUp;
using PlaywrightTests.Common.Utils.BaseUtils.UtilInterface;
using System.IO;
using System.Net.Http;
using System.Reflection;
using static PlaywrightTests.Common.Helper.EnumHelper;

namespace PlaywrightTests.Common.Utils.BaseUtils
{
    public class BaseCommonUtils
    {
        protected ISiteBar siteBar;
        protected ISiteBarMenu siteBarMenu;
        protected Language CurrentLanguage;
        protected string CurrentEnv = string.Empty;
        protected IPage CurrentIPage { get; set; }
        private string BaseUniqueSuffix { get; set; }
        private static Dictionary<string, string> SonUtilsWithUniqueSuffix = new Dictionary<string, string>();
        private static List<string> BaseUniqueProfileNameList = new List<string>();
        public BaseCommonUtils(IPage page, string env)
        {
            this.BaseUniqueSuffix = CreateUniqueText("");
            while (SonUtilsWithUniqueSuffix.ContainsKey(this.BaseUniqueSuffix))
            {
                this.BaseUniqueSuffix = CreateUniqueText("");
            }
            SonUtilsWithUniqueSuffix.Add(this.BaseUniqueSuffix, this.GetType().Namespace + "." + this.GetType().Name);
            this.CurrentIPage = page;
            this.CurrentEnv = env.ToUpper();

            this.siteBar = new FXSSiteBar(page, null, CurrentLanguage);
            this.siteBarMenu = new MSPortalFxMenuSiteBarMenu(page, null, CurrentLanguage);
        }
        public string BaseCreateUniqueProfileName(string profileName)
        {
            var uniqueProfileName = CreateUniqueText(profileName) + this.BaseUniqueSuffix;
            BaseUniqueProfileNameList.Add(uniqueProfileName);
            return uniqueProfileName;
        }
        public void BaseAddUniqueProfileName(string profileName, string baseUniqueSuffix, string nameSpaceWithClassName)
        {
            BaseUniqueProfileNameList.Add(profileName);
            if (!SonUtilsWithUniqueSuffix.ContainsKey(baseUniqueSuffix))
            {
                SonUtilsWithUniqueSuffix.Add(baseUniqueSuffix, nameSpaceWithClassName);
            }
        }
        public void BaseRemoveUniqueProfileName(string profileName)
        {
            if (BaseUniqueProfileNameList.Contains(profileName))
                BaseUniqueProfileNameList.Remove(profileName);
        }
        public string BaseGetUniqueProfileName(string profileName)
        {
            return BaseUniqueProfileNameList.Where(t => t.Contains(profileName)).FirstOrDefault() ?? string.Empty;
        }
        public static async Task ClearUniqueProfileNameListAsync(IPage page, string environment)
        {
            try
            {
                if (SonUtilsWithUniqueSuffix.Any() && BaseUniqueProfileNameList.Any())
                {
                    var values = SonUtilsWithUniqueSuffix.Values.Distinct().ToList();
                    values.Reverse();
                    foreach (var value in values)
                    {
                        var utilsGroup = SonUtilsWithUniqueSuffix.Where(x => x.Value == value).ToList();
                        var utilsNameSpaceWithClassName = utilsGroup.First().Value;
                        var utilsNameSpace = utilsNameSpaceWithClassName.Substring(0, utilsNameSpaceWithClassName.LastIndexOf('.'));
                        var utilsClassName = utilsNameSpaceWithClassName.Substring(utilsNameSpaceWithClassName.LastIndexOf('.') + 1);

                        var allProfileNameList = BaseUniqueProfileNameList.Where(t => utilsGroup.Select(ug => ug.Key).ToList().Any(ugk => t.Contains(ugk))).ToList();
                        if (allProfileNameList.Any())
                        {
                            var utilsType = ReflectHelper.GetFeatureUtilsType(utilsClassName, utilsNameSpace);
                            if (utilsType != null)
                            {
                                var utilsObj = Activator.CreateInstance(utilsType, new object[] { page, environment });
                                if (utilsObj != null)
                                {
                                    var staticFields = utilsType.GetFields(BindingFlags.Static | BindingFlags.NonPublic);
                                    staticFields.AddRange(utilsType.GetFields(BindingFlags.Static | BindingFlags.Public));
                                    foreach (var field in staticFields)
                                    {
                                        object defaultValue = Activator.CreateInstance(field.FieldType);
                                        field.SetValue(null, defaultValue);
                                    }
                                    var utils = (InterfaceUtils)utilsObj;
                                    allProfileNameList.Reverse();
                                    foreach (string specialUniqueProfileName in allProfileNameList)
                                    {
                                        try
                                        {
                                           // LogHelper.Info($"Start to delete profile: {specialUniqueProfileName}");
                                            await utils.ClearSpecialDataAsync(specialUniqueProfileName);
                                        }
                                        catch { }
                                        finally
                                        {
                                            BaseUniqueProfileNameList.Remove(specialUniqueProfileName);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                SonUtilsWithUniqueSuffix.Clear();
                BaseUniqueProfileNameList.Clear();
            }
        }

        public async Task RefreshCurrentPageAsync()
        {
            await CurrentIPage.ReloadAsync();
        }
        public async Task GoToHomePageAsync()
        {
            await RefreshCurrentPageAsync();
            await siteBar.ClickHomeAsync();
        }
        /// <summary>
        /// The main function for this utils
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sonUtils"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="CustomLogException"></exception>
        public async Task RunAsync<T>(T sonUtils, CommonEntity entity) where T : BaseCommonUtils, InterfaceUtils
        {
            Dictionary<string, string> parameter = new Dictionary<string, string>();
            var TClassName = sonUtils.GetType().Name;
            var TClassNameSpace = sonUtils.GetType().Namespace;
            if (entity.Scenarios.Any())
            {
                foreach (var scenario in entity.Scenarios)
                {
                    var scenarioClassName = string.Empty;
                    var scenarioNameSpace = string.Empty;
                    if (scenario.UtilsName.Contains('.'))
                    {
                        var scenarioNameArray = scenario.UtilsName.Split('.');
                        scenarioClassName = scenarioNameArray[scenarioNameArray.Length - 1];
                        scenarioNameSpace = scenario.UtilsName.Replace($".{scenarioClassName}", "");
                    }
                    else
                    {
                        scenarioClassName = scenario.UtilsName;
                    }

                    if (string.IsNullOrEmpty(scenario.UtilsName) ||
                        (string.IsNullOrEmpty(scenarioNameSpace) && TClassName == scenarioClassName) ||
                        (TClassName == scenarioClassName && TClassNameSpace == scenarioNameSpace))
                    {
                        parameter = await RunAsync(sonUtils, scenario, parameter, entity);
                    }
                    else
                    {
                        var SpecialUtilsType = string.IsNullOrEmpty(scenarioNameSpace) ? ReflectHelper.GetFeatureUtilsType(scenarioClassName) : ReflectHelper.GetFeatureUtilsType(scenarioClassName, scenarioNameSpace);
                        if (SpecialUtilsType != null)
                        {
                            var SpecialUtilsObj = Activator.CreateInstance(SpecialUtilsType, new object[] { this.CurrentIPage, this.CurrentEnv });
                            if (SpecialUtilsObj != null)
                            {
                                var SpecialUtils = (InterfaceUtils)SpecialUtilsObj;
                                if (SpecialUtils != null)
                                {
                                    parameter = await RunAsync(SpecialUtils, scenario, parameter, entity);
                                }
                                else
                                {
                                   // throw new CustomLogException($"The {scenario.UtilsName} object convert to InterfaceUtils error");
                                }
                            }
                            else
                            {
                                //throw new CustomLogException($"The {scenario.UtilsName} Utils object can't be created");
                            }
                        }
                        else
                        {
                           // throw new CustomLogException($"The {scenario.UtilsName} Type is not found in the utils folder");
                        }
                    }

                }
            }

        }
        private async Task<Dictionary<string, string>> RunAsync(InterfaceUtils utilsObject, Scenario scenario, Dictionary<string, string> parameter, CommonEntity entity)
        {
            foreach (var step in scenario.Steps)
            {
                if (step.Preparation.Any())
                {
                    foreach (var preparation in step.Preparation)
                    {
                        //LogHelper.Info($"Run Class:{utilsObject.GetType().Namespace}.{utilsObject.GetType().Name} Step:{preparation.Operation}{preparation.ControlType} value:{JsonConvert.SerializeObject(preparation.Value)}{preparation.OperationValue}");
                        preparation.Parameter = parameter;
                        var isContiune = await utilsObject.RunStepAsync(preparation);
                        parameter = isContiune.Parameter;
                        WriteAppAutomationResultToEntity(entity, parameter);
                        //LogHelper.Info($"Finish Class:{utilsObject.GetType().Namespace}.{utilsObject.GetType().Name} Step:{preparation.Operation}{preparation.ControlType} value:{JsonConvert.SerializeObject(preparation.Value)}{preparation.OperationValue} IsContiune:{isContiune.ToString()}");
                        if (!isContiune.IsContinue)
                        {
                            break;
                        }
                    }
                }
                if (step.TabConfigInfos.Any())
                {
                    foreach (var tabConfigInfo in step.TabConfigInfos)
                    {
                        if (tabConfigInfo.ConfigInfo.Any())
                        {
                            foreach (var configInfo in tabConfigInfo.ConfigInfo)
                            {
                              //  ConsoleHelper.ColoredResult(ConsoleColor.Yellow, $"Run Class:{utilsObject.GetType().Namespace}.{utilsObject.GetType().Name} Step:{configInfo.Operation}{configInfo.ControlType}  setting:{configInfo.Text} value:{JsonConvert.SerializeObject(configInfo.Value)}{configInfo.OperationValue}");
                                configInfo.Parameter = parameter;
                                var isContiune = await utilsObject.RunStepAsync(configInfo);
                                parameter = isContiune.Parameter;
                                WriteAppAutomationResultToEntity(entity, parameter);
                                //LogHelper.Info($"Finish Class:{utilsObject.GetType().Namespace}.{utilsObject.GetType().Name} Step:{configInfo.Operation}{configInfo.ControlType} value:{JsonConvert.SerializeObject(configInfo.Value)}{configInfo.OperationValue} IsContiune:{isContiune.ToString()}");
                                if (!isContiune.IsContinue)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return parameter;
        }
        private void WriteAppAutomationResultToEntity(CommonEntity entity, Dictionary<string, string> parameter)
        {
            // For AppAutomation write every step result to entity
            if (parameter != null)
            {
                if (parameter.TryGetValue("AppAutomationAppName", out string AppAutomationAppName))
                {
                    entity.AppAutomationAppName = AppAutomationAppName;
                }
                if (parameter.TryGetValue("AppAutomationAppCreation", out string AppAutomationAppCreation))
                {
                    entity.AppAutomationAppCreation = AppAutomationAppCreation;
                }
                if (parameter.TryGetValue("AppAutomationAppAssignment", out string AppAutomationAppAssignment))
                {
                    entity.AppAutomationAppAssignment = AppAutomationAppAssignment;
                }
                if (parameter.TryGetValue("AppAutomationAPPDependency", out string AppAutomationAPPDependency))
                {
                    entity.AppAutomationAPPDependency = AppAutomationAPPDependency;
                }
                if (parameter.TryGetValue("AppAutomationAPPSupersede", out string AppAutomationAPPSupersede))
                {
                    entity.AppAutomationAPPSupersede = AppAutomationAPPSupersede;
                }
                if (parameter.TryGetValue("AppAutomationAppUpdated", out string AppAutomationAppUpdated))
                {
                    entity.AppAutomationAppUpdated = AppAutomationAppUpdated;
                }
                if (parameter.TryGetValue("AppAutomationVerifyResult", out string AppAutomationVerifyResult))
                {
                    entity.AppAutomationVerifyResult = AppAutomationVerifyResult;
                }
                if (parameter.TryGetValue("AppAutomationAppDelete", out string AppAutomationAppDelete))
                {
                    entity.AppAutomationAppDelete = AppAutomationAppDelete;
                }
                if (parameter.TryGetValue("AppAutomationAppURL", out string AppAutomationAppURL))
                {
                    entity.AppAutomationAppURL = AppAutomationAppURL;
                }
                if (parameter.TryGetValue("AppAutomationAppPFN", out string AppAutomationAppPFN))
                {
                    entity.AppAutomationAppPFN = AppAutomationAppPFN;
                }
                if (parameter.TryGetValue("AppAutomationFileUrl", out string AppAutomationFileUrl))
                {
                    entity.AppAutomationFileUrl = AppAutomationFileUrl;
                }
            }
        }
        public async Task BaseExecuteStepAsync(string? operation, string? operationValue = null, string? iFrameName = null, ControlInfo controlInfo = null)
        {
            if (string.IsNullOrEmpty(operation))
            {
                //throw new CustomLogException($"The controlType is null, please set the right value!");
            }
            switch (operation)
            {
                #region Menu
                case "ClickPropertiesAsync":
                    await siteBarMenu.ClickPropertiesAsync();
                    break;
                #endregion

                #region Button
                case "ClickNextBtnAsync":
                    await ClickNextBtnAsync(iFrameName);
                    break;
                case "ClickNextToBtnAsync":
                    await ClickNextToBtnAsync(iFrameName);
                    break;
                case "ClickPreviousBtnAsync":
                    await ClickPreviousBtnAsync(iFrameName);
                    break;
                case "ClickCreateBtnAsync":
                    await ClickCreateBtnAsync(iFrameName);
                    break;
                case "ClickSelectBtnAsync":
                    await ClickSelectBtnAsync(iFrameName);
                    break;
                case "ClickReviewSaveBtnAsync":
                    await ClickReviewSaveBtnAsync(iFrameName);
                    break;
                case "ClickReviewCreateBtnAsync":
                    await ClickReviewCreateBtnAsync(iFrameName);
                    break;
                case "ClickCancelBtnAsync":
                    await ClickCancelBtnAsync(iFrameName);
                    break;
                case "ClickSaveBtnAsync":
                    await ClickSaveBtnAsync(iFrameName);
                    break;
                case "ClickAddBtnAsync":
                    await ClickAddBtnAsync(iFrameName);
                    break;
                case "ClickDeployBtnAsync":
                    await ClickDeployBtnAsync(iFrameName);
                    break;
                case "PressTabAsync":
                    await CurrentIPage.Keyboard.PressAsync("Tab");
                    break;
                case "ClickCloseContentBtnAsync":
                    await ClickCloseContentBtnAsync(this.CurrentIPage, operationValue, iFrameName);
                    break;            
                #endregion
                #region Tool Bar
                case "ClickToolBarCreateButtonAsync":
                    await ClickToolBarCreateButtonAsync();
                    break;
                case "ClickToolBarAddButtonAsync":
                    await ClickToolBarAddButtonAsync();
                    break;
                case "ClickToolBarRefreshButtonAsync":
                    await ClickToolBarRefreshButtonAsync();
                    break;
                case "ClickToolBarFilterButtonAsync":
                    await ClickToolBarFilterButtonAsync();
                    break;
                case "ClickToolBarColumnsButtonAsync":
                    await ClickToolBarColumnsButtonAsync();
                    break;
                case "ClickToolBarExportButtonAsync":
                    await ClickToolBarExportButtonAsync();
                    break;
                #endregion
                #region Set Every Step Status 
                case "InitAppAutomationAppCreation":
                    DictionaryItemProcess(controlInfo?.Parameter, "AppAutomationAppCreation", StepResultStatus.Failed.ToString());
                    break;
                case "SuccessAppAutomationAppCreation":
                    DictionaryItemProcess(controlInfo?.Parameter, "AppAutomationAppCreation", StepResultStatus.Success.ToString());
                    break;

                case "InitAppAutomationAppAssignment":
                    DictionaryItemProcess(controlInfo?.Parameter, "AppAutomationAppAssignment", StepResultStatus.Failed.ToString());
                    break;
                case "SuccessAppAutomationAppAssignment":
                    DictionaryItemProcess(controlInfo?.Parameter, "AppAutomationAppAssignment", StepResultStatus.Success.ToString());
                    break;

                case "InitAppAutomationAPPDependency":
                    DictionaryItemProcess(controlInfo?.Parameter, "AppAutomationAPPDependency", StepResultStatus.Failed.ToString());
                    break;
                case "SuccessAppAutomationAPPDependency":
                    DictionaryItemProcess(controlInfo?.Parameter, "AppAutomationAPPDependency", StepResultStatus.Success.ToString());
                    break;

                case "InitAppAutomationAPPSupersede":
                    DictionaryItemProcess(controlInfo?.Parameter, "AppAutomationAPPSupersede", StepResultStatus.Failed.ToString());
                    break;
                case "SuccessAppAutomationAPPSupersede":
                    DictionaryItemProcess(controlInfo?.Parameter, "AppAutomationAPPSupersede", StepResultStatus.Success.ToString());
                    break;

                case "InitAppAutomationAppUpdated":
                    DictionaryItemProcess(controlInfo?.Parameter, "AppAutomationAppUpdated", StepResultStatus.Failed.ToString());
                    break;
                case "SuccessAppAutomationAppUpdated":
                    DictionaryItemProcess(controlInfo?.Parameter, "AppAutomationAppUpdated", StepResultStatus.Success.ToString());
                    break;

                case "InitAppAutomationAppDelete":
                    DictionaryItemProcess(controlInfo?.Parameter, "AppAutomationAppDelete", StepResultStatus.Failed.ToString());
                    break;
                case "SuccessAppAutomationAppDelete":
                    DictionaryItemProcess(controlInfo?.Parameter, "AppAutomationAppDelete", StepResultStatus.Success.ToString());
                    break;

                case "initAppAutomationVerifyResult":
                    DictionaryItemProcess(controlInfo?.Parameter, "AppAutomationVerifyResult", StepResultStatus.Failed.ToString());
                    break;
                case "SuccessAppAutomationVerifyResult":
                    DictionaryItemProcess(controlInfo?.Parameter, "AppAutomationVerifyResult", StepResultStatus.Success.ToString());
                    break;
                #endregion
                #region DownLoad file
                case "DownLoadFileByUrl":
                    DictionaryItemProcess(controlInfo?.Parameter, "AppAutomationFileUrl", operationValue);
                    await DownLoadFileByUrlAsync(operationValue);
                    break;
                #endregion
                default:
                    throw new;

            }
        }
        #region Common Button
        public async Task ClickNextBtnAsync(string? iFrameName = null)
        {
            await ControlHelper.ClickByButtonRoleAndHasTextAsync(this.CurrentIPage, "Next", 0, iFrameName: iFrameName);
          //  ConsoleHelper.ColoredResult(ConsoleColor.Green, "Click \"Next\" done...");
        }
        public async Task ClickNextToBtnAsync(string? iFrameName = null)
        {
            await ControlHelper.ClickByButtonRoleAndHasTextAsync(this.CurrentIPage, "Next", 0, iFrameName: iFrameName);
           // ConsoleHelper.ColoredResult(ConsoleColor.Green, "Click \"Next\" done...");
        }
        public async Task ClickPreviousBtnAsync(string? iFrameName = null)
        {
            await ControlHelper.ClickByButtonRoleAndNameAsync(this.CurrentIPage, "Previous", 0, iFrameName: iFrameName);
          //  ConsoleHelper.ColoredResult(ConsoleColor.Green, "Click \"Previous\" done...");
        }
        public async Task ClickCreateBtnAsync(string? iFrameName = null)
        {
            await ControlHelper.ClickByButtonRoleAndNameAsync(this.CurrentIPage, "Create", 0, iFrameName: iFrameName);
          //  ConsoleHelper.ColoredResult(ConsoleColor.Green, "Click \"Create\" done...");
        }
        public async Task ClickSelectBtnAsync(string? iFrameName = null)
        {
            await ControlHelper.ClickByButtonRoleAndNameAsync(this.CurrentIPage, "Select", 0, iFrameName: iFrameName);
           // ConsoleHelper.ColoredResult(ConsoleColor.Green, "Click \"Select\" done...");
        }
        public async Task ClickReviewSaveBtnAsync(string? iFrameName = null)
        {
            await ControlHelper.ClickByButtonRoleAndNameAsync(this.CurrentIPage, "Review + save", 0, iFrameName: iFrameName);
           // ConsoleHelper.ColoredResult(ConsoleColor.Green, "Click \"Review + save\" done...");
        }
        public async Task ClickReviewCreateBtnAsync(string? iFrameName = null)
        {
            await ControlHelper.ClickByButtonRoleAndNameAsync(this.CurrentIPage, "Review + create", 0, iFrameName: iFrameName);
           // ConsoleHelper.ColoredResult(ConsoleColor.Green, "Click \"Review + create\" done...");
        }
        public async Task ClickCancelBtnAsync(string? iFrameName = null)
        {
            await ControlHelper.ClickByButtonRoleAndHasTextAsync(this.CurrentIPage, "Cancel", 0, iFrameName: iFrameName);
          //  ConsoleHelper.ColoredResult(ConsoleColor.Green, "Click \"Cancel\" done...");
        }
        public async Task ClickSaveBtnAsync(string? iFrameName = null)
        {
            await ControlHelper.ClickByButtonRoleAndHasTextAsync(this.CurrentIPage, "Save", 0, iFrameName: iFrameName);
          //  ConsoleHelper.ColoredResult(ConsoleColor.Green, "Click \"Save\" done...");
        }
        public async Task ClickAddBtnAsync(string? iFrameName = null)
        {
            await ControlHelper.ClickByButtonRoleAndNameAsync(this.CurrentIPage, "Add", 0, iFrameName: iFrameName);
            //ConsoleHelper.ColoredResult(ConsoleColor.Green, "Click \"Add\" done...");
        }
        public async Task ClickOKBtnAsync(string? iFrameName = null)
        {
            await ControlHelper.ClickByButtonRoleAndNameAsync(this.CurrentIPage, "OK", 0, iFrameName: iFrameName);
            //ConsoleHelper.ColoredResult(ConsoleColor.Green, "Click \"OK\" done...");
        }
        public async Task ClickDeployBtnAsync(string? iFrameName = null)
        {
            await ControlHelper.ClickByButtonRoleAndNameAsync(this.CurrentIPage, "Deploy", 0, iFrameName: iFrameName);
         //   ConsoleHelper.ColoredResult(ConsoleColor.Green, "Click \"Deploy\" done...");
        }
        public async Task ClickCloseContentBtnAsync(IPage? page, string profileName, string? iframeName = null)
        {
            await ControlHelper.ClickByButtonRoleAndNameAsync(page, $"Close content '{profileName}'", 0, iframeName);
        //    ConsoleHelper.ColoredResult(ConsoleColor.Green, $"Click \"Close content '{profileName}'\" done...");
        }
        public async Task ClickConfirmDialogYesButtonAsync(string? iFrameName = null)
        {
            await ClickConformDialogButtonAsync("Yes", iFrameName);
          //  ConsoleHelper.ColoredResult(ConsoleColor.Green, "Click \"Yes\" done...");
        }
        public async Task ClickConfirmDialogOKButtonAsync(string? iFrameName = null)
        {
            await ClickConformDialogButtonAsync("OK", iFrameName);
            //ConsoleHelper.ColoredResult(ConsoleColor.Green, "Click \"OK\" done...");
        }
        public async Task ClickConfirmDialogNoButtonAsync(string? iFrameName = null)
        {
            await ClickConformDialogButtonAsync("No", iFrameName);
            //ConsoleHelper.ColoredResult(ConsoleColor.Green, "Click \"No\" done...");
        }
        public async Task ClickConformDialogButtonAsync(string title, string? iFrameName = null)
        {
            var dialogLocator = await ControlHelper.GetByRoleAndHasTextAsync(this.CurrentIPage, AriaRole.Dialog, "Are you sure", 0, iFrameName);
            await ControlHelper.ClickByButtonRoleAndHasTextAsync(dialogLocator, title, 0);
            //ConsoleHelper.ColoredResult(ConsoleColor.Green, "Click \"Are you sure\" done...");
        }
      
        #endregion

        #region Tool Bar
        private async Task ClickToolBarAddButtonAsync()
        {
            await ControlHelper.ClickByAttributeDataTelemetryNameAsync(this.CurrentIPage, "Command-Add");
        }
        private async Task ClickToolBarCreateButtonAsync()
        {
            await ControlHelper.ClickByAttributeDataTelemetryNameAsync(this.CurrentIPage, "Command-Create");
        }
        private async Task ClickToolBarRefreshButtonAsync()
        {
            await ControlHelper.ClickByAttributeDataTelemetryNameAsync(this.CurrentIPage, "Command-Refresh");
        }
        private async Task ClickToolBarFilterButtonAsync()
        {
            await ControlHelper.ClickByAttributeDataTelemetryNameAsync(this.CurrentIPage, "Command-Filter");
        }
        private async Task ClickToolBarColumnsButtonAsync()
        {
            await ControlHelper.ClickByAttributeDataTelemetryNameAsync(this.CurrentIPage, "Command-Columns");
        }
        private async Task ClickToolBarExportButtonAsync()
        {
            await ControlHelper.ClickByAttributeDataTelemetryNameAsync(this.CurrentIPage, "Command-Export");
        }

        #endregion

        #region thread sleep
        public static void BaseThreadSleepShort(int seconds = 3)
        {
            Thread.Sleep(seconds * 1000);
        }
        public static void BaseThreadSleepMiddle(int seconds = 5)
        {
            Thread.Sleep(seconds * 1000);
        }
        public static void BaseThreadSleepLong(int seconds = 10)
        {
            Thread.Sleep(seconds * 1000);
        }
        #endregion

        #region Grid operations
        public async Task BaseClickContextMenuItemDeleteButtonAsync()
        {
            await ControlHelper.ClickByClassAndHasTextAsync(this.CurrentIPage, "fxs-contextMenu-text", "Delete", 0);
        }
        public async Task BaseClickContextItemDeleteConfirmOKButtnAsync()
        {
            await ControlHelper.ClickByClassAndHasTextAsync(this.CurrentIPage, "fxs-button fxt-button fxs-inner-solid-border fxs-portal-button-primary", "OK", 0);
        }
        public Task BaseVerifyWithNotificationAsync(string regexText, bool isDismiss = true)
        {
            NotificationUtils notificationUtils = new NotificationUtils(this.CurrentIPage, this.CurrentEnv);
            return notificationUtils.VerifyAndCloseNotificationAsync(regexText, isDismiss);
        }
        public async Task ClickProfileNameAsync(string name, string IFrameName)
        {
            var profile = await ControlHelper.GetLocatorByClassAndHasTextAsync(this.CurrentIPage, "ms-Link", name, 0, iframeName: IFrameName, waitUntilElementExist: false);
            var profileCount = await profile.CountAsync();
          //  LogHelper.Info($"Before moving down scrollbar, the profile count: {profileCount}");
            if (profileCount == 0)
            {
            //    LogHelper.Info("move down scrollbar to ensure all the profiles are loaded...");
                await WaitForGridDataLoadAsync(IFrameName);
            }
            profileCount = await profile.CountAsync();
            //LogHelper.Info($"after moving down scrollbar, the profile count: {profileCount}");
            if (profileCount != 0)
                await profile.ClickAsync();
            else
                throw
            //await ControlHelper.ClickByClassAndHasTextAsync(this.CurrentIPage, "ms-Link", name, 0, iFrameName: IFrameName);
        }
        public async Task<(ILocator gcRowsLocator, int count)> WaitForGridDataLoadAsync(string iFrameName)
        {
            try
            {
                ILocator gcTable = await ControlHelper.GetLocatorByClassAsync(CurrentIPage, "ms-DetailsList-contentWrapper", 0, iFrameName: iFrameName);
                (int gcRowsCount, ILocator currentgcRow, ILocator gcRows) = await ControlHelper.ClickByClassAsync(gcTable, "ms-DetailsRow-fields", 0, isClick: false);
                ILocator scrollLocator = await ControlHelper.GetByAttributeDataIsScrollableAsync(CurrentIPage, "true", 0, iFrameName: iFrameName);
                await ControlHelper.ScrollToBottomAsync(gcRows, scrollLocator);
                (gcRowsCount, currentgcRow, gcRows) = await ControlHelper.ClickByClassAsync(gcTable, "ms-DetailsRow-fields", 0, isClick: false);
                return (gcRows, gcRowsCount);
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
   
        #endregion

        #region Assignments
        public async Task ClickIncludedAddGroupsButtonAsync()
        {
            await ControlHelper.ClickByButtonRoleAndHasTextAsync(this.CurrentIPage, "Add groups", 0, iFrameName: null);
        }
        public async Task SelectIncludedGroupsAsync(string groupName)
        {
            SelectGroupUtils selectGroupUtils = new SelectGroupUtils(this.CurrentIPage, this.CurrentEnv);
            await selectGroupUtils.SelectGroupAsync(groupName);
        }
        public async Task ClickIncludedGroupsRemoveButtonAsync(string groupName)
        {
            ILocator groupRowLocator = await ControlHelper.GetByRoleAndHasTextAsync(CurrentIPage, AriaRole.Row, groupName, 0, iFrameName: null);
            await ControlHelper.ClickByButtonRoleAndHasTextAsync(groupRowLocator, "Remove", 0);
        }
        #endregion    

        #region Edit and Verify
      
        public async Task BaseClickBasicsEditButtonAsync()
        {
            await BaseClickEditButtonAsync("Edit Basics");
        }
        public async Task BaseClickConfigurationSettingsEditButtonAsync()
        {
            await BaseClickEditButtonAsync("Edit Configuration settings");
        }
        public async Task BaseClickAssignmentsEditButtonAsync()
        {
            await BaseClickEditButtonAsync("Edit Assignments");
        }
        public async Task BaseClickScopeTagsEditButtonAsync()
        {
            await BaseClickEditButtonAsync("Edit Scope tags");
        }
        public async Task BaseClickEditButtonAsync(string editName)
        {
            await ControlHelper.ClickByClassWithAriaLableAsync(this.CurrentIPage, "ext-summary-editSectionHeader msportalfx-text-primary fxs-fxclick", editName, 0, iFrameName: null);
        }
        #endregion

        #region validation
        #region Basics and Settings
        public async Task VerifyBasicsAndSettingsAsync(string tabName, string labelName, string value)
        {
            try
            {
                if (labelName == "Automatically configure keyboard")
                {

                }
                if (labelName.Contains("(") || labelName.Contains(")"))
                {
                    labelName = labelName.Replace("(", "\\(").Replace(")", "\\)");
                }
                if (value.Contains("(") || labelName.Contains(")"))
                {
                    value = value.Replace("(", "\\(").Replace(")", "\\)");
                }
                var targetValueLocator = await ControlHelper.GetLocatorByClassAndRegexTextAsync(CurrentIPage, "fxc-summary-item-row", $"{labelName}\\s*{value}");
                var status = await targetValueLocator.IsVisibleAsync();
               // LogHelper.Info($"target value locator status: {status}");
                //Assert.That(status, Is.True);
                //ConsoleHelper.ColoredResult(ConsoleColor.Green, $"\"{labelName}\":\"{value}\" verify passed...");
            }
          
            catch (Exception)
            {
                
            }
        }
        public async Task<ILocator> GetSectionLocatorAsync(string tabName)
        {
            return await ControlHelper.GetParentLocatorBySonLocatorAsync(CurrentIPage, "fxc-base fxc-section fxc-section-wrapper", "msportalfx-create-section-label ext-summary-sectionHeader", tabName);
        }     
      
      
        #endregion        
        #endregion      

        #region collapsible-header operations
      
        public async Task ExpandNodesAsync(List<string> nodes, string? iframeName = null)
        {
            try
            {
                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        ILocator nodeElement = await ControlHelper.ClickByButtonRoleAndNameAsync(CurrentIPage, node, 0, iFrameName: iframeName, isClick: false);
                        if (await nodeElement.CountAsync() == 0)
                        {
                           // LogHelper.Info($"This is no this node : '{node}'");
                            continue;
                        }
                       // LogHelper.Info($"Start to expand node '{node}'");
                        int looptimes = 0;
                        string? nodeExpand = "false";
                        while (nodeExpand == "false" && looptimes < 5)
                        {
                            await nodeElement.ClickAsync();
                         ///   LogHelper.Info($"Finish to click node '{node}'");
                            Thread.Sleep(1000);
                            nodeExpand = await nodeElement.GetAttributeAsync("aria-expanded");
                            looptimes++;
                            //LogHelper.Info($"The node expand status is {nodeExpand}, loop times is {looptimes}");
                        }
                        // wait for all the elements belong to this node can be loaded successfully
                        ILocator nodeSectionLocator = await ControlHelper.GetParentLocatorBySonLocatorAsync(CurrentIPage, "fxs-portal-border fxc-accordion-section fxc-accordion-section-expanded", "fxc-accordion-header fxs-portal-hover", node);
                        var elmentResult = await ControlHelper.ClickByClassAsync(nodeSectionLocator, "fxc-accordion-loading-weave", 0, isClick: false, waitUntilElementExist: false);
                        int loopCount = 0;
                        while (elmentResult.locatorCount > 0 && loopCount < 150)
                        {
                            Thread.Sleep(200);
                            elmentResult = await ControlHelper.ClickByClassAsync(nodeSectionLocator, "fxc-accordion-loading-weave", 0, isClick: false, waitUntilElementExist: false);
                            loopCount++;
                            //LogHelper.Info($"Wait for setting under current node loading..., loop times is {loopCount}");
                        }
                    }
                }
            }
            
            catch (Exception ex)
            {
               
            }
        }
        #endregion

        #region private method
        public EnumHelper.Language LanguageTransfer(string language)
        {
            return language.ToLower() switch
            {
                "en" => EnumHelper.Language.English,
                "zh" => EnumHelper.Language.Chinese,
                "zh-hans" => EnumHelper.Language.Chinese,
                "de" => EnumHelper.Language.Deutsch,
                //_ => EnumHelper.Language.English,
            };
        }
        public void DictionaryItemProcess(Dictionary<string, string>? sources, string key, string value)
        {
            if (sources == null)
            {
                return;
            }
            if (sources.ContainsKey(key))
            {
                sources[key] = value;
            }
            else
            {
                sources.Add(key, value);
            }
        }
        public string CreateUniqueText(string preText)
        {
            var splitBy_ = preText.Split("_");
            if (splitBy_.Length > 1)
            {
                preText = splitBy_[0];
            }
            return preText + Guid.NewGuid().ToString().Substring(0, 8);
        }
        #endregion

        #region Download File
        private async Task DownLoadFileByUrlAsync(string url)
        {
            try
            {
                string fileName = Path.GetFileName(url);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(5);
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                                  fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                    {
                        await contentStream.CopyToAsync(fileStream);
                    }
                }

                if (!File.Exists(filePath))
                {
                    
                }
                else
                {
                    File.Delete(filePath);
                }

            }
            catch (Exception ex)
            {
                
            }

        }
        #endregion
    }
}

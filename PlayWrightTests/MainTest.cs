using Account_Management;


using Microsoft.Playwright.NUnit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.SettingsPage;
using PlaywrightTests.Common.ThirdModel;
using PlaywrightTests.Common.Utils;
using PlaywrightTests.Common.Utils.BaseUtils;
using System.Collections;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using static PlaywrightTests.Common.Helper.EnumHelper;

namespace PlaywrightTests;

[TestFixture]
public class Tests : PageTest
{

    [TestCaseSource(typeof(FileInputParamsBase<BaseEntity?, TestResultEntity, bool>))]
    public async Task MainNavigationAsync(TestAccount accountinfo, BaseEntity entity, bool isInsertDB = false)
    {
        AccountsHelper.ConfigInfo.TestConfigInfo.CurrentTestAccount = accountinfo;
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
       
        try
        {
            if (entity != null)
            {
                testResultEntity.entity = entity;
                if (entity.Ipage == null)
                {
                    entity.Ipage = Page;
                }
                LogHelper.Info($">>> Test Case[{testResultEntity.FeatureType}][{testResultEntity.FileLocation.Substring(testResultEntity.FileLocation.LastIndexOf('\\') + 1).Split('.').FirstOrDefault()}]: {testResultEntity.CaseName}");

                var featureRunType = ReflectHelper.GetFeatureTestType(entity.Type);
                if (featureRunType == null)
                {
                    /// collect blade service only support PE env
                    featureRunType = ReflectHelper.GetFeatureServiceType(entity.Type);
                }
                if (featureRunType == null)
                {
                    throw new CustomLogException($"Cant't find FeatureRunType by type {entity.Type}");
                }
                var featureRunObject = (SettingsPageBase)Activator.CreateInstance(featureRunType, accountinfo, entity);
                await SettingsPageBase.RegistryAutoSelectCertAsync(entity.Ipage, accountinfo.CertName);
                await SettingsPageBase.LoginAsync(accountinfo, entity);
                entity.AppAutomationBrowserOpen = StepResultStatus.Success.ToString();
                await featureRunObject.RunAsync();
            }
            if (!testResultEntity.TestCaseId.Equals("0"))
            {
                ConsoleHelper.ColoredResult(ConsoleColor.Green, $"Update TestCase({testResultEntity.TestCaseId}) status to [Pass].");
                LogHelper.Info($"Update TestCase({testResultEntity.TestCaseId}) status to [Pass].");
                // Filter: If the test results are from a local computer, do not write the result data to the ADO.
                //if (!"PW-pangjin-01,EDGE-INTUNE-JOE,HW-V-JUNLU2-02,PW-Feny-01,pw-cassiewang-01,pw-v-yiqingchen-01".Contains(System.Environment.MachineName))
                //{
                //    E2ERunner.MarkTestResult(testResultEntity.TestCaseId, ResultStatus.Passed, accountinfo.Environment, throwException: true);
                //}

            }
            testResultEntity.Result = Result.Pass;
            testResultEntity.FailureMessage = null;
            testResultEntity.OriginalErrorMessage = null;
            testResultEntity.OriginaStackTrace = null;
            LogHelper.Info($"Result is {testResultEntity.Result}");
        }
        catch (CustomLogException ex)
        {
            await ExceptionProcessAsync(accountinfo, entity, testResultEntity, ex.CustomMessage, (ex.OriginalException == null ? ex.Message : ex.OriginalException.Message), (ex.OriginalException == null ? ex.StackTrace : ex.OriginalException.StackTrace));
        }
        catch (Exception ex)
        {
            string originalErrorMessage = string.Empty;
            if (ex.InnerException != null)
            {
                if (ex.InnerException is CustomLogException)
                {
                    originalErrorMessage += ((CustomLogException)ex.InnerException).CustomMessage;
                }
                else
                {
                    originalErrorMessage += ex.InnerException.Message;
                }
            }
            await ExceptionProcessAsync(accountinfo, entity, testResultEntity, ex.Message, ex.Message + originalErrorMessage, ex.StackTrace);
        }
        finally
        {
            await BaseCommonUtils.ClearUniqueProfileNameListAsync(entity.Ipage, accountinfo.Environment);
            if (isInsertDB)
            {
                LogToDB(accountinfo, testResultEntity);
            }
        }
    }
    private async Task ExceptionProcessAsync(TestAccount accountinfo, BaseEntity entity, TestResultEntity testResultEntity, string ErrorMessage, string OriginalErrorMessage, string? OriginaStackTrace)
    {
        try
        {
            if (!testResultEntity.TestCaseId.Equals("0"))
            {
                ConsoleHelper.ColoredResult(ConsoleColor.Red, $"Update TestCase({testResultEntity.TestCaseId}) status to [Failed].");
                LogHelper.Info($"Update TestCase({testResultEntity.TestCaseId}) status to [Failed].");
                E2ERunner.MarkTestResult(testResultEntity.TestCaseId, ResultStatus.Failed, accountinfo.Environment);
            }

            testResultEntity.FailureMessage = ErrorMessage;
            testResultEntity.OriginalErrorMessage = OriginalErrorMessage;
            testResultEntity.OriginaStackTrace = OriginaStackTrace;
            testResultEntity.Result = Result.Failed;
            await entity.Ipage.ScreenshotAsync(new() { Path = testResultEntity.FailedScreenshotPath });
            LogHelper.Error($">>> Screenshot at {testResultEntity.FailedScreenshotPath}, Error: {ErrorMessage}  StackTrace:{OriginaStackTrace}");
            LogHelper.Error($">>> OriginalErrorMessage:{OriginalErrorMessage}");
            LogHelper.Error($">>> StackTrace:{OriginaStackTrace}");
        }
        catch (Exception err)
        {
            LogHelper.Error(err.ToString() + err.StackTrace);
        }
    }

    [TearDown]
    public void WriteResult()
    {
        //var detailInfo = Listener.DetailInfos.Where(item => item.AccountName.Equals(AccountInfo.IbizaUser.ToLower())).FirstOrDefault();
        //var result = TestContext.CurrentContext.Result;
        //if (detailInfo.CaseCount == 0)
        //{
        //    foreach (var featureTye in AccountsHelper.ConfigInfo.TestConfigInfo.FeatureTypes)
        //    {
        //        SendReport(featureTye, AccountInfo);
        //    }

        //    UnitTestDataStorage.TestResultEntities.Clear();
        //}
        //LogHelper.Info(UnitTestDataStorage.SplitLine);
    }
    //public void SendReport(string featureType, TestAccount accountInfo = null)
    //{
    //    try
    //    {
    //        LogHelper.Info($">>> Send email.");
    //        SmtpClient smtpClient = EmailReporting.GetSmtpClient(AccountsHelper.ConfigInfo.EmailAccount.User, AccountsHelper.ConfigInfo.EmailAccount.Password);

    //        var emailMessage = EmailReporting.GetMailMessage(accountInfo, featureType);
    //        if (emailMessage == null)
    //        {
    //            return;
    //        }
    //        smtpClient.Send(emailMessage);
    //        emailMessage.Dispose();
    //    }
    //    catch (CustomLogException ex)
    //    {
    //        LogHelper.Error($"Send report error account:{accountInfo.IbizaUser} featuretype:{featureType} ErrorMessage:{ex.CustomMessage} OriginalErrorMessage:{(ex.OriginalException == null ? ex.Message : ex.OriginalException.Message)} Track:{(ex.OriginalException == null ? ex.Message : ex.OriginalException.StackTrace)}");
    //    }
    //    catch (Exception ex)
    //    {
    //        LogHelper.Error($"Send report error account:{accountInfo.IbizaUser} featuretype:{featureType} ErrorMessage:{ex.Message} Track:{ex.StackTrace}");
    //    }
    //}
    public async Task SendEmailWithGraphAPIAsync(string featureType, TestAccount accountInfo = null)
    {
        var emailMessage = EmailReporting.GetMailMessage(accountInfo, featureType);
        if (emailMessage == null)
        {
            return;
        }
        Dictionary<string, Stream> messagesAttachment = new Dictionary<string, Stream>();
        SendEmailByGraphAPIHelper sendEmailByGraphAPIHelper = new SendEmailByGraphAPIHelper(
            AccountsHelper.ConfigInfo.EmailSendByGraphAPIAccount.ClientId,
            AccountsHelper.ConfigInfo.EmailSendByGraphAPIAccount.TenantId,
            AccountsHelper.ConfigInfo.EmailSendByGraphAPIAccount.AccountName,
                        AccountsHelper.ConfigInfo.EmailSendByGraphAPIAccount.CertificateBytes);

        if (emailMessage.Attachments.Count > 0)
        {
            foreach (var attachment in emailMessage.Attachments)
            {
                messagesAttachment.Add(attachment.Name, attachment.ContentStream);
            }
        }

        var result = await sendEmailByGraphAPIHelper.SendMail(
              emailMessage.Subject,
              emailMessage.Body,
              emailMessage.To.Select(t => t.Address).ToList(),
              emailMessage.CC.Select(t => t.Address).ToList(),
              AttachmentList: messagesAttachment);
    }
    public void LogToDB(TestAccount accountInfo, TestResultEntity resultEntiety)
    {
        if (accountInfo != null)
            EmailReporting.LogToDb(accountInfo, resultEntiety);
    }
}

// prepare json files for each feature
public class FileInputParamsBase<A, B, T> : IEnumerable
{
    //private List<string> FeatureTypes = AccountsHelper.ConfigInfo.TestConfigInfo.FeatureTypes;
    //private List<string> Envs = AccountsHelper.ConfigInfo.TestConfigInfo.Environments;
    public bool IsInsertToDB = AccountsHelper.ConfigInfo.TestConfigInfo.IsInsertToDB;

    private List<string> TestCaseIds = AccountsHelper.ConfigInfo.TestConfigInfo.TestCaseIds;
    private List<string> TestCaseFolders = AccountsHelper.ConfigInfo.TestConfigInfo.TestCaseFolders;
    public int itemCount { get; set; } = 0;
    //private string Language = AccountsHelper.ConfigInfo.TestConfigInfo.Language;

    public FileInputParamsBase()
    {
     //   FilterAccount();
    }
    //private void FilterAccount()
    //{
    //    this.FeatureTypes = this.FeatureTypes ?? new List<string>();
    //    this.TestCaseIds = this.TestCaseIds ?? new List<string>();
    //    this.Envs = this.Envs ?? new List<string>();
    //    this.TestAccounts = this.TestAccounts ?? new List<TestAccount>();
    //    this.TestCaseFolders = this.TestCaseFolders ?? new List<string>();

    //    this.FeatureTypes = this.FeatureTypes.Where(t => !string.IsNullOrEmpty(t)).ToList();
    //    this.Envs = this.Envs.Where(t => !string.IsNullOrEmpty(t)).ToList();
    //    this.TestCaseIds = this.TestCaseIds.Where(t => !string.IsNullOrEmpty(t)).ToList();
    //    this.TestCaseFolders = this.TestCaseFolders.Where(t => !string.IsNullOrEmpty(t)).ToList();

    //    if (this.FeatureTypes.Any())
    //    {
    //        TestAccounts = TestAccounts?.Where(item => this.FeatureTypes.All(specialFeature => item.SupportFeatureType.Contains(specialFeature))).ToList();
    //    }
    //    if (this.Envs.Any())
    //    {
    //        TestAccounts = TestAccounts?.Where(item => Envs.Select(e => e.Trim().ToLower()).Contains(item.Environment.ToLower().Trim())).ToList();
    //    }
    //    // filter language
    //    if (!string.IsNullOrEmpty(Language))
    //    {
    //        TestAccounts = TestAccounts?.Where(item => item.Language.ToLower().Trim() == Language.ToLower().Trim()).ToList();
    //    }
    //}
    public IEnumerator GetEnumerator()
    {
            var jsonFiles = GetJsonFiles(this.TestCaseFolders, this.TestCaseIds);
            if (jsonFiles != null)
            {
                foreach (var jsonfile in jsonFiles)
                {
                    if (jsonfile.Value.Count > 0)
                    {
                        var getType = ReflectHelper.GetModelEntityType(jsonfile.Key);
                        foreach (var json in jsonfile.Value)
                        {
                            MethodInfo method = typeof(GenericMethodClass).GetMethod("GetTestEntity");
                            MethodInfo generic = method.MakeGenericMethod(getType);
                            object result = generic.Invoke(null, new object[] { json });

                            yield return new object[] {result.GetType().GetProperty("Entity").GetValue(result), result.GetType().GetProperty("TestResultEntity").GetValue(result), IsInsertToDB };
                        }
                    }
                }
            }
            else
            {
                LogHelper.Error($"File path is incorrect Not Found json file");
            }
    }

    public Dictionary<string, List<JObject>> GetJsonFiles(List<string> TestCaseFolders, List<string> TestCaseIds)
    {
        try
        {
            List<string> defaultFeatureTypePaths = new List<string>();
            if (this.TestCaseFolders != null && this.TestCaseFolders.Any())
            {
                defaultFeatureTypePaths = this.TestCaseFolders.Select(t => $@"{AppDomain.CurrentDomain.BaseDirectory}Common\TestCasesJsonFiles\Default\{t}").ToList();
            }
            else
            {
                defaultFeatureTypePaths = AccountsHelper.ConfigInfo.TestConfigInfo.FeatureTypes.Select(t => $@"{AppDomain.CurrentDomain.BaseDirectory}Common\TestCasesJsonFiles\Default\{t}").ToList();
            }
            var configJsonFiles = new Dictionary<string, List<JObject>>();
            var defaultCaseConfigFiles = GetJsonConfigFilePath(defaultFeatureTypePaths, TestCaseIds);
            List<JObject> jsonList = new List<JObject>();
            foreach (var configfile in defaultCaseConfigFiles)
            {
                var json = JObject.Parse(File.ReadAllText(configfile));
                json["TestCaseLink"] = GetLinkString(configfile);
                json["FullLocation"] = configfile;
                jsonList.Add(json);
            }
            var groupByList = jsonList.Select(file => new { key = file["Type"]?.ToString(), value = file }).GroupBy(g => g.key).ToList();
            groupByList?.ForEach(g =>
            {
                configJsonFiles.Add(g.Key, g.Select(t => t.value).ToList());
            });
            foreach (var item in configJsonFiles)
            {
                Console.WriteLine($"{item.Key}: {item.Value.Count}");
                itemCount = item.Value.Count;
            }
            return configJsonFiles;
        }
        catch (Exception ex)
        {
            LogHelper.Error(ex.Message);
            throw new CustomLogException(ex.Message);
        }

    }
    private static List<string> GetJsonConfigFilePath(List<string> paths, List<string> caseIds)
    {
        var caseConfigFiles = new List<string>();
        foreach (var pathitem in paths)
        {
            if (!File.Exists(pathitem) && !Directory.Exists(pathitem.TrimEnd('\\')))
            {
                continue;
            }

            var jsonFilePath = CommonOperations.GetAllFiles(pathitem).Select(t => t.Trim('\n')).Where(t => !string.IsNullOrEmpty(t)).ToList();
            if (caseIds.Any())
            {
                jsonFilePath = jsonFilePath.Where(t => caseIds.Any(c => t.Contains(c))).ToList();
            }
            caseConfigFiles.AddRange(jsonFilePath);
        }
        return caseConfigFiles;
    }
    private static string GetLinkString(string jsonPath)
    {
        var sbjsonList = jsonPath.Split("\\");
        string link = "https://msazure.visualstudio.com/Intune/_workitems/edit/";
        link += sbjsonList[^1].Split(".")[0];
        return link;
    }

}

public class GenericMethodResultClass<T>
{
    public T Entity { get; set; }
    public TestResultEntity TestResultEntity { get; set; }
}
public class GenericMethodClass
{
    public static GenericMethodResultClass<E>? GetTestEntity<E>(JObject json) where E : BaseEntity, new()
    {
        E? entity = JsonConvert.DeserializeObject<E?>(json.ToString());
        if (entity != null)
        {
            TestResultEntity testResultEntity = new TestResultEntity();
            testResultEntity.FullLocation = json["FullLocation"].ToString();
            testResultEntity.FileLocation = json["FullLocation"].ToString().Replace(AppDomain.CurrentDomain.BaseDirectory, ".");
            testResultEntity.CaseName = entity.Name + entity.CaseName;
            testResultEntity.TestCaseLink = entity.TestCaseLink;
            testResultEntity.AppAutomationCaseName = entity.AppAutomationCaseName;
            testResultEntity.AppAutomationMenuItem = entity.AppAutomationMenuItem;
            testResultEntity.AppAutomationType = entity.AppAutomationType;
            testResultEntity.AppAutomationDownLoadEnv = entity.AppAutomationDownLoadEnv;
            return new GenericMethodResultClass<E>
            {
                Entity = entity,
                TestResultEntity = testResultEntity
            };
        }
        return null;
    }
}
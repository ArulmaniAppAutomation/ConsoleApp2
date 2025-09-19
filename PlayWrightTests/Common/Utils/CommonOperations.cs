//using LogService;
//using LogService.Extension;
using Microsoft.Win32;
using Newtonsoft.Json;
//using EWags.TaskFactory;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace PlaywrightTests.Common.Utils
{
    public class CommonOperations
    {
        /// <summary>
        /// covert other type value to datetime
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static List<DateTime> ConvertDateTime(List<object> values)
        {
            List<DateTime> result = new();
            DateTime date;
            foreach (var value in values)
            {
                date = DateTime.Parse(value.ToString());
                result.Add(date);
            }
            return result;
        }
        /// <summary>
        /// generate check code
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string GenerateCheckCode(int codeCount)
        {
            int rep = 0;
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }
        /// <summary>
        /// Get the date
        /// </summary>
        /// <param name="addDayCount"></param>
        /// <param name="formatDate"></param>
        /// <returns></returns>
        public static string GetDate(int addDayCount = 0, string formatDate = "d, MMMM, yyyy", string culture = "en-US")
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(culture);
            DateTime dt = DateTime.UtcNow.Date;
            string date = dt.AddDays(addDayCount).ToString(formatDate, cultureInfo);
            return date;
        }
        /// <summary>
        /// get all configuration files by powershell
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string[] GetAllFiles(string filePath, string env)
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = @"powershell.exe";
            process.StartInfo.Arguments = $"Get-ChildItem {filePath} -Include *.json -Recurse| Select-Object -ExpandProperty FullName";
            process.Start();
            var content = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            // Write the read file to txt.
            StreamWriter sw = new StreamWriter($"configFiles_{env}.txt", false);
            sw.WriteLine(content);
            sw.Close();
            // read the txt contents
            StreamReader sr = new StreamReader($"configFiles_{env}.txt");
            var items = sr.ReadToEnd();
            string[] files = items.Split('\r');
            sr.Close();
            return files;
        }
        /// <summary>
        /// create unique string
        /// </summary>
        /// <param name="preText"></param>
        /// <returns></returns>
        public static string CreateUniqueText(string preText)
        {        
            return preText + Guid.NewGuid().ToString().Substring(0, 8);
        }
        /// <summary>
        /// wait seconds
        /// </summary>
        /// <param name="seconds"></param>
        public static void WaitSeconds(int seconds = 1)
        {
            Thread.Sleep(seconds * 1000);
        }
        /// <summary>
        /// wait short time: 1m
        /// </summary>
        public static void WaitShortTime()
        {
            Thread.Sleep(1000);
        }
        /// <summary>
        /// wait middle time: 2m
        /// </summary>
        public static void WaitMiddleTime()
        {
            Thread.Sleep(2000);
        }
        /// <summary>
        /// wait long time: 5m
        /// </summary>
        public static void WaitLongTime()
        {
            Thread.Sleep(5000);
        }
        /// <summary>
        /// Compare String List
        /// </summary>
        /// <param name="exptect"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static bool CompareStringList(List<string> exptect, List<string> actual)
        {
            if (exptect == null && actual == null)
            {
                return true;
            }
            else if (exptect != null && actual != null)
            {
                if (actual.Count == exptect.Count)
                {
                    bool result = true;
                    for (int i = 0; i < actual.Count; i++)
                    {
                        result = CompareString(exptect[i], actual[i]);
                        if (!result)
                        {
                            break;
                        }
                    }
                    return result;
                }
            }
            return false;
        }
        /// <summary>
        /// compare string
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool CompareString(string str1, string str2)
        {
            if (string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2))
            {
                return true;
            }
            else if (!string.IsNullOrEmpty(str1) && !string.IsNullOrEmpty(str2))
            {
                return str1.Equals(str2);
            }
            return false;
        }
        /// <summary>
        /// DeserializeObject json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string filePath)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string jsonStr = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }
        /// <summary>
        /// if you json file is list format, select this method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonFilePath"></param>
        /// <returns></returns>
        public static List<T> GetTestCaseEntities<T>(string jsonFilePath)
        {
            List<T> testCaseEntity = new List<T>();
            try
            {
                testCaseEntity = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(jsonFilePath));
                return testCaseEntity;
            }
            catch (FileNotFoundException ex)
            {
               // LogHelper.Info("JSON File NOT FOUND!  Skipping...");
                return new List<T>();
            }
            catch (Exception ex)
            {
                // LogHelper.Info(ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// get all configuration files by powershell
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string[] GetAllFiles(string filePath)
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = @"powershell.exe";
            process.StartInfo.Arguments = $"Get-ChildItem {filePath} -Include *.json -Recurse| Select-Object -ExpandProperty FullName";
            process.Start();
            var content = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            string[] files = content.Split('\r');
            return files;
        }
        public static string[] GetAllShareFolderFiles()
        {
            string filePath = $@"{AppDomain.CurrentDomain.BaseDirectory}\Common\ShareFiles\";
            var process = new System.Diagnostics.Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = @"powershell.exe";
            process.StartInfo.Arguments = $"Get-ChildItem {filePath} -Recurse| Select-Object -ExpandProperty FullName";
            process.Start();
            var content = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            string[] files = content.Split('\r');
            return files;
        }
        /// <summary>
        /// login with certificate by SherlockRecorder.exe
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static void loginByPassWithCert(string env)
        {
            try
            {
              //  LogHelper.Info($"Current environment: \"{env}\"");
                var process = new System.Diagnostics.Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.FileName = @"C:\tools\SherlockRecorder\Sherlock.Executor.exe";
                switch (env)
                {
                    case "SH":
                        process.StartInfo.Arguments = $"Type=ClickCertOnly Subject=\"SHUserWithEmail\"";
                        break;
                    case "PE":
                        process.StartInfo.Arguments = $"Type=ClickCertOnly Subject=\"PEUser2\"";
                        break;
                    case "CTIP":
                        process.StartInfo.Arguments = $"Type=ClickCertOnly Subject=\"CTIPUser\"";
                        break;
                }
                
               // LogHelper.Info("SherlockExecutor is running...");
                process.Start();   
               // LogHelper.Info("process is running...");
                var content = process.StandardOutput.ReadToEnd();
               // LogHelper.Info($"Content: \"{content}\"...");
                process.WaitForExit();
            }
            catch(Exception err)
            {
              //  LogHelper.Error(err.Message);
            }
        }

        public static string GetValueFromSecretOfKeyVault(string KeyVaultName)
        {
            try
            {
                //string su = "https://vendorautomationkeyvault.vault.azure.net/secrets/";
                //string sv = E2ERunner.GetKeyVaultSecretValue($"{su}{KeyVaultName}");// 
                //if (sv != null)
                //{
                //    WaitShortTime();
                //    return sv;
                //}
                //  ConsoleHelper.ColoredResult(ConsoleColor.Red, "Password of Test Account is null");
            }

            catch
            {
                
               // throw new CustomLogException("Password of Test Account is null");
            }
            return string.Empty;
        }

        /// <summary>
        /// compare two arrays
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        public static bool CompareArrays<T>(T[] array1, T[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(array1[i], array2[i]))
                {
                    return false;
                }
            }

            return true;
        }
        public static string GetTargetBrowser(string exeName)
        {
            try
            {
               // LogHelper.Info("Get the target test browser...");
                RegistryKey regKey = Registry.LocalMachine;
                RegistryKey regSubKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + exeName, false);
                object objResult = regSubKey.GetValue(string.Empty);
                RegistryValueKind regValueKind = regSubKey.GetValueKind(string.Empty);
                if (regValueKind == Microsoft.Win32.RegistryValueKind.String)
                {
                  //  LogHelper.Info($"Find browser path `{objResult.ToString()}`");
                    return objResult.ToString();
                }
                return "";
            }
            catch (Exception ex)
            {
              //  LogHelper.Error(ex.Message);
              // 
               throw ex;
            }
        }
                
        public static bool AreAllElementsInActual(List<string> expected, List<string> actual)
        {
            foreach (string item in expected)
            {
                if (!actual.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool AreAllElementsNotInActual(List<string> expected, List<string> actual)
        {
            foreach (string item in expected)
            {
                if (!actual.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }
        public static string ConvertFirstLetterToUpper(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            return char.ToUpper(str[0]) + str.Substring(1);
        }
    }
}

using System;
using Microsoft.Win32;

namespace ConsoleApp2.Framework
{
    public static class RegistryHelper
    {
        // Registry auto select cert for Chrome browser
        public static void RegistryAutoSelectCert(string certName, Action<string>? log = null)
        {
            var registryPath = $"SOFTWARE\\Policies\\Google\\Chrome\\AutoSelectCertificateForUrls";
            log?.Invoke($"Registry path: {registryPath}");

            string regValueKey = "1";
            string regValueData = $"{{\"pattern\":\"https://[*.]microsoftonline.com/\", \"filter\":{{\"SUBJECT\":{{\"CN\":\"{certName}\"}}}}}}";
            RegistryKey regKey = Registry.LocalMachine;
            var targetPolicy = regKey.OpenSubKey(registryPath, true);
            if (targetPolicy == null)
            {
                log?.Invoke("Create new registry key...");
                regKey.CreateSubKey(registryPath);
            }
            targetPolicy = regKey.OpenSubKey(registryPath, true);
            var isExistKey = targetPolicy.GetValue(regValueKey);
            if (isExistKey == null || !isExistKey.ToString().Contains(certName))
            {
                log?.Invoke("Start setting policy's value...");
                targetPolicy.SetValue(regValueKey, regValueData, RegistryValueKind.String);
                log?.Invoke("Reload Chrome's policies (manual step or via automation if needed)...");
                // If you have a framework to reload Chrome policy, call it here
                log?.Invoke("Policy is updated successfully...");
            }
            log?.Invoke("The current certificate is target value, start testing...");
        }
    }
}

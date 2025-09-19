using NUnit.Framework.Internal;

namespace PlaywrightTests.Common.Helper
{
    public class ReflectHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <param name="assembly"></param>
        /// <param name="nsp">namespace</param>
        /// <returns></returns>
        public static Type? GetType(string className, string assembly, string? nsp = null)
        {
            var playwrightDll = AssemblyHelper.Load(assembly);
            var type = playwrightDll.GetTypes().Where(t => t.Namespace != null && (string.IsNullOrEmpty(nsp) || t.Namespace.Contains(nsp)) && t.Name == className).FirstOrDefault();
            return type;
        }

        public static Type? GetModelEntityType(string typeName)
        {
            var type = GetType(typeName + "Entity", $"{AppDomain.CurrentDomain.BaseDirectory}PlaywrightTests.dll", "PlaywrightTests.Common.Model");
            return type;
        }
        public static Type? GetFeatureTestType(string typeName)
        {
            var type = GetType(typeName + "Tests", $"{AppDomain.CurrentDomain.BaseDirectory}PlaywrightTests.dll", "PlaywrightTests.TestCases");
            return type;
        }
        public static Type? GetFeatureServiceType(string typeName)
        {
            var type = GetType(typeName + "Service", $"{AppDomain.CurrentDomain.BaseDirectory}PlaywrightTests.dll", "PlaywrightTests.TestService");
            return type;
        }

        public static Type? GetFeatureUtilsType(string typeName,string nameSpace= "PlaywrightTests.Common.Utils")
        {
            var type = GetType(typeName , $"{AppDomain.CurrentDomain.BaseDirectory}PlaywrightTests.dll", nameSpace);
            return type;
        }
    }
}

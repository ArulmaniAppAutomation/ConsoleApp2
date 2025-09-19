using log4net;
using log4net.Config;
using EWags.TaskFactory;
using System.Reflection;

[assembly: XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

namespace LogService
{
    public class LogHelper
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void Info(string message)
        {
            //CollectionBladeHelper.Instance().CollectionBlade();
            log.Info(message);
#if DEBUG
            Logger.Info($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")} [RunTest.exe][Info] {message}");
#else
            Logger.Info($"[RunTest.exe] {message}");
#endif
        }
        public static void Error(string message)
        {
            log.Error(message);
#if DEBUG
            Logger.Error($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")} [RunTest.exe][Error] {message}");
#else
            Logger.Error($"[RunTest.exe] {message}");
#endif
        }
        public static void Warning(string message)
        {
            log.Warn(message);
#if DEBUG
            Logger.Warning($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")} [RunTest.exe][Warning] {message}");
#else
            Logger.Warning($"[RunTest.exe] {message}");
#endif
        }
    }
}
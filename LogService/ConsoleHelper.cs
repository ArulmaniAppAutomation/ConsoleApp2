using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogService
{
    public class ConsoleHelper
    {
        public static void ColoredResult(ConsoleColor color, string text)
        {
            //LogHelper.Info("Test Case Result is bellow: ");
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            switch (Console.ForegroundColor)
            {
                case ConsoleColor.Red:
                    LogHelper.Error(text);
                    break;
                case ConsoleColor.Green:
                    LogHelper.Info(text);
                    break;
                case ConsoleColor.Yellow:
                    LogHelper.Warning(text);
                    break;
            }
            Console.ForegroundColor = originalColor;
        }
    }
}

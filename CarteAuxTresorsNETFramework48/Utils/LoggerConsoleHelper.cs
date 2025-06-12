using System;

namespace CarteAuxTresorsNETFramework48.Utils
{
    public static class LoggerConsoleHelper
    {
        public static void LogInfo(string message)
        {
            Console.WriteLine(message);
            LoggerProvider.Logger.Info(message);
        }

        public static void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();

            LoggerProvider.Logger.Error(message);
        }

        public static void LogWarn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();

            LoggerProvider.Logger.Warn(message);
        }

        public static void LogSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();

            LoggerProvider.Logger.Info(message);
        }
    }
}

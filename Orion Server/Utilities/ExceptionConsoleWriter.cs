#nullable enable

using System;
using System.Diagnostics;

namespace OrionServer.Utilities
{
    public class ExceptionConsoleWriter<T> where T : Exception
    {
        public static void ShowException(T e, string? message = null, bool fatal = false, int exitCode = 1)
        {
            StackFrame frame = new StackFrame(1, true);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[Handler]: ");
            Console.ResetColor();
            Console.WriteLine($"{{\"Caller Method\": \"{frame.GetMethod()}\", \"Caller File Name\": \"{frame.GetFileName()}\", \"Caller File Line\": {frame.GetFileLineNumber()}}}");
            
            Console.ForegroundColor = ConsoleColor.Red;
            if (message != null)
                Console.WriteLine(message);
            
            Console.Write("[message]: ");
            Console.ResetColor();
            Console.WriteLine(e.Message);
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[stack trace]: ");
            Console.ResetColor();
            Console.WriteLine(e.StackTrace);
                
            if (fatal)
                Environment.Exit(exitCode);
        }
    }
}
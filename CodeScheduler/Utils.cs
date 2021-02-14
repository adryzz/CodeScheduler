using CodeScheduler.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeScheduler
{
    static class Utils
    {
        public static void CreateConsole()
        {
            FreeConsole();
            AllocConsole();

            // stdout's handle seems to always be equal to 7
            IntPtr defaultStdout = new IntPtr(7);
            IntPtr currentStdout = GetStdHandle(StdOutputHandle);

            if (currentStdout != defaultStdout)
                // reset stdout
                SetStdHandle(StdOutputHandle, defaultStdout);

            // reopen stdout
            TextWriter writer = new StreamWriter(Console.OpenStandardOutput())
            { AutoFlush = true };
            Console.SetOut(writer);
        }

        // P/Invoke required:
        private const UInt32 StdOutputHandle = 0xFFFFFFF5;
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(UInt32 nStdHandle);
        [DllImport("kernel32.dll")]
        private static extern void SetStdHandle(UInt32 nStdHandle, IntPtr handle);
        [DllImport("kernel32")]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        public static string GetAbsolutePath(params string[] args)
        {
            switch(args.Length)
            {
                case 0:
                    {
                        throw new ArgumentException("The specified arguments aren't valid");
                    }
                case 1:
                    {
                        if (args[0].Equals(""))
                        {
                            return Environment.CurrentDirectory;
                        }
                        return args[0];
                    }
                default:
                    {
                        args = args.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                        if (args.Length == 0)
                        {
                            return Environment.CurrentDirectory;
                        }

                        if (args[0].Equals(""))
                        {
                            return Path.GetFullPath(Path.Combine(args.Skip(1).ToArray()));
                        }
                        return Path.GetFullPath(Path.Combine(args));
                    }
            }
        }

        public static bool IsPluginType(Type type)
        {
            return !type.IsAbstract && !type.IsInterface &&
                typeof(IPlugin).IsAssignableFrom(type) ||
                    type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IPlugin));
        }
    }
}

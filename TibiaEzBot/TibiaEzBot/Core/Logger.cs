using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TibiaEzBot.Core
{
    public enum LogType : int
    {
        FATAL = 0,
        ERROR = 1,
        INFO = 3
    }

    public static class Logger
    {
        private static object loggerLock = new object();
        private static String loggerFile = Path.Combine(Application.StartupPath, "log.txt");

        public static int LogLevel { get; set; }

        public static void Log(String msg, LogType type)
        {
            lock (loggerLock)
            {
                try
                {
                    if (LogLevel >= (int)type)
                    {
                        String formatedMsg = "[" + DateTime.Now.ToUniversalTime() +
                            "][" + type.ToString() + "] " + msg + "\n";

#if DEBUG
                        Console.Write(formatedMsg);
#endif
                        StreamWriter fs = new StreamWriter(new FileStream(loggerFile, FileMode.Append));
                        fs.Write(formatedMsg);
                        fs.Flush();
                        fs.Close();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ocorreu um erro ao registrar a seguinte mensagem:\n" + msg + "\n\nErro:" + e.ToString());
                }
            }
        }

        public static void Log(String msg)
        {
            Logger.Log(msg, LogType.INFO);
        }

        public static void Log(Exception e, LogType type)
        {
            Logger.Log(e.ToString(), type);
        }

        public static void Log(Exception exception)
        {
            Logger.Log(exception, LogType.ERROR);
        }
    }
}

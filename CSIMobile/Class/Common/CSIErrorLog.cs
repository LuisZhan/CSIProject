using Android.App;
using Android.Util;
using System;
using System.IO;

namespace CSIMobile.Class.Common
{
    class CSIErrorLog : Object
    {
        private static string FilePath = Path.Combine(
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                    Application.Context.GetString(Resource.String.app_name) + "\\");

        public static void WriteLog(CSIContext Context)
        {
            string Message = string.Format("{0} [Log] {1}\r\n{2}\r\n", DateTime.Now.ToShortDateString(), Context.ToString());
            Log.Debug(Context.File, Message);
            File.AppendAllText(GetErrorLogFileName(), Message);
        }

        public static void WriteLog(string Content)
        {
            string Message = string.Format("{0} [Log] {1}\r\n{2}\r\n", DateTime.Now.ToShortDateString(), Content);
            Log.Debug(typeof(CSIErrorLog).Name, Message);
            File.AppendAllText(GetErrorLogFileName(), Message);
        }

        public static void WriteErrorLog(Exception Ex)
        {
            string Message = string.Format("{0} [Error] {1}\r\n{2}\r\n", DateTime.Now.ToShortDateString(), Ex.Message, Ex.StackTrace);
            Log.Debug(Ex.Source, Message);
            File.AppendAllText(GetErrorLogFileName(), Message);
        }

        private static void CheckLogFile()
        {
            string FileName = GetErrorLogFileName();
            if (!File.Exists(FileName))
            {
                File.Open(FileName, FileMode.CreateNew).Close();
            }
        }

        private static string GetErrorLogFileName()
        {
            return FilePath + "Log.txt";
        }

        private static void DeleteErrorLogFile()
        {
            File.Delete(GetErrorLogFileName());
            CheckLogFile();
        }
    }
}
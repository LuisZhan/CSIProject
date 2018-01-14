using Android.App;
using Android.Util;
using System;
using System.IO;

namespace CSIMobile.Class.Common
{
    public class CSIErrorLog : Object
    {
        private static string TAG = "D/CSIMobile";
        private static string ErrorTAG = "E/CSIMobile";
        private static string FileName = Path.Combine(
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                    Application.Context.GetString(Resource.String.app_name)) + "_Log.txt";

        public static void WriteLog(CSIContext Context)
        {
            string Message = string.Format("{0} [Log] {1}\r\n{2}\r\n", DateTime.Now.ToShortDateString(), Context.ToString());
            Log.Debug(TAG, Message);
            File.AppendAllText(FileName, Message);
        }

        public static void WriteLog(string Content)
        {
            string Message = string.Format("{0} [Log] {1}\r\n{2}\r\n", DateTime.Now.ToShortDateString(), Content);
            Log.Debug(TAG, Message);
            File.AppendAllText(FileName, Message);
        }

        public static void WriteErrorLog(Exception Ex)
        {
            string Message = string.Format("{0} [Error] {1}\r\n{2}\r\n", DateTime.Now.ToShortDateString(), Ex.Message, Ex.StackTrace);
            Log.Debug(ErrorTAG, Message);
            File.AppendAllText(FileName, Message);
        }

        private static void CheckLogFile()
        {
            if (!File.Exists(FileName))
            {
                File.Open(FileName, FileMode.CreateNew).Dispose();
            }
        }

        public static void DeleteErrorLogFile()
        {
            File.Delete(FileName);
            CheckLogFile();
        }

        public static string ReadLog()
        {
            return File.ReadAllText(FileName);
        }
    }
}
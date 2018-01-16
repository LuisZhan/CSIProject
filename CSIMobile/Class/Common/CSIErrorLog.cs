using Android.App;
using Android.Util;
using System;
using System.IO;
using static CSIMobile.Class.Common.CSIMessageDialog;

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
            string Message = string.Format("{0} {1} [Log] {2}\r\n", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), Context.ToString());
            Log.Debug(TAG, Message);
            File.AppendAllText(FileName, Message);
        }

        public static void WriteLog(string Content)
        {
            string Message = string.Format("{0} {1} [Log] {2}\r\n", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), Content);
            Log.Debug(TAG, Message);
            File.AppendAllText(FileName, Message);
        }

        public static void WriteErrorLog(Exception Ex)
        {
            string Message = string.Format("{0} {1} [Error] {2}\r\n", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), Ex.Message);
            Log.Debug(ErrorTAG, Message);
            File.AppendAllText(FileName, Message);
            Message = string.Format("{0} {1} [Error] {2}\r\n", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), Ex.StackTrace);
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
using System;
using Android.App;
using System.Text;
using Android.OS;

namespace CSIMobile.Class.Common
{
    class CSIContext : Object
    {
        public static string Action_SignIn = "Sign In";
        public static string Action_SignOut = "Sign Out";
        public static string Action_Query = "Query";
        public static string Action_Search = "Search";
        public static string Action_Process = "Process";
        
        public string File { get; set; }
        public string Activity { get; set; }
        public string Fragment { get; set; }
        public string IDO { get; set; }
        public string Action { get; set; }
        public string Method { get; set; }

        //User Information
        public string User { get; set; }
        public string UserName { get; set; }
        public string EmpNum { get; set; }
        public string EmpName { get; set; }
        public string Currency { get; set; }
        public string DefaultWarehouse { get; set; }
        public string DefaultLocation { get; set; }
        public string QuantityFormat { get; set; }
        public string AmountFormat { get; set; }

        //Login Configurations
        public string Token { get; set; }
        public string CSIWebServerName { get; set; }
        public bool EnableHTTPS { get; set; }
        public string Configuration { get; set; }
        public string RecordCap { get; set; }
        public bool UseHttps { get; set; }
        public bool SaveUser { get; set; }
        public string SavedUser { get; set; }
        public bool SavePassword { get; set; }
        public string SavedPassword { get; set; }
        public bool LoadPicture { get; set; }
        public bool UseRESTForRequest { get; set; }
        public bool DisplayWhenError { get; set; }
        

        //Passed Key Information
        public string Key { get; set; }
        public string LineSuffix { get; set; }
        public string Release { get; set; }
        public string Key2 { get; set; }
        public string LineSuffix2 { get; set; }
        public string Release2 { get; set; }


        public CSIContext()
        {
            ReadConfigurations();
        }

        public void ReadConfigurations()
        {
            CSIConfiguration.ReadConfigure(this);
        }

        public void WriteConfigurations()
        {
            CSIConfiguration.WriteConfigure(this);
        }

        public void WriteLog()
        {
            CSIErrorLog.WriteLog(this);
        }

        public override string ToString()
        {
            string OutputFormat = "[User: {0}; IDO: {1}; File: {2}; Action: {3}; Method: {4}; Activity: {5}; Fragment: {6}; ]\r\n";
            string Output = "";
            Output = string.Format(OutputFormat, User, IDO, File, Action, Method, Activity, Fragment);
            return Output;
        }

        public Bundle BuildBundle()
        {
            Bundle bundle = new Bundle();
            //User Information
            bundle.PutString("User", User);
            bundle.PutString("UserName", UserName);
            bundle.PutString("EmpNum", EmpNum);
            bundle.PutString("EmpName", EmpName);
            bundle.PutString("Currency", Currency);
            bundle.PutString("DefaultWarehouse", DefaultWarehouse);
            bundle.PutString("DefaultLocation", DefaultLocation);
            bundle.PutString("QuantityFormat", QuantityFormat);
            bundle.PutString("AmountFormat", AmountFormat);

            //Login Configurations
            bundle.PutString("Token", Token);
            bundle.PutString("CSIWebServerName", CSIWebServerName);
            bundle.PutBoolean("EnableHTTPS", EnableHTTPS);
            bundle.PutString("Configuration", Configuration);
            bundle.PutString("RecordCap", RecordCap);
            bundle.PutBoolean("UseHttps", UseHttps);
            bundle.PutBoolean("SaveUser", SaveUser);
            bundle.PutString("SavedUser", SavedUser);
            bundle.PutBoolean("SavePassword", SavePassword);
            bundle.PutString("SavedPassword", SavedPassword);
            bundle.PutBoolean("UseRESTForRequest", UseRESTForRequest);
            bundle.PutBoolean("LoadPicture", LoadPicture);
            bundle.PutBoolean("DisplayWhenError", DisplayWhenError);

            //Passed Key Information
            bundle.PutString("Key", Key);
            bundle.PutString("LineSuffix", LineSuffix);
            bundle.PutString("Release", Release);
            bundle.PutString("Key2", Key2);
            bundle.PutString("LineSuffix2", LineSuffix2);
            bundle.PutString("Release2", Release);

            return bundle;
        }

        public void ParseBundle(Bundle bundle)
        {
            if (bundle == null)
            {
                return;
            }
            //User Information
            User = bundle.GetString("User");
            UserName = bundle.GetString("UserName");
            EmpNum = bundle.GetString("EmpNum");
            EmpName = bundle.GetString("EmpName");
            Currency = bundle.GetString("Currency");
            DefaultWarehouse = bundle.GetString("DefaultWarehouse");
            DefaultLocation = bundle.GetString("DefaultLocation");
            QuantityFormat = bundle.GetString("QuantityFormat");
            AmountFormat = bundle.GetString("AmountFormat");

            //Login Configurations
            Token = bundle.GetString("Token");
            CSIWebServerName = bundle.GetString("CSIWebServerName");
            EnableHTTPS = bundle.GetBoolean("EnableHTTPS");
            CSIWebServerName = bundle.GetString("Configuration");
            RecordCap = bundle.GetString("RecordCap");
            UseHttps = bundle.GetBoolean("UseHttps");
            SaveUser = bundle.GetBoolean("SaveUser");
            SavedUser = bundle.GetString("SavedUser");
            SavePassword = bundle.GetBoolean("SavePassword");
            SavedPassword = bundle.GetString("SavedPassword");
            UseRESTForRequest = bundle.GetBoolean("UseRESTForRequest");
            LoadPicture = bundle.GetBoolean("LoadPicture");
            DisplayWhenError = bundle.GetBoolean("DisplayWhenError");

            //Passed Key Information
            Key = bundle.GetString("Key");
            LineSuffix = bundle.GetString("LineSuffix");
            Release = bundle.GetString("Release");
            Key2 = bundle.GetString("Key2");
            LineSuffix2 = bundle.GetString("LineSuffix2");
            Release2 = bundle.GetString("Release2");
        }
    }
}
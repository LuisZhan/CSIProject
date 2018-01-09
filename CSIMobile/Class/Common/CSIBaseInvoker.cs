using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CSIMobile.Class.Common
{
    public class CSIBaseInvoker : CSIBaseObject
    {
        private static string HTTP = "http://";
        private static string HTTPS = "https://";

        private static string RESTBaseURL = "/IDORequestService/MGRestService.svc";
        private static string SOAPBaseURL = "/IDORequestService/IDOWebService.asmx";

        public CSIBaseInvoker(CSIContext SrcContext = null) : base(SrcContext)
        {
            CSISystemContext.File = "CSIBaseInvoker";
        }

        public static string GetToken(CSIContext context)
        {
            if (context == null || string.IsNullOrEmpty(context.Token))
            {
                return "";
            }
            else
            {
                return context.Token;
            }
        }

        private static string GetURL(CSIContext context)
        {
            string URLPath = "";
            if (context == null)
            {
                URLPath = "";
            }
            else
            {
                if (string.IsNullOrEmpty(context.CSIWebServerName))
                {
                    CSIConfiguration.ReadConfigure(context);
                }
                if (string.IsNullOrEmpty(context.CSIWebServerName))
                {
                    URLPath = "";//still is null, return ""
                }
                else
                {
                    URLPath = (context.UseHttps ? HTTPS : HTTP) + context.CSIWebServerName + (context.UseRESTForRequest ? RESTBaseURL : SOAPBaseURL);
                }
            }
            return URLPath;
        }

        public static string CreateToken(CSIContext context)
        {
            string Token = "";
            string URL = GetURL(context);
            if (string.IsNullOrEmpty(URL))
            {
                return "";
            }
            try
            {
                if (context.UseRESTForRequest)
                {
                    //REST
                }
                else
                {
                    //SOAP
                    Token = new CSIWebService(URL).CreateSessionToken(context.User, context.Password, context.Configuration);
                }
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
            return Token;
        }

        public static string[] GetConfigurationList(CSIContext context)
        {
            string[] List = { "" };
            string URL = GetURL(context);
            if (string.IsNullOrEmpty(URL))
            {
                return List;
            }
            CSIWebService webService = new CSIWebService(URL)
            {
                //webservice调用完成后触发
                Timeout = 20000
            };
            try
            {
                if (context.UseRESTForRequest)
                {
                    //REST
                }
                else
                {
                    //SOAP
                    List = webService.GetConfigurationNames();
                }
            }catch(Exception Ex)
            {
                WriteErrorLog(Ex);
            }
            return List;
        }

        public static CSIBaseDataSet InvokeLoad(CSIContext context)
        {
            string URL = GetURL(context);
            string Token = GetToken(context);
            if (string.IsNullOrEmpty(URL) || string.IsNullOrEmpty(Token))
            {
                return null;
            }
            if (context.UseRESTForRequest)
            {
                //REST
            }
            else
            {
                //SOAP
            }
            return null;
        }

        public static bool InvokeUpdate(CSIContext context, CSIBaseDataSet DataSet)
        {
            string URL = GetURL(context);
            string Token = GetToken(context);
            if (string.IsNullOrEmpty(URL) || string.IsNullOrEmpty(Token))
            {
                return false;
            }
            if (context.UseRESTForRequest)
            {
                //REST
            }
            else
            {
                //SOAP
            }
            return true;
        }

        public static bool InvokeDelete(CSIContext context, CSIBaseDataSet DataSet)
        {
            string URL = GetURL(context);
            string Token = GetToken(context);
            if (string.IsNullOrEmpty(URL) || string.IsNullOrEmpty(Token))
            {
                return false;
            }
            if (context.UseRESTForRequest)
            {
                //REST
            }
            else
            {
                //SOAP
            }
            return true;
        }

        public static bool InvokeInsert(CSIContext context, CSIBaseDataSet DataSet)
        {
            string URL = GetURL(context);
            string Token = GetToken(context);
            if (string.IsNullOrEmpty(URL) || string.IsNullOrEmpty(Token))
            {
                return false;
            }
            if (context.UseRESTForRequest)
            {
                //REST
            }
            else
            {
                //SOAP
            }
            return true;
        }
    }
}
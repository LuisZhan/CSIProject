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
using System.Threading;

namespace CSIMobile.Class.Common
{
    public class CSIBaseInvoker : CSIBaseObject, CSIWebServiceEventInterface
    {
        private static string HTTP = "http://";
        private static string HTTPS = "https://";

        private static string RESTBaseURL = "/IDORequestService/MGRestService.svc";
        private static string SOAPBaseURL = "/IDORequestService/IDOWebService.asmx";

        private static int TimeOutInterval = 1000;
        private CSIWebService WebService;
        private string URL;
        public bool UseAsync = false;

        public event CreateSessionTokenCompletedEventHandler CreateSessionTokenCompleted;
        public event GetConfigurationNamesCompletedEventHandler GetConfigurationNamesCompleted;
        public event LoadDataSetCompletedEventHandler LoadDataSetCompleted;
        public event SaveDataSetCompletedEventHandler SaveDataSetCompleted;
        public event CallMethodCompletedEventHandler CallMethodCompleted;
        public event LoadJsonCompletedEventHandler LoadJsonCompleted;
        public event SaveJsonCompletedEventHandler SaveJsonCompleted;

        public CSIBaseInvoker(CSIContext SrcContext = null) : base(SrcContext)
        {
            CSISystemContext.File = "CSIBaseInvoker";
            InitWebService();
        }

        private void InitWebService()
        {
            URL = GetURL(CSISystemContext);
            WebService = new CSIWebService(URL)
            {
                Timeout = TimeOutInterval
            };
            WebService.GetConfigurationNamesCompleted += (o, e) => { GetConfigurationNamesCompleted(o, e); };
            WebService.CreateSessionTokenCompleted += (o, e) => { CreateSessionTokenCompleted(o, e); };
            WebService.LoadDataSetCompleted += (o, e) => { LoadDataSetCompleted(o, e); };
            WebService.SaveDataSetCompleted += (o, e) => { SaveDataSetCompleted(o, e); };
            WebService.CallMethodCompleted += (o, e) => { CallMethodCompleted(o, e); };
            WebService.LoadJsonCompleted += (o, e) => { LoadJsonCompleted(o, e); };
            WebService.SaveJsonCompleted += (o, e) => { SaveJsonCompleted(o, e); };
        }

        public string GetToken(CSIContext context)
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
                    URLPath = (context.EnableHTTPS ? HTTPS : HTTP) + context.CSIWebServerName + (context.UseRESTForRequest ? RESTBaseURL : SOAPBaseURL);
                }
            }
            return URLPath;
        }

        public string CreateToken(CSIContext context)
        {
            string Token = "";
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
                    if (UseAsync)
                    {
                        WebService.CreateSessionTokenAsync(context.User, context.Password, context.Configuration);
                    }
                    else
                    {
                        Token = WebService.CreateSessionToken(context.User, context.Password, context.Configuration);
                    }
                    
                }
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
            return Token;
        }

        public string[] GetConfigurationList(CSIContext context)
        {
            string[] List = { "" };
            if (string.IsNullOrEmpty(URL))
            {
                return List;
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
                    if (UseAsync)
                    {
                        WebService.GetConfigurationNamesAsync();
                    }
                    else
                    {
                        List = WebService.GetConfigurationNames();
                    }
                }
            }catch(Exception Ex)
            {
                WriteErrorLog(Ex);
            }
            return List;
        }

        public CSIBaseDataSet InvokeLoad(CSIContext context)
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

        public bool InvokeUpdate(CSIContext context, CSIBaseDataSet DataSet)
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

        public bool InvokeDelete(CSIContext context, CSIBaseDataSet DataSet)
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

        public bool InvokeInsert(CSIContext context, CSIBaseDataSet DataSet)
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
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
using System.Xml;

namespace CSIMobile.Class.Common
{
    public class CSIBaseInvoker : CSIBaseObject, ICSIWebServiceEventInterface
    {
        private static string HTTP = "http://";
        private static string HTTPS = "https://";

        private static string RESTBaseURL = "/IDORequestService/MGRestService.svc";
        private static string SOAPBaseURL = "/IDORequestService/IDOWebService.asmx";

        private static int TimeOutInterval = 10000;
        private CSIWebService WebService;

        private string URL;
        private string Token;
        public bool UseAsync = false;
        private System.Data.DataSet DataSet;

        public event CreateSessionTokenCompletedEventHandler CreateSessionTokenCompleted;
        public event GetConfigurationNamesCompletedEventHandler GetConfigurationNamesCompleted;
        public event LoadDataSetCompletedEventHandler LoadDataSetCompleted;
        public event SaveDataSetCompletedEventHandler SaveDataSetCompleted;
        public event CallMethodCompletedEventHandler CallMethodCompleted;
        //public event LoadJsonCompletedEventHandler LoadJsonCompleted;
        //public event SaveJsonCompletedEventHandler SaveJsonCompleted;

        public CSIBaseInvoker(CSIContext SrcContext = null) : base(SrcContext)
        {
            InitWebService();
        }

        private void InitWebService()
        {
            URL = GetURL();
            Token = GetToken();
            WebService = new CSIWebService(URL)
            {
                Timeout = TimeOutInterval
            };
            WebService.GetConfigurationNamesCompleted += (o, e) => { GetConfigurationNamesCompleted(o, e); };
            WebService.CreateSessionTokenCompleted += (o, e) => { CreateSessionTokenCompleted(o, e); };
            WebService.LoadDataSetCompleted += (o, e) => { LoadDataSetCompleted(o, e); };
            WebService.SaveDataSetCompleted += (o, e) => { SaveDataSetCompleted(o, e); };
            WebService.CallMethodCompleted += (o, e) => { CallMethodCompleted(o, e); };
            //WebService.LoadJsonCompleted += (o, e) => { LoadJsonCompleted(o, e); };
            //WebService.SaveJsonCompleted += (o, e) => { SaveJsonCompleted(o, e); };
        }

        public string GetToken()
        {
            if (CSISystemContext == null || string.IsNullOrEmpty(CSISystemContext.Token))
            {
                return "";
            }
            else
            {
                return CSISystemContext.Token;
            }
        }

        private string GetURL()
        {
            string URLPath = "";
            if (CSISystemContext == null)
            {
                URLPath = "";
            }
            else
            {
                if (string.IsNullOrEmpty(CSISystemContext.CSIWebServerName))
                {
                    CSIConfiguration.ReadConfigure(CSISystemContext);
                }
                if (string.IsNullOrEmpty(CSISystemContext.CSIWebServerName))
                {
                    URLPath = "";//still is null, return ""
                }
                else
                {
                    URLPath = (CSISystemContext.EnableHTTPS ? HTTPS : HTTP) + CSISystemContext.CSIWebServerName + (CSISystemContext.UseRESTForRequest ? RESTBaseURL : SOAPBaseURL);
                }
            }
            return URLPath;
        }

        public string CreateToken()
        {
            string Token = "";
            if (string.IsNullOrEmpty(URL))
            {
                return "";
            }
            try
            {
                if (CSISystemContext.UseRESTForRequest)
                {
                    //REST
                }
                else
                {
                    //SOAP
                    if (UseAsync)
                    {
                        WebService.CreateSessionTokenAsync(CSISystemContext.User, CSISystemContext.Password, CSISystemContext.Configuration);
                    }
                    else
                    {
                        Token = WebService.CreateSessionToken(CSISystemContext.User, CSISystemContext.Password, CSISystemContext.Configuration);
                    }
                    
                }
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
            return Token;
        }

        public string[] GetConfigurationList()
        {
            string[] List = { "" };
            if (string.IsNullOrEmpty(URL))
            {
                return List;
            }
            try
            {
                if (CSISystemContext.UseRESTForRequest)
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

        public System.Data.DataSet InvokeLoad(string strIDOName, string strPropertyList, string strFilter, string strOrderBy, string strPostQueryMethod, int iRecordCap)
        {
            if (string.IsNullOrEmpty(Token))
            {
                Token = GetToken();
            }
            if (string.IsNullOrEmpty(URL) || string.IsNullOrEmpty(Token))
            {
                return null;
            }
            if (CSISystemContext.UseRESTForRequest)
            {
                //REST
            }
            else
            {
                //SOAP
                if (UseAsync)
                {
                    WebService.LoadDataSetAsync(Token, strIDOName, strPropertyList, strFilter, strOrderBy, strPostQueryMethod, iRecordCap);
                }
                else
                {
                    DataSet = WebService.LoadDataSet(Token, strIDOName, strPropertyList, strFilter, strOrderBy, strPostQueryMethod, iRecordCap);
                }
            }
            return DataSet;
        }

        public bool InvokeUpdate(System.Data.DataSet DataSet)
        {
            if (string.IsNullOrEmpty(URL) || string.IsNullOrEmpty(Token))
            {
                return false;
            }
            if (CSISystemContext.UseRESTForRequest)
            {
                //REST
            }
            else
            {
                //SOAP
                if (UseAsync)
                {
                    WebService.SaveDataSetAsync(Token, DataSet, true, string.Empty, string.Empty, string.Empty);
                }
                else
                {
                    DataSet = WebService.SaveDataSet(Token, DataSet, true, string.Empty, string.Empty, string.Empty);
                }
            }
            return true;
        }

        public bool InvokeDelete(System.Data.DataSet DataSet)
        {
            if (string.IsNullOrEmpty(URL) || string.IsNullOrEmpty(Token))
            {
                return false;
            }
            if (CSISystemContext.UseRESTForRequest)
            {
                //REST
            }
            else
            {
                //SOAP
                if (UseAsync)
                {
                    WebService.SaveDataSetAsync(Token, DataSet, true, string.Empty, string.Empty, string.Empty);
                }
                else
                {
                    DataSet = WebService.SaveDataSet(Token, DataSet, true, string.Empty, string.Empty, string.Empty);
                }
            }
            return true;
        }

        public bool InvokeInsert(System.Data.DataSet DataSet)
        {
            if (string.IsNullOrEmpty(URL) || string.IsNullOrEmpty(Token))
            {
                return false;
            }
            if (CSISystemContext.UseRESTForRequest)
            {
                //REST
            }
            else
            {
                //SOAP
                if (UseAsync)
                {
                    WebService.SaveDataSetAsync(Token, DataSet, true, string.Empty,string.Empty,string.Empty);
                }
                else
                {
                    DataSet = WebService.SaveDataSet(Token, DataSet, true, string.Empty, string.Empty, string.Empty);
                }
            }
            return true;
        }

        public object InvokeMethod(string strIDOName, string strMethodName,string strMethodParameters)
        {
            if (string.IsNullOrEmpty(URL) || string.IsNullOrEmpty(Token))
            {
                return false;
            }
            if (CSISystemContext.UseRESTForRequest)
            {
                //REST
            }
            else
            {
                //SOAP
                if (UseAsync)
                {
                    WebService.CallMethodAsync(Token, strIDOName, strMethodName, strMethodParameters);
                }
                else
                {
                    WebService.CallMethod(Token, strIDOName, strMethodName, ref strMethodParameters);
                    return strMethodParameters;
                }
            }
            return true;
        }

        public static string TranslateError(Exception Ex)
        {
            string Message = "";
            switch (Ex.Message)
            {
                case "Error: NameResolutionFailure":
                    //e.Error.Source = "system";
                    Message = Application.Context.GetString(Resource.String.ConnectionError);
                    break;
                case "Error: ConnectFailure (Connection timed out)":
                    //e.Error.Source = "system";
                    Message = Application.Context.GetString(Resource.String.ConnectionError);
                    break;
                case "Error authenticating user: InvalidCredentials\r\n":
                case "Error authenticating user: InvalidCredentials":
                    Message = Application.Context.GetString(Resource.String.WrongUserOrPassword);
                    break;
                default:
                    Message = Ex.Message;
                    break;
            }
            return Message;
        }

        public static string BuildXMLParameters(string strXML, string strParm, bool bOutput = false)
        {
            XmlElement Root;
            XmlElement Node;
            XmlDocument DOM = new XmlDocument();
            if (string.IsNullOrEmpty(strXML))
            {
                DOM.AppendChild(DOM.CreateXmlDeclaration("1.0", "utf-8", null));
                Root = DOM.CreateElement("Parameters");
                DOM.AppendChild(Root);
            }
            else
            {
                DOM.LoadXml(strXML);
                Root = DOM.DocumentElement;
            }
            try
            {
                Node = DOM.CreateElement("Parameter");
                Node.InnerText = strParm;
                Node.SetAttribute("ByRef", bOutput ? "Y" : "N");
                Root.AppendChild(Node);
                strXML = DOM.OuterXml;
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
            return strXML;
        }

        public static string GetXMLParameters(string strXML, int position)
        {
            XmlElement Root;
            XmlElement Node;
            XmlDocument DOM = new XmlDocument();
            DOM.LoadXml(strXML);
            Root = DOM.DocumentElement;
            Node = (XmlElement)Root.GetElementsByTagName("Parameter").Item(position);
            return Node.InnerText;
        }
    }
}
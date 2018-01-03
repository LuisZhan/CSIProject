#pragma warning disable 1591

namespace SyteLine.Classes.Core.CSIWebServices
{
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Data;

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "IDOWebServiceSoap", Namespace = "http://frontstep.com/IDOWebService")]
    public partial class IDOWebService : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback GetConfigurationNamesOperationCompleted;

        private System.Threading.SendOrPostCallback CreateSessionTokenOperationCompleted;

        private System.Threading.SendOrPostCallback LoadDataSetOperationCompleted;

        private System.Threading.SendOrPostCallback SaveDataSetOperationCompleted;

        private System.Threading.SendOrPostCallback CallMethodOperationCompleted;

        private System.Threading.SendOrPostCallback LoadJsonOperationCompleted;

        private System.Threading.SendOrPostCallback SaveJsonOperationCompleted;

        private bool useDefaultCredentialsSetExplicitly;

        /// <remarks/>
        public IDOWebService(string url)
        {
            this.Url = url;
            if ((this.IsLocalFileSystemWebService(this.Url) == true))
            {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else
            {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        public new string Url
        {
            get
            {
                return base.Url;
            }
            set
            {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true)
                            && (this.useDefaultCredentialsSetExplicitly == false))
                            && (this.IsLocalFileSystemWebService(value) == false)))
                {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }

        public new bool UseDefaultCredentials
        {
            get
            {
                return base.UseDefaultCredentials;
            }
            set
            {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        /// <remarks/>
        public event GetConfigurationNamesCompletedEventHandler GetConfigurationNamesCompleted;

        /// <remarks/>
        public event CreateSessionTokenCompletedEventHandler CreateSessionTokenCompleted;

        /// <remarks/>
        public event LoadDataSetCompletedEventHandler LoadDataSetCompleted;

        /// <remarks/>
        public event SaveDataSetCompletedEventHandler SaveDataSetCompleted;

        /// <remarks/>
        public event CallMethodCompletedEventHandler CallMethodCompleted;

        /// <remarks/>
        public event LoadJsonCompletedEventHandler LoadJsonCompleted;

        /// <remarks/>
        public event SaveJsonCompletedEventHandler SaveJsonCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://frontstep.com/IDOWebService/GetConfigurationNames", RequestNamespace = "http://frontstep.com/IDOWebService", ResponseNamespace = "http://frontstep.com/IDOWebService", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string[] GetConfigurationNames()
        {
            object[] results = this.Invoke("GetConfigurationNames", new object[0]);
            return ((string[])(results[0]));
        }

        /// <remarks/>
        public void GetConfigurationNamesAsync()
        {
            this.GetConfigurationNamesAsync(null);
        }

        /// <remarks/>
        public void GetConfigurationNamesAsync(object userState)
        {
            if ((this.GetConfigurationNamesOperationCompleted == null))
            {
                this.GetConfigurationNamesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetConfigurationNamesOperationCompleted);
            }
            this.InvokeAsync("GetConfigurationNames", new object[0], this.GetConfigurationNamesOperationCompleted, userState);
        }

        private void OnGetConfigurationNamesOperationCompleted(object arg)
        {
            if ((this.GetConfigurationNamesCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetConfigurationNamesCompleted(this, new GetConfigurationNamesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://frontstep.com/IDOWebService/CreateSessionToken", RequestNamespace = "http://frontstep.com/IDOWebService", ResponseNamespace = "http://frontstep.com/IDOWebService", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string CreateSessionToken(string strUserId, string strPswd, string strConfig)
        {
            object[] results = this.Invoke("CreateSessionToken", new object[] {
                        strUserId,
                        strPswd,
                        strConfig});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void CreateSessionTokenAsync(string strUserId, string strPswd, string strConfig)
        {
            this.CreateSessionTokenAsync(strUserId, strPswd, strConfig, null);
        }

        /// <remarks/>
        public void CreateSessionTokenAsync(string strUserId, string strPswd, string strConfig, object userState)
        {
            if ((this.CreateSessionTokenOperationCompleted == null))
            {
                this.CreateSessionTokenOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateSessionTokenOperationCompleted);
            }
            this.InvokeAsync("CreateSessionToken", new object[] {
                        strUserId,
                        strPswd,
                        strConfig}, this.CreateSessionTokenOperationCompleted, userState);
        }

        private void OnCreateSessionTokenOperationCompleted(object arg)
        {
            if ((this.CreateSessionTokenCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CreateSessionTokenCompleted(this, new CreateSessionTokenCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://frontstep.com/IDOWebService/LoadDataSet", RequestNamespace = "http://frontstep.com/IDOWebService", ResponseNamespace = "http://frontstep.com/IDOWebService", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet LoadDataSet(string strSessionToken, string strIDOName, string strPropertyList, string strFilter, string strOrderBy, string strPostQueryMethod, int iRecordCap)
        {
            object[] results = this.Invoke("LoadDataSet", new object[] {
                        strSessionToken,
                        strIDOName,
                        strPropertyList,
                        strFilter,
                        strOrderBy,
                        strPostQueryMethod,
                        iRecordCap});
            return ((System.Data.DataSet)(results[0]));
        }

        /// <remarks/>
        public void LoadDataSetAsync(string strSessionToken, string strIDOName, string strPropertyList, string strFilter, string strOrderBy, string strPostQueryMethod, int iRecordCap)
        {
            this.LoadDataSetAsync(strSessionToken, strIDOName, strPropertyList, strFilter, strOrderBy, strPostQueryMethod, iRecordCap, null);
        }

        /// <remarks/>
        public void LoadDataSetAsync(string strSessionToken, string strIDOName, string strPropertyList, string strFilter, string strOrderBy, string strPostQueryMethod, int iRecordCap, object userState)
        {
            if ((this.LoadDataSetOperationCompleted == null))
            {
                this.LoadDataSetOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLoadDataSetOperationCompleted);
            }
            this.InvokeAsync("LoadDataSet", new object[] {
                        strSessionToken,
                        strIDOName,
                        strPropertyList,
                        strFilter,
                        strOrderBy,
                        strPostQueryMethod,
                        iRecordCap}, this.LoadDataSetOperationCompleted, userState);
        }

        private void OnLoadDataSetOperationCompleted(object arg)
        {
            if ((this.LoadDataSetCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LoadDataSetCompleted(this, new LoadDataSetCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://frontstep.com/IDOWebService/SaveDataSet", RequestNamespace = "http://frontstep.com/IDOWebService", ResponseNamespace = "http://frontstep.com/IDOWebService", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet SaveDataSet(string strSessionToken, System.Data.DataSet updateDataSet, bool refreshAfterSave, string strCustomInsert, string strCustomUpdate, string strCustomDelete)
        {
            object[] results = this.Invoke("SaveDataSet", new object[] {
                        strSessionToken,
                        updateDataSet,
                        refreshAfterSave,
                        strCustomInsert,
                        strCustomUpdate,
                        strCustomDelete});
            return ((System.Data.DataSet)(results[0]));
        }

        /// <remarks/>
        public void SaveDataSetAsync(string strSessionToken, System.Data.DataSet updateDataSet, bool refreshAfterSave, string strCustomInsert, string strCustomUpdate, string strCustomDelete)
        {
            this.SaveDataSetAsync(strSessionToken, updateDataSet, refreshAfterSave, strCustomInsert, strCustomUpdate, strCustomDelete, null);
        }

        /// <remarks/>
        public void SaveDataSetAsync(string strSessionToken, System.Data.DataSet updateDataSet, bool refreshAfterSave, string strCustomInsert, string strCustomUpdate, string strCustomDelete, object userState)
        {
            if ((this.SaveDataSetOperationCompleted == null))
            {
                this.SaveDataSetOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSaveDataSetOperationCompleted);
            }
            this.InvokeAsync("SaveDataSet", new object[] {
                        strSessionToken,
                        updateDataSet,
                        refreshAfterSave,
                        strCustomInsert,
                        strCustomUpdate,
                        strCustomDelete}, this.SaveDataSetOperationCompleted, userState);
        }

        private void OnSaveDataSetOperationCompleted(object arg)
        {
            if ((this.SaveDataSetCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SaveDataSetCompleted(this, new SaveDataSetCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://frontstep.com/IDOWebService/CallMethod", RequestNamespace = "http://frontstep.com/IDOWebService", ResponseNamespace = "http://frontstep.com/IDOWebService", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public object CallMethod(string strSessionToken, string strIDOName, string strMethodName, ref string strMethodParameters)
        {
            object[] results = this.Invoke("CallMethod", new object[] {
                        strSessionToken,
                        strIDOName,
                        strMethodName,
                        strMethodParameters});
            strMethodParameters = ((string)(results[1]));
            return ((object)(results[0]));
        }

        /// <remarks/>
        public void CallMethodAsync(string strSessionToken, string strIDOName, string strMethodName, string strMethodParameters)
        {
            this.CallMethodAsync(strSessionToken, strIDOName, strMethodName, strMethodParameters, null);
        }

        /// <remarks/>
        public void CallMethodAsync(string strSessionToken, string strIDOName, string strMethodName, string strMethodParameters, object userState)
        {
            if ((this.CallMethodOperationCompleted == null))
            {
                this.CallMethodOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCallMethodOperationCompleted);
            }
            this.InvokeAsync("CallMethod", new object[] {
                        strSessionToken,
                        strIDOName,
                        strMethodName,
                        strMethodParameters}, this.CallMethodOperationCompleted, userState);
        }

        private void OnCallMethodOperationCompleted(object arg)
        {
            if ((this.CallMethodCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CallMethodCompleted(this, new CallMethodCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://frontstep.com/IDOWebService/LoadJson", RequestNamespace = "http://frontstep.com/IDOWebService", ResponseNamespace = "http://frontstep.com/IDOWebService", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string LoadJson(string strSessionToken, string strIDOName, string strPropertyList, string strFilter, string strOrderBy, string strPostQueryMethod, int iRecordCap)
        {
            object[] results = this.Invoke("LoadJson", new object[] {
                        strSessionToken,
                        strIDOName,
                        strPropertyList,
                        strFilter,
                        strOrderBy,
                        strPostQueryMethod,
                        iRecordCap});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void LoadJsonAsync(string strSessionToken, string strIDOName, string strPropertyList, string strFilter, string strOrderBy, string strPostQueryMethod, int iRecordCap)
        {
            this.LoadJsonAsync(strSessionToken, strIDOName, strPropertyList, strFilter, strOrderBy, strPostQueryMethod, iRecordCap, null);
        }

        /// <remarks/>
        public void LoadJsonAsync(string strSessionToken, string strIDOName, string strPropertyList, string strFilter, string strOrderBy, string strPostQueryMethod, int iRecordCap, object userState)
        {
            if ((this.LoadJsonOperationCompleted == null))
            {
                this.LoadJsonOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLoadJsonOperationCompleted);
            }
            this.InvokeAsync("LoadJson", new object[] {
                        strSessionToken,
                        strIDOName,
                        strPropertyList,
                        strFilter,
                        strOrderBy,
                        strPostQueryMethod,
                        iRecordCap}, this.LoadJsonOperationCompleted, userState);
        }

        private void OnLoadJsonOperationCompleted(object arg)
        {
            if ((this.LoadJsonCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LoadJsonCompleted(this, new LoadJsonCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://frontstep.com/IDOWebService/SaveJson", RequestNamespace = "http://frontstep.com/IDOWebService", ResponseNamespace = "http://frontstep.com/IDOWebService", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SaveJson(string strSessionToken, string updateJsonObject, string strCustomInsert, string strCustomUpdate, string strCustomDelete)
        {
            object[] results = this.Invoke("SaveJson", new object[] {
                        strSessionToken,
                        updateJsonObject,
                        strCustomInsert,
                        strCustomUpdate,
                        strCustomDelete});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void SaveJsonAsync(string strSessionToken, string updateJsonObject, string strCustomInsert, string strCustomUpdate, string strCustomDelete)
        {
            this.SaveJsonAsync(strSessionToken, updateJsonObject, strCustomInsert, strCustomUpdate, strCustomDelete, null);
        }

        /// <remarks/>
        public void SaveJsonAsync(string strSessionToken, string updateJsonObject, string strCustomInsert, string strCustomUpdate, string strCustomDelete, object userState)
        {
            if ((this.SaveJsonOperationCompleted == null))
            {
                this.SaveJsonOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSaveJsonOperationCompleted);
            }
            this.InvokeAsync("SaveJson", new object[] {
                        strSessionToken,
                        updateJsonObject,
                        strCustomInsert,
                        strCustomUpdate,
                        strCustomDelete}, this.SaveJsonOperationCompleted, userState);
        }

        private void OnSaveJsonOperationCompleted(object arg)
        {
            if ((this.SaveJsonCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SaveJsonCompleted(this, new SaveJsonCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }

        private bool IsLocalFileSystemWebService(string url)
        {
            if (((url == null)
                        || (url == string.Empty)))
            {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024)
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0)))
            {
                return true;
            }
            return false;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void GetConfigurationNamesCompletedEventHandler(object sender, GetConfigurationNamesCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetConfigurationNamesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetConfigurationNamesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void CreateSessionTokenCompletedEventHandler(object sender, CreateSessionTokenCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CreateSessionTokenCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal CreateSessionTokenCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void LoadDataSetCompletedEventHandler(object sender, LoadDataSetCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LoadDataSetCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal LoadDataSetCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public System.Data.DataSet Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void SaveDataSetCompletedEventHandler(object sender, SaveDataSetCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SaveDataSetCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SaveDataSetCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public System.Data.DataSet Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void CallMethodCompletedEventHandler(object sender, CallMethodCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CallMethodCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal CallMethodCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public object Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((object)(this.results[0]));
            }
        }

        /// <remarks/>
        public string strMethodParameters
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void LoadJsonCompletedEventHandler(object sender, LoadJsonCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LoadJsonCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal LoadJsonCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void SaveJsonCompletedEventHandler(object sender, SaveJsonCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SaveJsonCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SaveJsonCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591
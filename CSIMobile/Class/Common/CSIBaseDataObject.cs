using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CSIMobile.Class.Common
{
    public class CSIBaseDataObject : CSIBaseObject, ICSIWebServiceEventInterface
    {
        protected string IDOName = "";
        protected CSIBaseInvoker Invoker;
        protected List<String> PreSetPropertyList = new List<string>();
        protected List<String> PropertyList = new List<string>();
        protected string Filter = "";
        protected string OrderBy = "";
        protected string PostQueryMethod = "";
        protected int RecordCap = -1;
        public int CurrentRow = 0;
        public DataSet CSIDataSet;
        public DataTable CurrentTable
        {
            get
            {
                if (CSIDataSet == null)
                {
                    return null;
                }
                if (CSIDataSet.Tables.Count <= 0)
                {
                    return null;
                }
                return CSIDataSet.Tables[0];
            }
        }

        public event GetConfigurationNamesCompletedEventHandler GetConfigurationNamesCompleted;
        public event CreateSessionTokenCompletedEventHandler CreateSessionTokenCompleted;
        public event LoadDataSetCompletedEventHandler LoadDataSetCompleted;
        public event SaveDataSetCompletedEventHandler SaveDataSetCompleted;
        public event CallMethodCompletedEventHandler CallMethodCompleted;
        //public event LoadJsonCompletedEventHandler LoadJsonCompleted;
        //public event SaveJsonCompletedEventHandler SaveJsonCompleted;

        public CSIBaseDataObject(CSIContext SrcContext = null) : base(SrcContext)
        {
            CSIDataSet = null;
            NewInvoker();

            Invoker.GetConfigurationNamesCompleted += (o, e) => { GetConfigurationNamesCompleted(this, e); };
            Invoker.CreateSessionTokenCompleted += (o, e) => { CreateSessionTokenCompleted(this, e); };
            Invoker.LoadDataSetCompleted += (o, e) => {
                if (e.Error == null)
                {
                    CSIDataSet = e.Result;
                }
                LoadDataSetCompleted(this, e);
            };
            Invoker.SaveDataSetCompleted += (o, e) => {
                if (e.Error == null)
                {
                    CSIDataSet = e.Result;
                }
                SaveDataSetCompleted(this, e); };
            Invoker.CallMethodCompleted += (o, e) => 
            {
                if (e.Error == null)
                {
                    //TO DO, No idea what's the return result
                    //CSIDataSet = e.Result;
                }
                CallMethodCompleted(this, e);
            };
            //Invoker.LoadJsonCompleted += (o, e) => { LoadJsonCompleted(this, e); };
            //Invoker.SaveJsonCompleted += (o, e) => { SaveJsonCompleted(this, e); };

            InitialPreopertyList();
            RecordCap = int.Parse(CSISystemContext.RecordCap);
        }

        protected virtual void InitialPreopertyList()
        {
            PreSetPropertyList.Add("RowPointer");
            PreSetPropertyList.Add("CreateDate");
            PreSetPropertyList.Add("CreateBy");
            PreSetPropertyList.Add("RecordDate");
            PreSetPropertyList.Add("UpdatedBy");
            PreSetPropertyList.Add("InWorkflow");
        }

        public void AddProperty(string PropertyName)
        {
            if (PreSetPropertyList.Contains(PropertyName))
            {
                if (!PropertyList.Contains(PropertyName))
                {
                    PropertyList.Add(PropertyName);
                }
            }
        }

        public string CreateToken()
        {
            return Invoker.CreateToken();
        }

        public List<string> GetConfigurationList()
        {
            return new List<string>(Invoker.GetConfigurationList());
        }

        public int GetRecordCap()
        {
            return RecordCap;
        }

        public void SetRecordCap(int RecordCap)
        {
            this.RecordCap = RecordCap;
        }

        public string GetPostQueryMethod()
        {
            return PostQueryMethod;
        }

        public void SetPostQueryMethod(string PostQueryMethod)
        {
            this.PostQueryMethod = PostQueryMethod;
        }

        public string GetOrderBy()
        {
            return OrderBy;
        }

        public void SetOrderBy(string OrderBy)
        {
            this.OrderBy = OrderBy;
        }

        public string GetFilter()
        {
            return Filter;
        }

        public void SetFilter(string Filter)
        {
            this.Filter = Filter;
        }

        public string GetPropertyList()
        {
            string ListStr = "";
            string Dot = "";
            if (PropertyList.Count == 0)
            {
                foreach (string property in PreSetPropertyList)
                {
                    ListStr += Dot + property;
                    Dot = ",";
                }
            }
            else
            {
                foreach (string property in PropertyList)
                {
                    ListStr += Dot + property;
                    Dot = ",";
                }
            }
            return ListStr;
        }

        private void NewInvoker()
        {
            Invoker = new CSIBaseInvoker(CSISystemContext)
            {
                UseAsync = true
            };
        }

        public virtual void LoadIDO()
        {
            CSIDataSet = Invoker.InvokeLoad(IDOName, GetPropertyList(), GetFilter(), GetOrderBy(), GetPostQueryMethod(), GetRecordCap());
        }

        public virtual bool InsertIDO()
        {
            bool rtn = Invoker.InvokeInsert(CSIDataSet);
            return rtn;
        }

        public virtual bool UpdateIDO()
        {
            bool rtn = Invoker.InvokeUpdate(CSIDataSet);
            return rtn;
        }

        public virtual bool InvokeMethod(string strMethodName, string strMethodParameters)
        {
            bool rtn = Invoker.InvokeMethod(IDOName, strMethodName, strMethodParameters);
            return rtn;
        }

        public virtual bool DeleteIDO()
        {
            bool rtn = Invoker.InvokeDelete(CSIDataSet);
            return rtn;
        }

        public object GetPropertyValue(int Row, int Column)
        {
            if (CurrentTable == null)
            {
                return null;
            }
            if (CurrentTable.Rows.Count <= 0)
            {
                return null;
            }
            return CurrentTable.Rows[Row].ItemArray[Column];
        }

        public object GetPropertyValue(int Row, string PropertyName)
        {
            if (CurrentTable == null)
            {
                return null;
            }
            if (CurrentTable.Columns.Count <= 0)
            {
                return null;
            }
            if (!CurrentTable.Columns.Contains(PropertyName))
            {
                return null;
            }
            int Column = CurrentTable.Columns.IndexOf(PropertyName);
            return GetPropertyValue(Row, Column);
        }

        public object GetCurrentPropertyValue(string PropertyName)
        {
            return GetPropertyValue(CurrentRow, PropertyName);
        }

        public string GetCurrentPropertyValueOfString(string PropertyName, string Default = "")
        {
            try
            {
                return (string)GetPropertyValue(CurrentRow, PropertyName)??string.Empty;
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return Default;
            }
        }
    }
}
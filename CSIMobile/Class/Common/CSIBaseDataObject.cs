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
    public class CSIBaseDataObject : CSIBaseObject, CSIWebServiceEventInterface
    {
        protected string IDOName = "";
        protected CSIBaseInvoker Invoker;
        protected List<String> PreSetPropertyList = new List<string>();
        protected List<String> PropertyList = new List<string>();
        protected string Filter = "";
        protected string OrderBy = "";
        protected string PostQueryMethod = "";
        protected int RecordCap = -1;
        public CSIBaseDataSet CSIDataSet;

        public event GetConfigurationNamesCompletedEventHandler GetConfigurationNamesCompleted;
        public event CreateSessionTokenCompletedEventHandler CreateSessionTokenCompleted;
        public event LoadDataSetCompletedEventHandler LoadDataSetCompleted;
        public event SaveDataSetCompletedEventHandler SaveDataSetCompleted;
        public event CallMethodCompletedEventHandler CallMethodCompleted;
        public event LoadJsonCompletedEventHandler LoadJsonCompleted;
        public event SaveJsonCompletedEventHandler SaveJsonCompleted;

        public CSIBaseDataObject(CSIContext SrcContext = null) : base(SrcContext)
        {
            Invoker = new CSIBaseInvoker(SrcContext)
            {
                UseAsync = true
            };

            Invoker.GetConfigurationNamesCompleted += (o, e) => { GetConfigurationNamesCompleted(o, e); };
            Invoker.CreateSessionTokenCompleted += (o, e) => { CreateSessionTokenCompleted(o, e); };
            Invoker.LoadDataSetCompleted += (o, e) => { ConvertDataSet(e.Result); LoadDataSetCompleted(o, e); };
            Invoker.SaveDataSetCompleted += (o, e) => { SaveDataSetCompleted(o, e); };
            Invoker.CallMethodCompleted += (o, e) => { CallMethodCompleted(o, e); };
            Invoker.LoadJsonCompleted += (o, e) => { LoadJsonCompleted(o, e); };
            Invoker.SaveJsonCompleted += (o, e) => { SaveJsonCompleted(o, e); };

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
                PropertyList.Add(PropertyName);
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
            foreach (string property in PropertyList)
            {
                ListStr += Dot + property;
                Dot = ",";
            }
            return ListStr;
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

        public virtual bool DeleteIDO()
        {
            bool rtn = Invoker.InvokeDelete(CSIDataSet);
            return rtn;
        }

        public void ConvertDataSet(System.Data.DataSet DataSet)
        {
            CSIDataSet = new CSIBaseDataSet(DataSet, CSISystemContext);
        }

        public System.Data.DataSet ConvertDataSet()
        {
            return ConvertDataSet(CSIDataSet);
        }

        public System.Data.DataSet ConvertDataSet(CSIBaseDataSet DataSet)
        {
            return DataSet.BuildSysDataSet();
        }
    }
}
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
using static CSIMobile.Class.Common.CSIBaseDataProperty;

namespace CSIMobile.Class.Common
{
    public class CSIBaseDataSet : CSIBaseObject
    {
        private bool IsModified;
        private Dictionary<string, CSIBaseDataRow> Rows = new Dictionary<string, CSIBaseDataRow>(); //key is rowpointer
        private Dictionary<string, Types> PropertyNameTypeList = new Dictionary<string, Types>();
        private int CurrentRowNumber = 0;

        public CSIBaseDataSet()
        {
            CurrentRowNumber = 0;
        }

        public void SetModified(bool IsModified)
        {
            this.IsModified = IsModified;
        }

        public Dictionary<string, Types> GetPropertyList()
        {
            return PropertyNameTypeList;
        }

        public int RowCount()
        {
            return Rows.Count();
        }

        public int Add()
        {
            CSIBaseDataRow DataRow = new CSIBaseDataRow(this);
            Rows.Add(DataRow.GetGUIDKey(), DataRow);
            return Rows.Count();
        }

        public int Add(CSIBaseDataRow DataRow)
        {
            Rows.Add(DataRow.GetGUIDKey(), DataRow);
            return Rows.Count();
        }

        public int Add(string GUIDKey)
        {
            Rows.Add(GUIDKey, new CSIBaseDataRow(this));
            return Rows.Count();
        }

        public int Add(string GUIDKey, CSIBaseDataRow DataRow)
        {
            Rows.Add(GUIDKey, DataRow);
            return Rows.Count();
        }

        public bool RemoveRow(int row)
        {
            foreach (string Key in Rows.Keys)
            {
                if (Rows[Key].GetRowNumber() == row)
                {
                    return Rows.Remove(Key);                     
                }
            }
            return false;
        }

        public bool RemoveRow(string Key)
        {
            try
            {
                return Rows.Remove(Key);
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return false;
            }
        }

        public void Reset()
        {
            Rows.Clear();
        }

        public bool SetPropertyValue(int row, string Name, object Value, bool IsModified = true)
        {
            foreach (string Key in Rows.Keys)
            {
                if (Rows[Key].GetRowNumber() == row)
                {
                    if (Rows[Key].SetPropertyValue(Name, Value))
                    {
                        Rows[Key].SetModified(IsModified);
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        public void SetRowModified(int row, bool IsModified)
        {
            foreach (string Key in Rows.Keys)
            {
                if (Rows[Key].GetRowNumber() == row)
                {
                    Rows[Key].SetModified(IsModified);
                    return;
                }
            }
        }

        public void SetRowDeleted(int row, bool IsDeleted)
        {
            foreach (string Key in Rows.Keys)
            {
                if (Rows[Key].GetRowNumber() == row)
                {
                    Rows[Key].SetDeleted(IsDeleted);
                    return;
                }
            }
        }

        public void SetRowNew(int row, bool IsNew)
        {
            foreach (string Key in Rows.Keys)
            {
                if (Rows[Key].GetRowNumber() == row)
                {
                    Rows[Key].SetNew(IsNew);
                    return;
                }
            }
        }

        public void SetRow(int row)
        {
            CurrentRowNumber = row;
        }

        public object GetCurrentObjectValue(string Name)
        {
            CSIBaseDataRow CurrentRow = GetCurrentObject();
            if (CurrentRow == null)
            {
                return null;
            }
            else
            {
                return GetCurrentObject().GetPropertyValue(Name);
            }
        }

        public string GetCurrentObjectString(string Name,string Default = "")
        {
            CSIBaseDataRow CurrentRow = GetCurrentObject();
            if (CurrentRow == null)
            {
                return Default;
            }
            else
            {
                return GetCurrentObject().GetPropertyString(Name);
            }
        }

        private CSIBaseDataRow GetCurrentObject()
        {
            foreach (string Key in Rows.Keys)
            {
                if (Rows[Key].GetRowNumber() == CurrentRowNumber)
                {
                    return Rows[Key];
                }
            }
            return null;
        }
    }
}
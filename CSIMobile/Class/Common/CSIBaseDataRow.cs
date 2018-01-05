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
    public class CSIBaseDataRow : CSIBaseObject
    {
        private Dictionary<string, CSIBaseDataProperty> Properties = new Dictionary<string, CSIBaseDataProperty>(); //key is property name
        private bool IsModified;
        private bool IsNew;
        private bool IsDeleted;
        private CSIBaseDataSet CurrentDataSet;
        private int RowNumber = 0;
        private string GUIDKey;
        private string RowPointer = "RowPointer";

        public CSIBaseDataRow(CSIBaseDataSet CurrentDataSet)
        {
            try
            {
                GUIDKey = Guid.NewGuid().ToString();
                this.CurrentDataSet = CurrentDataSet;
                Dictionary<string, Types> PropertyNameTypeList = CurrentDataSet.GetPropertyList();
                foreach (string Name in PropertyNameTypeList.Keys)
                {
                    Properties.Add(Name, new CSIBaseDataProperty(this, Name, null, PropertyNameTypeList[Name]));
                    if (Name == RowPointer)
                    {
                        Properties[Name].SetValue(GUIDKey);
                    }
                }
                RowNumber = CurrentDataSet.RowCount() + 1;
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return;
            }
        }

        public object GetPropertyValue(string Name)
        {
            return Properties[Name].GetValue();
        }

        public string GetPropertyString(string Name)
        {
            return Properties[Name].GetValueString();
        }

        public bool SetPropertyValue(string Name, object Value)
        {
            try
            {
                if (CurrentDataSet.GetPropertyList().TryGetValue(Name, out Types types))
                {
                    if (types == Properties[Name].GetValueType())
                    {
                        Properties[Name].SetValue(Value);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return false;
            }
        }

        public void SetModified(bool IsModified)
        {
            this.IsModified = IsModified;
            if (IsModified)
            {
                CurrentDataSet.SetModified(IsModified);
            }
            else
            {
                foreach (string Key in Properties.Keys)
                {
                    Properties[Key].SetModified(IsModified);
                }
            }
        }

        public void SetNew(bool IsNew)
        {
            this.IsNew = IsNew;
        }

        public void SetDeleted(bool IsDeleted)
        {
            this.IsDeleted = IsDeleted;
        }

        public string GetGUIDKey()
        {
            if (string.IsNullOrEmpty(GUIDKey))
            {
                if (Properties.TryGetValue(RowPointer, out CSIBaseDataProperty Key))
                {
                    GUIDKey = Key.GetValueString();
                }
            }
            return GUIDKey;
        }

        public int GetRowNumber()
        {
            return RowNumber;
        }
    }
}
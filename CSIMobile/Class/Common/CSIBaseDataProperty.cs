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
using Android.Graphics;

namespace CSIMobile.Class.Common
{
    public class CSIBaseDataProperty : CSIBaseObject
    {
        public enum Types { Type_String, Type_Date, Type_DateTime, Type_Boolean, Type_Int, Type_Decimal, Type_DataSet, Type_Bitmap };
        private string Name;
        private object Value;
        private Types Type;
        private bool IsModified;
        private CSIBaseDataRow CurrentRow;

        public CSIBaseDataProperty(CSIBaseDataRow CurrentRow, string Name, object Value, Types Type, bool IsModified = false)
        {
            CSISystemContext.File = GetType().ToString();

            this.Name = Name;
            this.Value = Value;
            this.Type = Type;
            this.IsModified = IsModified;
            this.CurrentRow = CurrentRow;
        }

        public Types GetValueType()
        {
            return Type;
        }

        public void SetModified(bool IsModified)
        {
            this.IsModified = IsModified;
            if (IsModified)
            {
                CurrentRow.SetModified(IsModified);
            }
        }

        public bool SetValue(object Value)
        {
            try
            {
                switch (Type)
                {
                    case Types.Type_DataSet:
                        break;
                    case Types.Type_Bitmap:
                        this.Value = Value;
                        break;
                    default:
                        if ((string)this.Value != (string)Value)
                        {
                            this.Value = Value;
                            IsModified = true;
                        }
                        break;
                }
                return true;
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return false;
            }
        }

        public object GetValue()
        {
            return Value;
        }

        public string GetValueString()
        {
            string StringValue = "";
            try
            {
                switch (Type)
                {
                    case Types.Type_String:
                    case Types.Type_Int:
                    case Types.Type_Decimal:
                    case Types.Type_Boolean:
                        StringValue = (string)Value;
                        break;
                    case Types.Type_Date:
                        StringValue = GetValueDate().ToShortDateString();
                        break;
                    case Types.Type_DateTime:
                        StringValue = GetValueDate().ToShortDateString() + " " + GetValueDate().ToShortTimeString();
                        break;
                    case Types.Type_DataSet:
                    default:
                        break;
                }
                return StringValue;
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return "";
            }
        }

        public int GetValueInt()
        {
            try
            {
                if (Type == Types.Type_Int)
                {
                    return (int)Value;
                }
                else
                {
                    return 0;
                }
                
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return 0;
            }
        }

        public bool GetValueBoolean()
        {
            try
            {
                if (Type == Types.Type_Boolean)
                {
                    return (bool)Value;
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

        public Decimal GetValueDecimal()
        {
            try
            {
                if (Type == Types.Type_Decimal)
                {
                    return (Decimal)Value;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return 0;
            }
        }

        public DateTime GetValueDate()
        {
            try
            {
                if (Type == Types.Type_Date)
                {
                    return DateTime.ParseExact((string)Value, "yyyyMMdd", null);
                }
                else
                {
                    return DateTime.Now;
                }

            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return DateTime.Now;
            }
        }

        public DateTime GetValueDateTime()
        {
            try
            {
                if (Type == Types.Type_DateTime)
                {
                    return DateTime.ParseExact((string)Value,"yyyyMMdd hh:mm:ss:fff",null);
                }
                else
                {
                    return DateTime.Now;
                }

            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return DateTime.Now;
            }
        }

        public CSIBaseDataSet GetValueDataSet()
        {
            try
            {
                if (Type == Types.Type_DataSet)
                {
                    return (CSIBaseDataSet)Value;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return null;
            }
        }

        public Bitmap GetValueBitmap()
        {
            try
            {
                if (Type == Types.Type_Bitmap)
                {
                    return (Bitmap)Value;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                return null;
            }
        }
    }
}
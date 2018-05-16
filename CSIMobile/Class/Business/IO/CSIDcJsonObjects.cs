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
using CSIMobile.Class.Common;
using System.IO;
using Android.Util;

namespace CSIMobile.Class.Business.IO
{
    public class CSIDcJsonObjects : CSIBaseObject
    {
        public static string BuildQtyMoveJson(string Item,string UM, string Qty,string Loc1, string Lot1, string Loc2, string Lot2)
        {
            string Output = "";
            try
            {
                MemoryStream QtyMoveStream = new MemoryStream();
                JsonWriter jWriter = new JsonWriter(new Java.IO.OutputStreamWriter(QtyMoveStream));
                jWriter.BeginObject();
                jWriter.Name("Item").Value(Item);
                jWriter.Name("UM").Value(UM);
                jWriter.Name("Qty").Value(Qty);
                jWriter.Name("Loc1").Value(Loc1);
                jWriter.Name("Lot1").Value(Loc1);
                jWriter.Name("Loc2").Value(Loc2);
                jWriter.Name("Lot2").Value(Lot2);
                jWriter.EndObject();
                jWriter.Close();
                // convert stream to string  
                QtyMoveStream.Position = 0;
                StreamReader reader = new StreamReader(QtyMoveStream);
                Output = reader.ReadToEnd();
                QtyMoveStream.Close();
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
            return Output;
        }

        public static bool ReadQtyMoveJson(string JsonString,out string Item, out string UM, out string Qty, out string Loc1, out string Lot1, out string Loc2, out string Lot2)
        {
            bool rtn = true;
            Item = string.Empty;
            UM = string.Empty;
            Qty = string.Empty;
            Loc1 = string.Empty;
            Lot1 = string.Empty;
            Loc2 = string.Empty;
            Lot2 = string.Empty;
            if (string.IsNullOrEmpty(JsonString)) return false;
            try
            {
                byte[] data = Encoding.Default.GetBytes(JsonString.ToString());
                MemoryStream QtyMoveStream = new MemoryStream(data);
                JsonReader jReader = new JsonReader(new Java.IO.InputStreamReader(QtyMoveStream));
                jReader.BeginObject();
                while (jReader.HasNext)
                {
                    string name = jReader.NextName();
                    if (name.ToUpper().Equals("Item".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Item = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("UM".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            UM = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Qty".ToUpper()) || name.ToUpper().Equals("Quantity".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Qty = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Loc1".ToUpper()) || name.ToUpper().Equals("FromLoc".ToUpper()) || name.ToUpper().Equals("FromLocation".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Loc1 = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Lot1".ToUpper()) || name.ToUpper().Equals("FromLot".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Lot1 = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Loc2".ToUpper()) || name.ToUpper().Equals("ToLoc".ToUpper()) || name.ToUpper().Equals("ToLocation".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Loc2 = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Lot2".ToUpper()) || name.ToUpper().Equals("ToLot".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Lot2 = jReader.NextString();
                        }
                    }
                    else
                    {
                        jReader.SkipValue();
                    }
                }
                jReader.EndObject();
                jReader.Close();
                QtyMoveStream.Close();
            }catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                rtn = false;
            }
            return rtn;
        }
        
        public static bool ReadJobReceiptJson(string JsonString, out string Job, out string Suffix, out string OperNum, out string Qty, out string Loc, out string Lot)
        {
            bool rtn = true;
            Job = string.Empty;
            Suffix = string.Empty;
            OperNum = string.Empty;
            Qty = string.Empty;
            Loc = string.Empty;
            Lot = string.Empty;
            if (string.IsNullOrEmpty(JsonString)) return false;
            try
            {
                byte[] data = Encoding.Default.GetBytes(JsonString.ToString());
                MemoryStream JobReceiptStream = new MemoryStream(data);
                JsonReader jReader = new JsonReader(new Java.IO.InputStreamReader(JobReceiptStream));
                jReader.BeginObject();
                while (jReader.HasNext)
                {
                    string name = jReader.NextName();
                    if (name.ToUpper().Equals("Job".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Job = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Suffix".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Suffix = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Qty".ToUpper()) || name.ToUpper().Equals("Quantity".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Qty = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("OperNum".ToUpper()) || name.ToUpper().Equals("Oper".ToUpper()) || name.ToUpper().Equals("Operation".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            OperNum = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Loc".ToUpper()) || name.ToUpper().Equals("Location".ToUpper()) || name.ToUpper().Equals("ToLoc".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Loc = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Lot".ToUpper()) || name.ToUpper().Equals("ToLot".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Lot = jReader.NextString();
                        }
                    }
                    else
                    {
                        jReader.SkipValue();
                    }
                }
                jReader.EndObject();
                jReader.Close();
                JobReceiptStream.Close();
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                rtn = false;
            }
            return rtn;
        }

        public static bool ReadMiscIssueReceiptAndQtyAdjustJson(string JsonString, out string Item, out string UM, out string Qty, out string Loc, out string Lot, out string Reason)
        {
            bool rtn = true;
            Item = string.Empty;
            UM = string.Empty;
            Qty = string.Empty;
            Loc = string.Empty;
            Lot = string.Empty;
            Reason = string.Empty;
            if (string.IsNullOrEmpty(JsonString)) return false;
            try
            {
                byte[] data = Encoding.Default.GetBytes(JsonString.ToString());
                MemoryStream QtyMoveStream = new MemoryStream(data);
                JsonReader jReader = new JsonReader(new Java.IO.InputStreamReader(QtyMoveStream));
                jReader.BeginObject();
                while (jReader.HasNext)
                {
                    string name = jReader.NextName();
                    if (name.ToUpper().Equals("Item".ToUpper()) || name.ToUpper().Equals("Itm".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Item = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("UM".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            UM = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Qty".ToUpper()) || name.ToUpper().Equals("Quantity".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Qty = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Loc".ToUpper()) || name.ToUpper().Equals("Location".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Loc = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Lot".ToUpper()) || name.ToUpper().Equals("Lot".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Lot = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Reason".ToUpper()) || name.ToUpper().Equals("ReasonCode".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Reason = jReader.NextString();
                        }
                    }
                    else
                    {
                        jReader.SkipValue();
                    }
                }
                jReader.EndObject();
                jReader.Close();
                QtyMoveStream.Close();
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                rtn = false;
            }
            return rtn;
        }

        public static bool ReadOrderShippingJson(string JsonString, out string CoNum, out string Line, out string Release, out string UM, out string Qty, out string Loc, out string Lot, out string ReasonCode)
        {
            bool rtn = true;
            CoNum = string.Empty;
            Line = string.Empty;
            Release = string.Empty;
            UM = string.Empty;
            Qty = string.Empty;
            Loc = string.Empty;
            Lot = string.Empty;
            ReasonCode = string.Empty;
            if (string.IsNullOrEmpty(JsonString)) return false;
            try
            {
                byte[] data = Encoding.Default.GetBytes(JsonString.ToString());
                MemoryStream OrderShippingStream = new MemoryStream(data);
                JsonReader jReader = new JsonReader(new Java.IO.InputStreamReader(OrderShippingStream));
                jReader.BeginObject();
                while (jReader.HasNext)
                {
                    string name = jReader.NextName();
                    if (name.ToUpper().Equals("CoNum".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            CoNum = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Line".ToUpper()) || name.ToUpper().Equals("CoLine".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Line = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Release".ToUpper()) || name.ToUpper().Equals("CoRelease".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Release = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("UM".ToUpper()) || name.ToUpper().Equals("UoM".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            UM = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Qty".ToUpper()) || name.ToUpper().Equals("Quantity".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Qty = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Loc".ToUpper()) || name.ToUpper().Equals("Location".ToUpper()) || name.ToUpper().Equals("ToLoc".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Loc = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Lot".ToUpper()) || name.ToUpper().Equals("ToLot".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Lot = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Reason".ToUpper()) || name.ToUpper().Equals("ReasonCode".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            ReasonCode = jReader.NextString();
                        }
                    }
                    else
                    {
                        jReader.SkipValue();
                    }
                }
                jReader.EndObject();
                jReader.Close();
                OrderShippingStream.Close();
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                rtn = false;
            }
            return rtn;
        }

        public static bool ReadPurchaseReceiveJson(string JsonString, out string PoNum, out string Line, out string Release, out string UM, out string Qty, out string Loc, out string Lot, out string ReasonCode)
        {
            bool rtn = true;
            PoNum = string.Empty;
            Line = string.Empty;
            Release = string.Empty;
            UM = string.Empty;
            Qty = string.Empty;
            Loc = string.Empty;
            Lot = string.Empty;
            ReasonCode = string.Empty;
            if (string.IsNullOrEmpty(JsonString)) return false;
            try
            {
                byte[] data = Encoding.Default.GetBytes(JsonString.ToString());
                MemoryStream OrderShippingStream = new MemoryStream(data);
                JsonReader jReader = new JsonReader(new Java.IO.InputStreamReader(OrderShippingStream));
                jReader.BeginObject();
                while (jReader.HasNext)
                {
                    string name = jReader.NextName();
                    if (name.ToUpper().Equals("PoNum".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            PoNum = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Line".ToUpper()) || name.ToUpper().Equals("CoLine".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Line = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Release".ToUpper()) || name.ToUpper().Equals("CoRelease".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Release = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("UM".ToUpper()) || name.ToUpper().Equals("UoM".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            UM = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Qty".ToUpper()) || name.ToUpper().Equals("Quantity".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Qty = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Loc".ToUpper()) || name.ToUpper().Equals("Location".ToUpper()) || name.ToUpper().Equals("ToLoc".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Loc = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Lot".ToUpper()) || name.ToUpper().Equals("ToLot".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            Lot = jReader.NextString();
                        }
                    }
                    else if (name.ToUpper().Equals("Reason".ToUpper()) || name.ToUpper().Equals("ReasonCode".ToUpper()))
                    {
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            ReasonCode = jReader.NextString();
                        }
                    }
                    else
                    {
                        jReader.SkipValue();
                    }
                }
                jReader.EndObject();
                jReader.Close();
                OrderShippingStream.Close();
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
                rtn = false;
            }
            return rtn;
        }

    }
}
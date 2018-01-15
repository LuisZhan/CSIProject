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

namespace CSIMobile.Class.Business
{
    public class CSIJsonObjects : CSIBaseObject
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
                    if (name.Equals("Item"))
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
                    else if (name.Equals("UM"))
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
                    else if (name.Equals("Qty"))
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
                    else if (name.Equals("Loc1"))
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
                    else if (name.Equals("Lot1"))
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
                    else if (name.Equals("Loc2"))
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
                    else if (name.Equals("Lot2"))
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
    }
}
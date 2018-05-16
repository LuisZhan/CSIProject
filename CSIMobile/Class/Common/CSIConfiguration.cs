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
using System.IO;
using Android.Util;

namespace CSIMobile.Class.Common
{
    public class CSIConfiguration : CSIBaseObject
    {
        private static string FileName = Path.Combine(
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                    "CSIConfig.dat");

        public CSIConfiguration(CSIContext SrcContext = null) : base(SrcContext)
        {
        }

        public static void WriteConfigure(CSIContext c)
        {
            try
            {
                File.Delete(FileName);
                FileStream ConfigureStream = File.Open(FileName, FileMode.OpenOrCreate);
                JsonWriter jWriter = new JsonWriter(new Java.IO.OutputStreamWriter(ConfigureStream));
                jWriter.BeginObject();
                jWriter.Name("CSIWebServerName").Value(c.CSIWebServerName ?? string.Empty);
                jWriter.Name("Configuration").Value(c.Configuration ?? string.Empty);
                jWriter.Name("ConfigurationList");
                jWriter.BeginArray();
                foreach(string config in c.ConfigurationList)
                {
                    jWriter.Value(config ?? string.Empty);
                }
                jWriter.EndArray();
                jWriter.Name("EnableHTTPS").Value(c.EnableHTTPS);
                jWriter.Name("UseRESTForRequest").Value(c.UseRESTForRequest);
                jWriter.Name("SaveUser").Value(c.SaveUser);
                jWriter.Name("SavePassword").Value(c.SavePassword);
                jWriter.Name("SavedUser").Value(c.SaveUser ? c.SavedUser : string.Empty);
                jWriter.Name("SavedPassword").Value(c.SaveUser && c.SavePassword ? c.SavedPassword : string.Empty);
                jWriter.Name("LoadPicture").Value(c.LoadPicture);
                jWriter.Name("RecordCap").Value(c.RecordCap ?? "10");
                jWriter.Name("ForceAutoPost").Value(c.ForceAutoPost);
                jWriter.Name("LicenseString").Value(c.LicenseString);
                jWriter.Name("ExpDate").Value(c.ExpDate);
                jWriter.EndObject();
                jWriter.Close();
                ConfigureStream.Close();
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
        }

        public static void NewConfigure()
        {
            try
            {
                CheckConfigureFile();
                FileStream ConfigureStream = File.Open(FileName, FileMode.OpenOrCreate);
                JsonWriter jWriter = new JsonWriter(new Java.IO.OutputStreamWriter(ConfigureStream));
                jWriter.BeginObject();
                jWriter.Name("CSIWebServerName").Value(string.Empty);
                jWriter.Name("Configuration").Value(string.Empty);
                jWriter.Name("EnableHTTPS").Value(false);
                jWriter.Name("UseRESTForRequest").Value(false);
                jWriter.Name("SaveUser").Value(false);
                jWriter.Name("SavePassword").Value(false);
                jWriter.Name("SavedUser").Value(string.Empty);
                jWriter.Name("SavedPassword").Value(string.Empty);
                jWriter.Name("LoadPicture").Value(false);
                jWriter.Name("RecordCap").Value(10);
                jWriter.Name("ConfigurationList");
                jWriter.BeginArray();
                jWriter.Value(string.Empty);
                jWriter.EndArray();
                jWriter.EndObject();
                jWriter.Close();
                ConfigureStream.Close();
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
        }

        public static void ReadConfigure(CSIContext c)
        {
            try
            {
                CheckConfigureFile();
                FileStream ConfigureStream = File.OpenRead(FileName);
                JsonReader jReader = new JsonReader(new Java.IO.InputStreamReader(ConfigureStream));
                jReader.BeginObject();
                while (jReader.HasNext)
                {
                    string name = jReader.NextName();
                    if (name.Equals("CSIWebServerName"))
                    {
                        //WriteLog("Read CSIWebServerName");
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            try
                            {
                                c.CSIWebServerName = jReader.NextString() ?? string.Empty;
                            }catch (Exception Ex)
                            {
                                WriteErrorLog(Ex);
                                c.CSIWebServerName = string.Empty;
                            }
                        }
                    }
                    else if (name.Equals("Configuration"))
                    {
                        //WriteLog("Read Configuration");
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            try
                            {
                                c.Configuration = jReader.NextString() ?? string.Empty;
                            }
                            catch (Exception Ex)
                            {
                                WriteErrorLog(Ex);
                                c.CSIWebServerName = string.Empty;
                            }
                        }
                    }
                    else if (name.Equals("ConfigurationList"))
                    {
                        //WriteLog("Read ConfigurationList");
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            c.ConfigurationList.Clear();
                            jReader.BeginArray();
                            while (jReader.HasNext)
                            {
                                if (jReader.Peek() == JsonToken.Null)
                                {
                                    jReader.SkipValue();
                                }
                                else
                                {
                                    c.ConfigurationList.Add(jReader.NextString() ?? string.Empty);
                                }
                            }
                            jReader.EndArray();
                        }
                    }
                    else if (name.Equals("SavedUser"))
                    {
                        //WriteLog("Read SavedUser");
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            c.SavedUser = jReader.NextString();
                        }
                    }
                    else if (name.Equals("SavedPassword"))
                    {
                        //WriteLog("Read SavedPassword");
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            c.SavedPassword = jReader.NextString() ?? string.Empty;
                        }
                    }
                    else if (name.Equals("EnableHTTPS"))
                    {
                        //WriteLog("Read EnableHTTPS");
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            c.EnableHTTPS = jReader.NextBoolean();
                        }
                    }
                    else if (name.Equals("UseRESTForRequest"))
                    {
                        //WriteLog("Read UseRESTForRequest");
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            c.UseRESTForRequest = jReader.NextBoolean();
                        }
                    }
                    else if (name.Equals("SaveUser"))
                    {
                        //WriteLog("Read SaveUser");
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            c.SaveUser = jReader.NextBoolean();
                        }
                    }
                    else if (name.Equals("SavePassword"))
                    {
                        //WriteLog("Read SavePassword");
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            c.SavePassword = jReader.NextBoolean();
                        }
                    }
                    else if (name.Equals("LoadPicture"))
                    {
                        //WriteLog("Read Load Picture");
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            c.LoadPicture = jReader.NextBoolean();
                        }
                    }
                    else if (name.Equals("ForceAutoPost"))
                    {
                        //WriteLog("Force Auto Post");
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            c.ForceAutoPost = jReader.NextBoolean();
                        }
                    }
                    else if (name.Equals("ExpDate"))
                    {
                        //WriteLog("ExpDate");
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            c.ExpDate = jReader.NextString() ?? string.Empty;
                        }
                    }
                    else if (name.Equals("LicenseString"))
                    {
                        //WriteLog("Force Auto Post");
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            c.LicenseString = jReader.NextString() ?? string.Empty;
                        }
                    }
                    else if (name.Equals("RecordCap"))
                    {
                        //WriteLog("Read RecordCap");
                        if (jReader.Peek() == JsonToken.Null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            c.RecordCap = jReader.NextString() ?? string.Empty;
                        }
                    }
                    else
                    {
                        jReader.SkipValue();
                    }
                }
                jReader.EndObject();
                jReader.Close();
                ConfigureStream.Close();
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
        }

        private static void CheckConfigureFile()
        {
            if (!File.Exists(FileName))
            {
                File.Open(FileName, FileMode.CreateNew).Dispose();
                NewConfigure();
            }
        }
    }
}
﻿using System;
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
                jWriter.Name("CSIWebServerName").Value(c.CSIWebServerName);
                jWriter.Name("Configuration").Value(c.Configuration);
                jWriter.Name("ConfigurationList");
                jWriter.BeginArray();
                foreach(string config in c.ConfigurationList)
                {
                    jWriter.Value(config);
                }
                jWriter.EndArray();
                jWriter.Name("EnableHTTPS").Value(c.EnableHTTPS);
                jWriter.Name("UseRESTForRequest").Value(c.UseRESTForRequest);
                jWriter.Name("SaveUser").Value(c.SaveUser);
                jWriter.Name("SavePassword").Value(c.SavePassword);
                jWriter.Name("SavedUser").Value(c.SaveUser ? c.SavedUser : "");
                jWriter.Name("SavedPassword").Value(c.SaveUser && c.SavePassword ? c.SavedPassword : "");
                jWriter.Name("LoadPicture").Value(c.LoadPicture);
                jWriter.Name("RecordCap").Value(c.RecordCap);
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
                jWriter.Name("CSIWebServerName").Value("");
                jWriter.Name("Configuration").Value("");
                jWriter.Name("EnableHTTPS").Value(false);
                jWriter.Name("UseRESTForRequest").Value(false);
                jWriter.Name("SaveUser").Value(false);
                jWriter.Name("SavePassword").Value(false);
                jWriter.Name("SavedUser").Value("");
                jWriter.Name("SavedPassword").Value("");
                jWriter.Name("LoadPicture").Value(false);
                jWriter.Name("RecordCap").Value(10);
                jWriter.Name("ConfigurationList");
                jWriter.BeginArray();
                jWriter.Value("");
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
                        if (jReader.Peek() == null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            c.CSIWebServerName = jReader.NextString();
                        }
                    }
                    else if (name.Equals("Configuration"))
                    {
                        if (jReader.Peek() == null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            c.Configuration = jReader.NextString();
                        }
                    }
                    else if (name.Equals("ConfigurationList"))
                    {
                        if (jReader.Peek() == null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            jReader.BeginArray();
                            c.ConfigurationList.Clear();
                            while (jReader.HasNext)
                            {
                                if (jReader.Peek() == null)
                                {
                                    jReader.SkipValue();
                                }
                                else
                                {
                                    c.ConfigurationList.Add(jReader.NextString());
                                }
                            }
                            jReader.EndArray();
                        }
                    }
                    else if (name.Equals("SavedUser"))
                    {
                        if (jReader.Peek() == null)
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
                        if (jReader.Peek() == null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            c.SavedPassword = jReader.NextString();
                        }
                    }
                    else if (name.Equals("EnableHTTPS"))
                    {
                        if (jReader.Peek() == null)
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
                        if (jReader.Peek() == null)
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
                        if (jReader.Peek() == null)
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
                        if (jReader.Peek() == null)
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
                        if (jReader.Peek() == null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            c.LoadPicture = jReader.NextBoolean();
                        }
                    }
                    else if (name.Equals("RecordCap"))
                    {
                        if (jReader.Peek() == null)
                        {
                            jReader.SkipValue();
                        }
                        else
                        {
                            c.RecordCap = jReader.NextString();
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
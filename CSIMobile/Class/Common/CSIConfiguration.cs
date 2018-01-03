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
    class CSIConfiguration : Object
    {
        private static string FileName = Path.Combine(
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                    Application.Context.GetString(Resource.String.app_name) + "\\CSIConfig.dat");

        public static void WriteConfigure(CSIContext Context)
        {
            try
            {
                CheckConfigureFile();
                FileStream ConfigureStream = File.OpenWrite(FileName);
                JsonWriter jWriter = new JsonWriter(new Java.IO.OutputStreamWriter(ConfigureStream));
                jWriter.BeginObject();
                jWriter.Name("CSIWebServerName").Value(Context.CSIWebServerName);
                jWriter.Name("Configuration").Value(Context.Configuration);
                jWriter.Name("EnableHTTPS").Value(Context.EnableHTTPS);
                jWriter.Name("UseRESTForRequest").Value(Context.UseRESTForRequest);
                jWriter.Name("SaveUser").Value(Context.SaveUser);
                jWriter.Name("SavePassword").Value(Context.SavePassword);
                jWriter.Name("SavedUser").Value(Context.SaveUser ? Context.SavedUser : "");
                jWriter.Name("SavedPassword").Value(Context.SaveUser && Context.SavePassword ? Context.SavedPassword : "");
                jWriter.Name("LoadPicture").Value(Context.LoadPicture);
                jWriter.Name("RecordCap").Value(Context.RecordCap);
                jWriter.EndObject();
                jWriter.Close();
                ConfigureStream.Close();
            } catch (Exception Ex)
            {
                throw Ex;
            }
        }


        public static void ReadConfigure(CSIContext Context)
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
                            Context.CSIWebServerName = jReader.NextString();
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
                            Context.Configuration = jReader.NextString();
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
                            Context.SavedUser = jReader.NextString();
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
                            Context.SavedPassword = jReader.NextString();
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
                            Context.EnableHTTPS = jReader.NextBoolean();
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
                            Context.UseRESTForRequest = jReader.NextBoolean();
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
                            Context.SaveUser = jReader.NextBoolean();
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
                            Context.SavePassword = jReader.NextBoolean();
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
                            Context.LoadPicture = jReader.NextBoolean();
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
                            Context.RecordCap = jReader.NextString();
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
            catch
            {
                WriteConfigure(Context);
            }
        }

        private static void CheckConfigureFile()
        {
            string FileName = GetErrorLogFileName();
            if (!File.Exists(FileName))
            {
                File.Open(FileName, FileMode.CreateNew).Close();
            }
        }

        private static string GetErrorLogFileName()
        {
            return FileName + "Log.txt";
        }

        private static void DeleteErrorLogFile()
        {
            File.Delete(GetErrorLogFileName());
            CheckConfigureFile();
        }
    }
}
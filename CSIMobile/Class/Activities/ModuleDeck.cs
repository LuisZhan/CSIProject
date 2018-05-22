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
using Android.Content.Res;
using System.IO;
using Android.Util;

namespace CSIMobile.Class.Activities
{
    public class Module : CSIBaseObject
    {
        public string ModuleName;
        public ModuleAction[] ModuleActions = { };
        public bool Visible = true;
        public int DisplayPosition;
        public Module(CSIContext SrcContext = null) : base(SrcContext)
        {
        }
    }

    public class ModuleAction : CSIBaseObject
    {
        public string ActionName;
        public Type ActivityType;
        public string[] InvokeCommands = { };//"GetToken"
        public int DrawableId;
        public bool Enabled = true;
        public bool Visible = true;
        public ModuleAction(CSIContext SrcContext = null) : base(SrcContext)
        {
        }
    }

    public class ModuleDeck : CSIBaseObject
    {
        public static Module[] Modules;

        public ModuleDeck(CSIContext SrcContext = null) : base(SrcContext)
        {
            Modules = BuildModulesByLicense();
        }

        public Module this[int i]
        {
            get { return Modules[i]; }
        }
        
        public int NumModules
        {
            get { return Modules.Length; }
        }
        
        public int NumVisibleModules()
        {
            int Count = 0;
            if (Modules.Count() > 0)
            {
                foreach (Module M in Modules)
                {
                    if (M.Visible)
                    {
                        Count++;
                    }
                }
            }
            return Count;
        }

        public Module[] GetBuildInModules()
        {
            List<Module> Modules = new List<Module>();
            string JsonString = string.Empty;

            AssetManager assets = Application.Context.Assets;
            using (StreamReader sr = new StreamReader(assets.Open("Modules.json")))
            {
                JsonString = sr.ReadToEnd();
            }

            if (string.IsNullOrEmpty(JsonString))
            {
                return Modules.ToArray();
            }

            try
            {
                byte[] data = Encoding.Default.GetBytes(JsonString.ToString());
                MemoryStream ModulesStream = new MemoryStream(data);
                JsonReader jReader = new JsonReader(new Java.IO.InputStreamReader(ModulesStream));
                jReader.BeginObject();
                while (jReader.HasNext)
                {
                    string name = jReader.NextName();
                    if (name.ToUpper().Equals("Modules".ToUpper()))
                    {
                        jReader.BeginArray();
                        while (jReader.HasNext)
                        {
                            Module module = new Module(CSISystemContext);
                            jReader.BeginObject();
                            while (jReader.HasNext)
                            {
                                name = jReader.NextName();
                                if (jReader.Peek() == JsonToken.Null)
                                {
                                    jReader.SkipValue();
                                }
                                else if (name.ToUpper().Equals("Name".ToUpper()))
                                {
                                    if (jReader.Peek() == JsonToken.Null)
                                    {
                                        jReader.SkipValue();
                                    }
                                    else
                                    {
                                        module.ModuleName = GetResourceStringByName(StringType, jReader.NextString());
                                    }
                                }
                                else if (name.ToUpper().Equals("Visible".ToUpper()))
                                {
                                    if (jReader.Peek() == JsonToken.Null)
                                    {
                                        jReader.SkipValue();
                                    }
                                    else
                                    {
                                        module.Visible = jReader.NextBoolean();
                                    }
                                }
                                else if (name.ToUpper().Equals("Position".ToUpper()))
                                {
                                    if (jReader.Peek() == JsonToken.Null)
                                    {
                                        jReader.SkipValue();
                                    }
                                    else
                                    {
                                        module.DisplayPosition = jReader.NextInt();
                                    }
                                }
                                else if (name.ToUpper().Equals("ModuleActions".ToUpper()))
                                {
                                    List<ModuleAction> ModuleActions = new List<ModuleAction>();
                                    jReader.BeginArray();
                                    while (jReader.HasNext)
                                    {
                                        ModuleAction action = new ModuleAction(CSISystemContext);
                                        jReader.BeginObject();
                                        while (jReader.HasNext)
                                        {
                                            name = jReader.NextName();
                                            if (jReader.Peek() == JsonToken.Null)
                                            {
                                                jReader.SkipValue();
                                            }
                                            else if (name.ToUpper().Equals("Name".ToUpper()))
                                            {
                                                if (jReader.Peek() == JsonToken.Null)
                                                {
                                                    jReader.SkipValue();
                                                }
                                                else
                                                {
                                                    action.ActionName = GetResourceStringByName(StringType, jReader.NextString());
                                                }
                                            }
                                            else if (name.ToUpper().Equals("Img".ToUpper()))
                                            {
                                                if (jReader.Peek() == JsonToken.Null)
                                                {
                                                    jReader.SkipValue();
                                                }
                                                else
                                                {
                                                    action.DrawableId = GetResourceIdByName(DrawableType, jReader.NextString());
                                                }
                                            }
                                            else if (name.ToUpper().Equals("Enabled".ToUpper()))
                                            {
                                                if (jReader.Peek() == JsonToken.Null)
                                                {
                                                    jReader.SkipValue();
                                                }
                                                else
                                                {
                                                    action.Enabled = jReader.NextBoolean();
                                                }
                                            }
                                            else if (name.ToUpper().Equals("Visible".ToUpper()))
                                            {
                                                if (jReader.Peek() == JsonToken.Null)
                                                {
                                                    jReader.SkipValue();
                                                }
                                                else
                                                {
                                                    action.Visible = jReader.NextBoolean();
                                                }
                                            }
                                            else if (name.ToUpper().Equals("InvokeCommands".ToUpper()))
                                            {
                                                if (jReader.Peek() == JsonToken.Null)
                                                {
                                                    jReader.SkipValue();
                                                }
                                                else
                                                {
                                                    List<string> InvokeCommands = new List<string>();
                                                    jReader.BeginArray();
                                                    while (jReader.HasNext)
                                                    {
                                                        if (jReader.Peek() == JsonToken.Null)
                                                        {
                                                            jReader.SkipValue();
                                                        }
                                                        else
                                                        {
                                                            InvokeCommands.Add(jReader.NextString());
                                                        }
                                                    }
                                                    jReader.EndArray();
                                                    action.InvokeCommands = InvokeCommands.ToArray();
                                                }
                                            }
                                            else
                                            {
                                                jReader.SkipValue();
                                            }
                                        }
                                        jReader.EndObject();
                                        ModuleActions.Add(action);
                                    }
                                    jReader.EndArray();
                                    module.ModuleActions = ModuleActions.ToArray();
                                }
                                else
                                {
                                    jReader.SkipValue();
                                }
                            }
                            jReader.EndObject();
                            Modules.Add(module);
                        }
                        jReader.EndArray();
                    }
                    else
                    {
                        jReader.SkipValue();
                    }
                }
                jReader.EndObject();
                jReader.Close();
                ModulesStream.Close();
            }
            catch (Exception Ex)
            {
                WriteErrorLog(Ex);
            }
            return Modules.ToArray();
        }

        public Module[] BuildModulesByLicense()
        {
            Module[] LicensedModules = null;
            return LicensedModules ?? GetBuildInModules();
        }
    }
}
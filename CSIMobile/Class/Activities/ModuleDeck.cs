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
using Java.Lang;

namespace CSIMobile.Class.Activities
{
    public class Module : CSIBaseObject
    {
        public string ModuleName;
        public ModuleAction[] ModuleActions;
        public Module(CSIContext SrcContext = null) : base(SrcContext)
        {
        }
    }

    public class ModuleAction : CSIBaseObject
    {
        public string ActionName;
        public Type ActivityType;
        public string[] InvokeCommands = { "GetToken" };
        public int DrawableId;
        public ModuleAction(CSIContext SrcContext = null) : base(SrcContext)
        {
        }
    }

    public class ModuleDeck : CSIBaseObject
    {
        private static Module[] builtInModules =
        {
            new Module
            {
                ModuleName = Application.Context.GetString(Resource.String.MasterData),
                ModuleActions = new ModuleAction[]
                {
                    new ModuleAction {
                        ActionName = Application.Context.GetString(Resource.String.Items),
                        DrawableId = Resource.Drawable.Logo,
                        //ActivityType = typeof(object)
                    },
                    new ModuleAction {
                        ActionName = Application.Context.GetString(Resource.String.PurchaseOrders),
                        DrawableId = Resource.Drawable.Logo,
                        //ActivityType = typeof(object)
                    },
                    new ModuleAction {
                        ActionName = Application.Context.GetString(Resource.String.SalesOrders),
                        DrawableId = Resource.Drawable.Logo,
                        //ActivityType = typeof(object)
                    },
                    new ModuleAction {
                        ActionName = Application.Context.GetString(Resource.String.JobOrders),
                        DrawableId = Resource.Drawable.Logo,
                        //ActivityType = typeof(object)
                    }
                }
            },
            new Module
            {
                ModuleName = Application.Context.GetString(Resource.String.InventoryActivities),
                ModuleActions = new ModuleAction[]
                {
                    new ModuleAction {
                        ActionName = Application.Context.GetString(Resource.String.QuantityMove),
                        DrawableId = Resource.Drawable.Logo,
                        //ActivityType = typeof(object)
                    },
                    new ModuleAction {
                        ActionName = Application.Context.GetString(Resource.String.MiscIssue),
                        DrawableId = Resource.Drawable.Logo,
                        //ActivityType = typeof(object)
                    },
                    new ModuleAction {
                        ActionName = Application.Context.GetString(Resource.String.MiscReceive),
                        DrawableId = Resource.Drawable.Logo,
                        //ActivityType = typeof(object)
                    },
                    new ModuleAction {
                        ActionName = Application.Context.GetString(Resource.String.PurchaseReceive),
                        DrawableId = Resource.Drawable.Logo,
                        //ActivityType = typeof(object)
                    },
                    new ModuleAction {
                        ActionName = Application.Context.GetString(Resource.String.SalesShip),
                        DrawableId = Resource.Drawable.Logo,
                        //ActivityType = typeof(object)
                    },
                    new ModuleAction {
                        ActionName = Application.Context.GetString(Resource.String.TransferShip),
                        DrawableId = Resource.Drawable.Logo,
                        //ActivityType = typeof(object)
                    },
                    new ModuleAction {
                        ActionName = Application.Context.GetString(Resource.String.TransferReceive),
                        DrawableId = Resource.Drawable.Logo,
                        //ActivityType = typeof(object)
                    }
                }
            },
            new Module
            {
                ModuleName = Application.Context.GetString(Resource.String.Shopfloor),
                ModuleActions = new ModuleAction[]
                {
                    new ModuleAction {
                        ActionName = Application.Context.GetString(Resource.String.JobMaterial),
                        DrawableId = Resource.Drawable.Logo,
                        //ActivityType = typeof(object)
                    },
                    new ModuleAction {
                        ActionName = Application.Context.GetString(Resource.String.JobTransaction),
                        DrawableId = Resource.Drawable.Logo,
                        //ActivityType = typeof(object)
                    },
                    new ModuleAction {
                        ActionName = Application.Context.GetString(Resource.String.WorkCenterTransaction),
                        DrawableId = Resource.Drawable.Logo,
                        //ActivityType = typeof(object)
                    },
                    new ModuleAction {
                        ActionName = Application.Context.GetString(Resource.String.ProductionSchedule),
                        DrawableId = Resource.Drawable.Logo,
                        //ActivityType = typeof(object)
                    },
                    new ModuleAction {
                        ActionName = Application.Context.GetString(Resource.String.JustInTime),
                        DrawableId = Resource.Drawable.Logo,
                        //ActivityType = typeof(object)
                    }
                }
            },
            new Module
            {
                ModuleName = Application.Context.GetString(Resource.String.Settings),
                ModuleActions = new ModuleAction[]
                {
                    new ModuleAction {
                        ActionName = Application.Context.GetString(Resource.String.Login),
                        DrawableId = Resource.Drawable.Logo,
                        InvokeCommands = new string[] { "ShowSignIn" }
                    },
                    new ModuleAction {
                        ActionName = Application.Context.GetString(Resource.String.Settings),
                        DrawableId = Resource.Drawable.Logo,
                        InvokeCommands = new string[] { "ShowSettings" }
                    },
                    new ModuleAction {
                        ActionName = Application.Context.GetString(Resource.String.About),
                        DrawableId = Resource.Drawable.Logo,
                        InvokeCommands = new string[] { "ShowAbout" }
                    },
                    new ModuleAction {
                        ActionName = Application.Context.GetString(Resource.String.Log),
                        DrawableId = Resource.Drawable.Logo,
                        InvokeCommands = new string[] { "ShowLog" }
                    }
                }
            }
        };

        public static Module[] Modules;

        public ModuleDeck(CSIContext SrcContext = null) : base(SrcContext)
        {
            Modules = builtInModules;
        }

        public Module this[int i]
        {
            get { return Modules[i]; }
        }
        
        public int NumModules
        {
            get { return Modules.Length; }
        }
    }
}
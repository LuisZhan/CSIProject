using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Support.V7.App;
using System;
using CSIMobile.Class.Common;

namespace CSIMobile.Class.Activites
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }
        
        // Launches the startup task
        protected override void OnResume()
        {
            try
            {
                base.OnResume();
                Task startupWork = new Task(() => { SimulateStartup(); });
                startupWork.Start();
            }catch(Exception Ex)
            {
                CSIErrorLog.WriteErrorLog(Ex);
            }
        }

        // Prevent the back button from canceling the startup process
        public override void OnBackPressed() { }

        // Simulates background work that happens behind the splash screen
        private async void SimulateStartup()
        {
            await Task.Delay(1000); // Simulate a bit of startup work.

            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}
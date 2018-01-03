using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Android.Widget;
using CSIMobile;

namespace com.xamarin.sample.splashscreen
{
    [Activity(Label = "@string/app_name")]
    public class MainActivity : Activity
    {
        static readonly string TAG = "X:" + typeof(MainActivity).Name;
        Button _button;
        int _clickCount;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            
            _button = FindViewById<Button>(Resource.Id.MyButton);
            _button.Click += (sender, args) =>
            {
                string message = string.Format("You clicked {0} times.", ++_clickCount);
                _button.Text = message;
                Log.Debug(TAG, message);
            };
            
            Log.Debug(TAG, "MainActivity is loaded.");
        }
    }
}
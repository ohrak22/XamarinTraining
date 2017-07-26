using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace _3D_with_MonoGame
{
	[Activity(Label = "3D with MonoGame"
		, MainLauncher = true
		, Icon = "@drawable/icon"
		, Theme = "@style/Theme.Splash"
		, AlwaysRetainTaskState = true
		, LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
		, ScreenOrientation = ScreenOrientation.Landscape
		, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
	public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			//var g = new Game1();
			//var g = new Game2();
			var g = new Game3();
			SetContentView((View)g.Services.GetService(typeof(View)));
			g.Run();
		}
	}
}


using Android.App;
using Android.Content.PM;
using Android.OS;
using Jakar.SettingsView.Droid;
using Jakar.Extensions.Xamarin.Forms.Droid;
using Jakar.SettingsView.Sample.Shared;
using Prism;
using Prism.Ioc;

namespace Sample.Droid
{
	[Activity(Label = "SettingsViewSample",
			  Icon = "@drawable/icon",
			  Theme = "@style/MyTheme",
			  MainLauncher = true,
			  ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
			 )]
	public class MainActivity : BaseApplication
	{
		protected override void OnCreate( Bundle bundle )
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			AiForms.Effects.Droid.Effects.Init();
			Xamarin.Forms.Svg.Droid.SvgImage.Init(this);	
			SettingsViewInit.Init(this);

			LoadApplication(new App(new AndroidInitializer()));
		}
	}

	public class AndroidInitializer : IPlatformInitializer
	{
		public void RegisterTypes( IContainerRegistry containerRegistry ) { }
	}
}
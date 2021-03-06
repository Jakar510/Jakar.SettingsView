﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Jakar.SettingsView.Droid;
using Prism;
using Prism.Ioc;

namespace Sample.Droid
{
    [Activity(Label = "Sample.Droid", Icon = "@drawable/icon",Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

            Xamarin.Forms.Forms.SetFlags("FastRenderers_Experimental");
			 Xamarin.Forms.Forms.Init(this, bundle);
            AiForms.Effects.Droid.Effects.Init();
            Xamarin.Forms.Svg.Droid.SvgImage.Init(this);
            SettingsViewInit.Init(this);


			LoadApplication(new App(new AndroidInitializer()));
		}
	}

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}

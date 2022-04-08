using Foundation;
using Jakar.Extensions.Xamarin.Forms.iOS;
using Jakar.SettingsView.iOS;
using Jakar.SettingsView.Sample.Shared;
using Prism;
using Prism.Ioc;
using UIKit;


namespace Jakar.SettingsView.Sample.iOS
{
	[Register(nameof(AppDelegate))]
	public class AppDelegate : BaseApplication
	{
		public override bool FinishedLaunching( UIApplication app, NSDictionary options )
		{
			Init("MediaElement_Experimental");

			SettingsViewInit.Init();
			AiForms.Effects.iOS.Effects.Init();
			Xamarin.Forms.Svg.iOS.SvgImage.Init();

			LoadApplication(new App(new IOSInitializer()));
			return base.FinishedLaunching(app, options);
		}
	}

	public class IOSInitializer : IPlatformInitializer
	{
		public void RegisterTypes( IContainerRegistry containerRegistry ) { }
	}
}
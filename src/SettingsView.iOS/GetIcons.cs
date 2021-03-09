using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Foundation;
using Jakar.SettingsView.iOS;
using Jakar.SettingsView.Shared.sv;
using UIKit;
using Xamarin.Forms;


[assembly: Dependency(typeof(GetIcons))]

namespace Jakar.SettingsView.iOS
{
	public class GetIcons : IIcons
	{
		public ImageSource GetImageSource( string name ) => ImageSource.FromResource(name, Assembly.GetAssembly(GetType()));
	}
}
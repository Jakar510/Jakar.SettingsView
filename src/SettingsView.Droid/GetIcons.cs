using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Droid;
using Jakar.SettingsView.Shared.sv;
using Xamarin.Forms;

[assembly: Dependency(typeof(GetIcons))]
namespace Jakar.SettingsView.Droid
{
	public class GetIcons : IIcons
	{
		public ImageSource GetImageSource( string name ) => ImageSource.FromResource(name, Assembly.GetAssembly(GetType()));
	}
}
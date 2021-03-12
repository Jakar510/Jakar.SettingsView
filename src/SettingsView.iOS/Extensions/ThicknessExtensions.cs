using System;
using UIKit;
using Xamarin.Forms;

namespace Jakar.SettingsView.iOS.Extensions
{
	[Foundation.Preserve(AllMembers = true)]
	public static class ThicknessExtensions
	{
		public static UIEdgeInsets ToUIEdgeInsets( this Thickness forms ) => new(forms.Top.ToNFloat(), forms.Left.ToNFloat(), forms.Bottom.ToNFloat(), forms.Right.ToNFloat());
	}
}
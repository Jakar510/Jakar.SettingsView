using System;
using UIKit;
using Xamarin.Forms;

namespace Jakar.SettingsView.iOS.Extensions
{
	/// <summary>
	/// Thickness extensions.
	/// </summary>
	[Foundation.Preserve(AllMembers = true)]
	public static class ThicknessExtensions
	{
		/// <summary>
		/// To the UIEdgeinsets.
		/// </summary>
		/// <returns>The UIE dge insets.</returns>
		/// <param name="forms">Forms.</param>
		public static UIEdgeInsets ToUIEdgeInsets( this Thickness forms ) => new UIEdgeInsets((nfloat) forms.Top, (nfloat) forms.Left, (nfloat) forms.Bottom, (nfloat) forms.Right);
	}
}
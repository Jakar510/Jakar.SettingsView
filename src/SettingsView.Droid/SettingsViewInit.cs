using System;

namespace Jakar.SettingsView.Droid
{
	/// <summary>
	/// SettingsViewInit.
	/// </summary>
	[Android.Runtime.Preserve(AllMembers = true)]
	public static class SettingsViewInit
	{
		internal static Xamarin.Forms.Platform.Android.FormsAppCompatActivity Current { get; set; }

		/// <summary>
		/// Init this instance.
		/// </summary>
		public static void Init( Xamarin.Forms.Platform.Android.FormsAppCompatActivity activity )
		{
			Current = activity ?? throw new NullReferenceException(nameof(activity));
		}
	}
}
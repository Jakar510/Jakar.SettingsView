using System;

namespace Jakar.SettingsView.Droid
{
	/// <summary>
	/// SettingsViewInit.
	/// </summary>
	[Android.Runtime.Preserve(AllMembers = true)]
	public static class SettingsViewInit
	{
#pragma warning disable 8618
		internal static Xamarin.Forms.Platform.Android.FormsAppCompatActivity Current { get; set; }
#pragma warning restore 8618

		/// <summary>
		/// Init this instance.
		/// </summary>
		public static void Init( Xamarin.Forms.Platform.Android.FormsAppCompatActivity activity )
		{
			Current = activity ?? throw new NullReferenceException(nameof(activity));
		}
	}
}
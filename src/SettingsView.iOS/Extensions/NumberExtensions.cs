// unset

using System;

namespace Jakar.SettingsView.iOS.Extensions
{
	[Foundation.Preserve(AllMembers = true)]
	public static class PlatformExtensions
	{
		public static nfloat ToNFloat( this double value ) => (nfloat) value;
		public static nfloat ToNFloat( this float value ) => value;
		public static nfloat ToNFloat( this int value ) => value;


	}
}
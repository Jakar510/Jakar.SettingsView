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

		public static double ToDouble( this nfloat value ) => value;
		public static float ToFloat( this nfloat value ) => (float) value;
		public static int ToInt( this nfloat value ) => (int) value;


		public static nint ToNInt( this double value ) => (nint) value;
		public static nint ToNInt( this float value ) => (nint) value;
		public static nint ToNInt( this int value ) => value;

		public static double ToDouble( this nint value ) => value;
		public static float ToFloat( this nint value ) => value;
		public static int ToInt( this nint value ) => (int) value;


		public static nuint ToNUInt( this double value ) => (nuint) value;
		public static nuint ToNUInt( this float value ) => (nuint) value;
		public static nuint ToNUInt( this int value ) => new ((uint) value);

		public static double ToDouble( this nuint value ) => value;
		public static float ToFloat( this nuint value ) => value;
		public static int ToInt( this nuint value ) => (int) value;
	}
}
// unset

using System;
using Xamarin.Forms.Internals;

namespace Jakar.SettingsView.Shared.Misc
{
	[Preserve(true, false)]
	public static class NumberExtensions
	{
		public static float ToFloat( this double value ) => (float) value;
		public static int ToInt( this double value ) => (int) value;
		
	}
}
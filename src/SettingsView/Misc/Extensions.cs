// unset

using System.Linq;

#nullable enable
namespace Jakar.SettingsView.Shared.Misc
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public static class Extensions
	{
		public static bool IsOneOf<TValue>( this TValue value, params TValue[] items ) => items.Any(item => value.IsEqual(item));
		public static bool IsEqual<TValue>( this TValue value, TValue other ) => value.Equals(other);
	}
}
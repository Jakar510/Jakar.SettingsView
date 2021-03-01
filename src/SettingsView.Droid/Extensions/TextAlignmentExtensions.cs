using Xamarin.Forms;
using GravityFlags = Android.Views.GravityFlags;
using ATextAlignment = Android.Views.TextAlignment;

namespace Jakar.SettingsView.Droid.Extensions
{
	/// <summary>
	/// Text alignment extensions.
	/// </summary>
	[Android.Runtime.Preserve(AllMembers = true)]
	public static class TextAlignmentExtensions
	{

		public static GravityFlags ToGravityFlags( this LayoutAlignment forms )
		{
			return forms switch
				   {
					   LayoutAlignment.Start => GravityFlags.Start | GravityFlags.CenterVertical,
					   LayoutAlignment.Center => GravityFlags.Center | GravityFlags.CenterVertical,
					   LayoutAlignment.End => GravityFlags.End | GravityFlags.CenterVertical,
					   LayoutAlignment.Fill => GravityFlags.Fill,
					   _ => GravityFlags.Fill
				   };
		}
		public static GravityFlags ToGravityFlags( this TextAlignment forms )
		{
			return forms switch
				   {
					   TextAlignment.Start => GravityFlags.Start | GravityFlags.CenterVertical,
					   TextAlignment.Center => GravityFlags.CenterHorizontal | GravityFlags.CenterVertical,
					   TextAlignment.End => GravityFlags.End | GravityFlags.CenterVertical,
					   _ => GravityFlags.Fill
				   };
		}
		public static ATextAlignment ToAndroidTextAlignment( this TextAlignment forms )
		{
			return forms switch
				   {
					   TextAlignment.Start => ATextAlignment.ViewStart,
					   TextAlignment.Center => ATextAlignment.Center,
					   TextAlignment.End => ATextAlignment.ViewEnd,
					   _ => ATextAlignment.Gravity
				   };
		}
	}
}
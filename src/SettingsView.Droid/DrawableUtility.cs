using Android.Content.Res;
using Android.Graphics.Drawables;

#nullable enable
namespace Jakar.SettingsView.Droid
{
	/// <summary>
	/// Drawable utility.
	/// </summary>
	[Android.Runtime.Preserve(AllMembers = true)]
	public static class DrawableUtility
	{
		/// <summary>
		/// Creates the ripple.
		/// </summary>
		/// <returns>The ripple.</returns>
		/// <param name="color">Color.</param>
		/// <param name="background">Background.</param>
		public static RippleDrawable CreateRipple( Android.Graphics.Color color, Drawable? background = null )
		{
			if ( background != null ) return new RippleDrawable(GetPressedColorSelector(color), background, null);
			var mask = new ColorDrawable(Android.Graphics.Color.White);
			return new RippleDrawable(GetPressedColorSelector(color), null, mask);

		}

		/// <summary>
		/// Gets the pressed color selector.
		/// </summary>
		/// <returns>The pressed color selector.</returns>
		/// <param name="pressedColor">Pressed color.</param>
		public static ColorStateList GetPressedColorSelector( int pressedColor )
		{
			return new(new[]
					   {
						   new int[]
						   { }
					   }, new[]
						  {
							  pressedColor,
						  });
		}
	}
}
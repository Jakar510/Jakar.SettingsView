// unset

using Xamarin.Forms;
using Xamarin.Forms.Internals;

#nullable enable
namespace Jakar.SettingsView.Shared.Config
{
	[Preserve(true, false)]
	public class FontConfig : IFontElement
	{
		public string? FontFamily { get; init; }
		public FontAttributes? Attributes { get; init; }
		public TextAlignment? Alignment { get; init; }
		public double FontSize { get; init; }
		FontAttributes IFontElement.FontAttributes => Attributes ?? FontAttributes.None;

		public FontConfig() : this(12) { }
		public FontConfig( Font font ) : this(font, TextAlignment.Center) { }
		public FontConfig( Font font, TextAlignment alignment ) : this(font.FontFamily, font.FontAttributes, alignment, font.FontSize) { }
		public FontConfig( double size ) : this(null, size) { }
		public FontConfig( string? family, double size ) : this(family, TextAlignment.Center, size) { }
		public FontConfig( string? family, TextAlignment alignment, double size ) : this(family, null, alignment, size) { }
		public FontConfig( string? family,
						   FontAttributes? attributes,
						   TextAlignment alignment,
						   double size )
		{
			FontFamily = family;
			Attributes = attributes;
			Alignment = alignment;
			FontSize = size;
		}


		public void OnFontFamilyChanged( string oldValue, string newValue ) { throw new System.NotImplementedException(); }
		public void OnFontSizeChanged( double oldValue, double newValue ) { throw new System.NotImplementedException(); }
		public double FontSizeDefaultValueCreator() => throw new System.NotImplementedException();
		public void OnFontAttributesChanged( FontAttributes oldValue, FontAttributes newValue ) { throw new System.NotImplementedException(); }
		public void OnFontChanged( Font oldValue, Font newValue ) { throw new System.NotImplementedException(); }
	}
}
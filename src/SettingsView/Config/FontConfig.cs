// unset

using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.Config
{
	public class FontConfig
	{
		public string? Family { get; init; }
		public FontAttributes? Attributes { get; init; }
		public TextAlignment? Alignment { get; init; }
		public int Size { get; init; }


		public FontConfig() : this(12) { }
		public FontConfig( int size ) : this(null, size) { }
		public FontConfig( string? family, int size ) : this(family, TextAlignment.Center, size) { }
		public FontConfig( string? family, TextAlignment alignment, int size ) : this(family, null, alignment, size) { }
		public FontConfig( string? family,
						   FontAttributes? attributes,
						   TextAlignment alignment,
						   int size )
		{
			Family = family;
			Attributes = attributes;
			Alignment = alignment;
			Size = size;
		}
	}
}
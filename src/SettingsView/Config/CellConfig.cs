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

	public class ItemConfig
	{
		public string? Text { get; init; }
		public Color Color { get; init; }
		public FontConfig Font { get; init; }


		public ItemConfig() : this(Color.Default) { }
		public ItemConfig( Color color ) : this(color, new FontConfig()) { }
		public ItemConfig( Color color, FontConfig font ) : this(null, color, font) { }
		public ItemConfig( string? text, Color color, FontConfig font )
		{
			Text = text;
			Color = color;
			Font = font;
		}
	}

	public class CellConfig
	{
		public Color BackgroundColor { get; init; }
		public Color AccentColor { get; init; }

		public ItemConfig? Title { get; init; }
		public ItemConfig? Description { get; init; }
		public ItemConfig? Hint { get; init; }
		public ItemConfig? Value { get; init; }

		public PopupConfig? Popup { get; init; }
		public CellConfig() { }
	}
}
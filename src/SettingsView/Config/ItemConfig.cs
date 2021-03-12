// unset

using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.Config
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
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
}
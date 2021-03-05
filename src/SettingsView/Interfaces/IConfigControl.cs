// unset

using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.Interfaces
{
	public interface IConfigControl
	{
		public Color Color { get; set; }

		[TypeConverter(typeof(FontSizeConverter))]
		public double FontSize { get; set; }

		public string? FontFamily { get; set; }
		public string? Text { get; set; }

		public FontAttributes FontAttributes { get; set; }

		public TextAlignment Alignment { get; set; }
	}
}
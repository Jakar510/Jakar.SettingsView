// unset

using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.Interfaces
{
	public interface ISectionFooterHeader
	{
		public string? Title { get; set; }
		public Color TitleColor { get; set; }
		public double FontSize { get; set; }
		public FontAttributes FontAttributes { get; set; }
		public string FontFamily { get; set; }

		public double HeightRequest { get; set; }
		public Thickness Padding { get; set; }
		public Color BackgroundColor { get; set; }
	}
}
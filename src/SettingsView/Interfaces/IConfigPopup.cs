// unset

using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Interfaces
{
	public interface IConfigPopup
	{
		public Color BackgroundColor { get; set; }

		// -----------------------------------------------------------------------------------

		public string Title { get; set; }

		public Color TitleColor { get; set; }

		[TypeConverter(typeof(FontSizeConverter))]
		public double TitleFontSize { get; set; }

		// -----------------------------------------------------------------------------------

		[TypeConverter(typeof(FontSizeConverter))]
		public double ItemFontSize { get; set; }

		public Color ItemColor { get; set; }

		// -----------------------------------------------------------------------------------

		public Color ItemDescriptionColor { get; set; }

		[TypeConverter(typeof(FontSizeConverter))]
		public double ItemDescriptionFontSize { get; set; }

		// -----------------------------------------------------------------------------------

		public Color AccentColor { get; set; }

		// -----------------------------------------------------------------------------------

		[TypeConverter(typeof(FontSizeConverter))]
		public double SelectedFontSize { get; set; }

		public Color SelectedColor { get; set; }

		public Color SeparatorColor { get; set; }

		// -----------------------------------------------------------------------------------

		public string Accept { get; set; }

		public string Cancel { get; set; }
	}
}
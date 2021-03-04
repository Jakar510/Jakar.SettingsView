// unset

using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.CellBase
{
	public class TitleCellBase : CellBase
	{
		public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(TitleCellBase), default(string));
		public static readonly BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor), typeof(Color), typeof(TitleCellBase), Color.Default);
		public static readonly BindableProperty TitleFontSizeProperty = BindableProperty.Create(nameof(TitleFontSize), typeof(double), typeof(TitleCellBase), -1.0);
		public static readonly BindableProperty TitleFontFamilyProperty = BindableProperty.Create(nameof(TitleFontFamily), typeof(string), typeof(TitleCellBase), default(string));
		public static readonly BindableProperty TitleFontAttributesProperty = BindableProperty.Create(nameof(TitleFontAttributes), typeof(FontAttributes?), typeof(TitleCellBase));
		public static readonly BindableProperty TitleAlignmentProperty = BindableProperty.Create(nameof(TitleAlignment), typeof(TextAlignment?), typeof(TitleCellBase));

		public string Title
		{
			get => (string) GetValue(TitleProperty);
			set => SetValue(TitleProperty, value);
		}

		public Color TitleColor
		{
			get => (Color) GetValue(TitleColorProperty);
			set => SetValue(TitleColorProperty, value);
		}

		[TypeConverter(typeof(FontSizeConverter))]
		public double TitleFontSize
		{
			get => (double) GetValue(TitleFontSizeProperty);
			set => SetValue(TitleFontSizeProperty, value);
		}

		public string TitleFontFamily
		{
			get => (string) GetValue(TitleFontFamilyProperty);
			set => SetValue(TitleFontFamilyProperty, value);
		}

		public FontAttributes? TitleFontAttributes
		{
			get => (FontAttributes?) GetValue(TitleFontAttributesProperty);
			set => SetValue(TitleFontAttributesProperty, value);
		}
		public TextAlignment? TitleAlignment
		{
			get => (TextAlignment?) GetValue(TitleAlignmentProperty);
			set => SetValue(TitleAlignmentProperty, value);
		}
	}
}
// unset

using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.CellBase
{
	public class DescriptionCellBase : IconCellBase
	{
		public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(nameof(Description), typeof(string), typeof(DescriptionCellBase), default(string));
		public static readonly BindableProperty DescriptionColorProperty = BindableProperty.Create(nameof(DescriptionColor), typeof(Color), typeof(DescriptionCellBase), Color.Default);
		public static readonly BindableProperty DescriptionFontSizeProperty = BindableProperty.Create(nameof(DescriptionFontSize), typeof(float), typeof(DescriptionCellBase), -1.0f);
		public static readonly BindableProperty DescriptionFontFamilyProperty = BindableProperty.Create(nameof(DescriptionFontFamily), typeof(string), typeof(DescriptionCellBase), default(string));
		public static readonly BindableProperty DescriptionFontAttributesProperty = BindableProperty.Create(nameof(DescriptionFontAttributes), typeof(FontAttributes?), typeof(DescriptionCellBase));
		public static readonly BindableProperty DescriptionAlignmentProperty = BindableProperty.Create(nameof(DescriptionAlignment), typeof(TextAlignment?), typeof(DescriptionCellBase));

		public string Description
		{
			get => (string) GetValue(DescriptionProperty);
			set => SetValue(DescriptionProperty, value);
		}

		public Color DescriptionColor
		{
			get => (Color) GetValue(DescriptionColorProperty);
			set => SetValue(DescriptionColorProperty, value);
		}

		[TypeConverter(typeof(FontSizeConverter))]
		public float DescriptionFontSize
		{
			get => (float) GetValue(DescriptionFontSizeProperty);
			set => SetValue(DescriptionFontSizeProperty, value);
		}

		public string DescriptionFontFamily
		{
			get => (string) GetValue(DescriptionFontFamilyProperty);
			set => SetValue(DescriptionFontFamilyProperty, value);
		}

		public FontAttributes? DescriptionFontAttributes
		{
			get => (FontAttributes?) GetValue(DescriptionFontAttributesProperty);
			set => SetValue(DescriptionFontAttributesProperty, value);
		}

		public TextAlignment? DescriptionAlignment
		{
			get => (TextAlignment?) GetValue(DescriptionAlignmentProperty);
			set => SetValue(DescriptionAlignmentProperty, value);
		}
	}
}
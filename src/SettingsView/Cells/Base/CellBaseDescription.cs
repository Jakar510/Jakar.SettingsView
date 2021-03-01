// unset

using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.Cells.Base
{
	public class CellBaseDescription : CellBaseIcon
	{
		public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(nameof(Description), typeof(string), typeof(CellBaseDescription), default(string));
		public static readonly BindableProperty DescriptionColorProperty = BindableProperty.Create(nameof(DescriptionColor), typeof(Color), typeof(CellBaseDescription), Color.Default);
		public static readonly BindableProperty DescriptionFontSizeProperty = BindableProperty.Create(nameof(DescriptionFontSize), typeof(double), typeof(CellBaseDescription), -1.0d);
		public static readonly BindableProperty DescriptionFontFamilyProperty = BindableProperty.Create(nameof(DescriptionFontFamily), typeof(string), typeof(CellBaseDescription), default(string));
		public static readonly BindableProperty DescriptionFontAttributesProperty = BindableProperty.Create(nameof(DescriptionFontAttributes), typeof(FontAttributes?), typeof(CellBaseDescription));
		public static readonly BindableProperty DescriptionAlignmentProperty = BindableProperty.Create(nameof(DescriptionAlignment), typeof(TextAlignment?), typeof(CellBaseDescription));

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
		public double DescriptionFontSize
		{
			get => (double) GetValue(DescriptionFontSizeProperty);
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
// unset

using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells.Base
{
	public class CellBaseDescription : CellBaseIcon
	{
		public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(nameof(Description), typeof(string), typeof(CellBase), default(string), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty DescriptionColorProperty = BindableProperty.Create(nameof(DescriptionColor), typeof(Color), typeof(CellBase), default(Color), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty DescriptionFontSizeProperty = BindableProperty.Create(nameof(DescriptionFontSize), typeof(double), typeof(CellBase), -1.0d, defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty DescriptionFontFamilyProperty = BindableProperty.Create(nameof(DescriptionFontFamily), typeof(string), typeof(CellBase), default(string), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty DescriptionFontAttributesProperty = BindableProperty.Create(nameof(DescriptionFontAttributes), typeof(FontAttributes?), typeof(CellBase), null, defaultBindingMode: BindingMode.OneWay);

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
	}
}
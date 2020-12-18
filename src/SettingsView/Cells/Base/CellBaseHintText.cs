// unset

using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells.Base
{
	public class CellBaseHintText : CellBaseDescription
	{
		public static readonly BindableProperty HintTextProperty = BindableProperty.Create(nameof(HintText), typeof(string), typeof(CellBase), default(string), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty HintTextColorProperty = BindableProperty.Create(nameof(HintTextColor), typeof(Color), typeof(CellBase), default(Color), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty HintFontSizeProperty = BindableProperty.Create(nameof(HintFontSize), typeof(double), typeof(CellBase), -1.0d, defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty HintFontFamilyProperty = BindableProperty.Create(nameof(HintFontFamily), typeof(string), typeof(CellBase), default(string), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty HintFontAttributesProperty = BindableProperty.Create(nameof(HintFontAttributes), typeof(FontAttributes?), typeof(CellBase), null, defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty HintColorProperty = BindableProperty.Create(nameof(HintColor), typeof(Color), typeof(CellBase), default(Color), defaultBindingMode: BindingMode.OneWay);

		public string HintText
		{
			get => (string) GetValue(HintTextProperty);
			set => SetValue(HintTextProperty, value);
		}

		public Color HintTextColor
		{
			get => (Color) GetValue(HintTextColorProperty);
			set => SetValue(HintTextColorProperty, value);
		}

		[TypeConverter(typeof(FontSizeConverter))]
		public double HintFontSize
		{
			get => (double) GetValue(HintFontSizeProperty);
			set => SetValue(HintFontSizeProperty, value);
		}

		public string HintFontFamily
		{
			get => (string) GetValue(HintFontFamilyProperty);
			set => SetValue(HintFontFamilyProperty, value);
		}

		public FontAttributes? HintFontAttributes
		{
			get => (FontAttributes?) GetValue(HintFontAttributesProperty);
			set => SetValue(HintFontAttributesProperty, value);
		}

		public Color HintColor
		{
			get => (Color) GetValue(HintColorProperty);
			set => SetValue(HintColorProperty, value);
		}
	}
}
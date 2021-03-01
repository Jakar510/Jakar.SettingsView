// unset

using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells.Base
{
	public class CellBaseHintText : CellBaseDescription
	{
		public static readonly BindableProperty HintProperty = BindableProperty.Create(nameof(Hint), typeof(string), typeof(CellBase), default(string));
		public static readonly BindableProperty HintColorProperty = BindableProperty.Create(nameof(HintColor), typeof(Color), typeof(CellBase), Color.Default);
		public static readonly BindableProperty HintFontSizeProperty = BindableProperty.Create(nameof(HintFontSize), typeof(double), typeof(CellBase), -1.0d);
		public static readonly BindableProperty HintFontFamilyProperty = BindableProperty.Create(nameof(HintFontFamily), typeof(string), typeof(CellBase), default(string));
		public static readonly BindableProperty HintFontAttributesProperty = BindableProperty.Create(nameof(HintFontAttributes), typeof(FontAttributes?), typeof(CellBase));

		public string Hint
		{
			get => (string) GetValue(HintProperty);
			set => SetValue(HintProperty, value);
		}

		public Color HintColor
		{
			get => (Color) GetValue(HintColorProperty);
			set => SetValue(HintColorProperty, value);
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
	}
}
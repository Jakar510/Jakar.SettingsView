// unset

using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.CellBase
{
	public class ValueCellBase : HintTextCellBase
	{
		public static readonly BindableProperty ValueTextColorProperty = BindableProperty.Create(nameof(ValueTextColor), typeof(Color), typeof(ValueTextCellBase), Color.Default);
		public static readonly BindableProperty ValueTextFontSizeProperty = BindableProperty.Create(nameof(ValueTextFontSize), typeof(double), typeof(ValueTextCellBase), -1.0d);
		public static readonly BindableProperty ValueTextFontFamilyProperty = BindableProperty.Create(nameof(ValueTextFontFamily), typeof(string), typeof(ValueTextCellBase), default(string));
		public static readonly BindableProperty ValueTextFontAttributesProperty = BindableProperty.Create(nameof(ValueTextFontAttributes), typeof(FontAttributes?), typeof(ValueTextCellBase));
		public static readonly BindableProperty ValueTextAlignmentProperty = BindableProperty.Create(nameof(ValueTextAlignment), typeof(TextAlignment?), typeof(ValueTextCellBase));


		public Color ValueTextColor
		{
			get => (Color) GetValue(ValueTextColorProperty);
			set => SetValue(ValueTextColorProperty, value);
		}

		[TypeConverter(typeof(FontSizeConverter))]
		public double ValueTextFontSize
		{
			get => (double) GetValue(ValueTextFontSizeProperty);
			set => SetValue(ValueTextFontSizeProperty, value);
		}

		public string ValueTextFontFamily
		{
			get => (string) GetValue(ValueTextFontFamilyProperty);
			set => SetValue(ValueTextFontFamilyProperty, value);
		}

		public FontAttributes? ValueTextFontAttributes
		{
			get => (FontAttributes?) GetValue(ValueTextFontAttributesProperty);
			set => SetValue(ValueTextFontAttributesProperty, value);
		}

		public TextAlignment? ValueTextAlignment
		{
			get => (TextAlignment?) GetValue(ValueTextAlignmentProperty);
			set => SetValue(ValueTextAlignmentProperty, value);
		}
	}
}
// unset

using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Converters;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.CellBase
{
	public abstract class ValueCellBase : HintTextCellBase
	{
		public static readonly BindableProperty ValueTextColorProperty = BindableProperty.Create(nameof(ValueTextColor), typeof(Color?), typeof(ValueTextCellBase), SVConstants.Cell.COLOR);
		public static readonly BindableProperty ValueTextFontSizeProperty = BindableProperty.Create(nameof(ValueTextFontSize), typeof(double?), typeof(ValueTextCellBase), SVConstants.Cell.FONT_SIZE);
		public static readonly BindableProperty ValueTextFontFamilyProperty = BindableProperty.Create(nameof(ValueTextFontFamily), typeof(string), typeof(ValueTextCellBase), default(string));
		public static readonly BindableProperty ValueTextFontAttributesProperty = BindableProperty.Create(nameof(ValueTextFontAttributes), typeof(FontAttributes?), typeof(ValueTextCellBase));
		public static readonly BindableProperty ValueTextAlignmentProperty = BindableProperty.Create(nameof(ValueTextAlignment), typeof(TextAlignment?), typeof(ValueTextCellBase));


		// [TypeConverter(typeof(NullableColorTypeConverter))]
		public Color ValueTextColor
		{
			get => (Color) GetValue(ValueTextColorProperty);
			set => SetValue(ValueTextColorProperty, value);
		}

		[TypeConverter(typeof(NullableFontSizeConverter))]
		public double? ValueTextFontSize
		{
			get => (double?) GetValue(ValueTextFontSizeProperty);
			set => SetValue(ValueTextFontSizeProperty, value);
		}

		public string? ValueTextFontFamily
		{
			get => (string?) GetValue(ValueTextFontFamilyProperty);
			set => SetValue(ValueTextFontFamilyProperty, value);
		}

		[TypeConverter(typeof(FontAttributesConverter))]
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


		// internal string? GetValueTextFontFamily() => ValueTextFontFamily ?? Parent.CellValueTextFontFamily;
		// internal FontAttributes GetValueTextFontAttributes() => ValueTextFontAttributes ?? Parent.CellValueTextFontAttributes;
		// internal TextAlignment GetValueTextTextAlignment() => ValueTextAlignment ?? Parent.CellValueTextAlignment;
		// internal Color GetValueTextColor() => ValueTextColor ?? Parent.CellValueTextColor;
		// internal double GetValueTextFontSize() => ValueTextFontSize ?? Parent.CellValueTextFontSize;


		private ValueTextConfiguration? _config;

		internal ValueTextConfiguration ValueTextConfig
		{
			get
			{
				_config ??= new ValueTextConfiguration(this);
				return _config;
			}
		}

		internal class ValueTextConfiguration
		{
			private readonly ValueCellBase _cell;
			public ValueTextConfiguration( ValueCellBase cell ) => _cell = cell;

			internal string? FontFamily => _cell.ValueTextFontFamily ?? _cell.Parent.CellValueTextFontFamily;
			internal FontAttributes FontAttributes => _cell.ValueTextFontAttributes ?? _cell.Parent.CellValueTextFontAttributes;
			internal TextAlignment TextAlignment => _cell.ValueTextAlignment ?? _cell.Parent.CellValueTextAlignment;

			internal Color Color =>
				_cell.ValueTextColor == SVConstants.Cell.COLOR
					? _cell.Parent.CellValueTextColor
					: _cell.ValueTextColor;

			internal double FontSize => _cell.ValueTextFontSize ?? _cell.Parent.CellValueTextFontSize;
		}
	}
}
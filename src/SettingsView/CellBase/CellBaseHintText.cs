// unset

using Jakar.Api.Converters;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Converters;
using Jakar.SettingsView.Shared.Interfaces;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.CellBase
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public class HintTextCellBase : DescriptionCellBase
	{
		public static readonly BindableProperty HintProperty = BindableProperty.Create(nameof(Hint), typeof(string), typeof(HintTextCellBase), default(string));
		public static readonly BindableProperty HintColorProperty = BindableProperty.Create(nameof(HintColor), typeof(Color?), typeof(HintTextCellBase), SvConstants.Cell.color);
		public static readonly BindableProperty HintFontSizeProperty = BindableProperty.Create(nameof(HintFontSize), typeof(double?), typeof(HintTextCellBase), SvConstants.Cell.font_Size);
		public static readonly BindableProperty HintFontFamilyProperty = BindableProperty.Create(nameof(HintFontFamily), typeof(string), typeof(HintTextCellBase), default(string));
		public static readonly BindableProperty HintFontAttributesProperty = BindableProperty.Create(nameof(HintFontAttributes), typeof(FontAttributes?), typeof(HintTextCellBase));
		public static readonly BindableProperty HintAlignmentProperty = BindableProperty.Create(nameof(HintAlignment), typeof(TextAlignment?), typeof(HintTextCellBase));

		public string? Hint
		{
			get => (string?) GetValue(HintProperty);
			set => SetValue(HintProperty, value);
		}

		// [TypeConverter(typeof(NullableColorTypeConverter))]
		public Color HintColor
		{
			get => (Color) GetValue(HintColorProperty);
			set => SetValue(HintColorProperty, value);
		}

		[TypeConverter(typeof(NullableFontSizeConverter))]
		public double? HintFontSize
		{
			get => (double?) GetValue(HintFontSizeProperty);
			set => SetValue(HintFontSizeProperty, value);
		}

		public string? HintFontFamily
		{
			get => (string?) GetValue(HintFontFamilyProperty);
			set => SetValue(HintFontFamilyProperty, value);
		}

		[TypeConverter(typeof(FontAttributesConverter))]
		public FontAttributes? HintFontAttributes
		{
			get => (FontAttributes?) GetValue(HintFontAttributesProperty);
			set => SetValue(HintFontAttributesProperty, value);
		}

		public TextAlignment? HintAlignment
		{
			get => (TextAlignment?) GetValue(HintAlignmentProperty);
			set => SetValue(HintAlignmentProperty, value);
		}


		// internal string? GetHintFontFamily() => HintFontFamily ?? Parent.CellHintFontFamily;
		// internal FontAttributes GetHintFontAttributes() => HintFontAttributes ?? Parent.CellTitleFontAttributes;
		// internal TextAlignment GetHintTextAlignment() => HintAlignment ?? Parent.CellHintAlignment;
		// internal Color GetHintColor() => HintColor ?? Parent.CellHintTextColor;
		// internal double GetHintFontSize() => HintFontSize ?? Parent.CellHintFontSize;


		private IUseConfiguration? _config;

		protected internal IUseConfiguration HintConfig
		{
			get
			{
				_config ??= new HintConfiguration(this);
				return _config;
			}
		}



		public sealed class HintConfiguration : IUseConfiguration
		{
			private readonly HintTextCellBase _cell;
			public HintConfiguration( HintTextCellBase cell ) => _cell = cell;

			public string? FontFamily => _cell.HintFontFamily ?? _cell.Parent.CellHintFontFamily;
			public FontAttributes FontAttributes => _cell.HintFontAttributes ?? _cell.Parent.CellHintFontAttributes;
			public TextAlignment TextAlignment => _cell.HintAlignment ?? _cell.Parent.CellHintAlignment;

			public Color Color =>
				_cell.HintColor == SvConstants.Cell.color
					? _cell.Parent.CellHintTextColor
					: _cell.HintColor;

			public double FontSize => _cell.HintFontSize ?? _cell.Parent.CellHintFontSize;
		}
	}
}
// unset

using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Converters;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.CellBase
{
	public abstract class TitleCellBase : CellBase
	{
		public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(TitleCellBase), default(string));
		public static readonly BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor), typeof(Color), typeof(TitleCellBase), SVConstants.Cell.COLOR);
		public static readonly BindableProperty TitleFontSizeProperty = BindableProperty.Create(nameof(TitleFontSize), typeof(double?), typeof(TitleCellBase), SVConstants.Cell.FONT_SIZE);
		public static readonly BindableProperty TitleFontFamilyProperty = BindableProperty.Create(nameof(TitleFontFamily), typeof(string), typeof(TitleCellBase), default(string));
		public static readonly BindableProperty TitleFontAttributesProperty = BindableProperty.Create(nameof(TitleFontAttributes), typeof(FontAttributes?), typeof(TitleCellBase));
		public static readonly BindableProperty TitleAlignmentProperty = BindableProperty.Create(nameof(TitleAlignment), typeof(TextAlignment?), typeof(TitleCellBase));

		public string? Title
		{
			get => (string?) GetValue(TitleProperty);
			set => SetValue(TitleProperty, value);
		}

		// [TypeConverter(typeof(NullableColorTypeConverter))]
		public Color TitleColor
		{
			get => (Color) GetValue(TitleColorProperty);
			set => SetValue(TitleColorProperty, value);
		}

		[TypeConverter(typeof(NullableFontSizeConverter))]
		public double? TitleFontSize
		{
			get => (double?) GetValue(TitleFontSizeProperty);
			set => SetValue(TitleFontSizeProperty, value);
		}

		public string? TitleFontFamily
		{
			get => (string?) GetValue(TitleFontFamilyProperty);
			set => SetValue(TitleFontFamilyProperty, value);
		}

		[TypeConverter(typeof(FontAttributesConverter))]
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


		// internal string? GetTitleFontFamily() => TitleFontFamily ?? Parent.CellTitleFontFamily;
		// internal FontAttributes GetTitleFontAttributes() => TitleFontAttributes ?? Parent.CellTitleFontAttributes;
		// internal TextAlignment GetTitleTextAlignment() => TitleAlignment ?? Parent.CellTitleAlignment;
		// internal Color GetTitleColor() => TitleColor ?? Parent.CellTitleColor;
		// internal double GetTitleFontSize() => TitleFontSize ?? Parent.CellTitleFontSize;


		private TitleConfiguration? _config;

		internal TitleConfiguration TitleConfig
		{
			get
			{
				_config ??= new TitleConfiguration(this);
				return _config;
			}
		}

		internal class TitleConfiguration
		{
			private readonly TitleCellBase _cell;
			public TitleConfiguration( TitleCellBase cell ) => _cell = cell;

			internal string? FontFamily => _cell.TitleFontFamily ?? _cell.Parent.CellTitleFontFamily;
			internal FontAttributes FontAttributes => _cell.TitleFontAttributes ?? _cell.Parent.CellTitleFontAttributes;
			internal TextAlignment TextAlignment => _cell.TitleAlignment ?? _cell.Parent.CellTitleAlignment;

			internal Color Color =>
				_cell.TitleColor == SVConstants.Cell.COLOR
					? _cell.Parent.CellTitleColor
					: _cell.TitleColor;

			internal double FontSize => _cell.TitleFontSize ?? _cell.Parent.CellTitleFontSize;
		}
	}
}
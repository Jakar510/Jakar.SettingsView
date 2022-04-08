// unset


namespace Jakar.SettingsView.Shared.CellBase;

[Xamarin.Forms.Internals.Preserve(true, false)]
public abstract class TitleCellBase : CellBase
{
    public static readonly BindableProperty titleProperty               = BindableProperty.Create(nameof(Title),               typeof(string), typeof(TitleCellBase));
    public static readonly BindableProperty titleColorProperty          = BindableProperty.Create(nameof(TitleColor),          typeof(Color), typeof(TitleCellBase), SvConstants.Cell.color);
    public static readonly BindableProperty titleFontSizeProperty       = BindableProperty.Create(nameof(TitleFontSize),       typeof(double?), typeof(TitleCellBase), SvConstants.Cell.font_Size);
    public static readonly BindableProperty titleFontFamilyProperty     = BindableProperty.Create(nameof(TitleFontFamily),     typeof(string), typeof(TitleCellBase));
    public static readonly BindableProperty titleFontAttributesProperty = BindableProperty.Create(nameof(TitleFontAttributes), typeof(FontAttributes?), typeof(TitleCellBase));
    public static readonly BindableProperty titleAlignmentProperty      = BindableProperty.Create(nameof(TitleAlignment),      typeof(TextAlignment?), typeof(TitleCellBase));

    public string? Title
    {
        get => (string?) GetValue(titleProperty);
        set => SetValue(titleProperty, value);
    }

    [TypeConverter(typeof(ColorTypeConverter))]
    public Color TitleColor
    {
        get => (Color) GetValue(titleColorProperty);
        set => SetValue(titleColorProperty, value);
    }

    [TypeConverter(typeof(NullableFontSizeConverter))]
    public double? TitleFontSize
    {
        get => (double?) GetValue(titleFontSizeProperty);
        set => SetValue(titleFontSizeProperty, value);
    }

    public string? TitleFontFamily
    {
        get => (string?) GetValue(titleFontFamilyProperty);
        set => SetValue(titleFontFamilyProperty, value);
    }

    [TypeConverter(typeof(FontAttributesConverter))]
    public FontAttributes? TitleFontAttributes
    {
        get => (FontAttributes?) GetValue(titleFontAttributesProperty);
        set => SetValue(titleFontAttributesProperty, value);
    }

    [TypeConverter(typeof(NullableTextAlignmentConverter))]
    public TextAlignment? TitleAlignment
    {
        get => (TextAlignment?) GetValue(titleAlignmentProperty);
        set => SetValue(titleAlignmentProperty, value);
    }


    // internal string? GetTitleFontFamily() => TitleFontFamily ?? Parent.CellTitleFontFamily;
    // internal FontAttributes GetTitleFontAttributes() => TitleFontAttributes ?? Parent.CellTitleFontAttributes;
    // internal TextAlignment GetTitleTextAlignment() => TitleAlignment ?? Parent.CellTitleAlignment;
    // internal Color GetTitleColor() => TitleColor ?? Parent.CellTitleColor;
    // internal double GetTitleFontSize() => TitleFontSize ?? Parent.CellTitleFontSize;


    private IUseConfiguration? _config;

    protected internal IUseConfiguration TitleConfig
    {
        get
        {
            _config ??= new TitleConfiguration(this);
            return _config;
        }
    }



    public sealed class TitleConfiguration : IUseConfiguration
    {
        private readonly TitleCellBase _cell;
        public TitleConfiguration( TitleCellBase cell ) => _cell = cell;

        public string?        FontFamily     => _cell.TitleFontFamily ?? _cell.Parent.CellTitleFontFamily;
        public FontAttributes FontAttributes => _cell.TitleFontAttributes ?? _cell.Parent.CellTitleFontAttributes;
        public TextAlignment  TextAlignment  => _cell.TitleAlignment ?? _cell.Parent.CellTitleAlignment;

        public Color Color =>
            _cell.TitleColor == SvConstants.Cell.color
                ? _cell.Parent.CellTitleColor
                : _cell.TitleColor;

        public double FontSize => _cell.TitleFontSize ?? _cell.Parent.CellTitleFontSize;
    }
}
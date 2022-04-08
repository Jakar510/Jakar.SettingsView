// unset




namespace Jakar.SettingsView.Shared.CellBase;

[Xamarin.Forms.Internals.Preserve(true, false)]
public class HintTextCellBase : DescriptionCellBase
{
    public static readonly BindableProperty hintProperty               = BindableProperty.Create(nameof(Hint),               typeof(string), typeof(HintTextCellBase));
    public static readonly BindableProperty hintColorProperty          = BindableProperty.Create(nameof(HintColor),          typeof(Color?), typeof(HintTextCellBase), SvConstants.Cell.color);
    public static readonly BindableProperty hintFontSizeProperty       = BindableProperty.Create(nameof(HintFontSize),       typeof(double?), typeof(HintTextCellBase), SvConstants.Cell.font_Size);
    public static readonly BindableProperty hintFontFamilyProperty     = BindableProperty.Create(nameof(HintFontFamily),     typeof(string), typeof(HintTextCellBase));
    public static readonly BindableProperty hintFontAttributesProperty = BindableProperty.Create(nameof(HintFontAttributes), typeof(FontAttributes?), typeof(HintTextCellBase));
    public static readonly BindableProperty hintAlignmentProperty      = BindableProperty.Create(nameof(HintAlignment),      typeof(TextAlignment?), typeof(HintTextCellBase));

    public string? Hint
    {
        get => (string?)GetValue(hintProperty);
        set => SetValue(hintProperty, value);
    }

    [TypeConverter(typeof(ColorTypeConverter))]
    public Color HintColor
    {
        get => (Color)GetValue(hintColorProperty);
        set => SetValue(hintColorProperty, value);
    }

    [TypeConverter(typeof(NullableFontSizeConverter))]
    public double? HintFontSize
    {
        get => (double?)GetValue(hintFontSizeProperty);
        set => SetValue(hintFontSizeProperty, value);
    }

    public string? HintFontFamily
    {
        get => (string?)GetValue(hintFontFamilyProperty);
        set => SetValue(hintFontFamilyProperty, value);
    }

    [TypeConverter(typeof(NullableFontAttributesConverter))]
    public FontAttributes? HintFontAttributes
    {
        get => (FontAttributes?)GetValue(hintFontAttributesProperty);
        set => SetValue(hintFontAttributesProperty, value);
    }

    [TypeConverter(typeof(NullableTextAlignmentConverter))]
    public TextAlignment? HintAlignment
    {
        get => (TextAlignment?)GetValue(hintAlignmentProperty);
        set => SetValue(hintAlignmentProperty, value);
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

        public string?        FontFamily     => _cell.HintFontFamily ?? _cell.Parent.CellHintFontFamily;
        public FontAttributes FontAttributes => _cell.HintFontAttributes ?? _cell.Parent.CellHintFontAttributes;
        public TextAlignment  TextAlignment  => _cell.HintAlignment ?? _cell.Parent.CellHintAlignment;

        public Color Color =>
            _cell.HintColor == SvConstants.Cell.color
                ? _cell.Parent.CellHintTextColor
                : _cell.HintColor;

        public double FontSize => _cell.HintFontSize ?? _cell.Parent.CellHintFontSize;
    }
}
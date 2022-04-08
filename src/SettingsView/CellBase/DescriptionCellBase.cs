// unset


namespace Jakar.SettingsView.Shared.CellBase;

[Xamarin.Forms.Internals.Preserve(true, false)]
public abstract class DescriptionCellBase : IconCellBase
{
    public static readonly BindableProperty descriptionProperty      = BindableProperty.Create(nameof(Description),      typeof(string), typeof(DescriptionCellBase));
    public static readonly BindableProperty descriptionColorProperty = BindableProperty.Create(nameof(DescriptionColor), typeof(Color?), typeof(DescriptionCellBase), SvConstants.Cell.color);

    public static readonly BindableProperty descriptionFontSizeProperty =
        BindableProperty.Create(nameof(DescriptionFontSize), typeof(double?), typeof(DescriptionCellBase), SvConstants.Cell.font_Size);

    public static readonly BindableProperty descriptionFontFamilyProperty     = BindableProperty.Create(nameof(DescriptionFontFamily), typeof(string), typeof(DescriptionCellBase));
    public static readonly BindableProperty descriptionFontAttributesProperty = BindableProperty.Create(nameof(DescriptionFontAttributes), typeof(FontAttributes?), typeof(DescriptionCellBase));
    public static readonly BindableProperty descriptionAlignmentProperty      = BindableProperty.Create(nameof(DescriptionAlignment), typeof(TextAlignment?), typeof(DescriptionCellBase));

    public string? Description
    {
        get => (string?) GetValue(descriptionProperty);
        set => SetValue(descriptionProperty, value);
    }

    [TypeConverter(typeof(ColorTypeConverter))]
    public Color DescriptionColor
    {
        get => (Color) GetValue(descriptionColorProperty);
        set => SetValue(descriptionColorProperty, value);
    }


    [TypeConverter(typeof(NullableFontSizeConverter))]
    public double? DescriptionFontSize
    {
        get => (double?) GetValue(descriptionFontSizeProperty);
        set => SetValue(descriptionFontSizeProperty, value);
    }

    public string? DescriptionFontFamily
    {
        get => (string?) GetValue(descriptionFontFamilyProperty);
        set => SetValue(descriptionFontFamilyProperty, value);
    }

    [TypeConverter(typeof(FontAttributesConverter))]
    public FontAttributes? DescriptionFontAttributes
    {
        get => (FontAttributes?) GetValue(descriptionFontAttributesProperty);
        set => SetValue(descriptionFontAttributesProperty, value);
    }
		
    [TypeConverter(typeof(NullableTextAlignmentConverter))]
    public TextAlignment? DescriptionAlignment
    {
        get => (TextAlignment?) GetValue(descriptionAlignmentProperty);
        set => SetValue(descriptionAlignmentProperty, value);
    }

    // internal string? GetDescriptionFontFamily() => DescriptionFontFamily ?? Parent.CellDescriptionFontFamily;
    // internal FontAttributes GetDescriptionFontAttributes() => DescriptionFontAttributes ?? Parent.CellTitleFontAttributes;
    // internal TextAlignment GetDescriptionTextAlignment() => DescriptionAlignment ?? Parent.CellDescriptionAlignment;
    // internal Color GetDescriptionColor() => DescriptionColor ?? Parent.CellDescriptionColor;
    // internal double GetDescriptionFontSize() => DescriptionFontSize ?? Parent.CellDescriptionFontSize;


    private DescriptionConfiguration? _config;

    protected internal DescriptionConfiguration DescriptionConfig
    {
        get
        {
            _config ??= new DescriptionConfiguration(this);
            return _config;
        }
    }



    public sealed class DescriptionConfiguration : IUseConfiguration
    {
        private readonly DescriptionCellBase _cell;
        public DescriptionConfiguration( DescriptionCellBase cell ) => _cell = cell;

        public string?        FontFamily     => _cell.DescriptionFontFamily ?? _cell.Parent.CellDescriptionFontFamily;
        public FontAttributes FontAttributes => _cell.DescriptionFontAttributes ?? _cell.Parent.CellDescriptionFontAttributes;
        public TextAlignment  TextAlignment  => _cell.DescriptionAlignment ?? _cell.Parent.CellDescriptionAlignment;

        public Color Color =>
            _cell.DescriptionColor == SvConstants.Cell.color
                ? _cell.Parent.CellDescriptionColor
                : _cell.DescriptionColor;

        public double FontSize => _cell.DescriptionFontSize ?? _cell.Parent.CellDescriptionFontSize;
    }
}
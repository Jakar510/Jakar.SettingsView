// unset

namespace Jakar.SettingsView.Shared.CellBase;

[Xamarin.Forms.Internals.Preserve(true, false)]
public abstract class ValueCellBase : HintTextCellBase
{
    public static readonly BindableProperty valueTextColorProperty = BindableProperty.Create(nameof(ValueTextColor), typeof(Color?), typeof(ValueTextCellBase), SvConstants.Cell.color);
    public static readonly BindableProperty valueTextFontSizeProperty = BindableProperty.Create(nameof(ValueTextFontSize), typeof(double?), typeof(ValueTextCellBase), SvConstants.Cell.font_Size);
    public static readonly BindableProperty valueTextFontFamilyProperty = BindableProperty.Create(nameof(ValueTextFontFamily), typeof(string), typeof(ValueTextCellBase));
    public static readonly BindableProperty valueTextFontAttributesProperty = BindableProperty.Create(nameof(ValueTextFontAttributes), typeof(FontAttributes?), typeof(ValueTextCellBase));
    public static readonly BindableProperty valueTextAlignmentProperty = BindableProperty.Create(nameof(ValueTextAlignment), typeof(TextAlignment?), typeof(ValueTextCellBase));


    [TypeConverter(typeof(ColorTypeConverter))]
    public Color ValueTextColor
    {
        get => (Color) GetValue(valueTextColorProperty);
        set => SetValue(valueTextColorProperty, value);
    }

    [TypeConverter(typeof(NullableFontSizeConverter))]
    public double? ValueTextFontSize
    {
        get => (double?) GetValue(valueTextFontSizeProperty);
        set => SetValue(valueTextFontSizeProperty, value);
    }

    public string? ValueTextFontFamily
    {
        get => (string?) GetValue(valueTextFontFamilyProperty);
        set => SetValue(valueTextFontFamilyProperty, value);
    }

    [TypeConverter(typeof(FontAttributesConverter))]
    public FontAttributes? ValueTextFontAttributes
    {
        get => (FontAttributes?) GetValue(valueTextFontAttributesProperty);
        set => SetValue(valueTextFontAttributesProperty, value);
    }

    [TypeConverter(typeof(NullableTextAlignmentConverter))]
    public TextAlignment? ValueTextAlignment
    {
        get => (TextAlignment?) GetValue(valueTextAlignmentProperty);
        set => SetValue(valueTextAlignmentProperty, value);
    }


    private IUseConfiguration? _config;

    protected internal IUseConfiguration ValueTextConfig
    {
        get
        {
            _config ??= new ValueTextConfiguration(this);
            return _config;
        }
    }



    public sealed class ValueTextConfiguration : IUseConfiguration
    {
        private readonly ValueCellBase _cell;
        public ValueTextConfiguration( ValueCellBase cell ) => _cell = cell;

        public string?        FontFamily     => _cell.ValueTextFontFamily ?? _cell.Parent.CellValueTextFontFamily;
        public FontAttributes FontAttributes => _cell.ValueTextFontAttributes ?? _cell.Parent.CellValueTextFontAttributes;
        public TextAlignment  TextAlignment  => _cell.ValueTextAlignment ?? _cell.Parent.CellValueTextAlignment;

        public Color Color =>
            _cell.ValueTextColor == SvConstants.Cell.color
                ? _cell.Parent.CellValueTextColor
                : _cell.ValueTextColor;

        public double FontSize => _cell.ValueTextFontSize ?? _cell.Parent.CellValueTextFontSize;
    }
}



public abstract class ValueCellBase<TValue> : ValueCellBase, IValueChanged<TValue>
{
    public event EventHandler<SVValueChangedEventArgs<TValue>>? ValueChanged;

    void IValueChanged<TValue>.SendValueChanged( TValue              value ) => ValueChanged?.Invoke(this, new SVValueChangedEventArgs<TValue>(value));
    void IValueChanged<TValue>.SendValueChanged( IEnumerable<TValue> value ) => ValueChanged?.Invoke(this, new SVValueChangedEventArgs<TValue>(value));


    internal IValueChanged<TValue> ValueChangedHandler => this;
}
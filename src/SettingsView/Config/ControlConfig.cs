// unset

namespace Jakar.SettingsView.Shared.Config;

[Xamarin.Forms.Internals.Preserve(true, false)]
public class ControlConfig : SvConfig, IConfigControl
{
    public static  readonly BindableProperty colorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(ControlConfig), SvConstants.Defaults.color);

    public static  readonly BindableProperty fontSizeProperty = BindableProperty.Create(nameof(FontSize),
                                                                                        typeof(double),
                                                                                        typeof(ControlConfig),
                                                                                        SvConstants.Defaults.FONT_SIZE,
                                                                                        BindingMode.OneWay,
                                                                                        defaultValueCreator: bindable => Device.GetNamedSize(NamedSize.Default, typeof(ControlConfig))
                                                                                       );

    public static  readonly BindableProperty textProperty       = BindableProperty.Create(nameof(Text),       typeof(string), typeof(ControlConfig));
    public static  readonly BindableProperty fontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(ControlConfig));

    public static  readonly BindableProperty fontAttributesProperty = BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(ControlConfig), FontAttributes.None);
    public static  readonly BindableProperty alignmentProperty      = BindableProperty.Create(nameof(Alignment),      typeof(TextAlignment),  typeof(ControlConfig), TextAlignment.Start);


    public string? Text
    {
        get => (string?) GetValue(textProperty);
        set => SetValue(textProperty, value);
    }

    public Color Color
    {
        get => (Color) GetValue(colorProperty);
        set => SetValue(colorProperty, value);
    }

    [TypeConverter(typeof(FontSizeConverter))]
    public double FontSize
    {
        get => (double) GetValue(fontSizeProperty);
        set => SetValue(fontSizeProperty, value);
    }


    public string? FontFamily
    {
        get => (string?) GetValue(fontFamilyProperty);
        set => SetValue(fontFamilyProperty, value);
    }

    public FontAttributes FontAttributes
    {
        get => (FontAttributes) GetValue(fontAttributesProperty);
        set => SetValue(fontAttributesProperty, value);
    }

    public TextAlignment Alignment
    {
        get => (TextAlignment) GetValue(alignmentProperty);
        set => SetValue(alignmentProperty, value);
    }
}
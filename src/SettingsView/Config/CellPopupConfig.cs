// unset

namespace Jakar.SettingsView.Shared.Config;

[Xamarin.Forms.Internals.Preserve(true, false)]
public class CellPopupConfig : SvConfig
{
    public static readonly BindableProperty titleProperty         = BindableProperty.Create(nameof(Title),         typeof(string), typeof(CellPopupConfig));
    public static readonly BindableProperty titleColorProperty    = BindableProperty.Create(nameof(TitleColor),    typeof(Color),  typeof(CellPopupConfig), SvConstants.Prompt.Title.color);
    public static readonly BindableProperty titleFontSizeProperty = BindableProperty.Create(nameof(TitleFontSize), typeof(double), typeof(CellPopupConfig), SvConstants.Prompt.Title.FONT_SIZE);


    public static readonly BindableProperty backgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color),  typeof(CellPopupConfig), SvConstants.Prompt.background_Color);
    public static readonly BindableProperty acceptProperty          = BindableProperty.Create(nameof(Accept),          typeof(string), typeof(CellPopupConfig), SvConstants.Prompt.ACCEPT_TEXT);
    public static readonly BindableProperty cancelProperty          = BindableProperty.Create(nameof(Cancel),          typeof(string), typeof(CellPopupConfig), SvConstants.Prompt.CANCEL_TEXT);


    public static readonly BindableProperty selectedFontSizeProperty =
        BindableProperty.Create(nameof(SelectedFontSize), typeof(double), typeof(CellPopupConfig), SvConstants.Prompt.Selected.FONT_SIZE);

    public static readonly BindableProperty selectedColorProperty  = BindableProperty.Create(nameof(SelectedColor),  typeof(Color), typeof(CellPopupConfig), SvConstants.Prompt.Selected.text_Color);
    public static readonly BindableProperty accentColorProperty    = BindableProperty.Create(nameof(AccentColor),    typeof(Color), typeof(CellPopupConfig), SvConstants.Defaults.accent);
    public static readonly BindableProperty separatorColorProperty = BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(CellPopupConfig), SvConstants.Prompt.separator_Color);


    public static readonly BindableProperty itemFontSizeProperty = BindableProperty.Create(nameof(ItemFontSize), typeof(double), typeof(CellPopupConfig), SvConstants.Prompt.Item.FONT_SIZE);
    public static readonly BindableProperty itemColorProperty    = BindableProperty.Create(nameof(ItemColor),    typeof(Color),  typeof(CellPopupConfig), SvConstants.Prompt.Item.color);


    public static readonly BindableProperty itemDescriptionColorProperty =
        BindableProperty.Create(nameof(ItemDescriptionColor), typeof(Color), typeof(CellPopupConfig), SvConstants.Prompt.Item.Description.color);

    public static readonly BindableProperty itemDescriptionFontSizeProperty =
        BindableProperty.Create(nameof(ItemDescriptionFontSize), typeof(double), typeof(CellPopupConfig), SvConstants.Prompt.Item.Description.FONT_SIZE);


    [TypeConverter(typeof(ColorTypeConverter))]
    public Color BackgroundColor
    {
        get => (Color)GetValue(backgroundColorProperty);
        set => SetValue(backgroundColorProperty, value);
    }

    // -----------------------------------------------------------------------------------

    public string Title
    {
        get => (string)GetValue(titleProperty);
        set => SetValue(titleProperty, value);
    }


    [TypeConverter(typeof(ColorTypeConverter))]
    public Color TitleColor
    {
        get => (Color)GetValue(titleColorProperty);
        set => SetValue(titleColorProperty, value);
    }


    [TypeConverter(typeof(FontSizeConverter))]
    public double TitleFontSize
    {
        get => (double)GetValue(titleFontSizeProperty);
        set => SetValue(titleFontSizeProperty, value);
    }

    // -----------------------------------------------------------------------------------

    [TypeConverter(typeof(FontSizeConverter))]
    public double ItemFontSize
    {
        get => (double)GetValue(itemFontSizeProperty);
        set => SetValue(itemFontSizeProperty, value);
    }


    [TypeConverter(typeof(ColorTypeConverter))]
    public Color ItemColor
    {
        get => (Color)GetValue(itemColorProperty);
        set => SetValue(itemColorProperty, value);
    }

    // -----------------------------------------------------------------------------------

    [TypeConverter(typeof(ColorTypeConverter))]
    public Color ItemDescriptionColor
    {
        get => (Color)GetValue(itemDescriptionColorProperty);
        set => SetValue(itemDescriptionColorProperty, value);
    }


    [TypeConverter(typeof(FontSizeConverter))]
    public double ItemDescriptionFontSize
    {
        get => (double)GetValue(itemDescriptionFontSizeProperty);
        set => SetValue(itemDescriptionFontSizeProperty, value);
    }

    // -----------------------------------------------------------------------------------

    [TypeConverter(typeof(ColorTypeConverter))]
    public Color AccentColor
    {
        get => (Color)GetValue(accentColorProperty);
        set => SetValue(accentColorProperty, value);
    }

    // -----------------------------------------------------------------------------------

    [TypeConverter(typeof(FontSizeConverter))]
    public double SelectedFontSize
    {
        get => (double)GetValue(selectedFontSizeProperty);
        set => SetValue(selectedFontSizeProperty, value);
    }


    [TypeConverter(typeof(ColorTypeConverter))]
    public Color SelectedColor
    {
        get => (Color)GetValue(selectedColorProperty);
        set => SetValue(selectedColorProperty, value);
    }


    [TypeConverter(typeof(ColorTypeConverter))]
    public Color SeparatorColor
    {
        get => (Color)GetValue(separatorColorProperty);
        set => SetValue(separatorColorProperty, value);
    }

    // -----------------------------------------------------------------------------------

    public string Accept
    {
        get => (string)GetValue(acceptProperty);
        set => SetValue(acceptProperty, value);
    }

    public string Cancel
    {
        get => (string)GetValue(cancelProperty);
        set => SetValue(cancelProperty, value);
    }
}

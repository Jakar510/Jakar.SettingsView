// unset


namespace Jakar.SettingsView.Shared.Config;

[Xamarin.Forms.Internals.Preserve(true, false)]
public class PopupConfig : CellConfig, IConfigPopup
{
    public static readonly BindableProperty titleProperty         = BindableProperty.Create(nameof(Title),         typeof(string),  typeof(PopupConfig));
    public static readonly BindableProperty titleColorProperty    = BindableProperty.Create(nameof(TitleColor),    typeof(Color),   typeof(PopupConfig), Color.Black);
    public static readonly BindableProperty titleFontSizeProperty = BindableProperty.Create(nameof(TitleFontSize), typeof(double?), typeof(PopupConfig), SvConstants.Prompt.Title.FONT_SIZE);


    public static readonly BindableProperty backgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color),  typeof(PopupConfig), Color.White);
    public static readonly BindableProperty acceptProperty          = BindableProperty.Create(nameof(Accept),          typeof(string), typeof(PopupConfig), SvConstants.Prompt.ACCEPT_TEXT);
    public static readonly BindableProperty cancelProperty          = BindableProperty.Create(nameof(Cancel),          typeof(string), typeof(PopupConfig), SvConstants.Prompt.CANCEL_TEXT);


    public static readonly BindableProperty selectedFontSizeProperty = BindableProperty.Create(nameof(SelectedFontSize), typeof(double?), typeof(PopupConfig), SvConstants.Prompt.Selected.FONT_SIZE);
    public static readonly BindableProperty selectedColorProperty    = BindableProperty.Create(nameof(SelectedColor),    typeof(Color),   typeof(PopupConfig), SvConstants.Prompt.Selected.text_Color);
    public static readonly BindableProperty accentColorProperty      = BindableProperty.Create(nameof(AccentColor),      typeof(Color),   typeof(PopupConfig), SvConstants.Defaults.accent);
    public static readonly BindableProperty separatorColorProperty   = BindableProperty.Create(nameof(SeparatorColor),   typeof(Color),   typeof(PopupConfig), SvConstants.Prompt.separator_Color);


    public static readonly BindableProperty itemFontSizeProperty = BindableProperty.Create(nameof(ItemFontSize), typeof(double?), typeof(PopupConfig), SvConstants.Prompt.Item.FONT_SIZE);
    public static readonly BindableProperty itemColorProperty    = BindableProperty.Create(nameof(ItemColor),    typeof(Color),   typeof(PopupConfig), SvConstants.Prompt.Item.color);


    public static readonly BindableProperty itemDescriptionColorProperty = BindableProperty.Create(nameof(ItemDescriptionColor), typeof(Color), typeof(PopupConfig), Color.SlateGray);

    public static readonly BindableProperty itemDescriptionFontSizeProperty =
        BindableProperty.Create(nameof(ItemDescriptionFontSize), typeof(double?), typeof(PopupConfig), SvConstants.Prompt.Item.Description.FONT_SIZE);


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


    [TypeConverter(typeof(NullableFontSizeConverter))]
    public double? TitleFontSize
    {
        get => (double?)GetValue(titleFontSizeProperty);
        set => SetValue(titleFontSizeProperty, value);
    }

    // -----------------------------------------------------------------------------------

    [TypeConverter(typeof(NullableFontSizeConverter))]
    public double? ItemFontSize
    {
        get => (double?)GetValue(itemFontSizeProperty);
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
    public double? ItemDescriptionFontSize
    {
        get => (double?)GetValue(itemDescriptionFontSizeProperty);
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
    public double? SelectedFontSize
    {
        get => (double?)GetValue(selectedFontSizeProperty);
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


    // -----------------------------------------------------------------------------------

    private PromptConfiguration? _config;

    internal PromptConfiguration Properties
    {
        get
        {
            _config ??= new PromptConfiguration(this);
            return _config;
        }
    }



    internal class PromptConfiguration
    {
        private readonly PopupConfig      _config;
        private          CellPopupConfig? _Sv => _config.Parent?.Parent.Popup;
        public PromptConfiguration( PopupConfig cell ) => _config = cell;

        internal Color BackgroundColor =>
            _config.BackgroundColor == SvConstants.Cell.color
                ? _Sv?.BackgroundColor ?? SvConstants.Prompt.background_Color
                : _config.BackgroundColor;


        internal string Title => _config.Title;

        internal Color TitleColor =>
            _config.TitleColor == SvConstants.Prompt.Title.color
                ? _Sv?.TitleColor ?? SvConstants.Prompt.Title.color
                : _config.TitleColor;

        internal double TitleFontSize => _config.TitleFontSize ?? _Sv?.TitleFontSize ?? SvConstants.Prompt.Title.FONT_SIZE;


        internal Color ItemColor =>
            _config.ItemColor == SvConstants.Prompt.Item.color
                ? _Sv?.ItemColor ?? SvConstants.Prompt.Item.color
                : _config.ItemColor;

        internal double ItemFontSize => _config.ItemFontSize ?? _Sv?.ItemFontSize ?? SvConstants.Prompt.Item.FONT_SIZE;


        internal Color ItemDescriptionColor =>
            _config.ItemDescriptionColor == SvConstants.Prompt.Item.Description.color
                ? _Sv?.ItemDescriptionColor ?? SvConstants.Prompt.Item.Description.color
                : _config.ItemDescriptionColor;

        internal double ItemDescriptionFontSize => _config.ItemDescriptionFontSize ?? _Sv?.ItemDescriptionFontSize ?? SvConstants.Prompt.Item.Description.FONT_SIZE;


        internal double SelectedFontSize => _config.SelectedFontSize ?? _Sv?.SelectedFontSize ?? SvConstants.Prompt.Selected.FONT_SIZE;

        internal Color AccentColor =>
            _config.AccentColor == SvConstants.Defaults.accent
                ? _Sv?.AccentColor ?? SvConstants.Defaults.accent
                : _config.AccentColor;

        internal Color SelectedColor =>
            _config.SelectedColor == SvConstants.Prompt.Selected.text_Color
                ? _Sv?.SelectedColor ?? SvConstants.Prompt.Selected.text_Color
                : _config.SelectedColor;

        internal Color SeparatorColor =>
            _config.SeparatorColor == SvConstants.Prompt.separator_Color
                ? _Sv?.SeparatorColor ?? SvConstants.Prompt.separator_Color
                : _config.SeparatorColor;
    }
}


// public class Config
// {
// 	public Color BackgroundColor { get; init; }
// 	public Color AccentColor { get; init; }
//
//
// 	public ItemConfig Title { get; init; }
// 	public ItemConfig Description { get; init; }
// 	public ItemConfig Selected { get; init; }
//
//
// 	public string Accept { get; init; }
// 	public string Cancel { get; init; }
//
//
// 	public PopupConfig( PopupCellBase cell )
// 	{
// 		Accept = cell.PopupAccept;
// 		Cancel = cell.PopupCancel;
// 		BackgroundColor = cell.PopupBackgroundColor;
// 		AccentColor = cell.PopupAccentColor;
//
// 		Title = new ItemConfig(cell.PopupTitle, cell.PopupTitleColor, new FontConfig(cell.PopupTitleFontSize));
// 		Description = new ItemConfig(cell.PopupItemDescriptionColor, new FontConfig(cell.PopupItemDescriptionFontSize));
// 		Selected = new ItemConfig(cell.PopupSelectedColor, new FontConfig(cell.PopupSelectedFontSize));
// 	}
// }

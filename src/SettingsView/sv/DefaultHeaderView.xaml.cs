using System.Diagnostics.CodeAnalysis;
using Xamarin.Forms.Xaml;


namespace Jakar.SettingsView.Shared.sv;

[XamlCompilation(XamlCompilationOptions.Compile)]
public sealed partial class DefaultHeaderView : HeaderView
{
    public override void SetText( string?    value ) { TitleLabel.Text      = value; }
    public override void SetTextColor( Color value ) { TitleLabel.TextColor = value; }
    public override void SetTextFont( double fontSize, string family, FontAttributes attributes )
    {
        TitleLabel.FontFamily     = family;
        TitleLabel.FontAttributes = attributes;
        TitleLabel.FontSize       = fontSize;
    }
    public override void SetBackground( Color value ) { BackgroundColor = value; }

    public override ImageSource? Source
    {
        get => Icon.Source;
        set => Icon.Source = value;
    }

    public override bool IsCollapsible
    {
        get => (bool) GetValue(isCollapsibleProperty);
        set => SetValue(isCollapsibleProperty, value);
    }

    public override bool IsCollapsed
    {
        get => (bool) GetValue(isCollapsedProperty);
        set => SetValue(isCollapsedProperty, value);
    }


    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public DefaultHeaderView() : base()
    {
        InitializeComponent();
        // CollapsedIcon = Icons.CollapseSolidWhite;
        // ExpandedIcon = Icons.ExpandSolidWhite;
        Source = ExpandedIcon;

        Padding             = SvConstants.Section.Header.padding;
        HeightRequest       = SvConstants.Section.Header.MIN_ROW_HEIGHT;
        BackgroundColor     = SvConstants.Section.Header.background_Color;
        TitleLabel.FontSize = SvConstants.Section.Header.FONT_SIZE;
        SetTextColor(SvConstants.Section.Header.text_Color);
    }
}
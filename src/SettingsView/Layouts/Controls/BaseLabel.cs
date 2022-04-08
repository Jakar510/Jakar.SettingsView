namespace Jakar.SettingsView.Shared.Layouts.Controls;

public abstract class BaseLabel : Label, IUpdateTextControl
{
    protected BaseLabel( in int row, in int column )
    {
        HorizontalOptions = LayoutOptions.FillAndExpand;
        VerticalOptions   = LayoutOptions.FillAndExpand;
        Grid.SetRow(this, row);
        Grid.SetColumn(this, column);
        BackgroundColor = Color.Transparent;
    }

    public void Update( IUseConfiguration configuration )
    {
        FontSize                = configuration.FontSize;
        FontFamily              = configuration.FontFamily;
        FontAttributes          = configuration.FontAttributes;
        HorizontalTextAlignment = VerticalTextAlignment = configuration.TextAlignment;
        TextColor               = configuration.Color;
    }

    public void SetText( string? text ) => Text = text;
}
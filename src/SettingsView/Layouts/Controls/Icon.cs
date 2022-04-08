namespace Jakar.SettingsView.Shared.Layouts.Controls;

public class Icon : Image, IUpdateIconControl
{
    public Icon() : this(0, 0, 2) { }

    protected Icon( in int row, in int column, in int rowSpan )
    {
        HorizontalOptions = LayoutOptions.FillAndExpand;
        VerticalOptions   = LayoutOptions.FillAndExpand;
        Grid.SetRow(this, row);
        Grid.SetColumn(this, column);
        Grid.SetRowSpan(this, rowSpan);
        BackgroundColor = Color.Transparent;
        Aspect          = Aspect.AspectFit;
    }


    public void Update( IUseIconConfiguration configuration )
    {
        Source        = configuration.Source;
        HeightRequest = configuration.IconSize.Height;
        WidthRequest  = configuration.IconSize.Width;

        // SizeRequest = configuration.IconSize;
        // CornerRadius = configuration.IconRadius;
    }
}
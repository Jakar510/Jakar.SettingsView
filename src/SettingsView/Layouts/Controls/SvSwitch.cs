

namespace Jakar.SettingsView.Shared.Layouts.Controls;

public class SvSwitch : Switch, IUpdateAccessoryControl
{
    public bool Checked
    {
        get => IsToggled;
        set => IsToggled = value;
    }

    public SvSwitch() : this(0, 2, 2) { }

    protected SvSwitch( in int row, in int column, in int rowSpan )
    {
        HorizontalOptions = LayoutOptions.FillAndExpand;
        VerticalOptions   = LayoutOptions.FillAndExpand;
        Grid.SetRow(this, row);
        Grid.SetColumn(this, column);
        Grid.SetRowSpan(this, rowSpan);
        BackgroundColor = Color.Transparent;
    }


    public void Update( IUseCheckableConfiguration configuration )
    {
        ThumbColor = configuration.OffColor;
        OnColor    = configuration.AccentColor;
    }

    public void Select() { Checked   = true; }
    public void Deselect() { Checked = false; }
}
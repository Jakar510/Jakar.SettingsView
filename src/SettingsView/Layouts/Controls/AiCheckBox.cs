namespace Jakar.SettingsView.Shared.Layouts.Controls;

public class AiCheckBox : CheckBox, IUpdateAccessoryControl
{
    public bool Checked
    {
        get => IsChecked;
        set => IsChecked = value;
    }

    public AiCheckBox() : this(0, 2, 2) { }

    protected AiCheckBox( in int row, in int column, in int rowSpan )
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
        BackgroundColor = configuration.OffColor;
        Color           = configuration.AccentColor;
    }

    public void Select() { Checked   = true; }
    public void Deselect() { Checked = false; }
}
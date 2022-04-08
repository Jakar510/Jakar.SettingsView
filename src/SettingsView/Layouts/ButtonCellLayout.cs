namespace Jakar.SettingsView.Shared.Layouts;

public abstract class ButtonCellLayout : BaseCellLayout
{
    public Button Button { get; set; }

    protected ButtonCellLayout() : base()
    {
        Button = new Button();
        SetRow(Button, 0);
        SetColumn(Button, 0);

        RowDefinitions    = new RowDefinitionCollection() { new() { Height   = GridLength.Star } };
        ColumnDefinitions = new ColumnDefinitionCollection() { new() { Width = GridLength.Star } };

        Children.Add(Button);
    }
}
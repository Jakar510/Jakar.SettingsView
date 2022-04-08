namespace Jakar.SettingsView.Shared.Cells;

[Xamarin.Forms.Internals.Preserve(true, false)]
public class TextPickerCell : PromptCellBase<string>
{
    public static readonly BindableProperty itemsProperty           = BindableProperty.Create(nameof(Items),           typeof(IList<string>), typeof(TextPickerCell), new List<string>());
    public static readonly BindableProperty selectedCommandProperty = BindableProperty.Create(nameof(SelectedCommand), typeof(ICommand),      typeof(TextPickerCell));
    public static readonly BindableProperty accentColorProperty     = BindableProperty.Create(nameof(AccentColor),     typeof(Color),         typeof(PickerCell),     Color.Default);

    public static readonly BindableProperty selectedItemProperty = BindableProperty.Create(nameof(SelectedItem),
                                                                                           typeof(string),
                                                                                           typeof(TextPickerCell),
                                                                                           default,
                                                                                           BindingMode.TwoWay
                                                                                          );


    public IList<string>? Items
    {
        get => (IList<string>?) GetValue(itemsProperty);
        set => SetValue(itemsProperty, value);
    }


    public Color AccentColor
    {
        get => (Color) GetValue(accentColorProperty);
        set => SetValue(accentColorProperty, value);
    }

    public string? SelectedItem
    {
        get => (string?) GetValue(selectedItemProperty);
        set => SetValue(selectedItemProperty, value);
    }

    public ICommand? SelectedCommand
    {
        get => (ICommand?) GetValue(selectedCommandProperty);
        set => SetValue(selectedCommandProperty, value);
    }
}
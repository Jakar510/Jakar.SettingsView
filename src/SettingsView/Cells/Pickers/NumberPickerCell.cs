namespace Jakar.SettingsView.Shared.Cells;

[Xamarin.Forms.Internals.Preserve(true, false)]
public class NumberPickerCell : PromptCellBase<int>
{
    public static readonly BindableProperty selectedCommandProperty = BindableProperty.Create(nameof(SelectedCommand), typeof(ICommand), typeof(NumberPickerCell));
    // public static BindableProperty PopupTitleProperty = BindableProperty.Create(nameof(PopupTitle), typeof(string), typeof(NumberPickerCell), default(string));
    public static readonly BindableProperty maxProperty    = BindableProperty.Create(nameof(Max),    typeof(int), typeof(NumberPickerCell), 9999);
    public static readonly BindableProperty minProperty    = BindableProperty.Create(nameof(Min),    typeof(int), typeof(NumberPickerCell), 0);
    public static readonly BindableProperty numberProperty = BindableProperty.Create(nameof(Number), typeof(int), typeof(NumberPickerCell), default(int), BindingMode.TwoWay);

    public int Number
    {
        get => (int) GetValue(numberProperty);
        set => SetValue(numberProperty, value);
    }

    public int Min
    {
        get => (int) GetValue(minProperty);
        set => SetValue(minProperty, value);
    }

    public int Max
    {
        get => (int) GetValue(maxProperty);
        set => SetValue(maxProperty, value);
    }

    // public string PopupTitle
    // {
    // 	get => (string) GetValue(PopupTitleProperty);
    // 	set => SetValue(PopupTitleProperty, value);
    // }

    public ICommand SelectedCommand
    {
        get => (ICommand) GetValue(selectedCommandProperty);
        set => SetValue(selectedCommandProperty, value);
    }
}
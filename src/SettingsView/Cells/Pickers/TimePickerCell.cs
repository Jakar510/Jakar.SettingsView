namespace Jakar.SettingsView.Shared.Cells;

[Xamarin.Forms.Internals.Preserve(true, false)]
public class TimePickerCell : PromptCellBase<TimeSpan>
{
    public static readonly BindableProperty timeProperty   = BindableProperty.Create(nameof(Time),   typeof(TimeSpan), typeof(TimePickerCell), default(TimeSpan), BindingMode.TwoWay);
    public static readonly BindableProperty formatProperty = BindableProperty.Create(nameof(Format), typeof(string),   typeof(TimePickerCell), "t");

    public TimeSpan Time
    {
        get => (TimeSpan) GetValue(timeProperty);
        set => SetValue(timeProperty, value);
    }

    public string Format
    {
        get => (string) GetValue(formatProperty);
        set => SetValue(formatProperty, value);
    }

}
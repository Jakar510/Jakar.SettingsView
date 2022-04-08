namespace Jakar.SettingsView.Shared.Cells;

[Xamarin.Forms.Internals.Preserve(true, false)]
public class DatePickerCell : PromptCellBase<DateTime>
{
    public static readonly BindableProperty dateProperty        = BindableProperty.Create(nameof(Date),        typeof(DateTime), typeof(DatePickerCell), default(DateTime), BindingMode.TwoWay);
    public static readonly BindableProperty maximumDateProperty = BindableProperty.Create(nameof(MaximumDate), typeof(DateTime), typeof(DatePickerCell), new DateTime(2500, 12, 31));
    public static readonly BindableProperty minimumDateProperty = BindableProperty.Create(nameof(MinimumDate), typeof(DateTime), typeof(DatePickerCell), new DateTime(1900, 1,  1));
    public static readonly BindableProperty formatProperty      = BindableProperty.Create(nameof(Format),      typeof(string),   typeof(DatePickerCell), "d");
    public static readonly BindableProperty todayTextProperty   = BindableProperty.Create(nameof(TodayText),   typeof(string),   typeof(DatePickerCell));

    public DateTime Date
    {
        get => (DateTime)GetValue(dateProperty);
        set => SetValue(dateProperty, value);
    }

    public DateTime MaximumDate
    {
        get => (DateTime)GetValue(maximumDateProperty);
        set => SetValue(maximumDateProperty, value);
    }

    public DateTime MinimumDate
    {
        get => (DateTime)GetValue(minimumDateProperty);
        set => SetValue(minimumDateProperty, value);
    }

    public string Format
    {
        get => (string)GetValue(formatProperty);
        set => SetValue(formatProperty, value);
    }

    public string TodayText
    {
        get => (string)GetValue(todayTextProperty);
        set => SetValue(todayTextProperty, value);
    }
}

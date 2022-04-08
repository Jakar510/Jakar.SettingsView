namespace Jakar.SettingsView.Shared.Cells;

[Xamarin.Forms.Internals.Preserve(true, false)]
public class CommandCell : DescriptionCellBase
{
    public static readonly BindableProperty commandProperty               = BindableProperty.Create(nameof(Command),               typeof(ICommand), typeof(CommandCell));
    public static readonly BindableProperty commandParameterProperty      = BindableProperty.Create(nameof(CommandParameter),      typeof(object),   typeof(CommandCell));
    public static readonly BindableProperty keepSelectedUntilBackProperty = BindableProperty.Create(nameof(KeepSelectedUntilBack), typeof(bool),     typeof(CommandCell), default(bool));
    public static readonly BindableProperty hideArrowIndicatorProperty    = BindableProperty.Create(nameof(HideArrowIndicator),    typeof(bool),     typeof(CommandCell), default(bool));

    public ICommand Command
    {
        get => (ICommand) GetValue(commandProperty);
        set => SetValue(commandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(commandParameterProperty);
        set => SetValue(commandParameterProperty, value);
    }

    public bool KeepSelectedUntilBack
    {
        get => (bool) GetValue(keepSelectedUntilBackProperty);
        set => SetValue(keepSelectedUntilBackProperty, value);
    }

    public bool HideArrowIndicator
    {
        get => (bool) GetValue(hideArrowIndicatorProperty);
        set => SetValue(hideArrowIndicatorProperty, value);
    }
}
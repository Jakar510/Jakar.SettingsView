namespace Jakar.SettingsView.Shared.Cells;

[Xamarin.Forms.Internals.Preserve(true, false)]
public class RadioCell : CheckableCellBase<object>
{
    public static readonly BindableProperty selectedValueProperty = BindableProperty.CreateAttached("selectedValue",
                                                                                                    typeof(object),
                                                                                                    typeof(RadioCell),
                                                                                                    default,
                                                                                                    BindingMode.TwoWay
                                                                                                   );

    public static void SetselectedValue( BindableObject    view, object? value ) { view.SetValue(selectedValueProperty, value); }
    public static object? GetselectedValue( BindableObject view ) => view.GetValue(selectedValueProperty);

    public static readonly BindableProperty valueProperty = BindableProperty.Create(nameof(Value), typeof(object), typeof(RadioCell));

    public object? Value
    {
        get => GetValue(valueProperty);
        set => SetValue(valueProperty, value);
    }
}



public static class RadioCellExtensions
{
    public static void SetSelectedValue( this    BindableObject view, object? value ) { RadioCell.SetselectedValue(view, value); }
    public static object? GetSelectedValue( this BindableObject view ) => RadioCell.GetselectedValue(view);
}
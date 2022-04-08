// unset

namespace Jakar.SettingsView.Shared.CellBase;

[Xamarin.Forms.Internals.Preserve(true, false)]
public abstract class ValueTextCellBase : ValueCellBase
{
    public static readonly BindableProperty maxLengthProperty = BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(ValueTextCellBase), -1);

    public static readonly BindableProperty valueTextProperty = BindableProperty.Create(nameof(ValueText),
                                                                                        typeof(string),
                                                                                        typeof(ValueTextCellBase),
                                                                                        default(string?),
                                                                                        BindingMode.TwoWay,
                                                                                        propertyChanging: ValueTextPropertyChanging
                                                                                       );

    public string? ValueText
    {
        get => (string?) GetValue(valueTextProperty);
        set => SetValue(valueTextProperty, value);
    }

    public int MaxLength
    {
        get => (int) GetValue(maxLengthProperty);
        set => SetValue(maxLengthProperty, value);
    }

    private static void ValueTextPropertyChanging( BindableObject bindable, object oldValue, object newValue )
    {
        var maxlength = (int) bindable.GetValue(maxLengthProperty);

        if ( maxlength < 0 ) return;

        string newString = newValue?.ToString() ?? string.Empty;

        if ( newString.Length <= maxlength ) return;
        string oldString = oldValue?.ToString() ?? string.Empty;

        if ( oldString.Length > maxlength )
        {
            string trimStr = oldString.Substring(0, maxlength);
            bindable.SetValue(valueTextProperty, trimStr);
        }
        else { bindable.SetValue(valueTextProperty, oldString); }
    }
}



public abstract class ValueTextCellBase<TValue> : ValueTextCellBase, IValueChanged<TValue>
{
    public event EventHandler<SVValueChangedEventArgs<TValue>>? ValueChanged;

    void IValueChanged<TValue>.SendValueChanged( TValue              value ) => ValueChanged?.Invoke(this, new SVValueChangedEventArgs<TValue>(value));
    void IValueChanged<TValue>.SendValueChanged( IEnumerable<TValue> value ) => ValueChanged?.Invoke(this, new SVValueChangedEventArgs<TValue>(value));
    internal IValueChanged<TValue> ValueChangedHandler => this;
}
namespace Jakar.SettingsView.Shared.Cells;

[Xamarin.Forms.Internals.Preserve(true, false)]
public class EntryCell : ValueTextCellBase<string?>, IEntryCellController
{
    public static readonly BindableProperty keyboardProperty         = BindableProperty.Create(nameof(Keyboard),         typeof(Keyboard),     typeof(EntryCell), Keyboard.Default);
    public static readonly BindableProperty completedCommandProperty = BindableProperty.Create(nameof(CompletedCommand), typeof(ICommand),     typeof(EntryCell));
    public static readonly BindableProperty placeholderProperty      = BindableProperty.Create(nameof(Placeholder),      typeof(string),       typeof(EntryCell));
    public static readonly BindableProperty placeholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color),        typeof(EntryCell), SvConstants.Defaults.color);
    public static readonly BindableProperty accentColorProperty      = BindableProperty.Create(nameof(AccentColor),      typeof(Color),        typeof(EntryCell), SvConstants.Defaults.color);
    public static readonly BindableProperty isPasswordProperty       = BindableProperty.Create(nameof(IsPassword),       typeof(bool),         typeof(EntryCell), default(bool));
    public static readonly BindableProperty onSelectActionProperty   = BindableProperty.Create(nameof(IsPassword),       typeof(SelectAction), typeof(EntryCell), default(SelectAction));


    public Keyboard Keyboard
    {
        get => (Keyboard) GetValue(keyboardProperty);
        set => SetValue(keyboardProperty, value);
    }


    public event EventHandler? Completed;
    public void SendCompleted()
    {
        Completed?.Invoke(this, EventArgs.Empty);
        if ( CompletedCommand?.CanExecute(null) ?? false ) { CompletedCommand.Execute(null); }
    }


    public ICommand? CompletedCommand
    {
        get => (ICommand?) GetValue(completedCommandProperty);
        set => SetValue(completedCommandProperty, value);
    }


    public string? Placeholder
    {
        get => (string?) GetValue(placeholderProperty);
        set => SetValue(placeholderProperty, value);
    }


    public Color PlaceholderColor
    {
        get => (Color) GetValue(placeholderColorProperty);
        set => SetValue(placeholderColorProperty, value);
    }

		

    public Color AccentColor
    {
        get => (Color) GetValue(accentColorProperty);
        set => SetValue(accentColorProperty, value);
    }


    public bool IsPassword
    {
        get => (bool) GetValue(isPasswordProperty);
        set => SetValue(isPasswordProperty, value);
    }


    public SelectAction OnSelectAction
    {
        get => (SelectAction) GetValue(onSelectActionProperty);
        set => SetValue(onSelectActionProperty, value);
    }


    internal Color GetPlaceholderColor() =>
        PlaceholderColor != SvConstants.Sv.Value.placeholder_Color
            ? PlaceholderColor
            : Parent.CellPlaceholderColor;
    internal Color GetAccentColor() =>
        AccentColor != SvConstants.Sv.accent_Color
            ? AccentColor
            : Parent.CellAccentColor;



    public event EventHandler<TextChangedEventArgs>? TextChanged;
    internal void SendTextChanged( string               oldTextValue, string newTextValue ) => SendTextChanged(new TextChangedEventArgs(oldTextValue, newTextValue));
    internal void SendTextChanged( TextChangedEventArgs args) { TextChanged?.Invoke(this, args); }


    internal event EventHandler? Focused;
    internal void SendFocus() { Focused?.Invoke(this, EventArgs.Empty); }
    public void SetFocus() => SendFocus();
}
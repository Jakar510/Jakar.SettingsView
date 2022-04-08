namespace Jakar.SettingsView.Shared.Cells;

[Xamarin.Forms.Internals.Preserve(true, false)]
public class ButtonCell : TitleCellBase // IBorderElement
{
    public static readonly BindableProperty buttonBackgroundColorProperty     = BindableProperty.Create(nameof(ButtonBackgroundColor), typeof(Color), typeof(TitleCellBase), SvConstants.Cell.color);
    public static readonly BindableProperty commandProperty                   = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ButtonCell));
    public static readonly BindableProperty commandParameterProperty          = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ButtonCell));
    public static readonly BindableProperty longClickCommandProperty          = BindableProperty.Create(nameof(LongClickCommand), typeof(ICommand), typeof(ButtonCell));
    public static readonly BindableProperty longClickCommandParameterProperty = BindableProperty.Create(nameof(LongClickCommandParameter), typeof(object), typeof(ButtonCell));


    // public static readonly BindableProperty BackgroundBrushProperty = BindableProperty.Create(nameof(BackgroundBrush), typeof(Brush), typeof(ButtonCell), SVConstants.Cell.Brush);
    // public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(nameof(BorderWidth), typeof(double), typeof(ButtonCell), SVConstants.Defaults.BorderWidth);
    // public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(int), typeof(ButtonCell), SVConstants.Defaults.CornerRadius);
    //
    // public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor),
    // 																					  typeof(Color),
    // 																					  typeof(ButtonCell),
    // 																					  SVConstants.Cell.COLOR,
    // 																					  propertyChanged: OnBorderColorPropertyChanged
    // 																					 );

    // private static void OnBorderColorPropertyChanged( BindableObject bindable, object oldValue, object newValue ) { ( (IBorderElement) bindable ).OnBorderColorPropertyChanged((Color) oldValue, (Color) newValue); }


    public Color ButtonBackgroundColor
    {
        get => (Color) GetValue(buttonBackgroundColorProperty);
        set => SetValue(buttonBackgroundColorProperty, value);
    }

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

    public ICommand LongClickCommand
    {
        get => (ICommand) GetValue(longClickCommandProperty);
        set => SetValue(longClickCommandProperty, value);
    }

    public object LongClickCommandParameter
    {
        get => GetValue(longClickCommandParameterProperty);
        set => SetValue(longClickCommandParameterProperty, value);
    }

    internal Color GetButtonColor() =>
        ButtonBackgroundColor == SvConstants.Cell.color
            ? Parent.CellButtonBackgroundColor
            : ButtonBackgroundColor;


    // public Color BorderColor
    // {
    // 	get => (Color) GetValue(BorderColorProperty);
    // 	set => SetValue(BorderColorProperty, value);
    // }
    //
    // public int CornerRadius
    // {
    // 	get => (int) GetValue(CornerRadiusProperty);
    // 	set => SetValue(CornerRadiusProperty, value);
    // }
    //
    // public double BorderWidth
    // {
    // 	get => (double) GetValue(BorderWidthProperty);
    // 	set => SetValue(BorderWidthProperty, value);
    // }
    //
    // public Brush BackgroundBrush
    // {
    // 	get => (Brush) GetValue(BackgroundBrushProperty);
    // 	set => SetValue(BackgroundBrushProperty, value);
    // }
    //
    // void IBorderElement.OnBorderColorPropertyChanged( Color oldValue, Color newValue ) {  }
    // bool IBorderElement.IsCornerRadiusSet() => !CornerRadius.Equals(SVConstants.Defaults.CornerRadius);
    // bool IBorderElement.IsBackgroundColorSet() => ButtonBackgroundColor != SVConstants.Cell.COLOR;
    // bool IBorderElement.IsBackgroundSet() => BackgroundColor != SVConstants.Cell.COLOR;
    // bool IBorderElement.IsBorderColorSet() => BorderColor != SVConstants.Cell.COLOR;
    // bool IBorderElement.IsBorderWidthSet() => !BorderWidth.Equals(SVConstants.Defaults.BorderWidth);
    //
    // Color IBorderElement.BorderColor => BorderColor;
    // int IBorderElement.CornerRadius => CornerRadius;
    // Brush IBorderElement.Background => BackgroundBrush;
    // double IBorderElement.BorderWidth => BorderWidth;
    // int IBorderElement.CornerRadiusDefaultValue => (int) CornerRadiusProperty.DefaultValue;
    // Color IBorderElement.BorderColorDefaultValue => (Color) BorderColorProperty.DefaultValue;
    // double IBorderElement.BorderWidthDefaultValue => (double) BorderWidthProperty.DefaultValue;
}
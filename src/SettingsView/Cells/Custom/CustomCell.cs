namespace Jakar.SettingsView.Shared.Cells;

[Xamarin.Forms.Internals.Preserve(true, false)]
[ContentProperty(nameof(Content))]
public class CustomCell : CommandCell
{
    public static readonly BindableProperty showArrowIndicatorProperty = BindableProperty.Create(nameof(ShowArrowIndicator), typeof(bool),     typeof(CustomCell), default(bool));
    public static readonly BindableProperty longCommandProperty        = BindableProperty.Create(nameof(LongCommand),        typeof(ICommand), typeof(CustomCell));
    public static readonly BindableProperty contentProperty            = BindableProperty.Create(nameof(Content),            typeof(View),     typeof(CustomCell));
    public static readonly BindableProperty isSelectableProperty       = BindableProperty.Create(nameof(IsSelectable),       typeof(bool),     typeof(CustomCell), true);
    public static readonly BindableProperty isMeasureOnceProperty      = BindableProperty.Create(nameof(IsMeasureOnce),      typeof(bool),     typeof(CustomCell), default(bool));
    public static readonly BindableProperty useFullSizeProperty        = BindableProperty.Create(nameof(UseFullSize),        typeof(bool),     typeof(CustomCell), default(bool));
    public static readonly BindableProperty isForceLayoutProperty      = BindableProperty.Create(nameof(IsForceLayout),      typeof(bool),     typeof(CustomCell), default(bool));


    public bool ShowArrowIndicator
    {
        get => (bool) GetValue(showArrowIndicatorProperty);
        set => SetValue(showArrowIndicatorProperty, value);
    }


    public View? Content
    {
        get => (View?) GetValue(contentProperty);
        set => SetValue(contentProperty, value);
    }


    public bool IsSelectable
    {
        get => (bool) GetValue(isSelectableProperty);
        set => SetValue(isSelectableProperty, value);
    }


    internal bool IsForceLayout
    {
        get => (bool) GetValue(isForceLayoutProperty);
        set => SetValue(isForceLayoutProperty, value);
    }


    public bool IsMeasureOnce
    {
        get => (bool) GetValue(isMeasureOnceProperty);
        set => SetValue(isMeasureOnceProperty, value);
    }


    public bool UseFullSize
    {
        get => (bool) GetValue(useFullSizeProperty);
        set => SetValue(useFullSizeProperty, value);
    }


    public ICommand? LongCommand
    {
        get => (ICommand?) GetValue(longCommandProperty);
        set => SetValue(longCommandProperty, value);
    }


    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        if ( Content != null ) { Content.BindingContext = BindingContext; }
    }

    protected override void OnParentSet()
    {
        base.OnParentSet();
        if ( Content != null ) { Content.Parent = Parent; }
    }

    public override void Reload()
    {
        IsForceLayout = true;
        base.Reload();
    }

    public void SendLongCommand()
    {
        if ( LongCommand == null ) { return; }

        if ( LongCommand.CanExecute(BindingContext) ) { LongCommand.Execute(BindingContext); }
    }
}
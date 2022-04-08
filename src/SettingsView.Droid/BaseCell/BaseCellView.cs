namespace Jakar.SettingsView.Droid.BaseCell;

[Preserve(AllMembers = true)]
public abstract class BaseCellView : AGridLayout, INativeElementView
{
    private Cell? _cell;

    protected internal Cell Cell
    {
        get => _cell ?? throw new NullPointerException(nameof(_cell));
        set => _cell = value;
    }

    public             Element                 Element    => Cell;
    protected internal CellBase?               CellBase   => Cell as CellBase;                      // ?? throw new NullReferenceException(nameof(CellBase));
    protected internal Shared.sv.SettingsView? CellParent => Cell.Parent as Shared.sv.SettingsView; // ?? throw new NullReferenceException(nameof(CellParent));


    protected internal AContext AndroidContext { get; set; }

    protected internal ColorDrawable  BackgroundColor { get; set; }
    protected internal ColorDrawable  SelectedColor   { get; set; }
    protected internal RippleDrawable Ripple          { get; set; }


    protected BaseCellView( AContext context, Cell cell ) : base(context)
    {
        AndroidContext = context;
        _cell          = cell;

        BackgroundColor = new ColorDrawable();
        SelectedColor   = new ColorDrawable(SvConstants.Cell.selected.ToAndroid());

        Background = Ripple = CreateRippleDrawable();

        // GridLengthTypeConverter
        // GridLength.Star
    }

    // Renderer
    protected BaseCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)

    {
        AndroidContext = SettingsViewInit.Current;

        BackgroundColor ??= new ColorDrawable();
        SelectedColor   ??= new ColorDrawable(SvConstants.Cell.selected.ToAndroid());

        Ripple     ??= CreateRippleDrawable();
        Background ??= Ripple;
    }


    protected AView CreateContentView( int id, bool attach = true ) => AndroidContext.CreateContentView(this, id, attach);

    protected RippleDrawable CreateRippleDrawable( AColor? color = null )
    {
        using var sel = new StateListDrawable();

        sel.AddState(new[]
                     {
                         Android.Resource.Attribute.StateSelected
                     },
                     SelectedColor);

        sel.AddState(new[]
                     {
                         -Android.Resource.Attribute.StateSelected
                     },
                     BackgroundColor);

        sel.SetExitFadeDuration(250);
        sel.SetEnterFadeDuration(250);

        AColor rippleColor = color ?? CellParent?.SelectedColor.ToAndroid() ?? SvConstants.Cell.ripple.ToAndroid();

        return DrawableUtility.CreateRipple(rippleColor, sel);
    }


    protected internal virtual void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
    {
        if ( e.PropertyName == CellBase.isVisibleProperty.PropertyName ) { UpdateIsVisible(); }
        else if ( e.PropertyName == Cell.IsEnabledProperty.PropertyName ) { UpdateIsEnabled(); }
        else if ( e.PropertyName == CellBase.backgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }
    }

    protected internal virtual void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
    {
        if ( e.PropertyName == Shared.sv.SettingsView.selectedColorProperty.PropertyName ) { UpdateSelectedColor(); }
    }

    protected internal virtual void SectionPropertyChanged( object sender, PropertyChangedEventArgs e ) { }

    protected internal virtual void RowSelected( SettingsViewRecyclerAdapter    adapter, int position ) { }
    protected internal virtual bool RowLongPressed( SettingsViewRecyclerAdapter adapter, int position ) => false;

    protected void UpdateIsVisible() { CellBase?.Section?.ShowVisibleCells(); }

    public void UpdateWithForceLayout( Action updateAction )
    {
        updateAction();
        Invalidate();
    }

    public bool UpdateWithForceLayout( Func<bool> updateAction )
    {
        bool result = updateAction();
        Invalidate();
        return result;
    }


    protected internal virtual void UpdateCell()
    {
        UpdateIsEnabled();
        UpdateBackgroundColor();
        UpdateSelectedColor();
        Invalidate();
    }

    protected void SetEnabledAppearance( bool isEnabled )
    {
        if ( isEnabled ) { EnableCell(); }
        else { DisableCell(); }
    }

    protected virtual void EnableCell()
    {
        // not to invoke a ripple effect and not to selected
        Focusable              = false;
        DescendantFocusability = DescendantFocusability.AfterDescendants;
    }

    protected virtual void DisableCell()
    {
        // not to invoke a ripple effect and not to selected
        Focusable              = true;
        DescendantFocusability = DescendantFocusability.BlockDescendants;
    }

    protected virtual void UpdateIsEnabled() { SetEnabledAppearance(Cell.IsEnabled); }


    protected virtual void UpdateBackgroundColor()
    {
        Color color = CellBase?.GetBackground() ?? Color.Transparent;

        // if ( CellBase is EntryCell ) { SetBackgroundColor(color.ToAndroid()); }
        // else { SetBackgroundColor(color.ToAndroid()); }

        SetBackgroundColor(color.ToAndroid());
    }

    protected void UpdateSelectedColor()
    {
        if ( CellParent != null &&
             CellParent.SelectedColor != Color.Default )
        {
            SelectedColor.Color = CellParent.SelectedColor.MultiplyAlpha(0.5).ToAndroid();
            Ripple.SetColor(DrawableUtility.GetPressedColorSelector(CellParent.SelectedColor.ToAndroid()));
        }
        else
        {
            SelectedColor.Color = AColor.Argb(125, 180, 180, 180);
            Ripple.SetColor(DrawableUtility.GetPressedColorSelector(AColor.Rgb(180, 180, 180)));
        }
    }


    protected internal void HideKeyboard( AView inputView )
    {
        AObject                  temp               = AndroidContext.GetSystemService(AContext.InputMethodService) ?? throw new NullReferenceException(nameof(Context.InputMethodService));
        using InputMethodManager inputMethodManager = (InputMethodManager)temp;
        IBinder?                 windowToken        = inputView.WindowToken;
        if ( windowToken != null ) { inputMethodManager.HideSoftInputFromWindow(windowToken, HideSoftInputFlags.None); }
    }

    protected internal void ShowKeyboard( AView inputView )
    {
        AObject                  temp               = AndroidContext.GetSystemService(AContext.InputMethodService) ?? throw new NullReferenceException(nameof(Context.InputMethodService));
        using InputMethodManager inputMethodManager = (InputMethodManager)temp;
        inputMethodManager.ShowSoftInput(inputView, ShowFlags.Forced);
        inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
    }

    protected override void Dispose( bool disposing )
    {
        if ( disposing )
        {
            if ( CellBase != null )
            {
                CellBase.PropertyChanged -= CellPropertyChanged;
                if ( CellParent != null ) CellParent.PropertyChanged -= ParentPropertyChanged;

                if ( CellBase.Section != null )
                {
                    CellBase.Section.PropertyChanged -= SectionPropertyChanged;
                    CellBase.Section                 =  null;
                }
            }

            BackgroundColor.Dispose();
            SelectedColor.Dispose();
            Ripple.Dispose();

            Background?.Dispose();
            Background = null;
        }

        base.Dispose(disposing);
    }
}

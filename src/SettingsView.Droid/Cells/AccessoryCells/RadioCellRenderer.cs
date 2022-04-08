[assembly: ExportRenderer(typeof(RadioCell), typeof(RadioCellRenderer))]


namespace Jakar.SettingsView.Droid.Cells;

[Preserve(AllMembers = true)]
public class RadioCellRenderer : CellBaseRenderer<RadioCellView> { }



[Preserve(AllMembers = true)]
public class RadioCellView : BaseAiAccessoryCell<RadioCheck>
{
    protected RadioCell _RadioCell => Cell as RadioCell ?? throw new NullReferenceException(nameof(_RadioCell));


    protected internal object? SelectedValue
    {
        get => _RadioCell.Section?.GetSelectedValue() ?? CellParent?.GetSelectedValue();
        set
        {
            if ( _RadioCell.Section?.GetSelectedValue() is not null ) { _RadioCell.Section.SetSelectedValue(value); }
            else { CellParent?.SetSelectedValue(value); }
        }
    }


    public RadioCellView( Context context,       Cell               cell ) : base(context, cell) => _Accessory.Focusable = false;
    public RadioCellView( IntPtr  javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) => _Accessory.Focusable = false;


    protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
    {
        if ( e.PropertyName == CheckableCellBase.accentColorProperty.PropertyName ) { UpdateAccentColor(); }
        else { base.CellPropertyChanged(sender, e); }
    }

    protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
    {
        if ( e.PropertyName == Shared.sv.SettingsView.cellAccentColorProperty.PropertyName ) { UpdateAccentColor(); }
        else if ( e.PropertyName == RadioCell.selectedValueProperty.PropertyName ) { UpdateSelectedValue(); }
        else { base.ParentPropertyChanged(sender, e); }
    }

    protected internal override void SectionPropertyChanged( object sender, PropertyChangedEventArgs e )
    {
        base.SectionPropertyChanged(sender, e);
        if ( e.PropertyName == RadioCell.selectedValueProperty.PropertyName ) { UpdateSelectedValue(); }
    }

    protected internal override void RowSelected( SettingsViewRecyclerAdapter adapter, int position )
    {
        if ( !_Accessory.Checked ) { SelectedValue = _RadioCell.Value; }
    }


    protected override void EnableCell()
    {
        base.EnableCell();
        _Title.Enable();
        _Description.Enable();
        _Accessory.Enabled = true;
        _Accessory.Alpha   = SvConstants.Cell.ENABLED_ALPHA;
    }

    protected override void DisableCell()
    {
        base.DisableCell();
        _Title.Disable();
        _Description.Disable();
        _Accessory.Enabled = false;
        _Accessory.Alpha   = SvConstants.Cell.DISABLED_ALPHA;
    }

    protected internal override void UpdateCell()
    {
        UpdateAccentColor();
        UpdateSelectedValue();
        base.UpdateCell();
    }

    private void UpdateSelectedValue()
    {
        if ( _RadioCell.Value is null )
        {
            _Accessory.Checked = false;
            return;
        }

        _Accessory.Checked = _RadioCell.Value.GetType().IsValueType
                                 ? Equals(_RadioCell.Value, SelectedValue)
                                 : ReferenceEquals(_RadioCell.Value, SelectedValue);
    }

    private void UpdateAccentColor()
    {
        _Accessory.Color = _RadioCell.CheckableConfig.AccentColor.ToAndroid();

        Invalidate();
    }


    protected override void Dispose( bool disposing )
    {
        if ( disposing )
        {
            _Accessory.RemoveFromParent();
            _Accessory.Dispose();

            _Title.Dispose();

            _Description.Dispose();

            _CellLayout.Dispose();
            _AccessoryStack.Dispose();
        }

        base.Dispose(disposing);
    }
}
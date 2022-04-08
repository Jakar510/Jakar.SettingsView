[assembly: ExportRenderer(typeof(CustomCell), typeof(CustomCellRenderer))]


namespace Jakar.SettingsView.Droid.Cells;

[Preserve(AllMembers = true)] public class CustomCellRenderer : CellBaseRenderer<CustomCellView> { }


[Preserve(AllMembers = true)]
public class CustomCellView : BaseAiDescriptionCell
{
    protected CustomCell _CustomCell => Cell as CustomCell ?? throw new NullReferenceException(nameof(_CustomCell));

    protected Action?   _Execute { get; set; }
    protected ICommand? _Command { get; set; }

    // protected ImageView _IndicatorView { get; set; }
    protected internal FormsViewContainer Container       { get; }
    protected          LinearLayout       _AccessoryStack { get; }

    public CustomCellView( Context context, Cell cell ) : base(context, cell)
    {
        RemoveHint();
        RemoveCellValueStack();
        _AccessoryStack = AccessoryStack();

        Container = new FormsViewContainer(AndroidContext, _CustomCell);
        this.Add(Container,
                 2,
                 0,
                 GridSpec.Fill,
                 GridSpec.Fill,
                 ALayout.Match
                );
        if ( !_CustomCell.ShowArrowIndicator )
        {
            // TODO: implement ShowArrowIndicator (_IndicatorView) _AccessoryStack
        }

        if ( !_CustomCell.UseFullSize ) return;
        _Icon.RemoveFromParent();
        _Title.RemoveFromParent();
        _Description.RemoveFromParent();

        var rMargin = (int) ( _CustomCell.ShowArrowIndicator ? AndroidContext.ToPixels(10) : 0 );
        Container.SetPadding(0, 0, rMargin, 0);
        _CellLayout.SetPadding(0, 0, 0, 0);
    }
    public CustomCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }


    protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
    {
        base.CellPropertyChanged(sender, e);

        if ( e.PropertyName == CommandCell.commandProperty.PropertyName ||
             e.PropertyName == CommandCell.commandParameterProperty.PropertyName ) { UpdateCommand(); }
    }
    // protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e ) { base.ParentPropertyChanged(sender, e); }


    protected internal override void RowSelected( SettingsViewRecyclerAdapter adapter, int position )
    {
        if ( !_CustomCell.IsSelectable ) { return; }

        _Execute?.Invoke();
        if ( _CustomCell.KeepSelectedUntilBack ) { adapter.SelectedRow(this, position); }
    }
    protected internal override bool RowLongPressed( SettingsViewRecyclerAdapter adapter, int position )
    {
        if ( _CustomCell.LongCommand == null ) { return false; }

        _CustomCell.SendLongCommand();

        return true;
    }

    protected internal override void UpdateCell()
    {
        base.UpdateCell();
        UpdateContent();
        UpdateCommand();
    }
    public void UpdateContent()
    {
        Container.CustomCell = _CustomCell;
        Container.FormsView  = _CustomCell.Content;
        double height     = Container.FormsView?.Height ?? 0; 
        double cellHeight = Height;
        System.Diagnostics.Debug.WriteLine($"_______CustomHeight_______  content.height {height}      cell.height{cellHeight}");
    }
    private void UpdateCommand()
    {
        if ( _Command != null ) { _Command.CanExecuteChanged -= Command_CanExecuteChanged; }

        _Command = _CustomCell.Command;

        if ( _Command != null )
        {
            _Command.CanExecuteChanged += Command_CanExecuteChanged;
            Command_CanExecuteChanged(_Command, EventArgs.Empty);
        }

        _Execute = () =>
                   {
                       if ( _Command == null ) { return; }

                       if ( _Command.CanExecute(_CustomCell.CommandParameter) ) { _Command.Execute(_CustomCell.CommandParameter); }
                   };
    }
    protected override void UpdateIsEnabled()
    {
        if ( _Command != null &&
             !_Command.CanExecute(_CustomCell.CommandParameter) ) { return; }

        base.UpdateIsEnabled();
    }

    private void Command_CanExecuteChanged( object sender, EventArgs e )
    {
        if ( !Cell.IsEnabled ) { return; }

        SetEnabledAppearance(_Command?.CanExecute(_CustomCell.CommandParameter) ?? true);
    }


    protected override void Dispose( bool disposing )
    {
        if ( disposing )
        {
            if ( _Command != null ) { _Command.CanExecuteChanged -= Command_CanExecuteChanged; }

            _Execute = null;
            _Command = null;
            // _IndicatorView?.RemoveFromParent();
            // _IndicatorView?.SetImageDrawable(null);
            // _IndicatorView?.SetImageBitmap(null);
            // _IndicatorView?.Dispose();

            // _CoreView?.RemoveFromParent();
            // _CoreView?.Dispose();

            _Icon.Dispose();
            _Title.Dispose();
            _Description.Dispose();
            Container.RemoveFromParent();
            Container.Dispose();
        }

        base.Dispose(disposing);
    }
}
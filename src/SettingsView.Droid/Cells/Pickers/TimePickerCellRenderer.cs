[assembly: ExportRenderer(typeof(TimePickerCell), typeof(TimePickerCellRenderer))]


namespace Jakar.SettingsView.Droid.Cells;

[Preserve(AllMembers = true)] public class TimePickerCellRenderer : CellBaseRenderer<TimePickerCellView> { }

[Preserve(AllMembers = true)]
public class TimePickerCellView : BaseAiValueCell
{
    protected TimePickerCell    _TimePickerCell => Cell as TimePickerCell ?? throw new NullReferenceException(nameof(_TimePickerCell));
    protected TimePickerDialog? _Dialog         { get; set; }
    protected string            _PopupTitle     { get; set; } = string.Empty;


    public TimePickerCellView( Context context,       Cell               cell ) : base(context, cell) { }
    public TimePickerCellView( IntPtr  javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }


    protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
    {
        if ( e.PropertyName == TimePickerCell.timeProperty.PropertyName ||
             e.PropertyName == TimePickerCell.formatProperty.PropertyName ) { UpdateTime(); }
        else if ( e.PropertyName == PopupConfig.titleProperty.PropertyName ) { UpdatePopupTitle(); }
        else { base.CellPropertyChanged(sender, e); }
    }
    protected internal override void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { CreateDialog(); }


    protected internal override void UpdateCell()
    {
        base.UpdateCell();
        UpdateTime();
        UpdatePopupTitle();
    }
    private void CreateDialog()
    {
        if ( _Dialog != null ) return;
        _Dialog = new TimePickerDialog(AndroidContext,
                                       TimeSelected,
                                       _TimePickerCell.Time.Hours,
                                       _TimePickerCell.Time.Minutes,
                                       DateFormat.Is24HourFormat(AndroidContext)
                                      );

        var title = new TextView(AndroidContext)
                    {
                        Gravity = GravityFlags.Center,
                        Text    = string.IsNullOrEmpty(_PopupTitle) ? "Select Time" : _PopupTitle,
                    };
			
        title.SetBackgroundColor(_TimePickerCell.Prompt.BackgroundColor.ToAndroid());
        title.SetTextColor(_TimePickerCell.Prompt.TitleColor.ToAndroid());
        title.SetPadding(10, 10, 10, 10);
			
        _Dialog.SetCustomTitle(title);
        _Dialog.SetCanceledOnTouchOutside(true);

        _Dialog.DismissEvent += ( sender, e ) =>
                                {
                                    title.Dispose();
                                    _Dialog.Dispose();
                                    _Dialog = null;
                                };

        _Dialog.Show();
    }
    private void UpdateTime() { _Value.Text       = DateTime.Today.Add(_TimePickerCell.Time).ToString(_TimePickerCell.Format); }
    private void UpdatePopupTitle() { _PopupTitle = _TimePickerCell.Prompt.Title; }
    private void TimeSelected( object sender, TimePickerDialog.TimeSetEventArgs e )
    {
        _TimePickerCell.Time = new TimeSpan(e.HourOfDay, e.Minute, 0);
        UpdateTime();
    }

    protected override void Dispose( bool disposing )
    {
        if ( disposing )
        {
            _Dialog?.Dispose();
            _Dialog = null;
        }

        base.Dispose(disposing);
    }
}
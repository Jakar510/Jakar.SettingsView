using EntryCellRenderer = Jakar.SettingsView.Droid.Cells.EntryCellRenderer;


[assembly: ExportRenderer(typeof(EntryCell), typeof(EntryCellRenderer))]


namespace Jakar.SettingsView.Droid.Cells;

[Preserve(AllMembers = true)]
public class EntryCellRenderer : CellBaseRenderer<EntryCellView> { }



[Preserve(AllMembers = true)]
public class EntryCellView : BaseValueCell<AiEditText>, IEntryCellRenderer
{
    protected EntryCell _EntryCell => Cell as EntryCell ?? throw new NullReferenceException(nameof(_EntryCell));

    public EntryCellView( Context context, Cell cell ) : base(context, cell)
    {
        Click              += EntryCellView_Click;
        _EntryCell.Focused += EntryCell_Focused;
        _Value.Init(_EntryCell, this);
    }

    public EntryCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)
    {
        _Value.Init(_EntryCell, this);

        Click              += EntryCellView_Click;
        _EntryCell.Focused += EntryCell_Focused;
    }

    protected void EntryCellView_Click( object sender, EventArgs e )
    {
        RequestFocus();
        _Value.PerformSelectAction();
        ShowKeyboard(_Value); // EntryCellView_Click
    }

    protected void EntryCell_Focused( object sender, EventArgs e )
    {
        RequestFocus();
        ShowKeyboard(_Value);
    }


    void ITextWatcher.AfterTextChanged( IEditable? s ) { }

    void ITextWatcher.BeforeTextChanged( ICharSequence? s,
                                         int            start,
                                         int            count,
                                         int            after
    ) { }

    void ITextWatcher.OnTextChanged( ICharSequence? s,
                                     int            start,
                                     int            before,
                                     int            count
    )
    {
        if ( string.IsNullOrEmpty(_EntryCell.ValueText) &&
             s != null &&
             s.Length() == 0 ) { return; }

        _EntryCell.ValueText = s?.ToString();
    }

    void IOnFocusChangeListener.OnFocusChange( Android.Views.View? v, bool hasFocus )
    {
        if ( hasFocus )
        {
            if ( Background != null )
                Background.Alpha = 100; // show underline when on focus.
        }
        else
        {
            if ( Background != null )
                Background.Alpha = 0; // hide underline

            _EntryCell.SendCompleted(); // consider as text input completed.
        }
    }

    bool TextView.IOnEditorActionListener.OnEditorAction( TextView? v, ImeAction actionId, KeyEvent? e )
    {
        if ( v is null )
            return true;

        if ( actionId != ImeAction.Done &&
             ( actionId != ImeAction.ImeNull || ( e != null && e.KeyCode != Keycode.Enter ) ) )
            return true;

        HideKeyboard(v);
        DoneEdit();

        return true;
    }

    public void DoneEdit()
    {
        _EntryCell.SendCompleted();
        ClearFocus();
    }


    protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
    {
        base.CellPropertyChanged(sender, e);

        _Value.Update(sender, e);
        _Hint.Update(sender, e);
    }

    protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
    {
        base.ParentPropertyChanged(sender, e);

        _Value.UpdateParent(sender, e);
        _Hint.UpdateParent(sender, e);
    }


    protected override void EnableCell()
    {
        base.EnableCell();
        _Value.Enable();
        _Hint.Enable();
    }

    protected override void DisableCell()
    {
        base.DisableCell();
        _Value.Disable();
        _Hint.Disable();
    }

    protected internal override void UpdateCell()
    {
        base.UpdateCell();
        _Hint.Update();
        _Value.Update();
    }


    protected override void Dispose( bool disposing )
    {
        if ( disposing )
        {
            Click              -= EntryCellView_Click;
            _EntryCell.Focused -= EntryCell_Focused;
            _Value.RemoveFromParent();
            _Value.Dispose();
            _Hint.Dispose();
            _CellValueStack.Dispose();
            OnFocusChangeListener = null;
        }

        base.Dispose(disposing);
    }
}

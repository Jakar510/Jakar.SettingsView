namespace Jakar.SettingsView.Droid.BaseCell;

[Preserve(AllMembers = true)]
public abstract class BaseValueCell<TCell> : BaseAiDescriptionCell where TCell : AView
{
    protected HintView     _Hint           { get; }
    protected TCell        _Value          { get; }
    protected LinearLayout _CellValueStack { get; }


    protected BaseValueCell( AContext context, Cell cell ) : base(context, cell)
    {
        RemoveAccessoryStack();
        _Hint           = BaseTextView.Create<HintView>(ContentView, this, Resource.Id.CellHint);
        _CellValueStack = ValueStack();

        _Value = InstanceCreator<TCell>.Create(this, AndroidContext);

        // _Value = InstanceCreator<BaseCellView, AContext, TCell>.Create(this, AndroidContext);
        _CellValueStack.Add(_Value, ALayout.Match, ALayout.Wrap, GravityFlags.Fill | GravityFlags.Center);
    }

    protected BaseValueCell( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }
}

// public abstract class BaseValueCell : BaseAiDescriptionCell
// {
// 	protected HintView _Hint { get; }
// 	protected ValueView _Value { get; }
//
//
// 	protected BaseValueCell( AContext context, Cell cell ) : base(context, cell)
// 	{
// 		RemoveAccessoryStack();
// 		RemoveEntryValue();
// 		_Hint = BaseTextView.Create<HintView>(ContentView, this, Resource.Id.CellHint);
// 		_Value = BaseTextView.Create<ValueView>(ContentView, this, Resource.Id.CellValue);
// 	}
// 	protected BaseValueCell( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }
// }


namespace Jakar.SettingsView.Droid.BaseCell;

[Preserve(AllMembers = true)]
public abstract class BaseAiAccessoryCell<TAccessory> : BaseAiDescriptionCell where TAccessory : AView
{
    protected LinearLayout _AccessoryStack { get; }
    protected TAccessory   _Accessory      { get; }


    protected BaseAiAccessoryCell( AContext context, Cell cell ) : base(context, cell)
    {
        RemoveHint();
        RemoveCellValueStack();

        _AccessoryStack = AccessoryStack();
        _Accessory      = InstanceCreator<AContext, TAccessory>.Create(AndroidContext);
        _AccessoryStack.Add(_Accessory, ALayout.Wrap, ALayout.Wrap, GravityFlags.Center);
    }

    protected BaseAiAccessoryCell( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }



    protected override void Dispose( bool disposing )
    {
        base.Dispose(disposing);
        _Accessory.Dispose();
        _AccessoryStack.Dispose();
    }
}
namespace Jakar.SettingsView.Droid;

[Preserve(AllMembers = true)]
// public class SettingsViewLayoutManager : GridLayoutManager
public class SettingsViewLayoutManager : LinearLayoutManager
{
    protected Shared.sv.SettingsView?             _SettingsView { get; set; }
    protected Context?                            _Context      { get; set; }
    protected Dictionary<Android.Views.View, int> _ItemHeights  { get; } = new Dictionary<Android.Views.View, int>();


    // public SettingsViewLayoutManager( Context context, int spanCount ) : base(context, spanCount) { _Context = context; }
    // public SettingsViewLayoutManager( Context context,
    // 								  IAttributeSet attrs,
    // 								  int defStyleAttr,
    // 								  int defStyleRes ) : base(context, attrs, defStyleAttr, defStyleRes)
    // {
    // 	_Context = context ?? throw new NullReferenceException(nameof(context));
    // }
    // public SettingsViewLayoutManager( Context context,
    // 								  int spanCount,
    // 								  int orientation,
    // 								  bool reverseLayout ) : base(context, spanCount, orientation, reverseLayout)
    // {
    // 	_Context = context ?? throw new NullReferenceException(nameof(context));
    // }
    public SettingsViewLayoutManager( Context context, Shared.sv.SettingsView settingsView ) : base(context) //, 1, Vertical, false)
    {
        _Context      = context ?? throw new NullReferenceException(nameof(context));
        _SettingsView = settingsView ?? throw new NullReferenceException(nameof(settingsView));
    }
    public SettingsViewLayoutManager( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }


    public override int GetDecoratedMeasuredHeight( Android.Views.View child )
    {
        int height = base.GetDecoratedMeasuredHeight(child);
        _ItemHeights[child] = height;
        return height;
    }

    public override void OnLayoutCompleted( RecyclerView.State state )
    {
        base.OnLayoutCompleted(state);

        int total = _ItemHeights.Sum(x => x.Value);

        if ( _SettingsView != null ) _SettingsView.VisibleContentHeight = _Context.FromPixels(total);
    }


    protected override void Dispose( bool disposing )
    {
        if ( disposing )
        {
            _ItemHeights.Clear();
            _Context      = null;
            _SettingsView = null;
        }

        base.Dispose(disposing);
    }
		
		
}
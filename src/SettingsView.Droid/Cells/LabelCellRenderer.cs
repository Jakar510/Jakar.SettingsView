[assembly: ExportRenderer(typeof(LabelCell), typeof(LabelCellRenderer))]


namespace Jakar.SettingsView.Droid.Cells;

[Preserve(AllMembers = true)] public class LabelCellRenderer : CellBaseRenderer<LabelCellView> { }

[Preserve(AllMembers = true)]
public class LabelCellView : BaseAiValueCell
{
    protected LabelCell _LabelCell => Cell as LabelCell ?? throw new NullReferenceException(nameof(_LabelCell));


    public LabelCellView( Context context, Cell cell ) : base(context, cell) { }

    public LabelCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }
}
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.NewCells;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(LabelCell), typeof(LabelCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.NewCells
{
	public class LabelCellRenderer : CellBaseRenderer<LabelCellView> { }

	[Foundation.Preserve(AllMembers = true)]
	public class LabelCellView : BaseAiValueCell
	{
		public LabelCellView( Cell cell ) : base(cell) { }
	}
}
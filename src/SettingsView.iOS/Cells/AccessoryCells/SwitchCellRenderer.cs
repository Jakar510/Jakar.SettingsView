using AiSwitchCellRenderer = Jakar.SettingsView.iOS.Cells.SwitchCellRenderer;

[assembly: ExportRenderer(typeof(SwitchCell), typeof(AiSwitchCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Foundation.Preserve(AllMembers = true)]
	public class SwitchCellRenderer : CellBaseRenderer<SwitchCellView> { }

	[Foundation.Preserve(AllMembers = true)]
	public class SwitchCellView : BaseAccessoryCell<SwitchCell, AiSwitch>
	{
		public SwitchCellView( SwitchCell formsCell ) : base(formsCell) { }
		
		protected override void Dispose( bool disposing )
		{
			if ( disposing ) { }

			base.Dispose(disposing);
		}
	}
}
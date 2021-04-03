using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Controls;
using Jakar.SettingsView.Shared.Config;
using Xamarin.Forms;
using AiSwitchCellRenderer = Jakar.SettingsView.iOS.Cells.SwitchCellRenderer;
using AiSwitchCell = Jakar.SettingsView.Shared.Cells.SwitchCell;

[assembly: ExportRenderer(typeof(AiSwitchCell), typeof(AiSwitchCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Foundation.Preserve(AllMembers = true)]
	public class SwitchCellRenderer : CellBaseRenderer<SwitchCellView> { }

	[Foundation.Preserve(AllMembers = true)]
	public class SwitchCellView : BaseAccessoryCell<AiSwitchCell, AiSwitch>
	{
		public SwitchCellView( AiSwitchCell formsCell ) : base(formsCell) { }
		
		protected override void Dispose( bool disposing )
		{
			if ( disposing ) { }

			base.Dispose(disposing);
		}
	}
}
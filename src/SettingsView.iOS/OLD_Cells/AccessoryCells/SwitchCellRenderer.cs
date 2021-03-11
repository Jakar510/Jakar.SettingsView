using System;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.Shared.CellBase;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using AiSwitchCellRenderer = Jakar.SettingsView.iOS.OLD_Cells.SwitchCellRenderer;
using AiSwitchCell = Jakar.SettingsView.Shared.Cells.SwitchCell;

// [assembly: ExportRenderer(typeof(AiSwitchCell), typeof(AiSwitchCellRenderer))]

namespace Jakar.SettingsView.iOS.OLD_Cells
{
	// [Foundation.Preserve(AllMembers = true)] public class SwitchCellRenderer : CellBaseRenderer<SwitchCellView> { }

	[Foundation.Preserve(AllMembers = true)]
	public class SwitchCellView : CellBaseView
	{
		private AiSwitchCell _SwitchCell => Cell as AiSwitchCell;
		private UISwitch _switch;

		public SwitchCellView( Cell formsCell ) : base(formsCell)
		{
			_switch = new UISwitch();
			_switch.ValueChanged += Switch_ValueChanged;

			AccessoryView = _switch;
			EditingAccessoryView = _switch;
		}

		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == CheckableCellBase.AccentColorProperty.PropertyName ) { UpdateAccentColor(); }

			if ( e.PropertyName == AiSwitchCell.CheckedProperty.PropertyName ) { UpdateOn(); }
		}

		public override void ParentPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);
			if ( e.PropertyName == Shared.sv.SettingsView.CellAccentColorProperty.PropertyName ) { UpdateAccentColor(); }
		}

		public override void UpdateCell( UITableView tableView )
		{
			base.UpdateCell(tableView);
			if ( _switch is null )
				return; // for HotReload

			UpdateAccentColor();
			UpdateOn();
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_switch.ValueChanged -= Switch_ValueChanged;
				AccessoryView = null;
				_switch?.Dispose();
				_switch = null;
			}

			base.Dispose(disposing);
		}

		protected override void SetEnabledAppearance( bool isEnabled )
		{
			if ( isEnabled ) { _switch.Alpha = 1.0f; }
			else { _switch.Alpha = 0.3f; }

			base.SetEnabledAppearance(isEnabled);
		}

		private void Switch_ValueChanged( object sender, EventArgs e ) { _SwitchCell.Checked = _switch.On; }

		private void UpdateOn()
		{
			if ( _switch.On != _SwitchCell.Checked ) { _switch.On = _SwitchCell.Checked; }
		}

		private void UpdateAccentColor()
		{
			if ( _SwitchCell.AccentColor != Color.Default ) { _switch.OnTintColor = _SwitchCell.AccentColor.ToUIColor(); }
			else if ( CellParent != null &&
					  CellParent.CellAccentColor != Color.Default ) { _switch.OnTintColor = CellParent.CellAccentColor.ToUIColor(); }
		}
	}
}
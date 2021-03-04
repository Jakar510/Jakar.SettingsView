using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using AiSwitchCellRenderer = Jakar.SettingsView.iOS.Cells.SwitchCellRenderer;
using AiSwitchCell = Jakar.SettingsView.Shared.Cells.SwitchCell;

[assembly: ExportRenderer(typeof(AiSwitchCell), typeof(AiSwitchCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	/// <summary>
	/// Switch cell renderer.
	/// </summary>
	[Foundation.Preserve(AllMembers = true)]
	public class SwitchCellRenderer : CellBaseRenderer<SwitchCellView> { }

	/// <summary>
	/// Switch cell view.
	/// </summary>
	[Foundation.Preserve(AllMembers = true)]
	public class SwitchCellView : CellBaseView
	{
		private AiSwitchCell _SwitchCell => Cell as AiSwitchCell;
		private UISwitch _switch;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.iOS.Cells.SwitchCellView"/> class.
		/// </summary>
		/// <param name="formsCell">Forms cell.</param>
		public SwitchCellView( Cell formsCell ) : base(formsCell)
		{
			_switch = new UISwitch();
			_switch.ValueChanged += _switch_ValueChanged;

			AccessoryView = _switch;
			EditingAccessoryView = _switch;
		}

		/// <summary>
		/// Cells the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == AiSwitchCell.AccentColorProperty.PropertyName ) { UpdateAccentColor(); }

			if ( e.PropertyName == AiSwitchCell.OnProperty.PropertyName ) { UpdateOn(); }
		}

		/// <summary>
		/// Parents the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void ParentPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);
			if ( e.PropertyName == Shared.sv.SettingsView.CellAccentColorProperty.PropertyName ) { UpdateAccentColor(); }
		}

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell( UITableView tableView )
		{
			base.UpdateCell(tableView);
			if ( _switch is null )
				return; // for HotReload

			UpdateAccentColor();
			UpdateOn();
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <returns>The dispose.</returns>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_switch.ValueChanged -= _switch_ValueChanged;
				AccessoryView = null;
				_switch?.Dispose();
				_switch = null;
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// Sets the enabled appearance.
		/// </summary>
		/// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
		protected override void SetEnabledAppearance( bool isEnabled )
		{
			if ( isEnabled ) { _switch.Alpha = 1.0f; }
			else { _switch.Alpha = 0.3f; }

			base.SetEnabledAppearance(isEnabled);
		}

		private void _switch_ValueChanged( object sender, EventArgs e ) { _SwitchCell.On = _switch.On; }

		private void UpdateOn()
		{
			if ( _switch.On != _SwitchCell.On ) { _switch.On = _SwitchCell.On; }
		}

		private void UpdateAccentColor()
		{
			if ( _SwitchCell.AccentColor != Color.Default ) { _switch.OnTintColor = _SwitchCell.AccentColor.ToUIColor(); }
			else if ( CellParent != null &&
					  CellParent.CellAccentColor != Color.Default ) { _switch.OnTintColor = CellParent.CellAccentColor.ToUIColor(); }
		}
	}
}
using System;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Misc;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.PlatformConfiguration;
using SwitchCell = Jakar.SettingsView.Shared.Cells.SwitchCell;
using SwitchCellRenderer = Jakar.SettingsView.iOS.Cells.SwitchCellRenderer;

[assembly: ExportRenderer(typeof(SwitchCell), typeof(SwitchCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)]
	public class SwitchCellRenderer : CellBaseRenderer<SwitchCellView> { }

	[Preserve(AllMembers = true)]
	public class SwitchCellView : BaseAiAccessoryCell<UISwitch>
	{
		protected SwitchCell _AccessoryCell => Cell as SwitchCell ?? throw new NullReferenceException(nameof(_AccessoryCell));

		public SwitchCellView( Cell cell ) : base(cell) { _Accessory.ValueChanged += AccessoryOnValueChanged; }
		protected void AccessoryOnValueChanged( object sender, EventArgs e )
		{
			_AccessoryCell.Checked = _Accessory.On;
			_AccessoryCell.ValueChangedHandler.SendValueChanged(_Accessory.On);
		}


		protected internal override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			if ( e.IsOneOf(Shared.sv.SettingsView.CellAccentColorProperty, Shared.sv.SettingsView.CellOffColorProperty) ) { UpdateAccentColor(); }
			else if ( e.IsEqual(CheckableCellBase.CheckedProperty) ) { UpdateOn(); }
			else { base.CellPropertyChanged(sender, e); }
		}
		protected internal override void ParentPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			if ( e.IsOneOf(Shared.sv.SettingsView.CellAccentColorProperty, Shared.sv.SettingsView.CellOffColorProperty) ) { UpdateAccentColor(); }
			else { base.ParentPropertyChanged(sender, e); }
		}


		protected internal override void RowSelected( UITableView tableView, NSIndexPath indexPath ) { _Accessory.On = !_Accessory.On; }

		protected override void EnableCell()
		{
			base.EnableCell();
			_Title.Enable();
			_Description.Enable();
			_Accessory.Enabled = true;
			_Accessory.Alpha = SVConstants.Cell.ENABLED_ALPHA;
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Title.Disable();
			_Description.Disable();
			_Accessory.Enabled = false;
			_Accessory.Alpha = SVConstants.Cell.DISABLED_ALPHA;
		}

		protected internal override void UpdateCell()
		{
			UpdateAccentColor();
			UpdateOn();
			base.UpdateCell();
		}
		private void UpdateOn() { _Accessory.On = _AccessoryCell.Checked; }

		private void UpdateAccentColor() { ChangeCheckColor(_AccessoryCell.GetAccentColor(), _AccessoryCell.GetOffColor()); }

		protected void ChangeCheckColor( Color accent ) { ChangeCheckColor(accent, Color.FromRgba(117, 117, 117, 76)); }
		protected void ChangeCheckColor( Color accent, Color off )
		{
			_Accessory.TintColor = accent.ToUIColor();
			_Accessory.OnTintColor = accent.ToUIColor();
			_Accessory.ThumbTintColor = off.ToUIColor();
			_Accessory.BackgroundColor = Color.Transparent.ToUIColor();
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing ) { _Accessory.ValueChanged -= AccessoryOnValueChanged; }

			base.Dispose(disposing);
		}
	}
}
using System;
using System.ComponentModel;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Controls;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Cells;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CheckboxCell), typeof(CheckboxCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.Cells
{
	[Foundation.Preserve(AllMembers = true)]
	public class CheckboxCellRenderer : CellBaseRenderer<CheckboxCellView> { }

	[Foundation.Preserve(AllMembers = true)]
	public class CheckboxCellView : BaseAiAccessoryCell<SimpleCheck>
	{
		protected CheckboxCell _AccessoryCell => Cell as CheckboxCell ?? throw new NullReferenceException(nameof(_AccessoryCell));


		public CheckboxCellView( Cell cell ) : base(cell) => _Accessory.ValueChanged += AccessoryOnValueChanged;
		protected void AccessoryOnValueChanged( object sender, EventArgs e )
		{
			_AccessoryCell.Checked = _Accessory.IsChecked;
			_AccessoryCell.ValueChangedHandler.SendValueChanged(_Accessory.IsChecked);
		}

		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CheckableCellBase.AccentColorProperty.PropertyName ) { UpdateAccentColor(); }

			else if ( e.PropertyName == CheckableCellBase.CheckedProperty.PropertyName ) { UpdateChecked(); }
			else { base.CellPropertyChanged(sender, e); }

			// if ( e.PropertyName == LabelCell.ValueTextFontSizeProperty.PropertyName ) { UpdateValueTextFontSize(); }
		}
		protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.sv.SettingsView.CellAccentColorProperty.PropertyName ) { UpdateAccentColor(); }
			else { base.ParentPropertyChanged(sender, e); }
		}

		protected internal override void RowSelected( UITableView tableView, NSIndexPath indexPath ) { _Accessory.IsChecked = !_Accessory.IsChecked; }


		protected override void EnableCell()
		{
			base.EnableCell();
			_Title.Enable();
			_Description.Enable();
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Title.Disable();
			_Description.Disable();
		}
		public void OnCheckedChanged( SimpleCheck? buttonView, bool isChecked )
		{
			_AccessoryCell.Checked = isChecked;
			// buttonView?.JumpDrawablesToCurrentState();
		}

		protected internal override void UpdateCell()
		{
			UpdateAccentColor();
			UpdateChecked();
			base.UpdateCell();
		}

		protected void UpdateChecked() { _Accessory.IsChecked = _AccessoryCell.Checked; }
		protected void UpdateAccentColor() { ChangeCheckColor(_AccessoryCell.GetAccentColor(), _AccessoryCell.GetOffColor()); }


		protected void ChangeCheckColor( Color accent, Color off )
		{
			_Accessory.CheckBoxTintColor = accent;
			// _Accessory.OffColor = off;
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing ) { _Accessory.ValueChanged -= AccessoryOnValueChanged; }

			base.Dispose(disposing);
		}
	}
}
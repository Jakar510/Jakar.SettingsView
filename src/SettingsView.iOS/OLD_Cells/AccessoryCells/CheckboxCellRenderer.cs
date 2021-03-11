using Foundation;
using CoreGraphics;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.OLD_Cells;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Cells;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CheckBox = Jakar.SettingsView.iOS.Controls.CheckBox;

// [assembly: ExportRenderer(typeof(CheckboxCell), typeof(CheckboxCellRenderer))]

namespace Jakar.SettingsView.iOS.OLD_Cells
{
	// [Foundation.Preserve(AllMembers = true)] public class CheckboxCellRenderer : CellBaseRenderer<CheckboxCellView> { }

	[Preserve(AllMembers = true)]
	public class CheckboxCellView : CellBaseView
	{
		private CheckBox _checkbox;
		private CheckboxCell _CheckboxCell => Cell as CheckboxCell;

		public CheckboxCellView( Cell formsCell ) : base(formsCell)
		{
			_checkbox = new CheckBox(new CGRect(0, 0, 20, 20));
			_checkbox.Layer.BorderWidth = 2;
			_checkbox.Layer.CornerRadius = 3;
			_checkbox.Inset = new UIEdgeInsets(10, 10, 10, 10);

			_checkbox.CheckChanged = CheckChanged;

			AccessoryView = _checkbox;
			EditingAccessoryView = _checkbox;
		}

		public override void UpdateCell( UITableView tableView )
		{
			UpdateAccentColor();
			UpdateChecked();
			base.UpdateCell(tableView);
		}

		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == CheckableCellBase.AccentColorProperty.PropertyName ) { UpdateAccentColor(); }

			if ( e.PropertyName == CheckableCellBase.CheckedProperty.PropertyName ) { UpdateChecked(); }
		}

		public override void ParentPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);
			if ( e.PropertyName == Shared.sv.SettingsView.CellAccentColorProperty.PropertyName ) { UpdateAccentColor(); }
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_checkbox.CheckChanged = null;
				_checkbox?.Dispose();
				_checkbox = null;
			}

			base.Dispose(disposing);
		}

		protected override void SetEnabledAppearance( bool isEnabled )
		{
			if ( isEnabled ) { _checkbox.Alpha = 1.0f; }
			else { _checkbox.Alpha = 0.3f; }

			base.SetEnabledAppearance(isEnabled);
		}

		private void CheckChanged( UIButton button ) { _CheckboxCell.Checked = button.Selected; }

		private void UpdateChecked() { _checkbox.Selected = _CheckboxCell.Checked; }

		private void UpdateAccentColor()
		{
			if ( _CheckboxCell.AccentColor != Color.Default ) { ChangeCheckColor(_CheckboxCell.AccentColor.ToCGColor()); }
			else if ( CellParent != null &&
					  CellParent.CellAccentColor != Color.Default ) { ChangeCheckColor(CellParent.CellAccentColor.ToCGColor()); }
		}

		private void ChangeCheckColor( CGColor accent )
		{
			_checkbox.Layer.BorderColor = accent;
			_checkbox.FillColor = accent;
			_checkbox.SetNeedsDisplay(); //update inner rect
		}
	}
}
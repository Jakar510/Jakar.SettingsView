using System.ComponentModel;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Cells;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RadioCell), typeof(RadioCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)]
	public class RadioCellRenderer : CellBaseRenderer<RadioCellView> { }

	[Preserve(AllMembers = true)]
	public class RadioCellView : BaseCellView
	{
		protected RadioCell _RadioCell => Cell as RadioCell;

		protected object _SelectedValue
		{
			get { return RadioCell.GetSelectedValue(_RadioCell.Section) ?? RadioCell.GetSelectedValue(CellParent); }
			set
			{
				if ( RadioCell.GetSelectedValue(_RadioCell.Section) is not null ) { RadioCell.SetSelectedValue(_RadioCell.Section, value); }
				else { RadioCell.SetSelectedValue(CellParent, value); }
			}
		}

		public RadioCellView( Cell formsCell ) : base(formsCell) { SelectionStyle = UITableViewCellSelectionStyle.Default; }

		protected override void Dispose( bool disposing ) { base.Dispose(disposing); }

		public override void UpdateCell( UITableView tableView )
		{
			UpdateAccentColor();
			UpdateSelectedValue();
			base.UpdateCell(tableView);
		}

		public override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == CheckableCellBase.AccentColorProperty.PropertyName ) { UpdateAccentColor(); }
		}

		public override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);
			if ( e.PropertyName == Shared.sv.SettingsView.CellAccentColorProperty.PropertyName ) { UpdateAccentColor(); }
			else if ( e.PropertyName == RadioCell.SelectedValueProperty.PropertyName ) { UpdateSelectedValue(); }
		}

		public override void SectionPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.SectionPropertyChanged(sender, e);
			if ( e.PropertyName == RadioCell.SelectedValueProperty.PropertyName ) { UpdateSelectedValue(); }
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			if ( Accessory == UITableViewCellAccessory.None ) { _SelectedValue = _RadioCell.Value; }

			tableView.DeselectRow(indexPath, true);
		}

		protected void UpdateSelectedValue()
		{
			if ( _RadioCell.Value is null )
				return; // for HotReload

			bool result;
			if ( _RadioCell.Value.GetType().IsValueType ) { result = Equals(_RadioCell.Value, _SelectedValue); }
			else { result = ReferenceEquals(_RadioCell.Value, _SelectedValue); }

			Accessory = result
							? UITableViewCellAccessory.Checkmark
							: UITableViewCellAccessory.None;
		}

		protected void UpdateAccentColor()
		{
			if ( !_RadioCell.AccentColor.IsDefault ) { TintColor = _RadioCell.AccentColor.ToUIColor(); }
			else if ( CellParent is not null &&
					  !CellParent.CellAccentColor.IsDefault ) { TintColor = CellParent.CellAccentColor.ToUIColor(); }
		}
	}
}
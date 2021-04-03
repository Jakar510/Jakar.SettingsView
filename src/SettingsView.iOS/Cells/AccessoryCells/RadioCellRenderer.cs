using System;
using System.ComponentModel;
using Foundation;
using Jakar.Api.Extensions;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Controls;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Cells;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#nullable enable
[assembly: ExportRenderer(typeof(RadioCell), typeof(RadioCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)] public class RadioCellRenderer : CellBaseRenderer<RadioCellView> { }

	[Preserve(AllMembers = true)]
	public class RadioCellView : BaseAccessoryCell<RadioCell, RadioCheck>
	{
		protected internal object? SelectedValue
		{
			get => RadioCell.GetSelectedValue(Cell.Section) ?? RadioCell.GetSelectedValue(CellParent);
			set
			{
				if ( RadioCell.GetSelectedValue(Cell.Section) is not null ) { RadioCell.SetSelectedValue(Cell.Section, value); }
				else { RadioCell.SetSelectedValue(CellParent, value); }
			}
		}

		public RadioCellView( RadioCell formsCell ) : base(formsCell) { SelectionStyle = UITableViewCellSelectionStyle.Default; }

		protected override void Dispose( bool disposing ) { base.Dispose(disposing); }

		public override void UpdateCell( UITableView tableView )
		{
			UpdateAccentColor();
			UpdateSelectedValue();
			base.UpdateCell(tableView);
		}

		public override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(CheckableCellBase.AccentColorProperty) ) { UpdateAccentColor(); }
			else { base.CellPropertyChanged(sender, e); }
		}

		public override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(Shared.sv.SettingsView.CellAccentColorProperty) ) { UpdateAccentColor(); }
			else if ( e.IsEqual(RadioCell.SelectedValueProperty) ) { UpdateSelectedValue(); }
			else { base.ParentPropertyChanged(sender, e); }
		}

		public override void SectionPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(RadioCell.SelectedValueProperty) ) { UpdateSelectedValue(); }
			else { base.SectionPropertyChanged(sender, e); }
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			if ( Accessory == UITableViewCellAccessory.None )
			{
				SelectedValue = Cell.Value;
				_Accessory.Select();
			}

			tableView.DeselectRow(indexPath, true);
		}

		protected void UpdateSelectedValue()
		{
			if ( Cell.Value is null ) return; // for HotReload

			bool result = Cell.Value.GetType().IsValueType
							  ? Equals(Cell.Value, SelectedValue)
							  : ReferenceEquals(Cell.Value, SelectedValue);

			Accessory = result
							? UITableViewCellAccessory.Checkmark
							: UITableViewCellAccessory.None;
		}

		protected void UpdateAccentColor() { TintColor = Cell.GetAccentColor().ToUIColor(); }
	}
}
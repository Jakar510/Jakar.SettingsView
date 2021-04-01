using System;
using System.ComponentModel;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
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
	public class RadioCellView : BaseDescriptiveTitleCell
	{
		protected RadioCell _RadioCell => Cell as RadioCell ?? throw new NullReferenceException(nameof(_RadioCell));

		protected object? _SelectedValue
		{
			get => RadioCell.GetSelectedValue(_RadioCell.Section) ?? RadioCell.GetSelectedValue(CellParent);
			set
			{
				if ( RadioCell.GetSelectedValue(_RadioCell.Section) is not null ) { RadioCell.SetSelectedValue(_RadioCell.Section, value); }
				else { RadioCell.SetSelectedValue(CellParent, value); }
			}
		}

		public RadioCellView( Cell formsCell ) : base(formsCell) => SelectionStyle = UITableViewCellSelectionStyle.Default;

		protected override void Dispose( bool disposing ) { base.Dispose(disposing); }

		public override void UpdateCell( UITableView tableView )
		{
			UpdateAccentColor();
			UpdateSelectedValue();
			base.UpdateCell(tableView);
		}

		public override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CheckableCellBase.AccentColorProperty.PropertyName ) { UpdateAccentColor(); }
			else { base.CellPropertyChanged(sender, e); }
		}

		public override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.sv.SettingsView.CellAccentColorProperty.PropertyName ) { UpdateAccentColor(); }
			else if ( e.PropertyName == RadioCell.SelectedValueProperty.PropertyName ) { UpdateSelectedValue(); }
			else { base.ParentPropertyChanged(sender, e); }
		}

		public override void SectionPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == RadioCell.SelectedValueProperty.PropertyName ) { UpdateSelectedValue(); }
			else { base.SectionPropertyChanged(sender, e); }
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			if ( Accessory == UITableViewCellAccessory.None ) { _SelectedValue = _RadioCell.Value; }

			tableView.DeselectRow(indexPath, true);
		}

		protected void UpdateSelectedValue()
		{
			if ( _RadioCell.Value is null ) return; // for HotReload

			bool result = _RadioCell.Value.GetType().IsValueType
							  ? Equals(_RadioCell.Value, _SelectedValue)
							  : ReferenceEquals(_RadioCell.Value, _SelectedValue);

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
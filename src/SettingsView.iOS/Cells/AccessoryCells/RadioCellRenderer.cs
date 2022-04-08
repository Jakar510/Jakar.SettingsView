[assembly: ExportRenderer(typeof(RadioCell), typeof(RadioCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)] public class RadioCellRenderer : CellBaseRenderer<RadioCellView> { }

	[Preserve(AllMembers = true)]
	public class RadioCellView : BaseAccessoryCell<RadioCell, RadioCheck>
	{
		protected internal object? SelectedValue
		{
			get => Cell.Section?.GetSelectedValue() ?? CellParent?.GetSelectedValue();
			set
			{
				if ( Cell.Section?.GetSelectedValue() is not null ) { Cell.Section.SetSelectedValue(value); }
				else { CellParent?.SetSelectedValue(value); }
			}
		}

		public RadioCellView( RadioCell formsCell ) : base(formsCell) => SelectionStyle = UITableViewCellSelectionStyle.Default;

		protected override void Dispose( bool disposing ) { base.Dispose(disposing); }

		public override void UpdateCell( UITableView tableView )
		{
			UpdateAccentColor();
			UpdateSelectedValue();
			base.UpdateCell(tableView);
		}

		public override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(CheckableCellBase.accentColorProperty) ) { UpdateAccentColor(); }
			else { base.CellPropertyChanged(sender, e); }
		}

		public override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(Shared.sv.SettingsView.cellAccentColorProperty) ) { UpdateAccentColor(); }
			else if ( e.IsEqual(RadioCell.selectedValueProperty) ) { UpdateSelectedValue(); }
			else { base.ParentPropertyChanged(sender, e); }
		}

		public override void SectionPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(RadioCell.selectedValueProperty) ) { UpdateSelectedValue(); }
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

		protected void UpdateAccentColor() { TintColor = Cell.CheckableConfig.AccentColor.ToUIColor(); }
	}
}
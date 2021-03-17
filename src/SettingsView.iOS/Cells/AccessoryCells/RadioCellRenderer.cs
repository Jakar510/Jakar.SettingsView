﻿using System.ComponentModel;
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
	/// <summary>
	/// Radio cell renderer.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class RadioCellRenderer : CellBaseRenderer<RadioCellView> { }

	/// <summary>
	/// Radio cell view.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class RadioCellView : BaseCellView 
	{
		private RadioCell _radioCell => Cell as RadioCell;

		private object SelectedValue
		{
			get { return RadioCell.GetSelectedValue(_radioCell.Section) ?? RadioCell.GetSelectedValue(CellParent); }
			set
			{
				if ( RadioCell.GetSelectedValue(_radioCell.Section) is not null ) { RadioCell.SetSelectedValue(_radioCell.Section, value); }
				else { RadioCell.SetSelectedValue(CellParent, value); }
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.iOS.Cells.RadioCellView"/> class.
		/// </summary>
		/// <param name="formsCell">Forms cell.</param>
		public RadioCellView( Cell formsCell ) : base(formsCell) { SelectionStyle = UITableViewCellSelectionStyle.Default; }

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose( bool disposing ) { base.Dispose(disposing); }

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell( UITableView tableView )
		{
			UpdateAccentColor();
			UpdateSelectedValue();
			base.UpdateCell(tableView);
		}

		/// <summary>
		/// Cells the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == CheckableCellBase.AccentColorProperty.PropertyName ) { UpdateAccentColor(); }
		}

		/// <summary>
		/// Parents the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);
			if ( e.PropertyName == Shared.sv.SettingsView.CellAccentColorProperty.PropertyName ) { UpdateAccentColor(); }
			else if ( e.PropertyName == RadioCell.SelectedValueProperty.PropertyName ) { UpdateSelectedValue(); }
		}

		/// <summary>
		/// Sections the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void SectionPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.SectionPropertyChanged(sender, e);
			if ( e.PropertyName == RadioCell.SelectedValueProperty.PropertyName ) { UpdateSelectedValue(); }
		}

		/// <summary>
		/// Rows the selected.
		/// </summary>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			if ( Accessory == UITableViewCellAccessory.None ) { SelectedValue = _radioCell.Value; }

			tableView.DeselectRow(indexPath, true);
		}

		private void UpdateSelectedValue()
		{
			if ( _radioCell.Value is null )
				return; // for HotReload

			bool result;
			if ( _radioCell.Value.GetType().IsValueType ) { result = Equals(_radioCell.Value, SelectedValue); }
			else { result = ReferenceEquals(_radioCell.Value, SelectedValue); }

			Accessory = result
							? UITableViewCellAccessory.Checkmark
							: UITableViewCellAccessory.None;
		}

		private void UpdateAccentColor()
		{
			if ( !_radioCell.AccentColor.IsDefault ) { TintColor = _radioCell.AccentColor.ToUIColor(); }
			else if ( CellParent is not null &&
					  !CellParent.CellAccentColor.IsDefault ) { TintColor = CellParent.CellAccentColor.ToUIColor(); }
		}
	}
}
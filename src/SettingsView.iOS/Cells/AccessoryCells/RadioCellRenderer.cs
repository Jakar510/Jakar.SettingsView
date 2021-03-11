using System;
using System.ComponentModel;
using System.Runtime.Remoting.Contexts;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Controls;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Config;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RadioCell), typeof(RadioCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)] public class RadioCellRenderer : CellBaseRenderer<RadioCellView> { }

	[Preserve(AllMembers = true)]
	public class RadioCellView : BaseAiAccessoryCell<RadioCheck>
	{
		protected RadioCell _RadioCell => Cell as RadioCell ?? throw new NullReferenceException(nameof(_RadioCell));


		protected object _SelectedValue
		{
			get => RadioCell.GetSelectedValue(_RadioCell.Section) ?? RadioCell.GetSelectedValue(CellParent);
			set
			{
				if ( RadioCell.GetSelectedValue(_RadioCell.Section) != null ) { RadioCell.SetSelectedValue(_RadioCell.Section, value); }
				else { RadioCell.SetSelectedValue(CellParent, value); }
			}
		}


		public RadioCellView( Cell cell ) : base(cell) { }


		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CheckableCellBase.AccentColorProperty.PropertyName ) { UpdateAccentColor(); }
			else if ( e.PropertyName == CheckableCellBase.OffColorProperty.PropertyName ) { UpdateOffColor(); }
			else { base.CellPropertyChanged(sender, e); }
		}
		protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CheckableCellBase.AccentColorProperty.PropertyName ) { UpdateAccentColor(); }
			else if ( e.PropertyName == CheckableCellBase.OffColorProperty.PropertyName ) { UpdateOffColor(); }
			else if ( e.PropertyName == RadioCell.SelectedValueProperty.PropertyName ) { UpdateSelectedValue(); }
			else { base.ParentPropertyChanged(sender, e); }
		}
		protected internal override void SectionPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == RadioCell.SelectedValueProperty.PropertyName ) { UpdateSelectedValue(); }
			else if ( e.PropertyName == Shared.sv.SettingsView.CellAccentColorProperty.PropertyName ) { UpdateAccentColor(); }
			else if ( e.PropertyName == Shared.sv.SettingsView.CellOffColorProperty.PropertyName ) { UpdateOffColor(); }
			else { base.SectionPropertyChanged(sender, e); }
		}

		protected internal override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			if ( !_Accessory.IsChecked ) { _SelectedValue = _RadioCell.Value; }
		}


		protected override void EnableCell()
		{
			base.EnableCell();
			_Title.Enable();
			_Description.Enable();
			_Accessory.Disable();
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Title.Disable();
			_Description.Disable();
			_Accessory.Enable();
		}

		protected internal override void UpdateCell()
		{
			UpdateAccentColor();
			UpdateSelectedValue();
			base.UpdateCell();
		}
		private void UpdateSelectedValue()
		{
			_Accessory.IsChecked = _RadioCell.Value.GetType().IsValueType
									   ? Equals(_RadioCell.Value, _SelectedValue)
									   : ReferenceEquals(_RadioCell.Value, _SelectedValue);
		}
		private void UpdateAccentColor()
		{
			if ( !_RadioCell.AccentColor.IsDefault ) { _Accessory.OnColor = _RadioCell.AccentColor.ToCGColor(); }
			else if ( CellParent != null &&
					  !CellParent.CellAccentColor.IsDefault ) { _Accessory.OnColor = CellParent.CellAccentColor.ToCGColor(); }

			SetNeedsDisplay();
		}
		private void UpdateOffColor()
		{
			if ( !_RadioCell.OffColor.IsDefault ) { _Accessory.OffColor = _RadioCell.OffColor.ToCGColor(); }
			else if ( CellParent != null &&
					  !CellParent.CellOffColor.IsDefault ) { _Accessory.OffColor = CellParent.CellOffColor.ToCGColor(); }

			SetNeedsDisplay();
		}
	}
}
using System;
using System.ComponentModel;
using Android.Content;
using Android.Runtime;
using Jakar.SettingsView.Droid.BaseCell;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Droid.Cells;
using Jakar.SettingsView.Droid.Controls;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(RadioCell), typeof(RadioCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)] public class RadioCellRenderer : CellBaseRenderer<RadioCellView> { }

	[Preserve(AllMembers = true)]
	public class RadioCellView : BaseAiAccessoryCell<RadioCheck>
	{
		protected RadioCell _RadioCell => Cell as RadioCell ?? throw new NullReferenceException(nameof(_RadioCell));


		private object? _SelectedValue
		{
			get => RadioCell.GetSelectedValue(_RadioCell.Section) ?? RadioCell.GetSelectedValue(CellParent);
			set
			{
				if ( RadioCell.GetSelectedValue(_RadioCell.Section) != null ) { RadioCell.SetSelectedValue(_RadioCell.Section, value); }
				else { RadioCell.SetSelectedValue(CellParent, value); }
			}
		}


		public RadioCellView( Context context, Cell cell ) : base(context, cell) => _Accessory.Focusable = false;
		public RadioCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) => _Accessory.Focusable = false;


		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CheckableCellBase.AccentColorProperty.PropertyName ) { UpdateAccentColor(); }
			else { base.CellPropertyChanged(sender, e); }
		}
		protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.sv.SettingsView.CellAccentColorProperty.PropertyName ) { UpdateAccentColor(); }
			else if ( e.PropertyName == RadioCell.SelectedValueProperty.PropertyName ) { UpdateSelectedValue(); }
			else { base.ParentPropertyChanged(sender, e); }
		}
		protected internal override void SectionPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.SectionPropertyChanged(sender, e);
			if ( e.PropertyName == RadioCell.SelectedValueProperty.PropertyName ) { UpdateSelectedValue(); }
		}

		protected internal override void RowSelected( SettingsViewRecyclerAdapter adapter, int position )
		{
			if ( !_Accessory.Checked ) { _SelectedValue = _RadioCell.Value; }
		}


		protected override void EnableCell()
		{
			base.EnableCell();
			_Title.Enable();
			_Description.Enable();
			_Accessory.Enabled = true;
			_Accessory.Alpha = SvConstants.Cell.ENABLED_ALPHA;
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Title.Disable();
			_Description.Disable();
			_Accessory.Enabled = false;
			_Accessory.Alpha = SvConstants.Cell.DISABLED_ALPHA;
		}

		protected internal override void UpdateCell()
		{
			UpdateAccentColor();
			UpdateSelectedValue();
			base.UpdateCell();
		}
		private void UpdateSelectedValue()
		{
			if ( _RadioCell.Value is null )
			{
				_Accessory.Checked = false;
				return;
			}

			_Accessory.Checked = _RadioCell.Value.GetType().IsValueType
									 ? Equals(_RadioCell.Value, _SelectedValue)
									 : ReferenceEquals(_RadioCell.Value, _SelectedValue);
		}
		private void UpdateAccentColor()
		{
			if ( !_RadioCell.AccentColor.IsDefault ) { _Accessory.Color = _RadioCell.AccentColor.ToAndroid(); }
			else if ( CellParent != null &&
					  !CellParent.CellAccentColor.IsDefault ) { _Accessory.Color = CellParent.CellAccentColor.ToAndroid(); }

			Invalidate();
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_Accessory.RemoveFromParent();
				_Accessory.Dispose();

				_Title.Dispose();

				_Description.Dispose();

				_CellLayout.Dispose();
				_AccessoryStack.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}
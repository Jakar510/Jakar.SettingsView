using System;
using System.ComponentModel;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Droid.BaseCell;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Droid.Cells;
using Jakar.SettingsView.Shared.CellBase;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ACheckBox = Android.Widget.CheckBox;
using AColor = Android.Graphics.Color;


[assembly: ExportRenderer(typeof(CheckboxCell), typeof(CheckboxCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)]
	public class CheckboxCellRenderer : CellBaseRenderer<CheckboxCellView> { }



	[Preserve(AllMembers = true)]
	public class CheckboxCellView : BaseAiAccessoryCell<ACheckBox>, CompoundButton.IOnCheckedChangeListener
	{
		protected CheckboxCell _AccessoryCell => Cell as CheckboxCell ?? throw new NullReferenceException(nameof(_AccessoryCell));


		public CheckboxCellView( Context context, Cell cell ) : base(context, cell)
		{
			_Accessory.Focusable = false;
			_Accessory.Gravity   = GravityFlags.Right;
			_Accessory.SetOnCheckedChangeListener(this);

			Focusable              = false;
			DescendantFocusability = DescendantFocusability.AfterDescendants;
		}

		public CheckboxCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)
		{
			_Accessory.Focusable = false;
			_Accessory.Gravity   = GravityFlags.Right;
			_Accessory.SetOnCheckedChangeListener(this);

			Focusable              = false;
			DescendantFocusability = DescendantFocusability.AfterDescendants;
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

		protected internal override void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { _Accessory.Checked = !_Accessory.Checked; }


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

		public void OnCheckedChanged( CompoundButton? buttonView, bool isChecked )
		{
			_AccessoryCell.Checked = isChecked;
			buttonView?.JumpDrawablesToCurrentState();
		}

		protected internal override void UpdateCell()
		{
			UpdateAccentColor();
			UpdateChecked();
			base.UpdateCell();
		}

		protected void UpdateChecked() { _Accessory.Checked = _AccessoryCell.Checked; }
		protected void UpdateAccentColor() { ChangeCheckColor(_AccessoryCell.CheckableConfig.AccentColor.ToAndroid(), _AccessoryCell.CheckableConfig.OffColor.ToAndroid()); }


		protected void ChangeCheckColor( AColor accent, AColor off )
		{
			if ( _Accessory.Background is not RippleDrawable ripple )
			{
				ripple                = CreateRippleDrawable(accent);
				_Accessory.Background = ripple;
			}

			ripple.SetColor(new ColorStateList(new[]
											   {
												   new[]
												   {
													   Android.Resource.Attribute.StateChecked
												   },
												   new[]
												   {
													   -Android.Resource.Attribute.StateChecked
												   }
											   },
											   new int[]
											   {
												   accent,
												   off
											   }
											  )
						   );
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_Accessory.SetOnCheckedChangeListener(null);
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

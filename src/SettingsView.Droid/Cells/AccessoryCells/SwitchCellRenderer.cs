using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Droid.BaseCell;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using SwitchCell = Jakar.SettingsView.Shared.Cells.SwitchCell;
using SwitchCellRenderer = Jakar.SettingsView.Droid.Cells.SwitchCellRenderer;
using AColor = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(SwitchCell), typeof(SwitchCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)] public class SwitchCellRenderer : CellBaseRenderer<SwitchCellView> { }

	[Preserve(AllMembers = true)]
	public class SwitchCellView : BaseAiAccessoryCell<Android.Widget.Switch>, CompoundButton.IOnCheckedChangeListener
	{
		protected SwitchCell _AccessoryCell => Cell as SwitchCell ?? throw new NullReferenceException(nameof(_AccessoryCell));

		public SwitchCellView( Context context, Cell cell ) : base(context, cell)
		{
			_Accessory.Gravity = GravityFlags.Right;
			_Accessory.Focusable = false;
			_Accessory.SetOnCheckedChangeListener(this);

			Focusable = false;
			DescendantFocusability = DescendantFocusability.AfterDescendants;
		}
		public SwitchCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }


		protected internal override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CheckableCellBase.AccentColorProperty.PropertyName ) { UpdateAccentColor(); }
			else if ( e.PropertyName == CheckableCellBase.CheckedProperty.PropertyName ) { UpdateOn(); }
			else { base.CellPropertyChanged(sender, e); }
		}
		protected internal override void ParentPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.sv.SettingsView.CellAccentColorProperty.PropertyName ) { UpdateAccentColor(); }
			else { base.ParentPropertyChanged(sender, e); }
		}


		protected internal override void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { _Accessory.Checked = !_Accessory.Checked; }
		public void OnCheckedChanged( CompoundButton? buttonView, bool isChecked ) { _AccessoryCell.Checked = isChecked; }

		protected override void EnableCell()
		{
			base.EnableCell();
			_Title.Enable();
			_Description.Enable();
			_Accessory.Enabled = true;
			_Accessory.Alpha = SVConstants.Cell.ENABLED_ALPHA;
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Title.Disable();
			_Description.Disable();
			_Accessory.Enabled = false;
			_Accessory.Alpha = SVConstants.Cell.DISABLED_ALPHA;
		}

		protected internal override void UpdateCell()
		{
			UpdateAccentColor();
			UpdateOn();
			base.UpdateCell();
		}
		private void UpdateOn() { _Accessory.Checked = _AccessoryCell.Checked; }

		private void UpdateAccentColor() { ChangeCheckColor(_AccessoryCell.GetAccentColor().ToAndroid(), _AccessoryCell.GetOffColor().ToAndroid()); }

		protected void ChangeCheckColor( AColor accent ) { ChangeCheckColor(accent, AColor.Argb(76, 117, 117, 117)); }
		protected void ChangeCheckColor( AColor accent, AColor off )
		{
			var trackColors = new ColorStateList(new[]
												 {
													 new[]
													 {
														 Android.Resource.Attribute.StateChecked
													 },
													 new[]
													 {
														 -Android.Resource.Attribute.StateChecked
													 },
												 },
												 new int[]
												 {
													 accent,
													 off
												 }
												);
			_Accessory.TrackDrawable?.SetTintList(trackColors);

			if ( _Accessory.Background is not RippleDrawable ripple )
			{
				ripple = CreateRippleDrawable(accent);
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
				_Accessory.Background?.Dispose();
				_Accessory.Background = null;
				_Accessory.ThumbDrawable?.Dispose();
				_Accessory.ThumbDrawable = null;
				_Accessory.Dispose();

				_CellLayout.Dispose();
				_AccessoryStack.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}
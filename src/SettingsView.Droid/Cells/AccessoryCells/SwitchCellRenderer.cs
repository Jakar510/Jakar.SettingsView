using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Droid.BaseCell;
using Jakar.SettingsView.Shared.Cells.Base;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using SwitchCell = Jakar.SettingsView.Shared.Cells.SwitchCell;
using SwitchCellRenderer = Jakar.SettingsView.Droid.Cells.SwitchCellRenderer;

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
			if ( e.PropertyName == BaseCheckableCell.AccentColorProperty.PropertyName ) { UpdateAccentColor(); }
			else if ( e.PropertyName == BaseCheckableCell.CheckedProperty.PropertyName ) { UpdateOn(); }
			else { base.CellPropertyChanged(sender, e); }
		}
		protected internal override void ParentPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.SettingsView.CellAccentColorProperty.PropertyName ) { UpdateAccentColor(); }
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
			_Accessory.Alpha = ENABLED_ALPHA;
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Title.Disable();
			_Description.Disable();
			_Accessory.Enabled = false;
			_Accessory.Alpha = DISABLED_ALPHA;
		}

		protected internal override void UpdateCell()
		{
			UpdateAccentColor();
			UpdateOn();
			base.UpdateCell();
		}
		private void UpdateOn() { _Accessory.Checked = _AccessoryCell.Checked; }

		private void UpdateAccentColor()
		{
			if ( _AccessoryCell.AccentColor != Color.Default ) { ChangeCheckColor(_AccessoryCell.AccentColor.ToAndroid()); }
			else if ( CellParent != null &&
					  CellParent.CellAccentColor != Color.Default ) { ChangeCheckColor(CellParent.CellAccentColor.ToAndroid()); }
		}

		protected void ChangeCheckColor( Android.Graphics.Color accent )
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
													 Android.Graphics.Color.Argb(76, 117, 117, 117)
												 }
												);
			_Accessory.TrackDrawable?.SetTintList(trackColors);

			RippleDrawable ripple = ( _Accessory.Background as RippleDrawable ) ?? CreateRippleDrawable(accent);
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
												   Android.Graphics.Color.Argb(76, 117, 117, 117)
											   }
											  )
						   );
			_Accessory.Background ??= ripple;
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
using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using SwitchCell = Jakar.SettingsView.Shared.Cells.SwitchCell;
using SwitchCellRenderer = Jakar.SettingsView.Droid.Cells.SwitchCellRenderer;

[assembly: ExportRenderer(typeof(SwitchCell), typeof(SwitchCellRenderer))]

namespace Jakar.SettingsView.Droid.Cells
{
	/// <summary>
	/// Switch cell renderer.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class SwitchCellRenderer : CellBaseRenderer<SwitchCellView> { }

	/// <summary>
	/// Switch cell view.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class SwitchCellView : CellBaseView, CompoundButton.IOnCheckedChangeListener
	{
		private SwitchCompat _switch { get; set; }
		private SwitchCell _SwitchCell => Cell as SwitchCell;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.Droid.Cells.SwitchCellView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="cell">Cell.</param>
		public SwitchCellView( Context context, Cell cell ) : base(context, cell)
		{
			_switch = new SwitchCompat(context);

			_switch.SetOnCheckedChangeListener(this);
			_switch.Gravity = GravityFlags.Right;

			var switchParam = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
							  { };

			using ( switchParam ) { AccessoryStack.AddView(_switch, switchParam); }

			_switch.Focusable = false;
			Focusable = false;
			DescendantFocusability = DescendantFocusability.AfterDescendants;
		}

		public SwitchCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell()
		{
			UpdateAccentColor();
			UpdateOn();
			base.UpdateCell();
		}

		/// <summary>
		/// Cells the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == SwitchCell.AccentColorProperty.PropertyName ) { UpdateAccentColor(); }

			if ( e.PropertyName == SwitchCell.OnProperty.PropertyName ) { UpdateOn(); }
		}

		/// <summary>
		/// Parents the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void ParentPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);
			if ( e.PropertyName == Shared.SettingsView.CellAccentColorProperty.PropertyName ) { UpdateAccentColor(); }
		}

		/// <summary>
		/// Ons the checked changed.
		/// </summary>
		/// <param name="buttonView">Button view.</param>
		/// <param name="isChecked">If set to <c>true</c> is checked.</param>
		public void OnCheckedChanged( CompoundButton buttonView, bool isChecked ) { _SwitchCell.On = isChecked; }

		/// <summary>
		/// Rows the selected.
		/// </summary>
		/// <param name="adapter">Adapter.</param>
		/// <param name="position">Position.</param>
		public override void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { _switch.Checked = !_switch.Checked; }

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <returns>The dispose.</returns>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_switch.SetOnCheckedChangeListener(null);
				_switch.Background?.Dispose();
				_switch.Background = null;
				_switch.ThumbDrawable?.Dispose();
				_switch.ThumbDrawable = null;
				_switch.Dispose();
				_switch = null;
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// Sets the enabled appearance.
		/// </summary>
		/// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
		protected override void SetEnabledAppearance( bool isEnabled )
		{
			if ( isEnabled )
			{
				_switch.Enabled = true;
				_switch.Alpha = 1.0f;
			}
			else
			{
				_switch.Enabled = false;
				_switch.Alpha = 0.3f;
			}

			base.SetEnabledAppearance(isEnabled);
		}

		private void UpdateOn() { _switch.Checked = _SwitchCell.On; }

		private void UpdateAccentColor()
		{
			if ( _SwitchCell.AccentColor != Color.Default ) { ChangeSwitchColor(_SwitchCell.AccentColor.ToAndroid()); }
			else if ( CellParent != null &&
					  CellParent.CellAccentColor != Color.Default ) { ChangeSwitchColor(CellParent.CellAccentColor.ToAndroid()); }
		}

		private void ChangeSwitchColor( Android.Graphics.Color accent )
		{
			var trackColors = new ColorStateList(new int[][]
												 {
													 new int[]
													 {
														 Android.Resource.Attribute.StateChecked
													 },
													 new int[]
													 {
														 -Android.Resource.Attribute.StateChecked
													 },
												 }, new int[]
													{
														Android.Graphics.Color.Argb(76, accent.R, accent.G, accent.B),
														Android.Graphics.Color.Argb(76, 117, 117, 117)
													});


			_switch.TrackDrawable.SetTintList(trackColors);

			var thumbColors = new ColorStateList(new int[][]
												 {
													 new int[]
													 {
														 Android.Resource.Attribute.StateChecked
													 },
													 new int[]
													 {
														 -Android.Resource.Attribute.StateChecked
													 },
												 }, new int[]
													{
														accent,
														Android.Graphics.Color.Argb(255, 244, 244, 244)
													});

			_switch.ThumbDrawable.SetTintList(thumbColors);

			var ripple = _switch.Background as RippleDrawable;
			ripple.SetColor(trackColors);
		}
	}
}
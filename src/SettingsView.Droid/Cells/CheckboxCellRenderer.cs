using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Droid.Cells;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CheckboxCell), typeof(CheckboxCellRenderer))]

namespace Jakar.SettingsView.Droid.Cells
{
	/// <summary>
	/// Checkbox cell renderer.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class CheckboxCellRenderer : CellBaseRenderer<CheckboxCellView> { }

	/// <summary>
	/// Checkbox cell view.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class CheckboxCellView : CellBaseView, CompoundButton.IOnCheckedChangeListener
	{
		private AppCompatCheckBox _checkbox;
		private CheckboxCell _CheckboxCell => Cell as CheckboxCell;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.Droid.Cells.CheckboxCellView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="cell">Cell.</param>
		public CheckboxCellView( Context context, Cell cell ) : base(context, cell)
		{
			_checkbox = new AppCompatCheckBox(context);
			_checkbox.SetOnCheckedChangeListener(this);
			_checkbox.Gravity = GravityFlags.Right;

			var lparam = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
						 {
							 Width = (int) context.ToPixels(30),
							 Height = (int) context.ToPixels(30)
						 };

			using ( lparam ) { AccessoryStack.AddView(_checkbox, lparam); }

			_checkbox.Focusable = false;
			Focusable = false;
			DescendantFocusability = DescendantFocusability.AfterDescendants;
		}

		public CheckboxCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell()
		{
			UpdateAccentColor();
			UpdateChecked();
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
			if ( e.PropertyName == CheckboxCell.AccentColorProperty.PropertyName ) { UpdateAccentColor(); }

			if ( e.PropertyName == CheckboxCell.CheckedProperty.PropertyName ) { UpdateChecked(); }
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
		/// Rows the selected.
		/// </summary>
		/// <param name="adapter">Adapter.</param>
		/// <param name="position">Position.</param>
		public override void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { _checkbox.Checked = !_checkbox.Checked; }

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <returns>The dispose.</returns>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_checkbox.SetOnCheckedChangeListener(null);
				_checkbox.Dispose();
				_checkbox = null;
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
				_checkbox.Enabled = true;
				_checkbox.Alpha = 1.0f;
			}
			else
			{
				_checkbox.Enabled = false;
				_checkbox.Alpha = 0.3f;
			}

			base.SetEnabledAppearance(isEnabled);
		}

		/// <summary>
		/// Ons the checked changed.
		/// </summary>
		/// <param name="buttonView">Button view.</param>
		/// <param name="isChecked">If set to <c>true</c> is checked.</param>
		public void OnCheckedChanged( CompoundButton buttonView, bool isChecked )
		{
			_CheckboxCell.Checked = isChecked;
			buttonView.JumpDrawablesToCurrentState();
		}

		private void UpdateChecked() { _checkbox.Checked = _CheckboxCell.Checked; }

		private void UpdateAccentColor()
		{
			if ( _CheckboxCell.AccentColor != Color.Default ) { ChangeCheckColor(_CheckboxCell.AccentColor.ToAndroid()); }
			else if ( CellParent != null &&
					  CellParent.CellAccentColor != Color.Default ) { ChangeCheckColor(CellParent.CellAccentColor.ToAndroid()); }
		}


		private void ChangeCheckColor( Android.Graphics.Color accent )
		{
			var colorList = new ColorStateList(new int[][]
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
													  accent
												  });

			_checkbox.SupportButtonTintList = colorList;

			var rippleColor = new ColorStateList(new int[][]
												 {
													 new int[]
													 {
														 Android.Resource.Attribute.StateChecked
													 },
													 new int[]
													 {
														 -Android.Resource.Attribute.StateChecked
													 }
												 }, new int[]
													{
														Android.Graphics.Color.Argb(76, accent.R, accent.G, accent.B),
														Android.Graphics.Color.Argb(76, 117, 117, 117)
													});
			var ripple = _checkbox.Background as RippleDrawable;
			ripple.SetColor(rippleColor);
			_checkbox.Background = ripple;
		}
	}
}
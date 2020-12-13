using System;
using CoreGraphics;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.iOS.Cells;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CheckboxCell), typeof(CheckboxCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	/// <summary>
	/// Checkbox cell renderer.
	/// </summary>
	[Foundation.Preserve(AllMembers = true)]
	public class CheckboxCellRenderer : CellBaseRenderer<CheckboxCellView> { }

	/// <summary>
	/// Checkbox cell view.
	/// </summary>
	[Foundation.Preserve(AllMembers = true)]
	public class CheckboxCellView : CellBaseView
	{
		private CheckBox _checkbox;
		private CheckboxCell _CheckboxCell => Cell as CheckboxCell;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.iOS.Cells.CheckboxCellView"/> class.
		/// </summary>
		/// <param name="formsCell">Forms cell.</param>
		public CheckboxCellView( Cell formsCell ) : base(formsCell)
		{
			_checkbox = new CheckBox(new CGRect(0, 0, 20, 20));
			_checkbox.Layer.BorderWidth = 2;
			_checkbox.Layer.CornerRadius = 3;
			_checkbox.Inset = new UIEdgeInsets(10, 10, 10, 10);

			_checkbox.CheckChanged = CheckChanged;

			AccessoryView = _checkbox;
			EditingAccessoryView = _checkbox;
		}

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell( UITableView tableView )
		{
			UpdateAccentColor();
			UpdateChecked();
			base.UpdateCell(tableView);
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
		/// Dispose the specified disposing.
		/// </summary>
		/// <returns>The dispose.</returns>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_checkbox.CheckChanged = null;
				_checkbox?.Dispose();
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
			if ( isEnabled ) { _checkbox.Alpha = 1.0f; }
			else { _checkbox.Alpha = 0.3f; }

			base.SetEnabledAppearance(isEnabled);
		}

		private void CheckChanged( UIButton button ) { _CheckboxCell.Checked = button.Selected; }

		private void UpdateChecked() { _checkbox.Selected = _CheckboxCell.Checked; }

		private void UpdateAccentColor()
		{
			if ( _CheckboxCell.AccentColor != Color.Default ) { ChangeCheckColor(_CheckboxCell.AccentColor.ToCGColor()); }
			else if ( CellParent != null &&
					  CellParent.CellAccentColor != Color.Default ) { ChangeCheckColor(CellParent.CellAccentColor.ToCGColor()); }
		}

		private void ChangeCheckColor( CGColor accent )
		{
			_checkbox.Layer.BorderColor = accent;
			_checkbox.FillColor = accent;
			_checkbox.SetNeedsDisplay(); //update inner rect
		}
	}

	/// <summary>
	/// Check box.
	/// </summary>
	public class CheckBox : UIButton
	{
		/// <summary>
		/// Gets or sets the inset.
		/// </summary>
		/// <value>The inset.</value>
		public UIEdgeInsets Inset { get; set; } = new UIEdgeInsets(20, 20, 20, 20);

		/// <summary>
		/// Gets or sets the color of the fill.
		/// </summary>
		/// <value>The color of the fill.</value>
		public CGColor FillColor { get; set; }

		/// <summary>
		/// Gets or sets the check changed.
		/// </summary>
		/// <value>The check changed.</value>
		public Action<UIButton> CheckChanged { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.iOS.Cells.CheckBox"/> class.
		/// </summary>
		/// <param name="rect">Rect.</param>
		public CheckBox( CGRect rect ) : base(rect)
		{
			AddGestureRecognizer(new UITapGestureRecognizer(( obj ) =>
																 {
																	 Selected = !Selected;
																	 CheckChanged?.Invoke(this);
																 }));
		}

		/// <summary>
		/// Draw the specified rect.
		/// </summary>
		/// <returns>The draw.</returns>
		/// <param name="rect">Rect.</param>
		public override void Draw( CGRect rect )
		{
			base.Draw(rect);

			nfloat lineWidth = rect.Size.Width / 10;

			// Draw check mark
			if ( Selected )
			{
				Layer.BackgroundColor = FillColor;


				var checkmark = new UIBezierPath();
				CGSize size = rect.Size;
				checkmark.MoveTo(new CGPoint(x: 22f / 100f * size.Width, y: 52f / 100f * size.Height));
				checkmark.AddLineTo(new CGPoint(x: 38f / 100f * size.Width, y: 68f / 100f * size.Height));
				checkmark.AddLineTo(new CGPoint(x: 76f / 100f * size.Width, y: 30f / 100f * size.Height));

				checkmark.LineWidth = lineWidth;
				UIColor.White.SetStroke();
				checkmark.Stroke();
			}

			else { Layer.BackgroundColor = new CGColor(0, 0, 0, 0); }
		}

		/// <summary>
		/// Points the inside.
		/// </summary>
		/// <returns><c>true</c>, if inside was pointed, <c>false</c> otherwise.</returns>
		/// <param name="point">Point.</param>
		/// <param name="uievent">Uievent.</param>
		public override bool PointInside( CGPoint point, UIEvent uievent )
		{
			CGRect rect = Bounds;
			rect.X -= Inset.Left;
			rect.Y -= Inset.Top;
			rect.Width += Inset.Left + Inset.Right;
			rect.Height += Inset.Top + Inset.Bottom;

			return rect.Contains(point);
		}
	}
}
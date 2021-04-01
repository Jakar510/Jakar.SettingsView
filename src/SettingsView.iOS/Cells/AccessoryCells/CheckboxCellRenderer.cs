using System;
using System.ComponentModel;
using CoreGraphics;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Controls;
using Jakar.SettingsView.iOS.Interfaces;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Cells;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#nullable enable
[assembly: ExportRenderer(typeof(CheckboxCell), typeof(CheckboxCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Foundation.Preserve(AllMembers = true)]
	public class CheckboxCellRenderer : CellBaseRenderer<CheckboxCellView> { }

	[Foundation.Preserve(AllMembers = true)]
	public class CheckboxCellView : BaseAccessoryCell<CheckBox>
	{
		private CheckboxCell _CheckboxCell => Cell as CheckboxCell ?? throw new NullReferenceException(nameof(_CheckboxCell));

		public CheckboxCellView( Cell formsCell ) : base(formsCell)
		{
			_Accessory = new CheckBox(this);
			_Accessory.Layer.BorderWidth = 2;
			_Accessory.Layer.CornerRadius = 3;
			_Accessory.Inset = new UIEdgeInsets(10, 10, 10, 10);

			_Accessory.CheckChanged = CheckChanged;

			AccessoryView = _Accessory;
			EditingAccessoryView = _Accessory;
		}

		public override void UpdateCell( UITableView tableView )
		{
			UpdateAccentColor();
			UpdateChecked();
			base.UpdateCell(tableView);
		}

		public override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == CheckableCellBase.AccentColorProperty.PropertyName ) { UpdateAccentColor(); }

			if ( e.PropertyName == CheckableCellBase.CheckedProperty.PropertyName ) { UpdateChecked(); }
		}

		public override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);
			if ( e.PropertyName == Shared.sv.SettingsView.CellAccentColorProperty.PropertyName ) { UpdateAccentColor(); }
		}

		protected override void SetEnabledAppearance( bool isEnabled )
		{
			if ( isEnabled ) { _Accessory.Alpha = 1.0f; }
			else { _Accessory.Alpha = 0.3f; }

			base.SetEnabledAppearance(isEnabled);
		}

		public void CheckChanged( UIButton button )
		{
			_CheckboxCell.Checked = button.Selected;
			_CheckboxCell.ValueChangedHandler.SendValueChanged(button.Selected);
		}

		private void UpdateChecked() { _Accessory.Selected = _CheckboxCell.Checked; }

		private void UpdateAccentColor()
		{
			if ( _CheckboxCell.AccentColor != Color.Default ) { ChangeCheckColor(_CheckboxCell.AccentColor.ToCGColor()); }
			else if ( CellParent is not null &&
					  CellParent.CellAccentColor != Color.Default ) { ChangeCheckColor(CellParent.CellAccentColor.ToCGColor()); }
		}

		private void ChangeCheckColor( CGColor accent )
		{
			_Accessory.Layer.BorderColor = accent;
			_Accessory.FillColor = accent;
			_Accessory.SetNeedsDisplay(); //update inner rect
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_Accessory.Dispose();
			}

			base.Dispose(disposing);
		}
	}

	public class CheckBox : UIButton, IRenderAccessory
	{
		public BaseAccessoryCell<CheckBox> Renderer { get; }

		public UIEdgeInsets Inset { get; set; } = new(20, 20, 20, 20);

		public CGColor? FillColor { get; set; }

		public Action<UIButton>? CheckChanged { get; set; }

		public UITapGestureRecognizer Recognizer { get; set; }

		public CheckBox( BaseAccessoryCell<CheckBox> renderer ) : base()
		{
			Renderer = renderer;
			Recognizer = new UITapGestureRecognizer(OnClick);
			AddGestureRecognizer(Recognizer);
		}


		private void OnClick( UITapGestureRecognizer obj )
		{
			Selected = !Selected;
			CheckChanged?.Invoke(this);
		}

		public override void Draw( CGRect rect )
		{
			base.Draw(rect);

			var lineWidth = rect.Size.Width / 10;

			// Draw check mark
			if ( Selected )
			{
				Layer.BackgroundColor = FillColor;

				var checkMark = new UIBezierPath();
				var size = rect.Size;
				checkMark.MoveTo(new CGPoint(x: 22f / 100f * size.Width, y: 52f / 100f * size.Height));
				checkMark.AddLineTo(new CGPoint(x: 38f / 100f * size.Width, y: 68f / 100f * size.Height));
				checkMark.AddLineTo(new CGPoint(x: 76f / 100f * size.Width, y: 30f / 100f * size.Height));

				checkMark.LineWidth = lineWidth;
				UIColor.White.SetStroke();
				checkMark.Stroke();
			}

			else { Layer.BackgroundColor = new CGColor(0, 0, 0, 0); }
		}

		public override bool PointInside( CGPoint point, UIEvent? uiEvent )
		{
			CGRect rect = Bounds;
			rect.X -= Inset.Left;
			rect.Y -= Inset.Top;
			rect.Width += Inset.Left + Inset.Right;
			rect.Height += Inset.Top + Inset.Bottom;

			return rect.Contains(point);
		}

		public void Initialize( Stack parent ) { }
		public void Update() { }
		public bool Update( object sender, PropertyChangedEventArgs e ) => throw new NotImplementedException();
		public bool UpdateParent( object sender, PropertyChangedEventArgs e ) => throw new NotImplementedException();
		public void Enable() { }
		public void Disable() { }

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				RemoveGestureRecognizer(Recognizer);
				Recognizer.Dispose();
				CheckChanged = null;
			}

			base.Dispose(disposing);
		}
	}
}
using System;
using System.ComponentModel;
using CoreGraphics;
using Jakar.Api.Extensions;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Interfaces;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Config;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#nullable enable
namespace Jakar.SettingsView.iOS.Controls
{
	[Foundation.Preserve(AllMembers = true)]
	public class BaseUIView : UIView
	{
		private bool _enabled;

		public virtual bool Enabled
		{
			get => _enabled;
			set
			{
				_enabled = value;
				Alpha = value
							? SvConstants.Cell.ENABLED_ALPHA
							: SvConstants.Cell.DISABLED_ALPHA;
			}
		}

		public void Enable()
		{
			UserInteractionEnabled = true;
			Alpha = SvConstants.Cell.ENABLED_ALPHA;
		}
		public void Disable()
		{
			UserInteractionEnabled = true;
			Alpha = SvConstants.Cell.DISABLED_ALPHA;
		}
	}


	[Foundation.Preserve(AllMembers = true)]
	public class RadioCheck : BaseUIView, IRenderAccessory
	{
		public readonly nfloat FACTOR = 0.75f;
		protected RadioCellView _Renderer { get; }
		protected UITapGestureRecognizer _Recognizer { get; set; }

		private bool _isChecked;

		public bool IsChecked
		{
			get => _isChecked;
			set
			{
				if ( _isChecked == value ) { return; }

				if ( !Enabled ) { return; }

				_isChecked = value;
				_Renderer.Cell.Checked = value;
				_Renderer.Cell.ValueChangedHandler.SendValueChanged(value);
				SetNeedsDisplay();
			}
		}

		public CGColor? OnColor { get; set; }

		public CGColor? OffColor { get; set; }


		public RadioCheck( RadioCellView renderer ) : base()
		{
			_Renderer = renderer;

			BackgroundColor = UIColor.Clear;

			_Recognizer = new UITapGestureRecognizer(Toggle);
			AddGestureRecognizer(_Recognizer);
		}


		public void Update()
		{
			UpdateChecked();
			UpdateAccentColor();
		}


		public bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(CheckableCellBase.AccentColorProperty) ) { return UpdateAccentColor(); }

			if ( e.IsEqual(CheckableCellBase.CheckedProperty) ) { return UpdateChecked(); }

			return false;
		}
		public bool UpdateSection( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(RadioCell.SelectedValueProperty) ) { return UpdateSelectedValue(); }

			return false;
		}
		public bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(Shared.sv.SettingsView.CellAccentColorProperty) ) { return UpdateAccentColor(); }

			if ( e.IsEqual(RadioCell.SelectedValueProperty) ) { return UpdateSelectedValue(); }

			if ( e.IsEqual(Shared.sv.SettingsView.CellAccentColorProperty) ) { return UpdateAccentColor(); }

			return false;
		}


		public void Toggle()
		{
			if ( IsChecked ) { Deselect(); }
			else { Select(); }
		}
		public void Select() { IsChecked = true; }
		public void Deselect() { IsChecked = false; }


		protected bool UpdateSelectedValue()
		{
			if ( _Renderer.Cell.Value is null ) return false; // for HotReload

			bool result = _Renderer.Cell.Value.GetType().IsValueType
							  ? Equals(_Renderer.Cell.Value, _Renderer.SelectedValue)
							  : ReferenceEquals(_Renderer.Cell.Value, _Renderer.SelectedValue);

			_Renderer.Accessory = result
									  ? UITableViewCellAccessory.Checkmark
									  : UITableViewCellAccessory.None;

			return true;
		}
		protected bool UpdateAccentColor()
		{
			OnColor = _Renderer.Cell.CheckableConfig.AccentColor.ToCGColor();
			return true;
		}
		protected bool UpdateChecked()
		{
			IsChecked = _Renderer.Cell.Checked;
			return true;
		}


		// public void Init( CGPoint pt ) => Init(pt.X, pt.Y, 150, 30);
		// public void Init( double x,
		// 				  double y,
		// 				  double width,
		// 				  double height ) =>
		// 	Init(new CGRect(x, y, width, height));
		// public void Init( CGRect frame ) { Frame = frame; }

		public override void Draw( CGRect bounds ) { bounds.CenterCircle(FACTOR, IsChecked, OnColor, OffColor); }

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				RemoveGestureRecognizer(_Recognizer);
				_Recognizer.Dispose();
			}

			base.Dispose(disposing);
		}
	}


	public static class CGContextExtensions
	{
		public static CGContext Context => UIGraphics.GetCurrentContext();

		public static void CenterCircle( this CGRect bounds,
										 in nfloat factor,
										 in bool isChecked,
										 in CGColor? on,
										 in CGColor? off ) =>
			bounds.CenterCircle(factor,
								isChecked,
								on,
								off,
								1
							   );
		public static void CenterCircle( this CGRect bounds,
										 in nfloat factor,
										 in bool isChecked,
										 in CGColor? on,
										 in CGColor? off,
										 in nfloat alpha )
		{
			nfloat width = bounds.Width * factor;
			nfloat height = bounds.Height * factor;
			nfloat x = bounds.X + bounds.Width - width;
			nfloat y = bounds.Y + bounds.Height - height;

			Context.CenterCircle(x,
								 y,
								 width,
								 height,
								 isChecked,
								 on,
								 off,
								 alpha
								);
		}
		public static void CenterCircle( this CGContext con,
										 in nfloat x,
										 in nfloat y,
										 in nfloat width,
										 in nfloat height,
										 in bool isChecked,
										 in CGColor? on,
										 in CGColor? off,
										 in nfloat alpha ) =>
			con.CenterCircle(new CGRect(x, y, width, height),
							 isChecked,
							 on,
							 off,
							 alpha
							);
		public static void CenterCircle( this CGContext con,
										 CGRect rect,
										 in bool isChecked,
										 in CGColor? on,
										 in CGColor? off,
										 in nfloat alpha )
		{
			con.SetAlpha(alpha);

			if ( isChecked )
			{
				CGColor color = on ?? Color.Accent.ToCGColor();
				con.DrawCircle(rect, 5, color);
				con.StrokePath();

				con.DrawCircle(rect, 8, color);
				con.FillPath();
			}
			else
			{
				con.DrawCircle(rect, 5, off ?? Color.Default.ToCGColor());
				con.StrokePath();
			}
		}


		public static void DrawCircle( this CGContext con,
									   in CGRect bounds,
									   in nfloat padding,
									   in CGColor color ) =>
			con.DrawCircle(color,
						   padding,
						   padding,
						   bounds.Width - 2 * padding,
						   bounds.Height - 2 * padding
						  );
		public static void DrawCircle( this CGContext con,
									   in CGColor color,
									   in nfloat x,
									   in nfloat y,
									   in nfloat width,
									   in nfloat height ) =>
			con.DrawCircle(color, new CGRect(x, y, width, height));
		public static void DrawCircle( this CGContext con, in CGColor color, in CGRect bounds )
		{
			con.SetStrokeColor(color);
			con.AddEllipseInRect(bounds);
		}
	}
}
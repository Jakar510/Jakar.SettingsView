using CoreGraphics;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Interfaces;
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
							? SVConstants.Cell.ENABLED_ALPHA
							: SVConstants.Cell.DISABLED_ALPHA;
			}
		}

		public void Disable() { Enabled = false; }
		public void Enable() { Enabled = true; }
	}


	[Foundation.Preserve(AllMembers = true)]
	public class RadioCheck : BaseUIView, IInitializeControl
	{
		public RadioCellView Renderer { get; }
		protected CircleView _CircleView { get; set; }


		public bool IsChecked
		{
			get => _CircleView.IsChecked;
			set => _CircleView.IsChecked = value;
		}

		public CGColor? OnColor
		{
			get => _CircleView.OnColor;
			set => _CircleView.OnColor = value;
		}

		public CGColor? OffColor
		{
			get => _CircleView.OffColor;
			set => _CircleView.OffColor = value;
		}

		public override bool Enabled
		{
			get => base.Enabled;
			set
			{
				base.Enabled = value;
				_CircleView.Enabled = value;
			}
		}

		public RadioCheck( RadioCellView renderer )
		{
			Renderer = renderer;
			_CircleView = new CircleView();
			AddSubview(_CircleView);
		}

		public void Init( CGPoint pt )
		{
			Frame = new CGRect(pt, new CGSize(150, 30));
			_CircleView.Init(new CGRect(0, 0, 30, 30));
			BackgroundColor = UIColor.Clear;

			var tapGR = new UITapGestureRecognizer(Toggle);
			AddGestureRecognizer(tapGR);
		}
		public void Toggle()
		{
			if ( !Enabled ) return;
			IsChecked = !IsChecked;
		}
		public void Initialize( Stack parent ) {  }
	}


	[Foundation.Preserve(AllMembers = true)]
	public class CircleView : BaseUIView
	{
		private bool isChecked;

		public bool IsChecked
		{
			get => isChecked;
			set
			{
				isChecked = value;
				SetNeedsDisplay();
			}
		}

		public CGColor? OnColor { get; set; }
		public CGColor? OffColor { get; set; }


		public CircleView() => BackgroundColor = UIColor.Clear;
		public void Init( CGRect frame ) { Frame = frame; }

		protected void DrawCircle( CGRect rect,
								   CGContext con,
								   float padding,
								   CGColor? color )
		{
			con.SetStrokeColor(color ?? Color.Accent.ToCGColor());
			con.AddEllipseInRect(new CGRect(padding, padding, rect.Width - 2 * padding, rect.Height - 2 * padding));
		}
		public override void Draw( CGRect rect )
		{
			CGContext con = UIGraphics.GetCurrentContext();

			if ( isChecked )
			{
				DrawCircle(rect, con, 5, OnColor);
				con.StrokePath();

				DrawCircle(rect, con, 8, OnColor);
				con.FillPath();
			}
			else
			{
				DrawCircle(rect, con, 5, OffColor);
				con.StrokePath();
			}
		}
	}
}
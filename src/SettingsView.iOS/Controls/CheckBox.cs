// unset

using System;
using CoreGraphics;
using UIKit;

#nullable enable
namespace Jakar.SettingsView.iOS.Controls
{
	public class CheckBox : UIButton
	{
		public UIEdgeInsets Inset { get; set; } = new(20, 20, 20, 20);

		public CGColor? FillColor { get; set; }

		public Action<CheckBox>? CheckChanged { get; set; }

		public CheckBox( CGRect rect ) : base(rect) { AddGestureRecognizer(new UITapGestureRecognizer(OnTap)); }
		private void OnTap( UITapGestureRecognizer recognizer )
		{
			Selected = !Selected;
			CheckChanged?.Invoke(this);
		}

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

		public override bool PointInside( CGPoint point, UIEvent? uiEvent )
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
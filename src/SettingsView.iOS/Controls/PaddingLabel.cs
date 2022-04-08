namespace Jakar.SettingsView.iOS.Controls
{
	public class PaddingLabel : UILabel
	{
		private UIEdgeInsets _padding;

		public UIEdgeInsets Padding
		{
			get => _padding;
			set
			{
				_padding = value;
				SetNeedsLayout();
				SetNeedsDisplay();
			}
		}

		public override void DrawText( CGRect rect ) { base.DrawText(Padding.InsetRect(rect)); }

		public override CGSize IntrinsicContentSize
		{
			get
			{
				CGSize contentSize = base.IntrinsicContentSize;
				contentSize.Height += Padding.Top + Padding.Bottom;
				contentSize.Width += Padding.Left + Padding.Right;
				return contentSize;
			}
		}
	}
}
﻿using CoreGraphics;
using UIKit;

namespace Jakar.SettingsView.iOS.OLD_Cells
{
	[Foundation.Preserve(AllMembers = true)]
	public class NoCaretField : UITextField
	{
		public NoCaretField() : base(new CGRect())
		{
			BorderStyle = UITextBorderStyle.None;
			BackgroundColor = UIColor.Clear;
		}
		public override CGRect GetCaretRectForPosition( UITextPosition position ) => new();
	}
}
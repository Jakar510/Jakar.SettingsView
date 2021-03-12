using UIKit;
using Xamarin.Forms;

namespace Jakar.SettingsView.iOS.Extensions
{
	[Foundation.Preserve(AllMembers = true)]
	public static class TextAlignmentExtensions
	{
		public static UITextAlignment ToUITextAlignment( this TextAlignment forms )
		{
			return forms switch
				   {
					   TextAlignment.Start => UITextAlignment.Left,
					   TextAlignment.Center => UITextAlignment.Center,
					   TextAlignment.End => UITextAlignment.Right,
					   _ => UITextAlignment.Right
				   };
		}
	}
}
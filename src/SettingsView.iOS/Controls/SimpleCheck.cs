using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#nullable enable
namespace Jakar.SettingsView.iOS.Controls
{
	[Foundation.Preserve(AllMembers = true)]
	public class SimpleCheck : FormsCheckBox // AView
	{
		public SimpleCheck() : base() { }

		// from CheckBoxRenderer 
		// 
		//
		// public override CGSize SizeThatFits( CGSize size )
		// {
		// 	var result = base.SizeThatFits(size);
		// 	var height = Math.Max(MinimumSize, result.Height);
		// 	var width = Math.Max(MinimumSize, result.Width);
		// 	var final = Math.Min(width, height);
		// 	return new CGSize(final, final);
		// }
		//
		// public override SizeRequest GetDesiredSize( double widthConstraint, double heightConstraint )
		// {
		// 	var sizeConstraint = base.GetDesiredSize(widthConstraint, heightConstraint);
		//
		// 	var set = false;
		//
		// 	var width = widthConstraint;
		// 	var height = heightConstraint;
		// 	if ( sizeConstraint.Request.Width == 0 )
		// 	{
		// 		if ( widthConstraint <= 0 || double.IsInfinity(widthConstraint) )
		// 		{
		// 			width = MinimumSize;
		// 			set = true;
		// 		}
		// 	}
		//
		// 	if ( sizeConstraint.Request.Height == 0 )
		// 	{
		// 		if ( heightConstraint <= 0 || double.IsInfinity(heightConstraint) )
		// 		{
		// 			height = MinimumSize;
		// 			set = true;
		// 		}
		// 	}
		//
		// 	if ( set )
		// 	{
		// 		sizeConstraint = new SizeRequest(new Size(width, height), new Size(MinimumSize, MinimumSize));
		// 	}
		//
		// 	return sizeConstraint;
		// }
	}
}
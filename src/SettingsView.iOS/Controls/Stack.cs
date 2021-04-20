// unset

using Jakar.Api.iOS.Enumerations;
using Jakar.Api.iOS.Extensions;
using Jakar.SettingsView.iOS.BaseCell;
using UIKit;


#nullable enable
namespace Jakar.SettingsView.iOS.Controls
{
	public static class Stack
	{
		public static UIStackView Value() => new()
											 {
												 Axis                                      = UILayoutConstraintAxis.Vertical,
												 Alignment                                 = UIStackViewAlignment.Fill,
												 Distribution                              = UIStackViewDistribution.FillProportionally,
												 Spacing                                   = 6,
												 TranslatesAutoresizingMaskIntoConstraints = false,
												 BackgroundColor = UIColor.Red	
											 };


		public static UIStackView Title() => new()
											 {
												 Axis                                      = UILayoutConstraintAxis.Vertical,
												 Alignment                                 = UIStackViewAlignment.Fill,
												 Distribution                              = UIStackViewDistribution.FillProportionally,
												 Spacing                                   = 4,
												 TranslatesAutoresizingMaskIntoConstraints = false,
												 BackgroundColor = UIColor.Blue
											 };


		public static UIStackView Main() => new()
											{
												Axis                                      = UILayoutConstraintAxis.Horizontal,
												Alignment                                 = UIStackViewAlignment.Top,
												Distribution                              = UIStackViewDistribution.FillProportionally,
												Spacing                                   = 10,
												TranslatesAutoresizingMaskIntoConstraints = false,
												BackgroundColor = UIColor.Black
											};
	}
}

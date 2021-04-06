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
												 Axis         = UILayoutConstraintAxis.Vertical,
												 Alignment    = UIStackViewAlignment.Fill,
												 Distribution = UIStackViewDistribution.Fill,
												 Spacing      = 6,
											 };


		public static UIStackView Title() => new()
											 {
												 Axis         = UILayoutConstraintAxis.Vertical,
												 Alignment    = UIStackViewAlignment.Fill,
												 Distribution = UIStackViewDistribution.Fill,
												 Spacing      = 4,
											 };


		public static UIStackView Content() => new()
											   {
												   Axis         = UILayoutConstraintAxis.Vertical,
												   Alignment    = UIStackViewAlignment.Fill,
												   Distribution = UIStackViewDistribution.Fill,
												   Spacing      = 6,
											   };


		public static UIStackView Main() => new()
											{
												Axis         = UILayoutConstraintAxis.Vertical,
												Alignment    = UIStackViewAlignment.Fill,
												Distribution = UIStackViewDistribution.Fill,
												Spacing      = 16,
											};




		//
		// public static void Root( this UIView contentView, in UIStackView root )
		// {
		// 	contentView.AddSubview(root);
		//
		// 	root.TranslatesAutoresizingMaskIntoConstraints                       = false;
		// 	root.TopAnchor.ConstraintEqualTo(contentView.TopAnchor).Active       = true;
		// 	root.LeftAnchor.ConstraintEqualTo(contentView.LeftAnchor).Active     = true;
		// 	root.BottomAnchor.ConstraintEqualTo(contentView.BottomAnchor).Active = true;
		// 	root.RightAnchor.ConstraintEqualTo(contentView.RightAnchor).Active   = true;
		//
		// 	root.Priorities();
		// }
		//
		//
		// public static void Title( this UIStackView root, in UIStackView stack, in IconView icon )
		// {
		// 	root.AddArrangedSubview(stack);
		//
		// 	stack.TranslatesAutoresizingMaskIntoConstraints = false;
		//
		// 	stack.TopAnchor.ConstraintEqualTo(root.TopAnchor).Active       = true;
		// 	stack.BottomAnchor.ConstraintEqualTo(root.BottomAnchor).Active = true;
		//
		// 	stack.LeftAnchor.ConstraintEqualTo(icon.Control.RightAnchor).Active = true;
		// 	stack.RightAnchor.ConstraintEqualTo(root.RightAnchor).Active        = true;
		//
		// 	stack.Priorities();
		// }
		//
		//
		// public static void Title( this UIStackView root, in UIStackView stack, in UIStackView value, in IconView icon )
		// {
		// 	root.AddArrangedSubview(stack);
		//
		// 	stack.TranslatesAutoresizingMaskIntoConstraints = false;
		//
		// 	stack.TopAnchor.ConstraintEqualTo(root.TopAnchor).Active       = true;
		// 	stack.BottomAnchor.ConstraintEqualTo(root.BottomAnchor).Active = true;
		//
		// 	stack.LeftAnchor.ConstraintEqualTo(icon.Control.RightAnchor).Active = true;
		// 	stack.RightAnchor.ConstraintEqualTo(value.LeftAnchor).Active        = true;
		//
		// 	stack.Priorities();
		// }
		//
		//
		// public static void Accessory( this UIStackView root, in UIStackView stack, in IconView icon, in UIView accessory )
		// {
		// 	root.AddArrangedSubview(stack);
		//
		// 	stack.TranslatesAutoresizingMaskIntoConstraints = false;
		//
		// 	stack.TopAnchor.ConstraintEqualTo(root.TopAnchor).Active       = true;
		// 	stack.BottomAnchor.ConstraintEqualTo(root.BottomAnchor).Active = true;
		//
		// 	stack.LeftAnchor.ConstraintEqualTo(icon.Control.RightAnchor).Active = true;
		// 	stack.RightAnchor.ConstraintEqualTo(accessory.LeftAnchor).Active    = true;
		//
		// 	stack.Priorities();
		// }
		//
		//
		// public static void Value( this UIStackView root, in UIStackView stack, in UIStackView title )
		// {
		// 	root.AddArrangedSubview(stack);
		//
		// 	stack.TranslatesAutoresizingMaskIntoConstraints = false;
		//
		// 	stack.TopAnchor.ConstraintEqualTo(root.TopAnchor).Active       = true;
		// 	stack.BottomAnchor.ConstraintEqualTo(root.BottomAnchor).Active = true;
		//
		// 	stack.LeftAnchor.ConstraintEqualTo(title.RightAnchor).Active = true;
		// 	stack.RightAnchor.ConstraintEqualTo(root.LeftAnchor).Active  = true;
		//
		// 	stack.Priorities();
		// }
	}
}

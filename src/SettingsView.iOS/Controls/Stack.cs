// unset

using System;
using Jakar.Api.iOS.Extensions;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.Shared.Config;
using UIKit;

#nullable enable
namespace Jakar.SettingsView.iOS.Controls
{
	public class Stack : UIStackView
	{
		public Stack( UILayoutConstraintAxis axis, nfloat spacing ) : this(axis, spacing, UIStackViewAlignment.Fill, UIStackViewDistribution.Fill) { }
		public Stack( UILayoutConstraintAxis axis,
					  nfloat spacing,
					  UIStackViewAlignment alignment,
					  UIStackViewDistribution distribution ) : base()
		{
			Axis = axis;
			Alignment = alignment;
			Spacing = spacing;
			Distribution = distribution;
			// LayoutMargins = new UIEdgeInsets(6, 16, 6, 16);
			LayoutMargins = SVConstants.Cell.PADDING.ToUIEdgeInsets();
			LayoutMarginsRelativeArrangement = true;
		}

		public static Stack ValueStack() => new (UILayoutConstraintAxis.Vertical, 6);
		public static Stack TitleStack() => new (UILayoutConstraintAxis.Vertical, 4);
		public static Stack ContentStack() => new (UILayoutConstraintAxis.Vertical, 6);
		public static Stack MainStack() => new (UILayoutConstraintAxis.Horizontal, 16);


		protected void Priorities()
		{
			SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Horizontal);
			SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Vertical);

			SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);
			SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Vertical);
		}


		public void Root( in BaseCellView cell ) => Root(cell.ContentView);
		public void Root( in UIView ContentView )
		{
			ContentView.AddSubview(this);

			TranslatesAutoresizingMaskIntoConstraints = false;
			TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
			LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor).Active = true;
			BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;
			RightAnchor.ConstraintEqualTo(ContentView.RightAnchor).Active = true;

			Priorities();
		}

		public void Title( in Stack root, in IconView icon )
		{
			root.AddArrangedSubview(this);

			TranslatesAutoresizingMaskIntoConstraints = false;

			TopAnchor.ConstraintEqualTo(root.TopAnchor).Active = true;
			BottomAnchor.ConstraintEqualTo(root.BottomAnchor).Active = true;

			LeftAnchor.ConstraintEqualTo(icon.RightAnchor).Active = true;
			RightAnchor.ConstraintEqualTo(root.RightAnchor).Active = true;

			Priorities();
		}
		public void Title( in Stack root, in Stack value, in IconView icon )
		{
			root.AddArrangedSubview(this);

			TranslatesAutoresizingMaskIntoConstraints = false;

			TopAnchor.ConstraintEqualTo(root.TopAnchor).Active = true;
			BottomAnchor.ConstraintEqualTo(root.BottomAnchor).Active = true;

			LeftAnchor.ConstraintEqualTo(icon.RightAnchor).Active = true;
			RightAnchor.ConstraintEqualTo(value.LeftAnchor).Active = true;

			Priorities();
		}

		public void Accessory( in Stack root, in IconView icon, in UIView accessory )
		{
			root.AddArrangedSubview(this);

			TranslatesAutoresizingMaskIntoConstraints = false;

			TopAnchor.ConstraintEqualTo(root.TopAnchor).Active = true;
			BottomAnchor.ConstraintEqualTo(root.BottomAnchor).Active = true;

			LeftAnchor.ConstraintEqualTo(icon.RightAnchor).Active = true;
			RightAnchor.ConstraintEqualTo(accessory.LeftAnchor).Active = true;

			Priorities();
		}

		public void Value( in Stack root, in Stack title )
		{
			root.AddArrangedSubview(this);

			TranslatesAutoresizingMaskIntoConstraints = false;

			TopAnchor.ConstraintEqualTo(root.TopAnchor).Active = true;
			BottomAnchor.ConstraintEqualTo(root.BottomAnchor).Active = true;

			LeftAnchor.ConstraintEqualTo(title.RightAnchor).Active = true;
			RightAnchor.ConstraintEqualTo(root.LeftAnchor).Active = true;

			Priorities();
		}


		
		protected override void Dispose( bool disposing )
		{
			if ( disposing ) { RemoveFromSuperview(); }

			base.Dispose(disposing);
		}
	}
}
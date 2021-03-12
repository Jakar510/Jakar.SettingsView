using System;
using Jakar.SettingsView.iOS.Controls;
using Jakar.SettingsView.iOS.Controls.Core;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Misc;
using UIKit;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.iOS.BaseCell
{
	[Foundation.Preserve(AllMembers = true)]
	public abstract class BaseValueCell<TCell> : BaseAiDescriptionCell where TCell : UIView
	{
		protected HintView<TCell> _Hint { get; }
		protected TCell _Value { get; }
		protected UIStackView _ValueStack { get; }

		protected BaseValueCell( Cell cell ) : base(cell)
		{
			_ValueStack = CreateStackView(UILayoutConstraintAxis.Vertical);
			_ValueStack.HeightAnchor.ConstraintEqualTo(_ContentView.HeightAnchor).Active = true;
			var valueStackWidth = NSLayoutConstraint.Create(_ValueStack,
													   NSLayoutAttribute.Width,
													   NSLayoutRelation.Equal,
													   _ContentView,
													   NSLayoutAttribute.Width,
													   SVConstants.Layout.ColumnFactors.ValueStack,
													   SVConstants.Layout.Factor.Zero
													  );
			_ValueStack.AddConstraint(valueStackWidth);
			_ValueStack.AddArrangedSubview(_Title);

			// -----------------------------------------------------------------------------------

			_Hint = new HintView<TCell>(this);
			_Hint.WidthAnchor.ConstraintEqualTo(_ValueStack.WidthAnchor).Active = true;

			_ValueStack.AddArrangedSubview(_Hint);

			// -----------------------------------------------------------------------------------

			_Value = InstanceCreator.Create<TCell>(this);
			_Value.WidthAnchor.ConstraintEqualTo(_ValueStack.WidthAnchor).Active = true;
			_ValueStack.AddArrangedSubview(_Value);

			// -----------------------------------------------------------------------------------

			_ContentView.AddArrangedSubview(_ValueStack);
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_Hint.RemoveFromSuperview();
				_Hint.Dispose();

				_Value.RemoveFromSuperview();
				_Value.Dispose();

				_ValueStack.RemoveFromSuperview();
				_ValueStack.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}
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
			_Hint = new HintView<TCell>(this);
			_Value = InstanceCreator.Create<TCell>(this);
			InitializeView();
		}

		protected override void InitializeView()
		{
			base.InitializeView();

			// ----------------------------------------------------------------------------------------------

			_ContentView.AddArrangedSubview(_ValueStack);
			_ValueStack.HeightAnchor.ConstraintEqualTo(_ContentView.HeightAnchor).Active = true;
			NSLayoutConstraint width = _ValueStack.WidthAnchor.ConstraintGreaterThanOrEqualTo(_ContentView.WidthAnchor, SVConstants.Layout.ColumnFactors.ValueStack);
			width.Active = true;
			width.Priority = SVConstants.Layout.Priority.HIGH;
			_ContentView.AddConstraint(width);

			_ValueStack.AddArrangedSubview(_Hint);
			_Hint.WidthAnchor.ConstraintEqualTo(_ValueStack.WidthAnchor).Active = true;

			_ValueStack.AddArrangedSubview(_Value);
			_Value.WidthAnchor.ConstraintEqualTo(_ValueStack.WidthAnchor).Active = true;

			InitRoot();
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_Hint.Dispose();

				_Value.Dispose();

				_ValueStack.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}
using System;
using Jakar.SettingsView.iOS.Controls;
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
		// protected UIStackView _CellValueStack { get; }


		protected BaseValueCell( Cell cell ) : base(cell)
		{
			_Hint = new HintView<TCell>(this);
			_Value = InstanceCreator.Create<TCell>(this);
		}
	}
}
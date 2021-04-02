﻿// unset

using Jakar.SettingsView.iOS.Controls.Core;
using Jakar.SettingsView.Shared.CellBase;
using UIKit;
using Xamarin.Forms;

namespace Jakar.SettingsView.iOS.BaseCell
{
	[Foundation.Preserve(AllMembers = true)]
	public abstract class BaseLabelCellView<TCell> : BaseValueCell<UITextView, TCell, ValueView<TCell>> where TCell : ValueTextCellBase
	{
		protected UITextView _Control => _Value.Control;

		protected BaseLabelCellView( Cell formsCell ) : base(formsCell) { }
	}
}
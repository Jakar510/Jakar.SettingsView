// unset

namespace Jakar.SettingsView.iOS.BaseCell
{
	[Foundation.Preserve(AllMembers = true)]
	public abstract class BaseLabelCellView<TCell> : BaseValueCell<UITextView, TCell, ValueView<TCell>> where TCell : ValueCellBase
	{
		protected UITextView _Control => _Value.Control;

		protected BaseLabelCellView( TCell formsCell ) : base(formsCell) { }
	}
}

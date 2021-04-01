// unset

using Jakar.SettingsView.Shared.Cells;

namespace Jakar.SettingsView.Shared.Interfaces
{
	public interface IUpdateEntryCell<in TRenderer, in TColor>
	{
		public void Init( EntryCell cell, TRenderer renderer );

		public void PerformSelectAction();
		public bool UpdateSelectAction();
		public bool UpdateKeyboard();
		public bool UpdateIsPassword();
		public bool UpdatePlaceholder();
		public bool UpdateTextAlignment();
		public bool UpdateAccentColor();
		public bool ChangeTextViewBack( TColor accent );
	}
}
// unset

namespace Jakar.SettingsView.Shared.Interfaces
{
	public interface IUpdateEntryCell<in TColor>
	{
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
// unset

namespace Jakar.SettingsView.Shared.Interfaces
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public interface IUpdateCell<out TColor, in TCell> : IUpdateCell
	{
		public TColor DefaultTextColor { get; }

		public void SetCell( TCell cell );
	}
}
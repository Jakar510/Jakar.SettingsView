// unset

namespace Jakar.SettingsView.Shared.Interfaces
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public interface IDefaultColors<out TColor>
	{
		public TColor DefaultBackgroundColor { get; }

		public TColor DefaultTextColor { get; }
	}
}
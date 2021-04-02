// unset

using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Interfaces
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public interface IUseConfiguration
	{
		public Color Color { get; }

		public double FontSize { get; }
		public string? FontFamily { get; }
		public FontAttributes FontAttributes { get; }
		
		public TextAlignment TextAlignment { get; }
	}
}
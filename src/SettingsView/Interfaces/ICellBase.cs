// unset

using Jakar.SettingsView.Shared.sv;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.Interfaces
{
	public interface ICellBase : IVisibleCell
	{
		public Cell Cell { get; }
		public Section? Section { get; set; }
		public sv.SettingsView Parent { get; set; }
		public Color BackgroundColor { get; set; }
		public void Reload();
	}
}
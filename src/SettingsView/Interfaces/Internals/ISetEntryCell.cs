// unset

using Jakar.SettingsView.Shared.Cells;

namespace Jakar.SettingsView.Shared.Interfaces
{
	public interface ISetEntryCell<in TRenderer>
	{
		public void Init( EntryCell cell, TRenderer renderer );
	}
}
// unset

using Jakar.SettingsView.Shared.Cells;


namespace Jakar.SettingsView.Shared.Interfaces
{
	public interface ISetCell<TCell, TRenderer>
	{
		public TCell     Cell         { get; }
		public TRenderer CellRenderer { get; }

		public void Init( TCell cell, TRenderer renderer );
	}
}

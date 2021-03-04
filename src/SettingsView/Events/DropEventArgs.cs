using System;
using Jakar.SettingsView.Shared.sv;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Events
{
	public class DropEventArgs : EventArgs
	{
		public Section Section { get; }
		public Cell Cell { get; }
		public object SectionSource { get; }
		public object CellSource { get; }

		public DropEventArgs( Section section, Cell cell )
		{
			Section = section;
			Cell = cell;
			SectionSource = section.BindingContext;
			CellSource = cell.BindingContext;
		}
	}
}
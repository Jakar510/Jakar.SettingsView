using System;
using Xamarin.Forms;


namespace Jakar.SettingsView.Shared.Events
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public class CellTappedEventArgs : EventArgs
	{
		public Cell Cell { get; }

		public CellTappedEventArgs( Cell cell ) => Cell = cell;
	}
}

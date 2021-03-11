// unset

using System;
using Jakar.SettingsView.Shared.Misc;

namespace Jakar.SettingsView.Shared.Interfaces
{
	public interface IValueChanged<TValue>
	{
		public event EventHandler<SVValueChangedEventArgs<TValue>>? ValueChanged;
		internal void SendValueChanged( TValue value );
	}
}
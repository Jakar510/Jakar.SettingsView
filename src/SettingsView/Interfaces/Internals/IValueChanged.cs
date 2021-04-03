// unset

using System;
using System.Collections;
using System.Collections.Generic;
using Jakar.SettingsView.Shared.Misc;

namespace Jakar.SettingsView.Shared.Interfaces
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public interface IValueChanged<TValue>
	{
		public event EventHandler<SVValueChangedEventArgs<TValue>>? ValueChanged;
		internal void SendValueChanged( TValue value );
	}

	[Xamarin.Forms.Internals.Preserve(true, false)]
	public interface IMultiValueChanged<TValue> : IValueChanged<TValue>
	{
		internal void SendValueChanged( IEnumerable<TValue> value );
	}
}
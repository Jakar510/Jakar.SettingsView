// unset

using System;

namespace Jakar.SettingsView.Shared.Misc
{

	public class SVValueChangedEventArgs<TValue> : EventArgs
	{
		public TValue Value { get; }
		public SVValueChangedEventArgs( TValue value ) { Value = value; }
	}

	public class SVValueChangedEventArgs : SVValueChangedEventArgs<object>
	{
		public SVValueChangedEventArgs( object value ) : base(value) { }
	}
}
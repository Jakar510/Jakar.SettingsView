// unset

using System;

namespace Jakar.SettingsView.Shared.Misc
{

	[Xamarin.Forms.Internals.Preserve(true, false)]
	public class SVValueChangedEventArgs<TValue> : EventArgs
	{
		public TValue Value { get; }
		public SVValueChangedEventArgs( TValue value ) => Value = value;
	}

	[Xamarin.Forms.Internals.Preserve(true, false)]
	public class SVValueChangedEventArgs : SVValueChangedEventArgs<object>
	{
		public SVValueChangedEventArgs( object value ) : base(value) { }
	}
}
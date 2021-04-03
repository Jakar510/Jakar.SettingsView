// unset

using System;
using System.Collections;
using System.Collections.Generic;

namespace Jakar.SettingsView.Shared.Misc
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public class SVValueChangedEventArgs<TValue> : EventArgs
	{
		public bool IsSingleValue => Items is null;

		public TValue? Value { get; }
		public IEnumerable<TValue>? Items { get; }


		public SVValueChangedEventArgs( TValue value ) => Value = value;
		public SVValueChangedEventArgs( IEnumerable<TValue> items ) => Items = items;
	}


	[Xamarin.Forms.Internals.Preserve(true, false)]
	public class SVValueChangedEventArgs : SVValueChangedEventArgs<object>
	{
		public SVValueChangedEventArgs( object value ) : base(value) { }
		public SVValueChangedEventArgs( IEnumerable<object> items ) : base(items) { }
	}
}
// unset

using System;
using System.Collections.Generic;


namespace Jakar.SettingsView.Shared.Misc
{
	[Xamarin.Forms.Internals.Preserve(true, false)]

	// ReSharper disable once InconsistentNaming
	public class SVValueChangedEventArgs<TValue> : EventArgs
	{
		public bool IsSingleValue => Items is null;


		public TValue?              Value { get; }
		public IEnumerable<TValue>? Items { get; }


		public SVValueChangedEventArgs( TValue value ) => Value = value;

		public SVValueChangedEventArgs( IEnumerable<TValue> items ) => Items = items;
	}
}

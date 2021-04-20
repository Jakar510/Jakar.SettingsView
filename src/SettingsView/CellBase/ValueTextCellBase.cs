// unset

using System;
using System.Collections.Generic;
using Jakar.SettingsView.Shared.Interfaces;
using Jakar.SettingsView.Shared.Misc;
using Xamarin.Forms;


#nullable enable
namespace Jakar.SettingsView.Shared.CellBase
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public abstract class ValueTextCellBase : ValueCellBase
	{
		public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(ValueTextCellBase), -1);

		public static readonly BindableProperty ValueTextProperty = BindableProperty.Create(nameof(ValueText),
																							typeof(string),
																							typeof(ValueTextCellBase),
																							default(string?),
																							BindingMode.TwoWay,
																							propertyChanging: ValueTextPropertyChanging
																						   );

		public string? ValueText
		{
			get => (string?) GetValue(ValueTextProperty);
			set => SetValue(ValueTextProperty, value);
		}

		public int MaxLength
		{
			get => (int) GetValue(MaxLengthProperty);
			set => SetValue(MaxLengthProperty, value);
		}

		private static void ValueTextPropertyChanging( BindableObject bindable, object oldValue, object newValue )
		{
			var maxlength = (int) bindable.GetValue(MaxLengthProperty);

			if ( maxlength < 0 ) return;

			string newString = newValue?.ToString() ?? string.Empty;

			if ( newString.Length <= maxlength ) return;
			string oldString = oldValue?.ToString() ?? string.Empty;

			if ( oldString.Length > maxlength )
			{
				string trimStr = oldString.Substring(0, maxlength);
				bindable.SetValue(ValueTextProperty, trimStr);
			}
			else { bindable.SetValue(ValueTextProperty, oldString); }
		}
	}



	public abstract class ValueTextCellBase<TValue> : ValueTextCellBase, IValueChanged<TValue>
	{
		public event EventHandler<SVValueChangedEventArgs<TValue>>? ValueChanged;

		void IValueChanged<TValue>.SendValueChanged( TValue value ) => ValueChanged?.Invoke(this, new SVValueChangedEventArgs<TValue>(value));
		void IValueChanged<TValue>.SendValueChanged( IEnumerable<TValue> value ) => ValueChanged?.Invoke(this, new SVValueChangedEventArgs<TValue>(value));
		internal IValueChanged<TValue> ValueChangedHandler => this;
	}
}

// unset

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Jakar.SettingsView.Shared.Converters
{
	[TypeConversion(typeof(double?))]
	public class NullableDoubleTypeConverter : FontSizeConverter // IExtendedTypeConverter 
	{
		public override bool CanConvertFrom( Type? sourceType ) => sourceType is null || sourceType == typeof(string);
		public override object? ConvertFromInvariantString( string? value ) => Convert(value);
		public double? Convert( string? value ) =>
			double.TryParse(value, out double d)
				? d
				: null;

		public override string? ConvertToInvariantString( object? value ) => value?.ToString();
	}
}
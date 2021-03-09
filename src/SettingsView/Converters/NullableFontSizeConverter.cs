// unset

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

#nullable enable
namespace Jakar.SettingsView.Shared.Converters
{
	[TypeConversion(typeof(double))]
	public class NullableFontSizeConverter : FontSizeConverter // IExtendedTypeConverter 
	{
		public override bool CanConvertFrom( Type? sourceType ) => sourceType is null || sourceType == typeof(string);
		public override object? ConvertFromInvariantString( string? value ) => Convert(value);
		public double? Convert( string? value ) =>
			value switch
			{
				null => default,
				_ => (double) base.ConvertFromInvariantString(value)
			};

		public override string? ConvertToInvariantString( object? value ) => value?.ToString();
	}
}
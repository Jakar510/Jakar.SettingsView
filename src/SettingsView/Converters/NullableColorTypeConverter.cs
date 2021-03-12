using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

#nullable enable
namespace Jakar.SettingsView.Shared.Converters
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	[TypeConversion(typeof(Color?))]
	public class NullableColorTypeConverter : ColorTypeConverter // IExtendedTypeConverter 
	{
		public override bool CanConvertFrom( Type? sourceType ) => sourceType is null || sourceType == typeof(string);
		public override object? ConvertFromInvariantString( string? value ) => Convert(value);
		public Color? Convert( string? value ) =>
			string.IsNullOrWhiteSpace(value)
				? null
				: (Color) base.ConvertFromInvariantString(value);
	
		public override string? ConvertToInvariantString( object? value ) =>
			value switch
			{
				null => string.Empty, 
				_ => base.ConvertToInvariantString(value)
			};
	}
}
// unset

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

#nullable enable
namespace Jakar.SettingsView.Shared.Converters
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	[TypeConversion(typeof(ImageSource))]
	public class SVImageSourceConverter : TypeConverter // IExtendedTypeConverter 
	{
		private readonly ImageSourceConverter _converter = new();
		public override bool CanConvertFrom( Type? sourceType ) => sourceType is null || sourceType == typeof(string);
		public override object? ConvertFromInvariantString( string? value ) => Convert(value);
		public ImageSource? Convert( string? value ) =>
			string.IsNullOrWhiteSpace(value)
				? null
				: (ImageSource) _converter.ConvertFromInvariantString(value);
	
		public override string? ConvertToInvariantString( object? value ) =>
			value switch
			{
				null => string.Empty, 
				_ => _converter.ConvertToInvariantString(value)
			};
	}
}
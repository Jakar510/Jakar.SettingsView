﻿// unset

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Jakar.SettingsView.Shared.Converters
{
	[TypeConversion(typeof(Size?))]
	public class NullableSizeConverter : TypeConverter
	{
		public override bool CanConvertFrom( Type? sourceType ) => sourceType is null || sourceType == typeof(string);
		public override object? ConvertFromInvariantString( string? value ) => Convert(value);
		public Size? Convert( string? value )
		{
			if ( string.IsNullOrWhiteSpace(value) ) return null;
			string[] items = value.Split(',');

			switch ( items.Length )
			{
				case 1:
					double w = double.Parse(items[0]);
					return new Size(w, w);

				case 2:
					return new Size(double.Parse(items[0]), double.Parse(items[1]));
			}

			throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(Size)}");
		}
		public override string? ConvertToInvariantString( object? value ) =>
			value switch
			{
				null => null,
				Size size => $"{size.Width},{size.Height}",
				_ => value.ToString()
			};
	}
}
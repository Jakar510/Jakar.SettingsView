﻿using System;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Converters
{
	/// <summary>
	/// Size converter.
	/// </summary>
	public class SizeConverter : TypeConverter
	{
		/// <summary>
		/// Converts from invariant string.
		/// </summary>
		/// <returns>The from invariant string.</returns>
		/// <param name="value">Value.</param>
		public override object ConvertFromInvariantString( string value )
		{
			if ( value is null ) throw new InvalidOperationException($"Cannot convert \"value\" into {typeof(Size)}");
			string[] size = value.Split(',');

			switch ( size.Length )
			{
				case 1:
					double w = double.Parse(size[0]);
					return new Size(w, w);
				case 2:
					return new Size(double.Parse(size[0]), double.Parse(size[1]));
			}

			throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(Size)}");
		}
	}
}
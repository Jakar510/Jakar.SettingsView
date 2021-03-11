using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.Misc
{
	public static class PropertyChangedEventArgsExtensions
	{
		public static bool IsOneOf( this PropertyChangedEventArgs e, params BindableProperty[] properties ) => e.IsOneOf(properties.Select(property => property.PropertyName));
		public static bool IsOneOf( this PropertyChangedEventArgs e, IEnumerable<string> properties ) => e.IsOneOf(properties.ToArray());
		public static bool IsOneOf( this PropertyChangedEventArgs e, params string[] properties ) => properties.Any(property => property == e.PropertyName);


		public static bool IsEqual( this PropertyChangedEventArgs e, BindableProperty properties ) => e.IsEqual(properties.PropertyName);
		public static bool IsEqual( this PropertyChangedEventArgs e, string properties ) => e.PropertyName == properties;
	}
}
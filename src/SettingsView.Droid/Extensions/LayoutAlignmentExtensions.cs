﻿using Android.Views;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Droid.Extensions
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public static class LayoutAlignmentExtensions
	{
		public static GravityFlags ToNativeVertical( this LayoutAlignment forms )
		{
			return forms switch
				   {
					   LayoutAlignment.Start => GravityFlags.Top,
					   LayoutAlignment.Center => GravityFlags.CenterVertical,
					   LayoutAlignment.End => GravityFlags.Bottom,
					   _ => GravityFlags.FillHorizontal
				   };
		}

		public static GravityFlags ToNativeHorizontal( this LayoutAlignment forms )
		{
			return forms switch
				   {
					   LayoutAlignment.Start => GravityFlags.Start,
					   LayoutAlignment.Center => GravityFlags.CenterHorizontal,
					   LayoutAlignment.End => GravityFlags.End,
					   _ => GravityFlags.FillVertical
				   };
		}
	}
}
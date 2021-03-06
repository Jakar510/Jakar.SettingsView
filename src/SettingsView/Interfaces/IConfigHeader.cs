﻿// unset

using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.Interfaces
{
	public interface IConfigHeader
	{
		public Color BackgroundColor { get; set; }
		public Color TextColor { get; set; }


		[TypeConverter(typeof(FontSizeConverter))]
		public double FontSize { get; set; }

		public string? FontFamily { get; set; }


		public FontAttributes FontAttributes { get; set; }
		public LayoutAlignment TextVerticalAlign { get; set; }
		public LayoutAlignment TextHorizontalAlign { get; set; }


		public Thickness Padding { get; set; }
		public double Height { get; set; }
	}
}
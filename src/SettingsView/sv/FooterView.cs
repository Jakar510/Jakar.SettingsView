// unset

using System;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Interfaces;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.sv
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public abstract class FooterView : BaseHeaderFooterView, ISectionFooter
	{
		Section? ISectionFooterHeader.Section { get; set; }

		public abstract void SetText( string? value );
		public abstract void SetTextColor( Color value );
		public abstract void SetBackground( Color value );
		public abstract void SetTextFont( double fontSize, string family, FontAttributes attributes );

		protected FooterView() { }
	}
}
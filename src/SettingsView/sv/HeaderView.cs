// unset

using System;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Interfaces;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.sv
{
	public abstract class HeaderView : BaseHeaderFooterView, ISectionHeader
	{
		public static readonly BindableProperty IsCollapsibleProperty = BindableProperty.Create(nameof(IsCollapsible), typeof(bool), typeof(HeaderView), default(bool));
		public static readonly BindableProperty IsCollapsedProperty = BindableProperty.Create(nameof(IsCollapsed), typeof(bool), typeof(HeaderView), default(bool));

		Section? ISectionFooterHeader.Section { get; set; }

		public abstract ImageSource? Source { get; set; }
		public abstract bool IsCollapsed { get; set; }
		public abstract bool IsCollapsible { get; set; }
		public virtual ImageSource? CollapsedIcon { get; set; }
		public virtual ImageSource? ExpandedIcon { get; set; }


		protected HeaderView() : base() { }


		public abstract void SetText( string? value );
		public abstract void SetTextColor( Color value );
		public abstract void SetBackground( Color value );

		public virtual bool Clicked()
		{
			if ( !IsCollapsible ) return false;

			if ( IsCollapsed ) { Expand(); }
			else { Collapse(); }

			return true;
		}
		public virtual void Collapse()
		{
			Source = CollapsedIcon;
			IsCollapsed = true;
		}
		public virtual void Expand()
		{
			Source = ExpandedIcon;
			IsCollapsed = false;
		}
	}
}
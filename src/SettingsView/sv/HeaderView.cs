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

		public abstract ImageSource Source { get; set; }
		public abstract bool IsCollapsed { get; set; }
		public abstract bool IsCollapsible { get; set; }
		public virtual ImageSource? CollapsedIcon { get; set; }
		public virtual ImageSource? ExpandedIcon { get; set; }

		protected HeaderView() : base() => BackgroundColor = SVConstants.HEADER_BACKGROUND_COLOR;

		public virtual bool Clicked()
		{
			if ( !IsCollapsible ) return false;

			IsCollapsed = !IsCollapsed;
			return true;
		}

#pragma warning disable CS8601 // Possible null reference assignment.
		public virtual void Collapse() { Source = CollapsedIcon; }
		public virtual void Expand() { Source = ExpandedIcon; }
#pragma warning restore CS8601 // Possible null reference assignment.

		// public new SettingsView? Parent
		// {
		// 	get => (SettingsView) base.Parent;
		// 	set
		// 	{
		// 		if ( Parent is not null ) { PropertyChanged -= Parent.ParentOnPropertyChanged; }
		//
		// 		base.Parent = value;
		//
		// 		if ( Parent is not null ) { PropertyChanged += Parent.ParentOnPropertyChanged; }
		// 	}
		// }
	}
}
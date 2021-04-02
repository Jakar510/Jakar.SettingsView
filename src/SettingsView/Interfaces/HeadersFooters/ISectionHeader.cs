using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.Interfaces
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public interface ISectionHeader : ISectionFooterHeader
	{
		public bool IsCollapsed { get; set; }
		public bool IsCollapsible { get; set; }
		public ImageSource? CollapsedIcon { get; set; }
		public ImageSource? ExpandedIcon { get; set; }
		public ImageSource? Source { get; set; }


		public bool Clicked();
		public void Collapse();
		public void Expand();
	}
}
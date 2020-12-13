using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Interfaces
{
	public interface ISectionHeader
	{
		public string Title { get; set; }
		public bool IsCollapsed { get; set; }
		public bool IsCollapsible { get; set; }
		public ImageSource CollapsedIcon { get; set; }
		public ImageSource ExpandedIcon { get; set; }
	}
}

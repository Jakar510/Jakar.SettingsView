using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.Interfaces
{
	public interface ISectionHeader: ISectionFooterHeader
	{
		public bool IsCollapsed { get; set; }
		public bool IsCollapsible { get; set; }
		public ImageSource? CollapsedIcon { get; set; }
		public ImageSource? ExpandedIcon { get; set; }
		public ImageSource Source { get; set; }

		// public virtual SizeRequest HeightRequest { get; set; } = new(Size.Zero, new Size(-1, SVConstants.MIN_ROW_HEIGHT));
		// public virtual Thickness Padding { get; set; } = new(SVConstants.PADDING);
		// public virtual Color BackgroundColor { get; set; } = SVConstants.BACKGROUND_COLOR;

		public bool Clicked();
		public void Collapse();
		public void Expand();
	}
}
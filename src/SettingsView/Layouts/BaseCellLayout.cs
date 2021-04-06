using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;


#nullable enable
namespace Jakar.SettingsView.Shared.Layouts
{
	public abstract class BaseCellLayout : Grid
	{
		protected BaseCellLayout() : base()
		{
			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions   = LayoutOptions.FillAndExpand;
		}
	}
}

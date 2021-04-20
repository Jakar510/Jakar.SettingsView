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

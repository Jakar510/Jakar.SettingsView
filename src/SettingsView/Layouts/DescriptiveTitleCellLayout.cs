using Jakar.SettingsView.Shared.Config;
using Xamarin.Forms;


namespace Jakar.SettingsView.Shared.Layouts
{
	public class DescriptiveTitleCellLayout : BaseCellTitleLayout
	{
		public DescriptiveTitleCellLayout()
		{
			RowDefinitions = Definitions.Rows();
			ColumnDefinitions = Definitions.Columns.Title();

			Children.Add(Icon);
			Children.Add(Title);
			Children.Add(Description);
		}
	}
}
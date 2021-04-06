using Jakar.SettingsView.Shared.Interfaces;
using Jakar.SettingsView.Shared.Layouts.Controls;
using Xamarin.Forms;


namespace Jakar.SettingsView.Shared.Layouts
{
	public class BaseAccessoryCellLayout<TAccessory> : BaseCellTitleLayout where TAccessory : View, IUpdateAccessoryControl, new()
	{
		public TAccessory Accessory { get; protected set; }


		public BaseAccessoryCellLayout()
		{
			Accessory = new TAccessory();
			
			RowDefinitions    = Definitions.Rows();
			ColumnDefinitions = Definitions.Columns.Accessory();

			Children.Add(Icon);
			Children.Add(Title);
			Children.Add(Description);
			Children.Add(Accessory);
		}
	}



	public class RadioCellLayout : BaseAccessoryCellLayout<AiRadioButton> { }



	public class SwitchCellLayout : BaseAccessoryCellLayout<AiSwitch> { }



	public class CheckCellLayout : BaseAccessoryCellLayout<AiCheckBox> { }
}

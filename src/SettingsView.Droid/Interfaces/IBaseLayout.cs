using Android.Widget;

namespace Jakar.SettingsView.Droid.Interfaces
{
	internal interface IBaseLayout : IBaseCell
	{
		public GridLayout CellLayout { get; set; }
	}

	internal interface IAccessory : IBaseCell
	{
		public LinearLayout AccessoryStack { get; set; }
	}
}
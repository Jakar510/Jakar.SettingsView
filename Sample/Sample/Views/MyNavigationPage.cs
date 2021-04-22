using AiForms.Effects;
using Xamarin.Forms;


namespace Jakar.SettingsView.Sample.Shared.Views
{
	public class MyNavigationPage : NavigationPage
	{
		public MyNavigationPage()
		{
			BarTextColor = Color.FromHex("#CC9900");
			//BarBackgroundColor = Color.White;
			AlterColor.SetOn(this, true);
			AlterColor.SetAccent(this, Color.FromHex("#CC9900"));
		}
	}
}
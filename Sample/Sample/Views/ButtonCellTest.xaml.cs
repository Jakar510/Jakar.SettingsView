using System;
using Xamarin.Forms;


namespace Jakar.SettingsView.Sample.Shared.Views
{
	public partial class ButtonCellTest : ContentPage
	{
		public ButtonCellTest() { InitializeComponent(); }

		private void Handle_Tapped( object sender, EventArgs e ) { DisplayAlert("", "Tapped", "OK"); }
	}
}
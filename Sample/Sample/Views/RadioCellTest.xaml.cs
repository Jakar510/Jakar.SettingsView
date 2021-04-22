using System;
using Xamarin.Forms;


namespace Jakar.SettingsView.Sample.Shared.Views
{
	public partial class RadioCellTest : ContentPage
	{
		public RadioCellTest() { InitializeComponent(); }

		private void Handle_Tapped( object sender, EventArgs e ) { DisplayAlert("", "Tapped", "OK"); }
	}
}
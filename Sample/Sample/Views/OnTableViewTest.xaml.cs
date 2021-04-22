using System;
using Xamarin.Forms;


namespace Jakar.SettingsView.Sample.Shared.Views
{
	public partial class OnTableViewTest : ContentPage
	{
		public OnTableViewTest() { InitializeComponent(); }

		private void Handle_Tapped( object sender, EventArgs e ) { DisplayAlert("", "Tapped", "OK"); }

		private void Handle_Completed( object sender, EventArgs e ) { DisplayAlert("", "Completed", "OK"); }
	}
}
using System;
using Xamarin.Forms;


namespace Jakar.SettingsView.Sample.Shared.Views
{
	public partial class EntryCellTest : ContentPage
	{
		public EntryCellTest() { InitializeComponent(); }

		private void Handle_Tapped( object sender, EventArgs e )
		{
			//DisplayAler11t("","Tapped","OK");
		}

		private void Handle_Completed( object sender, EventArgs e ) { DisplayAlert("", "Completed", "OK"); }

		private void Button_Tapped( object sender, EventArgs e ) { entryCell.SetFocus(); }
	}
}
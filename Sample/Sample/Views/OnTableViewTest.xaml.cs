using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Sample.Views
{
	public partial class OnTableViewTest : ContentPage
	{
		public OnTableViewTest() { InitializeComponent(); }

		private void Handle_Tapped( object sender, EventArgs e ) { DisplayAlert("", "Tapped", "OK"); }

		private void Handle_Completed( object sender, EventArgs e ) { DisplayAlert("", "Completed", "OK"); }
	}
}
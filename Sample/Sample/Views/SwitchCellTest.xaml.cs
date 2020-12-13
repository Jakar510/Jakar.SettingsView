using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Sample.Views
{
	public partial class SwitchCellTest : ContentPage
	{
		public SwitchCellTest() { InitializeComponent(); }

		private void Handle_Tapped( object sender, EventArgs e ) { DisplayAlert("", "Tapped", "OK"); }
	}
}
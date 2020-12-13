using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Sample.Views
{
	public partial class ButtonCellTest : ContentPage
	{
		public ButtonCellTest() { InitializeComponent(); }

		private void Handle_Tapped( object sender, EventArgs e ) { DisplayAlert("", "Tapped", "OK"); }
	}
}
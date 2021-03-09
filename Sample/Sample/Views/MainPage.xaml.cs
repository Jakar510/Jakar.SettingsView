using System;
using Xamarin.Forms;

namespace Sample.Views
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			try { InitializeComponent(); }
			catch ( Exception e )
			{
				Console.WriteLine(e);
				throw;
			}
		}
	}
}
using System.Collections.ObjectModel;
using Jakar.SettingsView.Sample.Shared.ViewModels;
using Xamarin.Forms;


namespace Jakar.SettingsView.Sample.Shared.Views
{
	public partial class ShellTestPage : ContentPage
	{
		public ObservableCollection<Person> ItemsSource { get; set; } = new ObservableCollection<Person>();

		public ShellTestPage()
		{
			InitializeComponent();

			for ( var i = 0; i < 30; i++ )
			{
				ItemsSource.Add(new Person()
								{
									Name = $"Name{i}",
									Age = 30 - i
								});
			}


			BindingContext = this;
		}
	}
}
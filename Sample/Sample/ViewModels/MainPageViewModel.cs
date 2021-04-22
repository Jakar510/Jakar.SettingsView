using System;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;


namespace Jakar.SettingsView.Sample.Shared.ViewModels
{
	public class MainPageViewModel : BindableBase, INavigationAware
	{
		public ReactiveCommand<string> GoToPage { get; set; } = new();
		public ReactiveCommand<string> GoToTest { get; set; } = new();

		public MainPageViewModel( INavigationService navigationService )
		{
			GoToPage.Subscribe(async p => { await navigationService.NavigateAsync(p + "Page"); });
			GoToTest.Subscribe(async p => { await navigationService.NavigateAsync(p); });
		}

		public void OnNavigatedFrom( NavigationParameters parameters ) { }

		public void OnNavigatedTo( NavigationParameters parameters ) { }

		public void OnNavigatingTo( NavigationParameters parameters ) { }

		public void OnNavigatedFrom( INavigationParameters parameters ) { }

		public void OnNavigatedTo( INavigationParameters parameters ) { }
	}
}
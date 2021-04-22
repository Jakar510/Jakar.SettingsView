using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;


namespace Jakar.SettingsView.Sample.Shared.ViewModels
{
	public class SurveyPageViewModel : BindableBase, INavigatedAware
	{
		public ObservableCollection<Hoge> ItemsSource { get; set; }
		public ReactivePropertySlim<string> Text { get; } = new();
		public ReactiveCommand ChangeCommand { get; } = new();

		public SurveyPageViewModel()
		{
			ItemsSource = new ObservableCollection<Hoge>(new List<Hoge>
														 {
															 new()
															 {
																 Name = "A",
																 Value = 1
															 },
															 new()
															 {
																 Name = "B",
																 Value = 2
															 },
															 new()
															 {
																 Name = "C",
																 Value = 3
															 }
														 });

			Text.Value = "テキスト";

			var toggle = true;
			ChangeCommand.Subscribe(_ =>
									{
										if ( toggle ) { Text.Value = "テキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキストテキスト"; }
										else { Text.Value = "テキスト"; }

										toggle = !toggle;
									});
		}

		public void OnNavigatedFrom( NavigationParameters parameters ) { }

		public void OnNavigatedTo( NavigationParameters parameters )
		{
			//RaisePropertyChanged(nameof(ItemsSource));
		}

		public void OnNavigatedFrom( INavigationParameters parameters ) { }

		public void OnNavigatedTo( INavigationParameters parameters ) { }

		public class Hoge
		{
			public string Name { get; set; }
			public int Value { get; set; }
		}
	}
}
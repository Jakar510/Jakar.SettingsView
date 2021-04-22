using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Reactive.Bindings;


namespace Jakar.SettingsView.Sample.Shared.ViewModels
{
	public class SettingsViewPageViewModel : BindableBase
	{
		[Required(ErrorMessage = "Required")]
		[StringLength(15, ErrorMessage = "Input text less than or equal to 15 characters")]
		public ReactiveProperty<string> InputText { get; }

		public ReadOnlyReactiveProperty<string> InputError { get; }

		public ReactiveProperty<bool> InputSectionVisible { get; } = new(true);

		public ReactiveCommand ToProfileCommand { get; set; } = new();
		public AsyncReactiveCommand SectionToggleCommand { get; set; }

		public ObservableCollection<Person> ItemsSource { get; } = new();
		public ObservableCollection<Person> SelectedItems { get; } = new();

		public ObservableCollection<string> TextItems { get; } = new(new List<string>
																	 {
																		 "Red",
																		 "Blue",
																		 "Green",
																		 "Pink",
																		 "Black",
																		 "White"
																	 }
																	);

		public ReactiveProperty<string> SelectedText { get; } = new("Green");

		private readonly string[] languages =
		{
			"Java",
			"C#",
			"JavaScript",
			"PHP",
			"Perl",
			"C++",
			"Swift",
			"Kotlin",
			"Python",
			"Ruby",
			"Scala",
			"F#"
		};

		public SettingsViewPageViewModel( INavigationService navigationService, IPageDialogService pageDlg )
		{
			InputText = new ReactiveProperty<string>().SetValidateAttribute(() => InputText);

			InputError = InputText.ObserveErrorChanged.Select(x => x?.Cast<string>()?.FirstOrDefault()).ToReadOnlyReactiveProperty();

			SectionToggleCommand = InputText.ObserveHasErrors.Select(x => !x).ToAsyncReactiveCommand();
			SectionToggleCommand.Subscribe(async () =>
										   {
											   InputSectionVisible.Value = !InputSectionVisible.Value;
											   await Task.Delay(250);
										   }
										  );

			ToProfileCommand.Subscribe(async () => { await navigationService.NavigateAsync("DummyPage"); });

			var randomAge = new Random();
			foreach ( string item in languages )
			{
				ItemsSource.Add(new Person()
								{
									Name = item,
									Age = randomAge.Next(100)
								}
							   );
			}

			// var random = new Random();
			// var items = new List<int>();
			// for ( var i = 0; i < 3; i++ )
			// {
			// 	int item = random.Next(ItemsSource.Count);
			// 	while ( items.Contains(item) ) { item = random.Next(ItemsSource.Count); }
			//
			// 	items.Add(item);
			// 	SelectedItems.Add(ItemsSource[item]);
			// }
			//
			// SelectedItems.CollectionChanged += SelectedItemsOnCollectionChanged;
		}
		private void SelectedItemsOnCollectionChanged( object sender, NotifyCollectionChangedEventArgs e ) { Console.WriteLine(e); }

		public class Person
		{
			public string Name { get; set; }
			public int Age { get; set; }
		}
	}
}
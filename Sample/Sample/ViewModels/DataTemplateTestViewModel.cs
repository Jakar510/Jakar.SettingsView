using System;
using Reactive.Bindings;
using Prism.Services;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace Sample.ViewModels
{
	public class DataTemplateTestViewModel : ViewModelBase
	{
		public ReactiveCommand AddCommand { get; set; } = new();
		public ReactiveCommand DelCommand { get; set; } = new();
		public ReactiveCommand RepCommand { get; set; } = new();
		public ReactiveCommand ClrCommand { get; set; } = new();
		public ReactiveCommand BtmCommand { get; set; } = new();
		public ReactiveCommand TopCommand { get; set; } = new();

		public ReactiveProperty<bool> ScrollToBottom { get; } = new();
		public ReactiveProperty<bool> ScrollToTop { get; } = new();

		public ReactiveCommand DoCommand { get; } = new();
		public ObservableCollection<Person> ItemsSource { get; set; }

		public DataTemplateTestViewModel( IPageDialogService pageDlg )
		{
			ItemsSource = new ObservableCollection<Person>(new List<Person>
														   {
															   new()
															   {
																   Name = "ABC",
																   Days = "1,2,3,4"
															   },
															   new()
															   {
																   Name = "DEF",
																   Days = "5,6,7,8"
															   },
														   });

			DoCommand.Subscribe(async _ => { await pageDlg.DisplayAlertAsync("", "Command", "OK"); });

			AddCommand.Subscribe(_ =>
								 {
									 ItemsSource.Add(new Person
													 {
														 Name = "Add",
														 Days = "9,9,9,9"
													 });
								 });

			DelCommand.Subscribe(_ => { ItemsSource.Remove(ItemsSource.Last()); });

			RepCommand.Subscribe(_ =>
								 {
									 ItemsSource[0] = new Person
													  {
														  Name = "Rep",
														  Days = "1,1,1,1"
													  };
								 });

			ClrCommand.Subscribe(_ => { ItemsSource.Clear(); });

			BtmCommand.Subscribe(_ => { ScrollToBottom.Value = true; });

			TopCommand.Subscribe(_ => { ScrollToTop.Value = true; });
		}

		public class Person
		{
			public string Name { get; set; }
			public string Days { get; set; }
		}
	}
}
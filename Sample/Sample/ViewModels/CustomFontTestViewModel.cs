using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Xamarin.Forms;


namespace Jakar.SettingsView.Sample.Shared.ViewModels
{
	public class CustomFontTestViewModel : BindableBase
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
																	 });

		public ReactiveProperty<string> SelectedText { get; } = new("Green");

		private string[] languages =
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


		public ReactivePropertySlim<bool> IsHeaderFont { get; } = new();
		public ReactivePropertySlim<bool> IsFooterFont { get; } = new();
		public ReactivePropertySlim<bool> IsParentTitle { get; } = new();
		public ReactivePropertySlim<bool> IsParentValue { get; } = new();
		public ReactivePropertySlim<bool> IsParentDesc { get; } = new();
		public ReactivePropertySlim<bool> IsParentHint { get; } = new();
		public ReactivePropertySlim<bool> IsParentBold { get; } = new();
		public ReactivePropertySlim<bool> IsParentItalic { get; } = new();
		public ReactivePropertySlim<bool> IsTitle { get; } = new();
		public ReactivePropertySlim<bool> IsValue { get; } = new();
		public ReactivePropertySlim<bool> IsDesc { get; } = new();
		public ReactivePropertySlim<bool> IsHint { get; } = new();
		public ReactivePropertySlim<bool> IsBold { get; } = new();
		public ReactivePropertySlim<bool> IsItalic { get; } = new();

		public ReactivePropertySlim<string> HeaderFont { get; } = new();
		public ReactivePropertySlim<string> FooterFont { get; } = new();
		public ReactivePropertySlim<string> ParentTitle { get; } = new();
		public ReactivePropertySlim<string> ParentValue { get; } = new();
		public ReactivePropertySlim<string> ParentDesc { get; } = new();
		public ReactivePropertySlim<string> ParentHint { get; } = new();
		public ReactivePropertySlim<string> Title { get; } = new();
		public ReactivePropertySlim<string> Value { get; } = new();
		public ReactivePropertySlim<string> Desc { get; } = new();
		public ReactivePropertySlim<string> Hint { get; } = new();
		public ReactivePropertySlim<FontAttributes> ParentAttr { get; } = new(FontAttributes.None);
		public ReactivePropertySlim<FontAttributes> ChildAttr { get; } = new(FontAttributes.None);


		public CustomFontTestViewModel( INavigationService navigationService )
		{
			InputText = new ReactiveProperty<string>().SetValidateAttribute(() => InputText);

			InputError = InputText.ObserveErrorChanged.Select(x => x?.Cast<string>()?.FirstOrDefault()).ToReadOnlyReactiveProperty();

			SectionToggleCommand = InputText.ObserveHasErrors.Select(x => !x).ToAsyncReactiveCommand();
			SectionToggleCommand.Subscribe(async _ =>
										   {
											   InputSectionVisible.Value = !InputSectionVisible.Value;
											   await Task.Delay(250);
										   });

			ToProfileCommand.Subscribe(async _ => { await navigationService.NavigateAsync("DummyPage"); });

			foreach ( string item in languages )
			{
				ItemsSource.Add(new Person()
								{
									Name = item,
									Age = 1
								});
			}

			SelectedItems.Add(ItemsSource[1]);
			SelectedItems.Add(ItemsSource[2]);
			SelectedItems.Add(ItemsSource[3]);

			IsHeaderFont.Subscribe(o => { HeaderFont.Value = o ? "Anzu" : null; });

			IsFooterFont.Subscribe(o => { FooterFont.Value = o ? "Anzu" : null; });

			IsParentTitle.Subscribe(o => { ParentTitle.Value = o ? "Anzu" : null; });

			IsParentValue.Subscribe(o => { ParentValue.Value = o ? "Anzu" : null; });

			IsParentDesc.Subscribe(o => { ParentDesc.Value = o ? "Anzu" : null; });

			IsParentHint.Subscribe(o => { ParentHint.Value = o ? "Anzu" : null; });

			IsTitle.Subscribe(o => { Title.Value = o ? "" : null; });
			IsValue.Subscribe(o => { Value.Value = o ? "" : null; });
			IsDesc.Subscribe(o => { Desc.Value = o ? "" : null; });
			IsHint.Subscribe(o => { Hint.Value = o ? "" : null; });

			IsParentBold.Merge(IsParentItalic)
						.Subscribe(o =>
								   {
									   if ( IsParentBold.Value && IsParentItalic.Value ) { ParentAttr.Value = FontAttributes.Bold | FontAttributes.Italic; }
									   else if ( IsParentBold.Value ) { ParentAttr.Value = FontAttributes.Bold; }
									   else if ( IsParentItalic.Value ) { ParentAttr.Value = FontAttributes.Italic; }
									   else { ParentAttr.Value = FontAttributes.None; }
								   });

			IsBold.Merge(IsItalic)
				  .Subscribe(o =>
							 {
								 if ( IsBold.Value && IsItalic.Value ) { ChildAttr.Value = FontAttributes.Bold | FontAttributes.Italic; }
								 else if ( IsBold.Value ) { ChildAttr.Value = FontAttributes.Bold; }
								 else if ( IsItalic.Value ) { ChildAttr.Value = FontAttributes.Italic; }
								 else { ChildAttr.Value = FontAttributes.None; }
							 });
		}

		public class Person
		{
			public string Name { get; set; }
			public int Age { get; set; }
		}
	}
}
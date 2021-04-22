using System;
using System.Diagnostics;
using Reactive.Bindings;


namespace Jakar.SettingsView.Sample.Shared.ViewModels
{
	public class CollectionChangedTestViewModel : ViewModelBase
	{
		public ReactiveCommand<string> SectionCommand { get; set; } = new();
		public ReactiveProperty<string> HeaderText { get; } = new();
		public ReactiveProperty<string> FooterText { get; } = new();

		public CollectionChangedTestViewModel()
		{
			HeaderText.Value = "Section1";
			FooterText.Value = "FooterText1";

			var headers = new string[]
						  {
							  "Hoge",
							  "Fuga",
							  "Piyo"
						  };
			var footers = new string[]
						  {
							  "Fumufumu",
							  "Houhou",
							  "Naninai"
						  };

			var idxH = 0;
			var idxF = 0;
			SectionCommand.Subscribe(p =>
									 {
										 if ( p == "Header" )
										 {
											 HeaderText.Value = headers[idxH++];
											 if ( idxH >= 3 ) { idxH = 0; }
										 }
										 else
										 {
											 FooterText.Value = footers[idxF++];
											 if ( idxF >= 3 ) { idxF = 0; }
										 }
									 });

			CellIconSize.Subscribe(x => { Debug.WriteLine($"CellIconSizeChange {x.Width} {x.Height}"); });
		}
	}
}
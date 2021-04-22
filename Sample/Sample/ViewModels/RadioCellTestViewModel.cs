using System.Collections.Generic;
using Reactive.Bindings;
using Xamarin.Forms;


namespace Jakar.SettingsView.Sample.Shared.ViewModels
{
	public class RadioCellTestViewModel : ViewModelBase
	{
		public ReactiveProperty<Color> OwnAccentColor { get; } = new();
		public ReactiveProperty<RadioItem> Selected { get; set; } = new();
		public ReactiveProperty<RadioItem> GlobalSelected { get; set; }
		public List<RadioItem> RadioItems { get; } = new();
		public ReactivePropertySlim<bool> ToggleGlobal { get; } = new();


		public RadioCellTestViewModel()
		{
			RadioItems.AddRange(new List<RadioItem>
								{
									new()
									{
										Name = "TypeA",
										Value = 1
									},
									new()
									{
										Name = "TypeB",
										Value = 2
									},
									new()
									{
										Name = "TypeC",
										Value = 3
									},
									new()
									{
										Name = "TypeD",
										Value = 4
									},
									new()
									{
										Name = "TypeE",
										Value = 5
									},
									new()
									{
										Name = "TypeF",
										Value = 6
									},
								});

			Selected.Value = RadioItems[1];
		}

		protected override void CellChanged( object obj )
		{
			base.CellChanged(obj);

			string text = ( obj as Label ).Text;

			switch ( text )
			{
				case nameof(OwnAccentColor):
					NextVal(OwnAccentColor, AccentColors);
					break;
				case nameof(ToggleGlobal):
					if ( !ToggleGlobal.Value )
					{
						Selected = null;
						GlobalSelected = new ReactiveProperty<RadioItem>(RadioItems[1]);
					}
					else
					{
						GlobalSelected = null;
						Selected = new ReactiveProperty<RadioItem>(RadioItems[1]);
					}

					ToggleGlobal.Value = !ToggleGlobal.Value;
					RaisePropertyChanged(nameof(Selected));
					RaisePropertyChanged(nameof(GlobalSelected));
					break;
				case "ChangeValue":
					if ( !ToggleGlobal.Value ) { NextVal(Selected, RadioItems.ToArray()); }
					else { NextVal(GlobalSelected, RadioItems.ToArray()); }

					break;
			}
		}

		public class RadioItem
		{
			public string Name { get; set; }
			public int Value { get; set; }
		}
	}
}
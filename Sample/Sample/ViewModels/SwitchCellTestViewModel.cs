using System;
using Reactive.Bindings;
using Xamarin.Forms;
using System.Diagnostics;

namespace Sample.ViewModels
{
	public class SwitchCellTestViewModel : ViewModelBase
	{
		public ReactiveProperty<Color> OwnAccentColor { get; } = new ReactiveProperty<Color>();
		public ReactiveProperty<bool> Checked { get; } = new ReactiveProperty<bool>();
		public ReactiveProperty<bool> IsVisible { get; } = new ReactiveProperty<bool>(true);

		// ReSharper disable once IdentifierTypo
		private static readonly bool[] Bools =
		{
			false,
			true
		};

		public SwitchCellTestViewModel()
		{
			BackgroundColor.Value = Color.White;
			OwnAccentColor.Value = AccentColor;
			Checked.Value = false;
			Checked.Value = false;
		}

		protected override void CellChanged( object obj )
		{
			base.CellChanged(obj);

			string text = ( obj as Label )?.Text;

			switch ( text )
			{
				case nameof(OwnAccentColor):
					NextVal(OwnAccentColor, AccentColors);
					break;
				case nameof(Checked):
					NextVal(Checked, Bools);
					break;
				
				case nameof(IsVisible):
					NextVal(IsVisible, Bools);
					break;
			}
		}
	}
}
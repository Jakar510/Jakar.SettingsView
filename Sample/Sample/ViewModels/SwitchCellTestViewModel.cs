using Reactive.Bindings;
using Xamarin.Forms;


namespace Jakar.SettingsView.Sample.Shared.ViewModels
{
	public class SwitchCellTestViewModel : ViewModelBase
	{
		public ReactiveProperty<Color> OwnAccentColor { get; } = new();
		public ReactiveProperty<bool> Checked { get; } = new();
		public ReactiveProperty<bool> IsVisible { get; } = new(true);

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
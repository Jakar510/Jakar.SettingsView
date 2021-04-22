using System;
using Prism.Navigation;
using Prism.Services;
using Reactive.Bindings;
using Xamarin.Forms;


namespace Jakar.SettingsView.Sample.Shared.ViewModels
{
	public class EntryCellTestViewModel : ViewModelBase, IDestructible
	{
		public ReactiveProperty<Color> OwnAccentColor { get; } = new();
		public ReactiveProperty<int> MaxLength { get; } = new();
		public ReactiveProperty<Keyboard> KeyboardType { get; } = new();
		public ReactiveProperty<string> Placeholder { get; } = new();
		public ReactiveProperty<string> InputText { get; } = new();
		public ReactiveProperty<TextAlignment> TextAlignment { get; } = new();
		public ReactiveProperty<bool> IsPassword { get; } = new();

		public ReactiveCommand CompletedCommand { get; } = new();

		private static int[] MaxLengths =
		{
			-1,
			10,
			20,
			0
		};

		private static string[] InputTexts =
		{
			"",
			"TextText10",
			"LongTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextEnd",
			"TextText10TextText20"
		};

		private static Keyboard[] Keyboards =
		{
			Keyboard.Numeric,
			Keyboard.Email,
			Keyboard.Default,
			Keyboard.Plain,
			Keyboard.Telephone,
			Keyboard.Text,
			Keyboard.Url,
			Keyboard.Chat
		};

		private static string[] Placeholders =
		{
			"",
			"Placeholder",
			"LongPlaceholderTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextEnd"
		};

		private static TextAlignment[] TextAlignments =
		{
			Xamarin.Forms.TextAlignment.Start,
			Xamarin.Forms.TextAlignment.Center,
			Xamarin.Forms.TextAlignment.End
		};

		private static bool[] IsPasswords =
		{
			false,
			true
		};

		public EntryCellTestViewModel( IPageDialogService pageDialog )
		{
			OwnAccentColor.Value = AccentColors[0];
			MaxLength.Value = MaxLengths[0];
			KeyboardType.Value = Keyboards[0];
			Placeholder.Value = Placeholders[0];
			InputText.Value = InputTexts[0];
			TextAlignment.Value = TextAlignments[2];
			//ValueTextFontSize.Value = 32;

			CompletedCommand.Subscribe(_ => { pageDialog.DisplayAlertAsync("", "CompletedCommand", "OK"); });
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
				case nameof(MaxLength):
					NextVal(MaxLength, MaxLengths);
					break;
				case nameof(InputText):
					NextVal(InputText, InputTexts);
					break;
				case nameof(KeyboardType):
					NextVal(KeyboardType, Keyboards);
					break;
				case nameof(Placeholder):
					NextVal(Placeholder, Placeholders);
					break;
				case nameof(TextAlignment):
					NextVal(TextAlignment, TextAlignments);
					break;
				case nameof(IsPassword):
					NextVal(IsPassword, IsPasswords);
					break;
			}
		}

		public void Destroy() { InputText.Dispose(); }
	}
}
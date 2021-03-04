using System;
using Reactive.Bindings;
using Xamarin.Forms;
using Prism.Services;

namespace Sample.ViewModels
{
	public class ButtonCellTestViewModel : ViewModelBase
	{
		public ReactiveProperty<TextAlignment> TitleAlignment { get; } = new();
		public ReactiveProperty<object> CommandParameter { get; } = new();
		public ReactiveProperty<bool> CanExecute { get; } = new();

		public ReactiveProperty<ReactiveCommand> Command { get; set; } = new();

		private static ReactiveCommand[] Commands =
		{
			null,
			null
		};

		private static object[] Parameters =
		{
			null,
			"Def",
			"Xzy"
		};

		private static bool[] CanExecutes =
		{
			true,
			false
		};

		private static TextAlignment[] TitleAlignments =
		{
			TextAlignment.Start,
			TextAlignment.Center,
			TextAlignment.End
		};

		public ButtonCellTestViewModel( IPageDialogService pageDialog )
		{
			CanExecute.Value = CanExecutes[0];

			Commands[0] = CanExecute.ToReactiveCommand();
			Commands[1] = CanExecute.ToReactiveCommand();

			CommandParameter.Value = Parameters[0];
			Command.Value = Commands[0];

			Commands[0].Subscribe(async p => { await pageDialog.DisplayAlertAsync("Command1", p?.ToString(), "OK"); });

			Commands[1].Subscribe(async p => { await pageDialog.DisplayAlertAsync("Command2", p?.ToString(), "OK"); });
		}

		protected override void CellChanged( object obj )
		{
			base.CellChanged(obj);

			string text = ( obj as Label ).Text;

			switch ( text )
			{
				case nameof(CanExecute):
					NextVal(CanExecute, CanExecutes);
					break;
				case nameof(Command):
					NextVal(Command, Commands);
					break;
				case nameof(CommandParameter):
					NextVal(CommandParameter, Parameters);
					break;
				case nameof(TitleAlignment):
					NextVal(TitleAlignment, TitleAlignments);
					break;
			}
		}
	}
}
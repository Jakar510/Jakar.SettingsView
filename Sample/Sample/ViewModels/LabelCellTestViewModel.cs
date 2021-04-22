using System;
using System.Collections;
using System.Collections.Generic;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Config;
using Prism.Navigation;
using Prism.Services;
using Reactive.Bindings;
using Xamarin.Forms;


namespace Jakar.SettingsView.Sample.Shared.ViewModels
{
	/// <summary>
	/// 各Cellの各項目の
	/// テキスト変更
	/// 色・フォントサイズ・背景色 変更
	/// アイコンソース・サイズ・角丸 変更
	/// それからそれらの親項目を変更してみて子が優先されるかを確認する
	/// 
	/// </summary>
	public class LabelCellTestViewModel : ViewModelBase
	{
		public ReactiveProperty<TimeSpan> Time { get; } = new();
		public ReactiveProperty<string> TimeFormat { get; } = new();
		public ReactiveProperty<string> PopupTitle { get; } = new();
		public ReactiveProperty<int> Number { get; } = new();
		public ReactiveProperty<int> MaxNum { get; } = new();
		public ReactiveProperty<int> MinNum { get; } = new();
		public ReactiveProperty<DateTime> Date { get; } = new();
		public ReactiveProperty<string> DateFormat { get; } = new();
		public ReactiveProperty<DateTime> MaxDate { get; } = new();
		public ReactiveProperty<DateTime> MinDate { get; } = new();
		public ReactiveProperty<string> TodayText { get; } = new();
		public ReactiveProperty<object> CommandParameter { get; } = new();
		public ReactiveProperty<bool> CanExecute { get; } = new();
		public ReactiveProperty<bool> KeepSelected { get; } = new();
		public ReactiveProperty<IList> TextItems { get; } = new();
		public ReactiveProperty<object> TextSelectedItem { get; } = new();

		public ReactiveCommand<int> NumberSelectedCommand { get; set; } = new();
		public ReactiveCommand TextSelectedCommand { get; set; } = new();
		public ReactiveProperty<ReactiveCommand> Command { get; set; } = new();

		private enum Fruit
		{
			Apple,
			Grape,
			Orange
		}

		private static int[] Numbers =
		{
			0,
			5,
			10,
			15
		};

		private static int[] MaxNumbers =
		{
			0,
			10,
			15,
			1
		};

		private static int[] MinNumbers =
		{
			0,
			1,
			15,
			10
		};

		private static string[] PopupTitles =
		{
			"Hoge",
			"LongTitleFugaFugaFugaFuga",
			""
		};

		private static TimeSpan[] Times =
		{
			new(0, 0, 0),
			new(12, 30, 0),
			new(23, 20, 15),
			new(47, 55, 0)
		};

		private static string[] TimeFormats =
		{
			"t",
			"hh:mm",
			"H:m"
		};

		private static DateTime[] Dates =
		{
			new(2017, 1, 1),
			new(2015, 1, 1),
			new(2017, 6, 10)
		};

		private static DateTime[] MinDates =
		{
			new(2016, 1, 1),
			new(2017, 4, 1),
			new(2017, 10, 10),
			new(2017, 12, 15)
		};

		private static DateTime[] MaxDates =
		{
			new(2025, 12, 31),
			new(2017, 5, 15),
			new(2017, 10, 10),
			new(2017, 6, 15)
		};

		private static string[] DateFormats =
		{
			"d",
			"yyyy/M/d (ddd)",
			"ddd MMM d yyyy"
		};

		private static string[] TodayTexts =
		{
			"Today",
			"今日",
			""
		};

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

		private static IList[] TextLists =
		{
			new List<string>
			{
				"A",
				"B",
				"C"
			},
			new List<int>
			{
				1,
				2,
				3
			},
			new List<Fruit>
			{
				Fruit.Apple,
				Fruit.Grape,
				Fruit.Orange
			}
		};

		private static object[] TextSelectedItems =
		{
			"B",
			2,
			Fruit.Orange
		};

		public LabelCellTestViewModel( INavigationService navigationService, IPageDialogService pageDialog )
		{
			BackgroundColor.Value = Color.White;
			PopupTitle.Value = "Hoge";

			TimeFormat.Value = "t";
			Time.Value = new TimeSpan(12, 0, 0);

			DateFormat.Value = DateFormats[0];
			TodayText.Value = TodayTexts[0];
			Date.Value = Dates[0];
			MaxDate.Value = MaxDates[0];
			MinDate.Value = MinDates[0];

			CanExecute.Value = CanExecutes[0];

			Commands[0] = CanExecute.ToReactiveCommand();
			Commands[1] = CanExecute.ToReactiveCommand();

			CommandParameter.Value = Parameters[0];
			Command.Value = Commands[0];

			Commands[0]
				.Subscribe(async p =>
						   {
							   await pageDialog.DisplayAlertAsync("Command1", p?.ToString(), "OK");
							   await navigationService.NavigateAsync("ContentPage");
						   }
						  );

			Commands[1].Subscribe(async p => { await pageDialog.DisplayAlertAsync("Command2", p?.ToString(), "OK"); });
			NumberSelectedCommand.Subscribe(async p => { await pageDialog.DisplayAlertAsync("", p.ToString(), "OK"); });
			TextSelectedCommand.Subscribe(async p => { await pageDialog.DisplayAlertAsync("", p?.ToString(), "OK"); });
		}

		protected override void CellChanged( object obj )
		{
			base.CellChanged(obj);

			string text = ( obj as Label )?.Text;

			switch ( text )
			{
				case nameof(NumberPickerCell.Number):
					NextVal(Number, Numbers);
					break;
				case "MaxMinChange":
					NextVal(MaxNum, MaxNumbers);
					NextVal(MinNum, MinNumbers);

					break;
				case nameof(PopupConfig.Title):
					NextVal(PopupTitle, PopupTitles);
					break;
				case nameof(Time):
					NextVal(Time, Times);
					break;
				case nameof(TimeFormat):
					NextVal(TimeFormat, TimeFormats);
					break;
				case nameof(Date):
					NextVal(Date, Dates);
					break;
				case nameof(DateFormat):
					NextVal(DateFormat, DateFormats);
					break;
				case "MinMaxDateChange":
					NextVal(MinDate, MinDates);
					NextVal(MaxDate, MaxDates);
					break;
				case nameof(TodayText):
					NextVal(TodayText, TodayTexts);
					break;
				case nameof(CanExecute):
					NextVal(CanExecute, CanExecutes);
					break;
				case nameof(Command):
					NextVal(Command, Commands);
					break;
				case nameof(CommandParameter):
					NextVal(CommandParameter, Parameters);
					break;
				case nameof(KeepSelected):
					NextVal(KeepSelected, CanExecutes);
					break;
				case nameof(TextItems):
					NextVal(TextItems, TextLists);
					break;
				case nameof(TextSelectedItem):
					TextSelectedItem.Value = TextItems.Value switch
											 {
												 List<string> => TextSelectedItems[0],
												 List<int> => TextSelectedItems[1],
												 _ => TextSelectedItems[2]
											 };

					break;
			}
		}
	}
}
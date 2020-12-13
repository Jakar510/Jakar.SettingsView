﻿using System;
using Reactive.Bindings;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Prism.Services;
using System.Reactive.Linq;
using Prism.Navigation;
using System.Collections;
using System.Collections.Generic;
using Jakar.SettingsView.Shared.Cells;

namespace Sample.ViewModels
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
		public ReactiveProperty<TimeSpan> Time { get; } = new ReactiveProperty<TimeSpan>();
		public ReactiveProperty<string> TimeFormat { get; } = new ReactiveProperty<string>();
		public ReactiveProperty<string> PickerTitle { get; } = new ReactiveProperty<string>();
		public ReactiveProperty<int> Number { get; } = new ReactiveProperty<int>();
		public ReactiveProperty<int> MaxNum { get; } = new ReactiveProperty<int>();
		public ReactiveProperty<int> MinNum { get; } = new ReactiveProperty<int>();
		public ReactiveProperty<DateTime> Date { get; } = new ReactiveProperty<DateTime>();
		public ReactiveProperty<string> DateFormat { get; } = new ReactiveProperty<string>();
		public ReactiveProperty<DateTime> MaxDate { get; } = new ReactiveProperty<DateTime>();
		public ReactiveProperty<DateTime> MinDate { get; } = new ReactiveProperty<DateTime>();
		public ReactiveProperty<string> TodayText { get; } = new ReactiveProperty<string>();
		public ReactiveProperty<object> CommandParameter { get; } = new ReactiveProperty<object>();
		public ReactiveProperty<bool> CanExecute { get; } = new ReactiveProperty<bool>();
		public ReactiveProperty<bool> KeepSelected { get; } = new ReactiveProperty<bool>();
		public ReactiveProperty<IList> TextItems { get; } = new ReactiveProperty<IList>();
		public ReactiveProperty<object> TextSelectedItem { get; } = new ReactiveProperty<object>();

		public ReactiveCommand<int> NumberSelectedCommand { get; set; } = new ReactiveCommand<int>();
		public ReactiveCommand TextSelectedCommand { get; set; } = new ReactiveCommand();
		public ReactiveProperty<ReactiveCommand> Command { get; set; } = new ReactiveProperty<ReactiveCommand>();

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

		private static string[] PickerTitles =
		{
			"Hoge",
			"LongTitleFugaFugaFugaFuga",
			""
		};

		private static TimeSpan[] Times =
		{
			new TimeSpan(0, 0, 0),
			new TimeSpan(12, 30, 0),
			new TimeSpan(23, 20, 15),
			new TimeSpan(47, 55, 0)
		};

		private static string[] TimeFormats =
		{
			"t",
			"hh:mm",
			"H:m"
		};

		private static DateTime[] Dates =
		{
			new DateTime(2017, 1, 1),
			new DateTime(2015, 1, 1),
			new DateTime(2017, 6, 10)
		};

		private static DateTime[] MinDates =
		{
			new DateTime(2016, 1, 1),
			new DateTime(2017, 4, 1),
			new DateTime(2017, 10, 10),
			new DateTime(2017, 12, 15)
		};

		private static DateTime[] MaxDates =
		{
			new DateTime(2025, 12, 31),
			new DateTime(2017, 5, 15),
			new DateTime(2017, 10, 10),
			new DateTime(2017, 6, 15)
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
			PickerTitle.Value = "Hoge";

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
						   });

			Commands[1].Subscribe(async p => { await pageDialog.DisplayAlertAsync("Command2", p?.ToString(), "OK"); });
			NumberSelectedCommand.Subscribe(async p => { await pageDialog.DisplayAlertAsync("", p.ToString(), "OK"); });
			TextSelectedCommand.Subscribe(async p => { await pageDialog.DisplayAlertAsync("", p?.ToString(), "OK"); });
		}

		protected override void CellChanged( object obj )
		{
			base.CellChanged(obj);

			string text = ( obj as Label ).Text;

			switch ( text )
			{
				case nameof(NumberPickerCell.Number):
					NextVal(Number, Numbers);
					break;
				case "MaxMinChange":
					NextVal(MaxNum, MaxNumbers);
					NextVal(MinNum, MinNumbers);

					break;
				case nameof(NumberPickerCell.PickerTitle):
					NextVal(PickerTitle, PickerTitles);
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
					if ( TextItems.Value is List<string> ) { TextSelectedItem.Value = TextSelectedItems[0]; }
					else if ( TextItems.Value is List<int> ) { TextSelectedItem.Value = TextSelectedItems[1]; }
					else { TextSelectedItem.Value = TextSelectedItems[2]; }

					break;
			}
		}
	}
}
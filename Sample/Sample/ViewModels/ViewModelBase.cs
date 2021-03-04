using System;
using System.Collections.Generic;
using System.Linq;
using Jakar.SettingsView;
using Jakar.SettingsView.Shared;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Cells;
using Prism.Mvvm;
using Reactive.Bindings;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Svg;
using SettingsView = Jakar.SettingsView.Shared.sv.SettingsView;

namespace Sample.ViewModels
{
	public class ViewModelBase : BindableBase
	{
		public ReactiveProperty<Color> BackgroundColor { get; } = new();
		public ReactiveProperty<Color> SeparatorColor { get; } = new();
		public ReactiveProperty<Color> SelectedColor { get; } = new();
		public ReactiveProperty<Thickness> HeaderPadding { get; } = new();
		public ReactiveProperty<Color> HeaderTextColor { get; } = new();
		public ReactiveProperty<double> HeaderFontSize { get; } = new();
		public ReactiveProperty<LayoutAlignment> HeaderTextVerticalAlign { get; } = new();
		public ReactiveProperty<Color> HeaderBackgroundColor { get; } = new();
		public ReactiveProperty<double> HeaderHeight { get; } = new();
		public ReactiveProperty<Color> FooterTextColor { get; } = new();
		public ReactiveProperty<double> FooterFontSize { get; } = new();
		public ReactiveProperty<Color> FooterBackgroundColor { get; } = new();
		public ReactiveProperty<Thickness> FooterPadding { get; } = new();
		public ReactiveProperty<int> RowHeight { get; } = new();
		public ReactiveProperty<bool> HasUnevenRows { get; } = new();
		public ReactiveProperty<Color> CellTitleColor { get; } = new();
		public ReactiveProperty<double> CellTitleFontSize { get; } = new();
		public ReactiveProperty<Color> CellValueTextColor { get; } = new();
		public ReactiveProperty<double> CellValueTextFontSize { get; } = new();
		public ReactiveProperty<Color> CellDescriptionColor { get; } = new();
		public ReactiveProperty<double> CellDescriptionFontSize { get; } = new();
		public ReactiveProperty<Color> CellBackgroundColor { get; } = new();
		public ReactiveProperty<Size> CellIconSize { get; } = new();
		public ReactiveProperty<double> CellIconRadius { get; } = new();
		public ReactiveProperty<Color> CellAccentColor { get; } = new();
		public ReactiveProperty<Color> CellHintTextColor { get; } = new();
		public ReactiveProperty<double> CellHintFontSize { get; } = new();
		public ReactiveProperty<bool> UseDescriptionAsValue { get; } = new();
		public ReactiveProperty<bool> ShowSectionTopBottomBorder { get; } = new();

		public ReactiveCommand PropertyChangeCommand { get; } = new();

		public ReactiveProperty<ImageSource> IconSource { get; } = new();
		public ReactiveProperty<Size> IconSize { get; } = new();
		public ReactiveProperty<double> IconRadius { get; } = new();
		public ReactiveProperty<string> Title { get; } = new();
		public ReactiveProperty<string> Description { get; } = new();
		public ReactiveProperty<string> HintText { get; } = new();
		public ReactiveProperty<string> ValueText { get; } = new();
		public ReactiveProperty<Color> BgColor { get; } = new();
		public ReactiveProperty<Color> TitleColor { get; } = new();
		public ReactiveProperty<double> TitleFontSize { get; } = new();
		public ReactiveProperty<Color> ValueTextColor { get; } = new();
		public ReactiveProperty<double> ValueTextFontSize { get; } = new();
		public ReactiveProperty<Color> DescriptionColor { get; } = new();
		public ReactiveProperty<double> DescriptionFontSize { get; } = new();
		public ReactiveProperty<Color> HintTextColor { get; } = new();
		public ReactiveProperty<double> HintFontSize { get; } = new();
		public ReactiveProperty<bool> IsEnabled { get; } = new(true);

		public ReactiveCommand CellChangeCommand { get; } = new();


		public static Color OuterColor = Color.DeepPink;
		public static Color AccentColor = Color.FromHex("#1133CC");
		public static Color BackColor = Color.FromHex("#ADB8CC");
		public static Color CellBackColor = Color.White;
		public static Color DeepTextColor = Color.FromHex("#3367CC");
		public static Color PaleTextColor = Color.FromHex("#A3B1CC");

		public static Color OuterColor2 = Color.Blue;
		public static Color AccentColor2 = Color.FromHex("#FF407F");
		public static Color BackColor2 = Color.FromHex("#CCB8BE");
		public static Color CellBackColor2 = Color.FromHex("#B8CCAD");
		public static Color DeepTextColor2 = Color.FromHex("#EE508F");
		public static Color PaleTextColor2 = Color.FromHex("#CCA3B0");

		public static Color[] OuterColors =
		{
			OuterColor,
			OuterColor2,
			Color.Transparent,
			Color.White
		};

		public static Color[] AccentColors =
		{
			AccentColor,
			AccentColor2,
			Color.Transparent
		};

		public static Color[] BackColors =
		{
			BackColor,
			BackColor2,
			Color.Transparent
		};

		public static Color[] CellBackColors =
		{
			CellBackColor,
			CellBackColor2,
			Color.Transparent
		};

		public static Color[] DeepTextColors =
		{
			DeepTextColor,
			DeepTextColor2,
			Color.Transparent
		};

		public static Color[] PaleTextColors =
		{
			PaleTextColor,
			PaleTextColor2,
			Color.Transparent
		};

		public static string[] TitleTexts =
		{
			"Title",
			"LongTitleTextTextTextTextTextTextTextTextTextTextTextTextTextTextEnd",
			""
		};

		public static string[] ValueTexts =
		{
			"Value",
			"LongValueTextTextTextTextTextTextTextTextTextTextTextTextTextTextEnd",
			""
		};

		public static string[] DescriptionTexts =
		{
			"Description",
			"LongLongDescription\nTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextEnd",
			""
		};

		public static string[] HintTexts =
		{
			"hint",
			"LongHintTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextEnd",
			""
		};

		public static ImageSource[] IconSources =
		{
			"icon.png",
			SvgImageSource.FromSvg("umbrella.svg"),
			null
		};

		public ViewModelBase()
		{
			BackgroundColor.Value = OuterColors[3];
			SeparatorColor.Value = AccentColor;
			SelectedColor.Value = BackColor;
			HeaderTextColor.Value = DeepTextColor;
			HeaderBackgroundColor.Value = Color.Transparent;
			FooterTextColor.Value = DeepTextColor;
			FooterBackgroundColor.Value = Color.Transparent;
			CellTitleColor.Value = DeepTextColor;
			CellValueTextColor.Value = PaleTextColor;
			CellDescriptionColor.Value = PaleTextColor;
			CellBackgroundColor.Value = CellBackColor;
			CellAccentColor.Value = AccentColor;
			CellHintTextColor.Value = AccentColor;

			HeaderFontSize.Value = 10;
			HeaderHeight.Value = 16;
			FooterFontSize.Value = 10;
			CellTitleFontSize.Value = 14;
			CellValueTextFontSize.Value = 12;
			CellDescriptionFontSize.Value = 10;
			CellIconSize.Value = new Size(30, 30);
			CellIconRadius.Value = 4;
			CellHintFontSize.Value = 9;

			HasUnevenRows.Value = false;
			UseDescriptionAsValue.Value = false;
			ShowSectionTopBottomBorder.Value = true;


			IconSource.Value = null;
			IconSize.Value = new Size();
			IconRadius.Value = 0;
			Title.Value = "Title";
			Description.Value = "Description";
			HintText.Value = "hint";
			ValueText.Value = "value";
			BgColor.Value = Color.White;
			TitleColor.Value = DeepTextColor;
			TitleFontSize.Value = 12;
			ValueTextColor.Value = PaleTextColor;
			ValueTextFontSize.Value = 12;
			DescriptionColor.Value = PaleTextColor;
			DescriptionFontSize.Value = 10;
			HintTextColor.Value = Color.Red;
			HintFontSize.Value = 9;


			PropertyChangeCommand.Subscribe(ParentChanged);
			CellChangeCommand.Subscribe(CellChanged);
		}

		protected virtual void ParentChanged( object obj )
		{
			string text = ( obj as Label ).Text;
			switch ( text )
			{
				case nameof(SettingsView.BackgroundColor):
					NextColor(BackgroundColor, OuterColors);
					break;
				case nameof(SettingsView.SeparatorColor):
					NextColor(SeparatorColor, AccentColors);
					break;
				case nameof(SettingsView.SelectedColor):
					NextColor(SelectedColor, BackColors);
					break;
				case nameof(SettingsView.HeaderTextColor):
					NextColor(HeaderTextColor, DeepTextColors);
					break;
				case nameof(SettingsView.HeaderBackgroundColor):
					NextColor(HeaderBackgroundColor, BackColors);
					break;
				case nameof(SettingsView.FooterTextColor):
					NextColor(FooterTextColor, DeepTextColors);
					break;
				case nameof(SettingsView.FooterBackgroundColor):
					NextColor(FooterBackgroundColor, BackColors);
					break;
				case nameof(SettingsView.CellTitleColor):
					NextColor(CellTitleColor, DeepTextColors);
					break;
				case nameof(SettingsView.CellValueTextColor):
					NextColor(CellValueTextColor, PaleTextColors);
					break;
				case nameof(SettingsView.CellDescriptionColor):
					NextColor(CellDescriptionColor, PaleTextColors);
					break;
				case nameof(SettingsView.CellBackgroundColor):
					NextColor(CellBackgroundColor, CellBackColors);
					break;
				case nameof(SettingsView.CellAccentColor):
					NextColor(CellAccentColor, AccentColors);
					break;
				case nameof(SettingsView.CellHintTextColor):
					NextColor(CellHintTextColor, AccentColors);
					break;
				case nameof(SettingsView.ShowSectionTopBottomBorder):
					ToggleBool(ShowSectionTopBottomBorder);
					break;
				case nameof(SettingsView.UseDescriptionAsValue):
					ToggleBool(UseDescriptionAsValue);
					break;
				case nameof(SettingsView.HasUnevenRows):
					ToggleBool(HasUnevenRows);
					break;
				case nameof(SettingsView.HeaderPadding):
					ChangeThickness(HeaderPadding);
					break;
				case nameof(SettingsView.FooterPadding):
					ChangeThickness(FooterPadding);
					break;
				case nameof(SettingsView.HeaderFontSize):
					ChangeFontSize(HeaderFontSize);
					break;
				case nameof(SettingsView.FooterFontSize):
					ChangeFontSize(FooterFontSize);
					break;
				case nameof(SettingsView.CellTitleFontSize):
					ChangeFontSize(CellTitleFontSize);
					break;
				case nameof(SettingsView.CellValueTextFontSize):
					ChangeFontSize(CellValueTextFontSize);
					break;
				case nameof(SettingsView.CellDescriptionFontSize):
					ChangeFontSize(CellDescriptionFontSize);
					break;
				case nameof(SettingsView.CellHintFontSize):
					ChangeFontSize(CellHintFontSize);
					break;
				case nameof(SettingsView.HeaderHeight):
					ChangeHeight(HeaderHeight);
					break;
				case nameof(SettingsView.HeaderTextVerticalAlign):
					ChangeAlign(HeaderTextVerticalAlign);
					break;
				case nameof(SettingsView.CellIconSize):
					ChangeSize(CellIconSize);
					break;
				case nameof(SettingsView.CellIconRadius):
					ChangeHeight(CellIconRadius);
					break;
				case nameof(SettingsView.RowHeight):
					ChangeRowHeight(RowHeight);
					break;
				case "CacheClear":
					SettingsView.ClearCache();
					break;
				default:
					break;
			}
		}

		protected virtual void CellChanged( object obj )
		{
			string text = ( obj as Label )?.Text;
			switch ( text )
			{
				case nameof(TitleCellBase.Title):
					NextText(Title, TitleTexts);
					break;
				case nameof(LabelCell.ValueText):
					NextText(ValueText, ValueTexts);
					break;
				case nameof(DescriptionCellBase.Description):
					NextText(Description, DescriptionTexts);
					break;
				case nameof(HintTextCellBase.Hint):
					NextText(HintText, HintTexts);
					break;
				case nameof(TitleCellBase.TitleColor):
					NextColor(TitleColor, DeepTextColors);
					break;
				case nameof(LabelCell.ValueTextColor):
					NextColor(ValueTextColor, PaleTextColors);
					break;
				case nameof(DescriptionCellBase.DescriptionColor):
					NextColor(DescriptionColor, PaleTextColors);
					break;
				case nameof(CellBase.BackgroundColor):
					NextColor(BgColor, CellBackColors);
					break;
				case nameof(HintTextCellBase.HintColor):
					NextColor(HintTextColor, AccentColors);
					break;
				case nameof(TitleCellBase.TitleFontSize):
					ChangeFontSize(TitleFontSize);
					break;
				case nameof(LabelCell.ValueTextFontSize):
					ChangeFontSize(ValueTextFontSize);
					break;
				case nameof(DescriptionCellBase.DescriptionFontSize):
					ChangeFontSize(DescriptionFontSize);
					break;
				case nameof(DescriptionCellBase.IconSource):
					ChangeIconSource(IconSource);
					break;
				case nameof(DescriptionCellBase.IconRadius):
					ChangeHeight(IconRadius);
					break;
				case nameof(DescriptionCellBase.IconSize):
					ChangeSize(IconSize);
					break;
				case nameof(HintTextCellBase.HintFontSize):
					ChangeFontSize(HintFontSize);
					break;
				case nameof(CellBase.IsEnabled):
					ToggleBool(IsEnabled);
					break;
			}
		}

		protected void NextVal<T>( ReactiveProperty<T> current, T[] array )
		{
			int idx = array.IndexOf(current.Value);
			if ( idx == array.Length - 1 )
			{
				current.Value = array[0];
				return;
			}

			current.Value = array[idx + 1];
		}

		private void ChangeIconSource( ReactiveProperty<ImageSource> current )
		{
			int idx = IconSources.IndexOf(current.Value);
			if ( idx == IconSources.Length - 1 )
			{
				current.Value = IconSources[0];
				return;
			}

			current.Value = IconSources[idx + 1];
		}

		private void NextText( ReactiveProperty<string> current, string[] texts )
		{
			int idx = texts.IndexOf(current.Value);
			if ( idx == texts.Length - 1 )
			{
				current.Value = texts[0];
				return;
			}

			current.Value = texts[idx + 1];
		}

		private void NextColor( ReactiveProperty<Color> current, Color[] colors )
		{
			int idx = colors.IndexOf(current.Value);
			if ( idx == colors.Length - 1 )
			{
				current.Value = colors[0];
				return;
			}

			current.Value = colors[idx + 1];
		}

		private void ToggleBool( ReactiveProperty<bool> current ) { current.Value = !current.Value; }

		private void ChangeFontSize( ReactiveProperty<double> current )
		{
			if ( current.Value > 30 ) { current.Value = 0; }
			else { current.Value += 1.0d; }
		}

		private void ChangeThickness( ReactiveProperty<Thickness> current )
		{
			double t = current.Value.Top;
			double b = current.Value.Bottom;
			double l = current.Value.Left;
			double r = current.Value.Right;

			var list = new List<double>
					   {
						   l,
						   t,
						   r,
						   b
					   };

			if ( t + b + l + r < 80d )
			{
				int idx = list.IndexOf(list.Min());
				list[idx] += 5d;

				current.Value = new Thickness(list[0], list[1], list[2], list[3]);
			}
			else { current.Value = new Thickness(0, 0, 0, 0); }
		}

		private void ChangeHeight( ReactiveProperty<double> current )
		{
			if ( current.Value > 80 ) { current.Value = 0; }
			else { current.Value += 2; }
		}

		private void ChangeRowHeight( ReactiveProperty<int> current )
		{
			if ( current.Value > 150 ) { current.Value = 0; }
			else { current.Value += 5; }
		}

		private void ChangeAlign( ReactiveProperty<LayoutAlignment> current )
		{
			if ( current.Value == LayoutAlignment.Start )
			{
				current.Value = LayoutAlignment.Center;
				return;
			}

			if ( current.Value == LayoutAlignment.Center )
			{
				current.Value = LayoutAlignment.End;
				return;
			}

			if ( current.Value == LayoutAlignment.End )
			{
				current.Value = LayoutAlignment.Fill;
				return;
			}

			current.Value = LayoutAlignment.Start;
		}

		private void ChangeSize( ReactiveProperty<Size> current )
		{
			Size size = current.Value;

			if ( size.Width + size.Height > 200 )
			{
				current.Value = new Size(0, 0);
				return;
			}

			if ( size.Width < 100 )
			{
				size.Width += 10;
				current.Value = new Size(size.Width, 10);
				return;
			}

			size.Height += 10;

			current.Value = new Size(size.Width, size.Height);
		}
	}
}
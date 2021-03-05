// unset

using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Config
{
	public static class SVConstants
	{
		public static class Defaults
		{
			public const double FONT_SIZE = -1;
			public const double MIN_ROW_HEIGHT = 44;
			public static readonly Color COLOR = Color.Default;
			public static readonly Color ACCENT = Color.Accent;
		}


	#region CELLs

		public static class Cell
		{
			public static readonly Color BACKGROUND_COLOR = Color.WhiteSmoke;

			public static class Title
			{
				public const double FONT_SIZE = 12;
				public static readonly Color TEXT_COLOR = Color.Black;
			}

			public static class Description
			{
				public const double FONT_SIZE = 10;
				public static readonly Color TEXT_COLOR = Color.LightSlateGray;
			}

			public static class Hint
			{
				public static readonly Color TEXT_COLOR = Color.Red;
				public const double FONT_SIZE = 10;
			}

			public static class Value
			{
				public static readonly Color TEXT_COLOR = Color.Black;
				public const double FONT_SIZE = 10;
			}

			public static class Padding
			{
				public const double X = 4;
				public const double Y = 10;
			}

			public static readonly TextAlignment TITLE_COLUMN = TextAlignment.Start;
			public static readonly TextAlignment VALUE_COLUMN = TextAlignment.End;
		}

	#endregion


	#region PROMPTs

		public static class Prompt
		{
			public static readonly Color BACKGROUND_COLOR = Color.White;

			public static class Title
			{
				public static readonly Color ITEM_COLOR = Color.Black;
				public const double FONT_SIZE = 15;
			}

			public static class Selected
			{
				public static readonly Color TEXT_COLOR = Color.Blue;
				public const double FONT_SIZE = 12;
			}

			public static class Item
			{
				public static readonly Color COLOR = Color.Black;
				public const double FONT_SIZE = 12;

				public static class Description
				{
					public static readonly Color Color = Color.Black;
					public const double FontSize = 10;
				}
			}

			public const double PADDING = 4;

			public static readonly Color SEPARATOR_COLOR = Color.LightGray;

			public const string ACCEPT_TEXT = "OK";
			public const string CANCEL_TEXT = "Cancel";
		}

	#endregion


	#region SECTIONs

		public static class Section
		{
			public static class Header
			{
				public static readonly Color TEXT_COLOR = Color.White;
				public static readonly Color BACKGROUND_COLOR = Color.SteelBlue;
				public const double FONT_SIZE = 18;
				public const double PADDING = 4;
				public const double MinRowHeight = 40;
			}

			public static class Footer
			{
				public static readonly Color TEXT_COLOR = Color.Black;
				public static readonly Color BACKGROUND_COLOR = Color.White;
				public const double FONT_SIZE = 9;
				public const double PADDING = 4;
				public const bool VISIBLE = true;
				public const double MinRowHeight = 20;
			}
		}

	#endregion
	}
}
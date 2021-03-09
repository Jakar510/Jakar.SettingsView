// unset

using Xamarin.Forms;

#nullable enable
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
			public const int CornerRadius = -1;
			public const double BorderWidth = -1;
		}


	#region CELLs

		public static class Cell
		{
			public const float DISABLED_ALPHA = 0.3f;
			public const float ENABLED_ALPHA = 1.0f;

			public static readonly Color Ripple = Color.FromRgba(200, 200, 100, 255);   //YELLOW
			public static readonly Color Selected = Color.FromRgba(180, 180, 180, 125); // GRAY
			public static readonly Color COLOR = Color.Default;
			public static readonly Color PlaceholderColor = Color.LightGray;
			public static readonly Brush Brush = Brush.Default;

			public static readonly double? FONT_SIZE = null;
			public static readonly double? ICON_SIZE = null;

			public const bool VISIBLE = true;
			public static readonly Size? IconSize = default;
			public static readonly Thickness PADDING = new(10, 4);

			public const string FONT_FAMILY = default;
			public static readonly FontAttributes FONT_ATTRIBUTES = FontAttributes.None;


			public static class ButtonCell
			{
				public static readonly Color BACKGROUND_COLOR = Color.SlateGray;
			}
		}

	#endregion


	#region SV

		public static class SV
		{
			public static readonly Color ACCENT_COLOR = Color.Accent;
			public static readonly Color OFF_COLOR = Color.FromRgba(117, 117, 117, 76);
			public static readonly Color BACKGROUND_COLOR = Color.WhiteSmoke;
			public static readonly Color SEPARATOR_COLOR = Color.FromRgb(199, 199, 204);


			public const bool VISIBLE = true;
			public static readonly Color? COLOR = null;

			public static class Icon
			{
				public const double Radius = 6;
				public static readonly Size Size = new(36, 36);
			}

			public static class Title
			{
				public static class Font
				{
					public const double Size = 13;
					public const string? Family = null;
					public static readonly FontAttributes Attributes = FontAttributes.Bold;
				}

				public static readonly Color TEXT_COLOR = Color.Black;
				public static readonly TextAlignment Alignment = TextAlignment.Start;
			}

			public static class Description
			{
				public static class Font
				{
					public const double Size = 10;
					public const string? Family = null;
					public static readonly FontAttributes Attributes = FontAttributes.Italic;
				}

				public static readonly Color TEXT_COLOR = Color.LightSlateGray;
				public static readonly TextAlignment Alignment = TextAlignment.Start;
			}

			public static class Hint
			{
				public static class Font
				{
					public const double Size = 9;
					public const string? Family = null;
					public static readonly FontAttributes Attributes = FontAttributes.None;
				}

				public static readonly Color TEXT_COLOR = Color.Red;
				public static readonly TextAlignment Alignment = TextAlignment.End;
			}

			public static class Value
			{
				public static class Font
				{
					public const double Size = 11;
					public const string? Family = null;
					public static readonly FontAttributes Attributes = FontAttributes.None;
				}

				public static readonly Color TEXT_COLOR = Color.Black;
				public static readonly TextAlignment Alignment = TextAlignment.End;
			}

			public static class Padding
			{
				public const double X = 4;
				public const double Y = 10;
			}

			// public static readonly TextAlignment TITLE_COLUMN = TextAlignment.Start;
			// public static readonly TextAlignment VALUE_COLUMN = TextAlignment.End;
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
			public const bool VISIBLE = true;

			public static class Header
			{
				public static readonly Color TEXT_COLOR = Color.White;
				public static readonly Color BACKGROUND_COLOR = Color.SteelBlue;

				public const double FONT_SIZE = 17;
				public const string FONT_FAMILY = default;
				public static readonly FontAttributes FONT_ATTRIBUTES = FontAttributes.None;

				public static readonly Thickness PADDING = new(8, 4);
				public const double MinRowHeight = 40;
			}

			public static class Footer
			{
				public static readonly Color TEXT_COLOR = Color.Black;
				public static readonly Color BACKGROUND_COLOR = Color.White;

				public const double FONT_SIZE = 10;
				public const string FONT_FAMILY = default;
				public static readonly FontAttributes FONT_ATTRIBUTES = FontAttributes.None;

				public static readonly Thickness PADDING = new(8, 4);
				public const double MinRowHeight = 20;
				public const bool Visible = true;
			}
		}

	#endregion
	}
}
// unset

namespace Jakar.SettingsView.Shared.Config;

[Xamarin.Forms.Internals.Preserve(true, false)]
public static class SvConstants
{
    public static class Defaults
    {
        public const           double FONT_SIZE      = -1;
        public const           double MIN_ROW_HEIGHT = 44;
        public static readonly Color  color          = Color.Default;
        public static readonly Color  accent         = Color.Accent;
        public const           int    CORNER_RADIUS  = -1;
        public const           double BORDER_WIDTH   = -1;
    }



#region CELLs

    public static class Cell
    {
        public const float DISABLED_ALPHA = 0.3f;
        public const float ENABLED_ALPHA  = 1.0f;

        public static readonly Color ripple           = Color.FromRgba(200, 200, 100, 255); //YELLOW
        public static readonly Color selected         = Color.FromRgba(180, 180, 180, 125); // GRAY
        public static readonly Color color            = Color.Default;
        public static readonly Color placeholderColor = Color.LightGray;
        public static readonly Brush brush            = Brush.Default;

        public static readonly double? font_Size  = null;
        public static readonly double? iconRadius = null;

        public const           bool      VISIBLE  = true;
        public static readonly Size?     iconSize = null;
        public static readonly Thickness padding  = new(10, 4);

        public const           string         FONT_FAMILY     = default;
        public static readonly FontAttributes font_Attributes = FontAttributes.None;



        public static class ButtonCell
        {
            public static readonly Color background_Color = Color.SlateGray;
        }
    }

#endregion


#region SV

    public static class Sv
    {
        public static readonly Color accent_Color     = Color.Accent;
        public static readonly Color off_Color        = Color.FromRgba(117, 117, 117, 76);
        public static readonly Color background_Color = Color.WhiteSmoke;
        public static readonly Color separator_Color  = Color.FromRgb(199, 199, 204);


        public const           bool   VISIBLE = true;
        public static readonly Color? color   = null;



        public static class Icon
        {
            public const           double RADIUS = 6;
            public static readonly Size   size   = new(36, 36);
        }



        public static class Title
        {
            public static class Font
            {
                public const           double         SIZE       = 13;
                public const           string?        FAMILY     = null;
                public static readonly FontAttributes attributes = FontAttributes.Bold;
            }



            public static readonly Color         text_Color = Color.Black;
            public static readonly TextAlignment alignment  = TextAlignment.Start;
        }



        public static class Description
        {
            public static class Font
            {
                public const           double         SIZE       = 10;
                public const           string?        FAMILY     = null;
                public static readonly FontAttributes attributes = FontAttributes.Italic;
            }



            public static readonly Color         text_Color = Color.LightSlateGray;
            public static readonly TextAlignment alignment  = TextAlignment.Start;
        }



        public static class Hint
        {
            public static class Font
            {
                public const           double         SIZE       = 9;
                public const           string?        FAMILY     = null;
                public static readonly FontAttributes attributes = FontAttributes.None;
            }



            public static readonly Color         text_Color = Color.Red;
            public static readonly TextAlignment alignment  = TextAlignment.End;
        }



        public static class Value
        {
            public static class Font
            {
                public const           double         SIZE       = 11;
                public const           string?        FAMILY     = null;
                public static readonly FontAttributes attributes = FontAttributes.None;
            }



            public static readonly Color         placeholder_Color = Color.SlateGray;
            public static readonly Color         text_Color        = Color.Black;
            public static readonly TextAlignment alignment         = TextAlignment.End;
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
        public static readonly Color background_Color = Color.White;



        public static class Title
        {
            public static readonly Color  color     = Color.Black;
            public const           double FONT_SIZE = 15;
        }



        public static class Selected
        {
            public static readonly Color  text_Color = Color.Blue;
            public const           double FONT_SIZE  = 12;
        }



        public static class Item
        {
            public static readonly Color  color     = Color.Black;
            public const           double FONT_SIZE = 12;



            public static class Description
            {
                public static readonly Color  color     = Color.SlateGray;
                public const           double FONT_SIZE = 10;
            }
        }



        public const double PADDING = 4;

        public static readonly Color separator_Color = Color.LightGray;

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
            public static readonly Color text_Color       = Color.White;
            public static readonly Color background_Color = Color.SteelBlue;

            public const           double         FONT_SIZE       = 17;
            public const           string         FONT_FAMILY     = default;
            public static readonly FontAttributes font_Attributes = FontAttributes.None;

            public static readonly Thickness padding        = new(8, 4);
            public const           double    MIN_ROW_HEIGHT = 40;
        }



        public static class Footer
        {
            public static readonly Color text_Color       = Color.Black;
            public static readonly Color background_Color = Color.White;

            public const           double         FONT_SIZE       = 10;
            public const           string         FONT_FAMILY     = default;
            public static readonly FontAttributes font_Attributes = FontAttributes.None;

            public static readonly Thickness padding        = new(8, 4);
            public const           double    MIN_ROW_HEIGHT = 20;
            public const           bool      VISIBLE        = true;
        }
    }

#endregion


#region LAYOUT

    public static class Layout
    {
        public static class ColumnFactors
        {
            public const float ICON        = 0.05f;
            public const float TITLE_STACK = 0.4f;
            public const float VALUE_STACK = 0.5f;
            public const float ACCESSORY   = 0.05f;
        }



        public static class Factor // iOS only
        {
            public const float ZERO = 0;
            public const float ONE  = 1;
        }
    }

#endregion
}
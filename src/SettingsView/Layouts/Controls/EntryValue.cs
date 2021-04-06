using Jakar.SettingsView.Shared.Interfaces;
using Xamarin.Forms;


namespace Jakar.SettingsView.Shared.Layouts.Controls
{
	public class EntryValue : Entry, IUpdateTextControl
	{
		public EntryValue() : this(1, 2) { }

		protected EntryValue( in int row, in int column ) : base()
		{
			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions   = LayoutOptions.FillAndExpand;
			Grid.SetRow(this, row);
			Grid.SetColumn(this, column);
			BackgroundColor = Color.Transparent;
		}

		public void Update( IUseConfiguration configuration )
		{
			FontSize                = configuration.FontSize;
			FontFamily              = configuration.FontFamily;
			FontAttributes          = configuration.FontAttributes;
			HorizontalTextAlignment = VerticalTextAlignment = configuration.TextAlignment;
			TextColor               = configuration.Color;
		}

		public void SetText( string? text ) => Text = text;
	}
}

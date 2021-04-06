using Jakar.SettingsView.Shared.Interfaces;
using Xamarin.Forms;


namespace Jakar.SettingsView.Shared.Layouts.Controls
{
	public class AiSwitch : Switch, IUpdateAccessoryControl
	{
		public bool Checked
		{
			get => IsToggled;
			set => IsToggled = value;
		}

		public AiSwitch() : this(0, 2, 2) { }

		protected AiSwitch( in int row, in int column, in int rowSpan )
		{
			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions   = LayoutOptions.FillAndExpand;
			Grid.SetRow(this, row);
			Grid.SetColumn(this, column);
			Grid.SetRowSpan(this, rowSpan);
			BackgroundColor = Color.Transparent;
		}


		public void Update( IUseCheckableConfiguration configuration )
		{
			ThumbColor = configuration.OffColor;
			OnColor    = configuration.AccentColor;
		}

		public void Select() { Checked   = true; }
		public void Deselect() { Checked = false; }
	}
}

using Jakar.SettingsView.Shared.Interfaces;
using Xamarin.Forms;


namespace Jakar.SettingsView.Shared.Layouts.Controls
{
	public class AiRadioButton : RadioButton, IUpdateAccessoryControl
	{
		public bool Checked
		{
			get => IsChecked;
			set => IsChecked = value;
		}

		public AiRadioButton() : this(0, 2, 2) { }

		protected AiRadioButton( in int row, in int column, in int rowSpan )
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
			BackgroundColor = configuration.OffColor;
			BorderColor     = configuration.AccentColor;
		}

		public void Select() { Checked   = true; }
		public void Deselect() { Checked = false; }
	}
}

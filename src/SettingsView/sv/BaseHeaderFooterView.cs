// unset

using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Interfaces;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.sv
{
	public abstract class BaseHeaderFooterView : Grid
	{
		// public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(BaseHeaderFooterView), default(string?));
		// public static readonly BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor), typeof(Color), typeof(BaseHeaderFooterView), Color.Default);
		//
		// public string? Title
		// {
		// 	get => (string?) GetValue(TitleProperty);
		// 	set => SetValue(TitleProperty, value);
		// }
		//
		// public Color TitleColor
		// {
		// 	get => (Color) GetValue(TitleColorProperty);
		// 	set => SetValue(TitleColorProperty, value);
		// }

		public View View => this;


		// public Color BorderColor { get; set; } = SVConstants.DEFAULT.BACKGROUND_COLOR;
		// public Thickness BorderThickness { get; set; } = new(0);

		// public virtual SizeRequest HeightRequest { get; set; } = new(Size.Zero, new Size(-1, SVConstants.MIN_ROW_HEIGHT));
		// public virtual Thickness Padding { get; set; } = new(SVConstants.Cell.PADDING);
		// public virtual Color BackgroundColor { get; set; } = SVConstants.BACKGROUND_COLOR;


		protected BaseHeaderFooterView() : base()
		{
			BindingContext = this;
			HorizontalOptions = LayoutOptions.Fill;
			VerticalOptions = LayoutOptions.Fill;

			HeightRequest = SVConstants.Section.Footer.MinRowHeight; 
			Margin = new Thickness(0);
		}
	}
}
// unset

using Jakar.SettingsView.Shared.Config;
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

		public abstract string? Title { get; set; }
		public abstract Color TitleColor { get; set; }
		public abstract double FontSize { get; set; }
		public abstract FontAttributes FontAttributes { get; set; }
		public abstract string FontFamily { get; set; }
		public Section? Section { get; set; }

		// public Color BorderColor { get; set; } = SVConstants.BACKGROUND_COLOR;
		// public Thickness BorderThickness { get; set; } = new(0);

		// public virtual SizeRequest HeightRequest { get; set; } = new(Size.Zero, new Size(-1, SVConstants.MIN_ROW_HEIGHT));
		// public virtual Thickness Padding { get; set; } = new(SVConstants.PADDING);
		// public virtual Color BackgroundColor { get; set; } = SVConstants.BACKGROUND_COLOR;


		protected BaseHeaderFooterView() : base()
		{
			BindingContext = this;
			HorizontalOptions = LayoutOptions.Fill;
			VerticalOptions = LayoutOptions.Fill;

			HeightRequest = SVConstants.MIN_ROW_HEIGHT; // new SizeRequest(Size.Zero, new Size(-1, SVConstants.MIN_ROW_HEIGHT));
			Padding = new Thickness(SVConstants.PADDING);
			Margin = new Thickness(0);
		}
	}
}
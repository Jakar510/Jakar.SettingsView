using System.Diagnostics.CodeAnalysis;
using Jakar.SettingsView.Shared.Config;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

#nullable enable
namespace Jakar.SettingsView.Shared.sv
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public sealed partial class DefaultFooterView : FooterView
	{
		public override void SetText( string? value ) { TitleLabel.Text = value; }
		public override void SetTextColor( Color value ) { TitleLabel.TextColor = value; }
		public override void SetBackground( Color value ) { BackgroundColor = value; }
		public override void SetTextFont( double fontSize, string family, FontAttributes attributes )
		{
			TitleLabel.FontFamily = family;
			TitleLabel.FontAttributes = attributes;
			TitleLabel.FontSize = fontSize;
		}

		[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
		public DefaultFooterView()
		{
			InitializeComponent();

			Padding = SVConstants.Section.Footer.PADDING;
			BackgroundColor = SVConstants.Section.Footer.BACKGROUND_COLOR;
			TitleLabel.FontSize = SVConstants.Section.Footer.FONT_SIZE;
			SetTextColor(SVConstants.Section.Footer.TEXT_COLOR);
		}
	}
}
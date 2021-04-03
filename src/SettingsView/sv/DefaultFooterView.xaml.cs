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

			Padding = SvConstants.Section.Footer.padding;
			BackgroundColor = SvConstants.Section.Footer.background_Color;
			TitleLabel.FontSize = SvConstants.Section.Footer.FONT_SIZE;
			SetTextColor(SvConstants.Section.Footer.text_Color);
		}
	}
}
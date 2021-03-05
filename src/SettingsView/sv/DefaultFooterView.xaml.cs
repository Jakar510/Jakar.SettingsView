using System.Diagnostics.CodeAnalysis;
using Jakar.SettingsView.Shared.Config;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

#nullable enable
namespace Jakar.SettingsView.Shared.sv
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DefaultFooterView : FooterView
	{
		public override string? Title
		{
			get => TitleLabel.Text;
			set => TitleLabel.Text = value;
		}

		public override Color TitleColor
		{
			get => TitleLabel.TextColor;
			set => TitleLabel.TextColor = value;
		}

		public override double FontSize
		{
			get => TitleLabel.FontSize;
			set => TitleLabel.FontSize = value;
		}

		public override FontAttributes FontAttributes
		{
			get => TitleLabel.FontAttributes;
			set => TitleLabel.FontAttributes = value;
		}

		public override string FontFamily
		{
			get => TitleLabel.FontFamily;
			set => TitleLabel.FontFamily = value;
		}


		[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
		public DefaultFooterView()
		{
			InitializeComponent();

			FontSize = SVConstants.FOOTER_FONT_SIZE;
			TitleColor = SVConstants.FOOTER_TITLE_COLOR;

			TitleLabel.FontSize = SVConstants.TITLE_FONT_SIZE;
			TitleLabel.TextColor = SVConstants.TEXT_COLOR;
		}
	}
}
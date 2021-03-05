using System.Diagnostics.CodeAnalysis;
using Jakar.SettingsView.Shared.Config;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

#nullable enable
namespace Jakar.SettingsView.Shared.sv
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DefaultHeaderView : HeaderView
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

		public override ImageSource Source
		{
			get => Icon.Source;
			set => Icon.Source = value;
		}

		public override bool IsCollapsible
		{
			get => (bool) GetValue(IsCollapsibleProperty);
			set => SetValue(IsCollapsibleProperty, value);
		}

		public override bool IsCollapsed
		{
			get => (bool) GetValue(IsCollapsedProperty);
			set => SetValue(IsCollapsedProperty, value);
		}


		[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
		public DefaultHeaderView() : base()
		{
			InitializeComponent();
			CollapsedIcon = Icons.CollapseSolidWhite;
			ExpandedIcon = Icons.ExpandSolidWhite;

			FontSize = SVConstants.HEADER_FONT_SIZE;
			TitleColor = SVConstants.HEADER_TITLE_COLOR;
			Source = ExpandedIcon;
			
			TitleLabel.FontSize = SVConstants.TITLE_FONT_SIZE;
			TitleLabel.TextColor = SVConstants.TEXT_COLOR;
		}


	}
}
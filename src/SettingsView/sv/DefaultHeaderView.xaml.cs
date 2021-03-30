using System;
using System.Diagnostics.CodeAnalysis;
using Jakar.SettingsView.Shared.Config;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

#nullable enable
namespace Jakar.SettingsView.Shared.sv
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public sealed partial class DefaultHeaderView : HeaderView
	{
		public override void SetText( string? value ) { TitleLabel.Text = value; }
		public override void SetTextColor( Color value ) { TitleLabel.TextColor = value; }
		public override void SetTextFont( double fontSize, string family, FontAttributes attributes )
		{
			TitleLabel.FontFamily = family;
			TitleLabel.FontAttributes = attributes;
			TitleLabel.FontSize = fontSize;
		}
		public override void SetBackground( Color value ) { BackgroundColor = value; }

		public override ImageSource? Source
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
			// CollapsedIcon = Icons.CollapseSolidWhite;
			// ExpandedIcon = Icons.ExpandSolidWhite;
			Source = ExpandedIcon;

			Padding = SVConstants.Section.Header.PADDING;
			HeightRequest = SVConstants.Section.Header.MinRowHeight;
			BackgroundColor = SVConstants.Section.Header.BACKGROUND_COLOR;
			TitleLabel.FontSize = SVConstants.Section.Header.FONT_SIZE;
			SetTextColor(SVConstants.Section.Header.TEXT_COLOR);
		}
	}
}
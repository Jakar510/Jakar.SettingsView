// unset

using System.Reflection;
using Jakar.SettingsView.Shared.Config;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.sv
{
	// public sealed class DefaultHeaderView : HeaderView
	// {
	// 	public override bool IsCollapsible
	// 	{
	// 		get => (bool) GetValue(IsCollapsibleProperty);
	// 		set => SetValue(IsCollapsibleProperty, value);
	// 	}
	//
	// 	public override bool IsCollapsed
	// 	{
	// 		get => (bool) GetValue(IsCollapsedProperty);
	// 		set => SetValue(IsCollapsedProperty, value);
	// 	}
	//
	// 	public override string? Title
	// 	{
	// 		get => TitleLabel.Text;
	// 		set => TitleLabel.Text = value;
	// 	}
	//
	//
	// 	public Image Icon { get; }
	// 	public Label TitleLabel { get; }
	//
	// 	public DefaultHeaderView()
	// 	{
	// 		CollapsedIcon = Icons.CollapseSolidBlack;
	// 		ExpandedIcon = Icons.ExpandSolidBlack;
	//
	// 		HeightRequest = SVConstants.MIN_ROW_HEIGHT;
	// 		BackgroundColor = Color.SteelBlue;
	// 		HorizontalOptions = LayoutOptions.Fill;
	// 		VerticalOptions = LayoutOptions.Fill;
	// 		Padding = new Thickness(0);
	// 		Margin = new Thickness(0);
	//
	// 		Icon = new Image()
	// 			   {
	// 				   HorizontalOptions = LayoutOptions.Start,
	// 				   VerticalOptions = LayoutOptions.Fill,
	// 			   };
	// 		TitleLabel = new Label()
	// 				{
	// 					HorizontalOptions = LayoutOptions.Fill,
	// 					VerticalOptions = LayoutOptions.Fill,
	// 					FontSize = SVConstants.TITLE_FONT_SIZE,
	// 					TextColor = SVConstants.TEXT_COLOR,
	// 				};
	//
	// 		ColumnDefinitions = new ColumnDefinitionCollection()
	// 							{
	// 								new()
	// 								{
	// 									Width = new GridLength(0.1, GridUnitType.Star)
	// 								},
	// 								new()
	// 								{
	// 									Width = GridLength.Star
	// 								}
	// 							};
	// 		RowDefinitions = new RowDefinitionCollection()
	// 						 {
	// 							 new()
	// 							 {
	// 								 Height = GridLength.Star
	// 							 }
	// 						 };
	//
	// 		Children.AddHorizontal(Icon);
	// 		Children.AddHorizontal(TitleLabel);
	// 	}
	//
	// 	public override bool Clicked()
	// 	{
	// 		if ( !IsCollapsible ) return false;
	//
	// 		IsCollapsed = !IsCollapsed;
	// 		return true;
	// 	}
	// 	public override void Collapse() { Icon.Source = CollapsedIcon; }
	// 	public override void Expand() { Icon.Source = ExpandedIcon; }
	// }
}
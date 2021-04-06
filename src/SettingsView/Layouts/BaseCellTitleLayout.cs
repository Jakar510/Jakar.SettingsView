using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Layouts.Controls;
using Xamarin.Forms;


namespace Jakar.SettingsView.Shared.Layouts
{
	public abstract class BaseCellTitleLayout : BaseCellLayout
	{
		public Icon        Icon        { get; protected set; }
		public Title       Title       { get; protected set; }
		public Description Description { get; protected set; }


		protected BaseCellTitleLayout()
		{
			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions   = LayoutOptions.FillAndExpand;

			Icon        = new Icon();
			Title       = new Title();
			Description = new Description();
		}



		public static class Definitions
		{
			public static RowDefinitionCollection Rows() => new()
															{
																new RowDefinition() { Height = GridLength.Star },
																new RowDefinition() { Height = GridLength.Star },
															};



			public static class Columns
			{
				public static ColumnDefinitionCollection Title() => new()
																	{
																		new ColumnDefinition() { Width = new GridLength(SvConstants.Layout.ColumnFactors.ICON, GridUnitType.Star) },
																		new ColumnDefinition() { Width = GridLength.Star },
																	};

				public static ColumnDefinitionCollection Value() => new()
																	{
																		new ColumnDefinition() { Width = new GridLength(SvConstants.Layout.ColumnFactors.ICON,        GridUnitType.Star) },
																		new ColumnDefinition() { Width = new GridLength(SvConstants.Layout.ColumnFactors.TITLE_STACK, GridUnitType.Star) },
																		new ColumnDefinition() { Width = new GridLength(SvConstants.Layout.ColumnFactors.VALUE_STACK, GridUnitType.Star) },
																		new ColumnDefinition() { Width = GridLength.Star },
																	};

				public static ColumnDefinitionCollection Accessory() => new()
																		{
																			new ColumnDefinition() { Width = new GridLength(SvConstants.Layout.ColumnFactors.ICON, GridUnitType.Star) },
																			new ColumnDefinition() { Width = GridLength.Star },
																			new ColumnDefinition() { Width = new GridLength(SvConstants.Layout.ColumnFactors.ACCESSORY, GridUnitType.Star) },
																		};
			}
		}
	}
}

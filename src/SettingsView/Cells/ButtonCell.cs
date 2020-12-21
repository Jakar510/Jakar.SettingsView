using System.Windows.Input;
using Jakar.SettingsView.Shared.Cells.Base;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells
{
	/// <summary>
	/// Button cell.
	/// </summary>
	public class ButtonCell : CellBaseIcon
	{
		/// <summary>
		/// The title alignment property.
		/// </summary>
		public static BindableProperty TitleAlignmentProperty = BindableProperty.Create(nameof(TitleAlignment), typeof(TextAlignment), typeof(ButtonCell), TextAlignment.Center);

		/// <summary>
		/// Gets or sets the title alignment.
		/// </summary>
		/// <value>The title alignment.</value>
		public TextAlignment TitleAlignment
		{
			get => (TextAlignment) GetValue(TitleAlignmentProperty);
			set => SetValue(TitleAlignmentProperty, value);
		}

		/// <summary>
		/// The command property.
		/// </summary>
		public static BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ButtonCell), default(ICommand));

		/// <summary>
		/// Gets or sets the command.
		/// </summary>
		/// <value>The command.</value>
		public ICommand Command
		{
			get => (ICommand) GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		/// <summary>
		/// The command parameter property.
		/// </summary>
		public static BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ButtonCell));

		/// <summary>
		/// Gets or sets the command parameter.
		/// </summary>
		/// <value>The command parameter.</value>
		public object CommandParameter
		{
			get => GetValue(CommandParameterProperty);
			set => SetValue(CommandParameterProperty, value);
		}
	}
}
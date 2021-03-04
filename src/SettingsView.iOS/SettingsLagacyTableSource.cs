using Foundation;
using Jakar.SettingsView.Shared;
using Jakar.SettingsView.Shared.sv;
using UIKit;
using Xamarin.Forms;

namespace Jakar.SettingsView.iOS
{
	/// <summary>
	/// Settings lagacy table source.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class SettingsLagacyTableSource : SettingsTableSource
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.iOS.SettingsLagacyTableSource"/> class.
		/// </summary>
		/// <param name="settingsView">Settings view.</param>
		public SettingsLagacyTableSource( Shared.sv.SettingsView settingsView ) : base(settingsView) { }

		/// <summary>
		/// Cans the move row.
		/// </summary>
		/// <returns><c>true</c>, if move row was caned, <c>false</c> otherwise.</returns>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
		public override bool CanMoveRow( UITableView tableView, NSIndexPath indexPath )
		{
			Section section = _settingsView.Model.GetSection(indexPath.Section);
			return section.UseDragSort;
		}

		/// <summary>
		/// Moves the row.
		/// </summary>
		/// <param name="tableView">Table view.</param>
		/// <param name="sourceIndexPath">Source index path.</param>
		/// <param name="destinationIndexPath">Destination index path.</param>
		public override void MoveRow( UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath )
		{
			if ( sourceIndexPath.Section != destinationIndexPath.Section )
			{
				_tableView.ReloadData();
				return;
			}

			Section section = _settingsView.Model.GetSection(sourceIndexPath.Section);

			if ( section.ItemsSource == null )
			{
				Cell tmp = section[sourceIndexPath.Row];
				section.RemoveAt(sourceIndexPath.Row);
				section.Insert(destinationIndexPath.Row, tmp);
			}
			else
			{
				object tmp = section.ItemsSource[sourceIndexPath.Row];
				section.ItemsSource.RemoveAt(sourceIndexPath.Row);
				section.ItemsSource.Insert(destinationIndexPath.Row, tmp);
			}
		}

		/// <summary>
		/// Editings the style for row.
		/// </summary>
		/// <returns>The style for row.</returns>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
		public override UITableViewCellEditingStyle EditingStyleForRow( UITableView tableView, NSIndexPath indexPath ) => UITableViewCellEditingStyle.None;

		/// <summary>
		/// Shoulds the indent while editing.
		/// </summary>
		/// <returns><c>true</c>, if indent while editing was shoulded, <c>false</c> otherwise.</returns>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
		public override bool ShouldIndentWhileEditing( UITableView tableView, NSIndexPath indexPath ) => false;
	}
}
namespace Jakar.SettingsView.iOS
{
	[Preserve(AllMembers = true)]
	public class SettingsLegacyTableSource : SettingsTableSource
	{
		public SettingsLegacyTableSource( Shared.sv.SettingsView settingsView ) : base(settingsView) { }

		public override bool CanMoveRow( UITableView tableView, NSIndexPath indexPath )
		{
			Section? section = _SettingsView.Model.GetSection(indexPath.Section);
			if ( section is null ) { throw new NullReferenceException(nameof(section)); }

			return section.UseDragSort;
		}

		public override void MoveRow( UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath )
		{
			if ( sourceIndexPath.Section != destinationIndexPath.Section )
			{
				tableView.ReloadData();
				return;
			}

			Section? section = _SettingsView.Model.GetSection(sourceIndexPath.Section);

			if ( section is null ) { throw new NullReferenceException(nameof(section)); }

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

		public override UITableViewCellEditingStyle EditingStyleForRow( UITableView tableView, NSIndexPath indexPath ) => UITableViewCellEditingStyle.None;

		public override bool ShouldIndentWhileEditing( UITableView tableView, NSIndexPath indexPath ) => false;
	}
}
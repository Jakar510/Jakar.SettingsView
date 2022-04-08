namespace Jakar.SettingsView.iOS.BaseCell
{
	[Foundation.Preserve(AllMembers = true)]
	public class CellBaseRenderer<TNativeCell> : CellRenderer where TNativeCell : BaseCellView
	{
		public override UITableViewCell GetCell( Cell item, UITableViewCell reusableCell, UITableView table )
		{
			if ( reusableCell is not TNativeCell nativeCell ) { nativeCell = InstanceCreator<TNativeCell>.Create(item); }

			ClearPropertyChanged(nativeCell);

			nativeCell.Cell = item;

			SetUpPropertyChanged(nativeCell);

			nativeCell.UpdateCell(table);

			return nativeCell;
		}

		protected void SetUpPropertyChanged( TNativeCell nativeCell )
		{
			if ( nativeCell.Cell is not CellBase formsCell ) return;
			Shared.sv.SettingsView parentElement = formsCell.Parent;

			formsCell.PropertyChanged += nativeCell.CellPropertyChanged;

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if ( parentElement is null ) return;
			parentElement.PropertyChanged += nativeCell.ParentPropertyChanged;
			// Section? section = parentElement.Model.GetSectionFromCell(formsCell);
			Section? section = parentElement.Model.GetSection(SettingsModel.GetPath(formsCell).Item1);
			if ( section is null ) return;
			formsCell.Section = section;
			formsCell.Section.PropertyChanged += nativeCell.SectionPropertyChanged;
		}
		protected void ClearPropertyChanged( TNativeCell nativeCell )
		{
			if ( nativeCell.Cell is not CellBase formsCell ) return;
			Shared.sv.SettingsView parentElement = formsCell.Parent;

			formsCell.PropertyChanged -= nativeCell.CellPropertyChanged;
			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if ( parentElement is null ) return;
			parentElement.PropertyChanged -= nativeCell.ParentPropertyChanged;
			if ( formsCell.Section != null ) { formsCell.Section.PropertyChanged -= nativeCell.SectionPropertyChanged; }
		}


		// protected void SetUpPropertyChanged( CellBaseView nativeCell )
		// {
		// 	var formsCell = nativeCell.Cell as CellBase;
		// 	var parentElement = formsCell.Parent as Shared.sv.SettingsView;
		//
		// 	formsCell.PropertyChanged += nativeCell.CellPropertyChanged;
		//
		// 	if ( parentElement != null )
		// 	{
		// 		parentElement.PropertyChanged += nativeCell.ParentPropertyChanged;
		// 		Section section = parentElement.Model.GetSection(SettingsModel.GetPath(formsCell).Item1);
		// 		if ( section != null )
		// 		{
		// 			formsCell.Section = section;
		// 			formsCell.Section.PropertyChanged += nativeCell.SectionPropertyChanged;
		// 		}
		// 	}
		// }
		//
		// private void ClearPropertyChanged( CellBaseView nativeCell )
		// {
		// 	var formsCell = nativeCell.Cell as CellBase;
		//
		// 	if ( formsCell is null )
		// 		return; // for HotReload
		//
		// 	var parentElement = formsCell.Parent as Shared.sv.SettingsView;
		//
		// 	formsCell.PropertyChanged -= nativeCell.CellPropertyChanged;
		// 	if ( parentElement != null )
		// 	{
		// 		parentElement.PropertyChanged -= nativeCell.ParentPropertyChanged;
		// 		if ( formsCell.Section != null ) { formsCell.Section.PropertyChanged -= nativeCell.SectionPropertyChanged; }
		// 	}
		// }
	}
}
using System;
using System.Linq.Expressions;
using System.Reflection;
using Jakar.Api.Extensions;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.sv;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#nullable enable
namespace Jakar.SettingsView.iOS.BaseCell
{
	[Foundation.Preserve(AllMembers = true)]
	public class CellBaseRenderer<TnativeCell> : CellRenderer where TnativeCell : BaseCellView
	{
		public override UITableViewCell GetCell( Cell item, UITableViewCell reusableCell, UITableView table )
		{
			if ( reusableCell is not TnativeCell nativeCell ) { nativeCell = InstanceCreator.Create<TnativeCell>(item); }

			ClearPropertyChanged(nativeCell);

			nativeCell.Cell = item;

			SetUpPropertyChanged(nativeCell);

			nativeCell.UpdateCell(table);

			return nativeCell;
		}

		protected void SetUpPropertyChanged( BaseCellView  nativeCell )
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
		protected void ClearPropertyChanged( BaseCellView  nativeCell )
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
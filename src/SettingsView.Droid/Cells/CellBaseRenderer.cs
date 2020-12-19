using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Android.Content;
using Jakar.SettingsView.Droid.Cells.Base;
using Jakar.SettingsView.Shared;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Cells.Base;
using Xamarin.Forms.Platform.Android;
using BaseCellView = Jakar.SettingsView.Droid.Cells.Base.BaseCellView;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public class CellBaseRenderer<TnativeCell> : CellRenderer where TnativeCell : BaseCellView
	{
		protected override Android.Views.View GetCellCore( Xamarin.Forms.Cell item,
														   Android.Views.View? convertView,
														   Android.Views.ViewGroup parent,
														   Context context )
		{
			if ( !( convertView is TnativeCell nativeCell ) ) { nativeCell = InstanceCreator<Context, Xamarin.Forms.Cell, TnativeCell>.Create(context, item); }

			ClearPropertyChanged(nativeCell);

			nativeCell.Cell = item;

			SetUpPropertyChanged(nativeCell);

			nativeCell.UpdateCell();

			return nativeCell;
		}
		// protected override void OnCellPropertyChanged( object sender, PropertyChangedEventArgs e ) { base.OnCellPropertyChanged(sender, e); }

		protected void SetUpPropertyChanged( BaseCellView nativeCell )
		{
			if ( !( nativeCell.Cell is CellBase formsCell ) ) return;
			Shared.SettingsView parentElement = formsCell.Parent;

			formsCell.PropertyChanged += nativeCell.CellPropertyChanged;

			if ( parentElement is null ) return;
			parentElement.PropertyChanged += nativeCell.ParentPropertyChanged;
			Section section = parentElement.Model.GetSectionFromCell(formsCell);
			if ( section is null ) return;
			formsCell.Section = section;
			formsCell.Section.PropertyChanged += nativeCell.SectionPropertyChanged;
		}

		protected void ClearPropertyChanged( BaseCellView nativeCell )
		{
			if ( !( nativeCell.Cell is CellBase formsCell ) ) return;
			Shared.SettingsView parentElement = formsCell.Parent;

			formsCell.PropertyChanged -= nativeCell.CellPropertyChanged;
			if ( parentElement == null ) return;
			parentElement.PropertyChanged -= nativeCell.ParentPropertyChanged;
			if ( formsCell.Section != null ) { formsCell.Section.PropertyChanged -= nativeCell.SectionPropertyChanged; }
		}
	}
}
﻿using System;
using System.Linq.Expressions;
using System.Reflection;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.sv;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Jakar.SettingsView.iOS.BaseCell
{
	[Foundation.Preserve(AllMembers = true)]
	public class CellBaseRenderer<TnativeCell> : CellRenderer where TnativeCell : BaseCellView
	{
		/// http://neue.cc/2014/09/16_478.html
		internal static class InstanceCreator<T1, TInstance>
		{
			public static Func<T1, TInstance> Create { get; } = CreateInstance();

			private static Func<T1, TInstance> CreateInstance()
			{
				ConstructorInfo constructor = typeof(TInstance).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
																			   Type.DefaultBinder,
																			   new[]
																			   {
																				   typeof(T1)
																			   },
																			   null
																			  );
				ParameterExpression arg1 = Expression.Parameter(typeof(T1));
				return Expression.Lambda<Func<T1, TInstance>>(Expression.New(constructor, arg1), arg1).Compile();
			}
		}
		
		public override UITableViewCell GetCell( Cell item, UITableViewCell reusableCell, UITableView tv )
		{
			var nativeCell = reusableCell as TnativeCell;
			if ( nativeCell == null ) { nativeCell = InstanceCreator<Cell, TnativeCell>.Create(item); }

			ClearPropertyChanged(nativeCell);

			nativeCell.Cell = item;

			SetUpPropertyChanged(nativeCell);

			nativeCell.UpdateCell(tv);

			return nativeCell;
		}

		protected void SetUpPropertyChanged( BaseCellView nativeCell )
		{
			if ( nativeCell.Cell is not CellBase formsCell )
				return;
			Shared.sv.SettingsView? parentElement = formsCell.Parent;

			formsCell.PropertyChanged += nativeCell.CellPropertyChanged;

			if ( parentElement is null )
				return;
			parentElement.PropertyChanged += nativeCell.ParentPropertyChanged;
			Section section = parentElement.Model.GetSectionFromCell(formsCell);
			if ( section is null )
				return;
			formsCell.Section = section;
			formsCell.Section.PropertyChanged += nativeCell.SectionPropertyChanged;
		}
		protected void ClearPropertyChanged( BaseCellView nativeCell )
		{
			if ( nativeCell.Cell is not CellBase formsCell )
				return;
			Shared.sv.SettingsView? parentElement = formsCell.Parent;

			formsCell.PropertyChanged -= nativeCell.CellPropertyChanged;
			if ( parentElement is null )
				return;
			parentElement.PropertyChanged -= nativeCell.ParentPropertyChanged;
			if ( formsCell.Section != null )
			{ formsCell.Section.PropertyChanged -= nativeCell.SectionPropertyChanged; }
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
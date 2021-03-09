﻿using System;
using System.Linq.Expressions;
using System.Reflection;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.sv;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Jakar.SettingsView.iOS.Cells
{
	/// <summary>
	/// Cell base renderer.
	/// </summary>
	[Foundation.Preserve(AllMembers = true)]
	public class CellBaseRenderer<TnativeCell> : CellRenderer where TnativeCell : CellBaseView
	{
		/// <summary>
		/// Refer to 
		/// http://qiita.com/Temarin/items/d6f00428743b0971ec95
		/// http://neue.cc/2014/09/16_478.html
		/// </summary>
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

		/// <summary>
		/// Gets the cell.
		/// </summary>
		/// <returns>The cell.</returns>
		/// <param name="item">Item.</param>
		/// <param name="reusableCell">Reusable cell.</param>
		/// <param name="tv">Tv.</param>
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

		/// <summary>
		/// Sets up property changed.
		/// </summary>
		/// <param name="nativeCell">Native cell.</param>
		protected void SetUpPropertyChanged( CellBaseView nativeCell )
		{
			var formsCell = nativeCell.Cell as CellBase;
			var parentElement = formsCell.Parent as Shared.sv.SettingsView;

			formsCell.PropertyChanged += nativeCell.CellPropertyChanged;

			if ( parentElement != null )
			{
				parentElement.PropertyChanged += nativeCell.ParentPropertyChanged;
				Section section = parentElement.Model.GetSection(SettingsModel.GetPath(formsCell).Item1);
				if ( section != null )
				{
					formsCell.Section = section;
					formsCell.Section.PropertyChanged += nativeCell.SectionPropertyChanged;
				}
			}
		}

		private void ClearPropertyChanged( CellBaseView nativeCell )
		{
			var formsCell = nativeCell.Cell as CellBase;

			if ( formsCell is null )
				return; // for HotReload

			var parentElement = formsCell.Parent as Shared.sv.SettingsView;

			formsCell.PropertyChanged -= nativeCell.CellPropertyChanged;
			if ( parentElement != null )
			{
				parentElement.PropertyChanged -= nativeCell.ParentPropertyChanged;
				if ( formsCell.Section != null ) { formsCell.Section.PropertyChanged -= nativeCell.SectionPropertyChanged; }
			}
		}
	}
}
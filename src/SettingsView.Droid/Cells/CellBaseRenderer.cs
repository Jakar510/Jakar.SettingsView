using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Android.Content;
using Jakar.SettingsView.Droid.Cells.Base;
using Jakar.SettingsView.Shared;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms.Platform.Android;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	/// <summary>
	/// Cell base renderer.
	/// </summary>
	[Android.Runtime.Preserve(AllMembers = true)]
	public class CellBaseRenderer<TnativeCell> : CellRenderer where TnativeCell : CellBaseView
	{
		internal static class InstanceCreator<T1, T2, TInstance>
		{
			public static Func<T1, T2, TInstance> Create { get; } = CreateInstance();

			private static Func<T1, T2, TInstance> CreateInstance()
			{
				Type[] argsTypes =
				{
					typeof(T1),
					typeof(T2)
				};
				ConstructorInfo constructor = typeof(TInstance).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder, argsTypes, null) ?? throw new NullReferenceException(nameof(constructor));
				ParameterExpression[] args = argsTypes.Select(Expression.Parameter).ToArray();
				return Expression.Lambda<Func<T1, T2, TInstance>>(Expression.New(constructor, args), args).Compile();
			}
		}


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


		protected void SetUpPropertyChanged( CellBaseView nativeCell )
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

		protected void ClearPropertyChanged( CellBaseView nativeCell )
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
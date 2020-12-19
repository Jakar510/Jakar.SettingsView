using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using Jakar.SettingsView.Droid.Cells.Controls;
using Xamarin.Forms;
using AContext = Android.Content.Context;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Base
{
	public abstract class BaseAiTitledCell : BaseCellView
	{
		protected internal Android.Views.View ContentView { get; set; }
		protected GridLayout _CellLayout { get; }
		protected TitleView _Title { get; }

		protected BaseAiTitledCell( AContext context, Cell cell ) : base(context, cell)
		{
			ContentView = CreateContentView(Resource.Layout.CellLayout);
			_CellLayout = ContentView.FindViewById<GridLayout>(Resource.Id.CellLayout) ?? throw new NullReferenceException(nameof(_CellLayout));
			_Title = BaseTextView.Create<TitleView>(ContentView, this, Resource.Id.CellTitle);
		}
		protected BaseAiTitledCell( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }


		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);

			_Title.Update(sender, e);
		}
		protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);

			_Title.UpdateParent(sender, e);
		}

		protected override void EnableCell()
		{
			base.EnableCell();
			_Title.Enable();
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Title.Disable();
		}

		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			_Title.Update();
		}

		protected override void Dispose( bool disposing )
		{
			base.Dispose(disposing);
			_Title.Dispose();
			_CellLayout.Dispose();
		}
	}
}
using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Base
{
	public abstract class BaseTitledCell : CellBaseView
	{
		protected internal Android.Views.View ContentView { get; set; }
		protected TitleView _Title { get; }

		protected GridLayout _CellLayout { get; }

		protected BaseTitledCell( Context context, Cell cell ) : base(context, cell)
		{
			ContentView = CreateContentView(Resource.Layout.CellLayout);
			_CellLayout = ContentView.FindViewById<GridLayout>(Resource.Id.CellLayout) ?? throw new NullReferenceException(nameof(_CellLayout));
			_Title = BaseTextView.Create<TitleView>(ContentView, this, Resource.Id.CellTitle);
		}
		protected BaseTitledCell( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)
		{
			ContentView = CreateContentView(Resource.Layout.CellLayout);
			_CellLayout = ContentView.FindViewById<GridLayout>(Resource.Id.CellLayout) ?? throw new NullReferenceException(nameof(_CellLayout));
			_Title = BaseTextView.Create<TitleView>(ContentView, this, Resource.Id.CellTitle);
		}

		protected override void Dispose( bool disposing )
		{
			base.Dispose(disposing);
			_Title.Dispose();
			_CellLayout.Dispose();
		}
	}
}
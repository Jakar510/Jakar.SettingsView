using System;
using System.ComponentModel;
using Android.Runtime;
using Android.Widget;
using Jakar.SettingsView.Droid.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AContext = Android.Content.Context;
using AView = Android.Views.View;

#nullable enable
namespace Jakar.SettingsView.Droid.BaseCell
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public abstract class BaseAiTitledCell : BaseCellView
	{
		protected internal AView ContentView { get; set; }
		protected GridLayout _CellLayout { get; }
		protected TitleView _Title { get; }


		protected BaseAiTitledCell( AContext context, Cell cell ) : base(context, cell)
		{
			ContentView = CreateContentView(Resource.Layout.CellLayout);
			_CellLayout = Layout();
			_Title = BaseTextView.Create<TitleView>(ContentView, this, Resource.Id.CellTitle);
		}
		protected BaseAiTitledCell( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }

		private GridLayout Layout() => ContentView.FindViewById<GridLayout>(Resource.Id.CellLayout) ?? throw new NullReferenceException(nameof(Resource.Id.CellLayout));
		protected LinearLayout ValueStack() => ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellValueStack));
		protected LinearLayout AccessoryStack() => ContentView.FindViewById<LinearLayout>(Resource.Id.CellAccessoryStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellAccessoryStack));


		protected void RemoveAccessoryStack() => ContentView.FindViewById<LinearLayout>(Resource.Id.CellAccessoryStack)?.RemoveFromParent();
		protected void RemoveCellValueStack() => ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack)?.RemoveFromParent();
		protected void RemoveHint() => ContentView.FindViewById<HintView>(Resource.Id.CellHint)?.RemoveFromParent();
		protected void RemoveIcon() => ContentView.FindViewById<IconView>(Resource.Id.CellIcon)?.RemoveFromParent();
		protected void RemoveTitle() => ContentView.FindViewById<TitleView>(Resource.Id.CellTitle)?.RemoveFromParent();
		protected void RemoveDescription() => ContentView.FindViewById<DescriptionView>(Resource.Id.CellDescription)?.RemoveFromParent();

		// protected void RemoveValue() => ContentView.FindViewById<ValueView>(Resource.Id.CellValue)?.RemoveFromParent();
		// protected void RemoveEntryValue() => ContentView.FindViewById<AiEditText>(Resource.Id.CellEditorValue)?.RemoveFromParent();


		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Title.Update(sender, e) ) return;
			base.CellPropertyChanged(sender, e);
		}
		protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Title.UpdateParent(sender, e) ) return;
			base.ParentPropertyChanged(sender, e);
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
using System;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Base
{
	public abstract class BaseValueCell : BaseDescriptionCell //<TCellTitle, TCell> : CellBaseView where TCell: TextView where TCellTitle : BaseView<TCell>, new()
	{
		protected HintView _Hint { get; }
		protected ValueView _Value { get; }
		protected LinearLayout _CellValueStack { get; }


		protected BaseValueCell( Context context, Cell cell ) : base(context, cell)
		{
			_Hint = BaseTextView.Create<HintView>(ContentView, this, Resource.Id.CellHint);
			_CellValueStack = ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellValueStack));

			LinearLayout accessoryStack = ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellValueStack));
			accessoryStack.RemoveFromParent();

			var textView = new TextView(AndroidContext);
			AddAccessory(_CellValueStack, textView);
			_Value = new ValueView(this, AndroidContext);
		}
		protected BaseValueCell( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)
		{
			_Hint = BaseTextView.Create<HintView>(ContentView, this, Resource.Id.CellHint);
			_CellValueStack = ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellValueStack));

			LinearLayout accessoryStack = ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellValueStack));
			accessoryStack.RemoveFromParent();

			var textView = new TextView(AndroidContext);
			AddAccessory(_CellValueStack, textView);
			_Value = new ValueView(this, AndroidContext);
		}


		protected override void Dispose( bool disposing )
		{
			base.Dispose(disposing);
			_Hint.Dispose();
			_Value.Dispose();
			_CellValueStack.Dispose();
		}
	}
}
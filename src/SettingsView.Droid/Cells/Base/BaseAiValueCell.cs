using System;
using System.ComponentModel;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Base
{
	public abstract class BaseAiValueCell : BaseAiDescriptionCell //<TCellTitle, TCell> : CellBaseView where TCell: TextView where TCellTitle : BaseView<TCell>, new()
	{
		protected HintView _Hint { get; }
		protected ValueView _Value { get; }
		protected LinearLayout _CellValueStack { get; }


		protected BaseAiValueCell( Context context, Cell cell ) : base(context, cell)
		{
			_Hint = BaseTextView.Create<HintView>(ContentView, this, Resource.Id.CellHint);
			_CellValueStack = ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellValueStack));

			LinearLayout accessoryStack = ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellValueStack));
			accessoryStack.RemoveFromParent();

			var textView = new TextView(AndroidContext);
			AddAccessory(_CellValueStack, textView);
			_Value = new ValueView(this, AndroidContext);
		}
		protected BaseAiValueCell( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)
		{
			_Hint = BaseTextView.Create<HintView>(ContentView, this, Resource.Id.CellHint);
			_CellValueStack = ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellValueStack));

			LinearLayout accessoryStack = ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellValueStack));
			accessoryStack.RemoveFromParent();

			var textView = new TextView(AndroidContext);
			AddAccessory(_CellValueStack, textView);
			_Value = new ValueView(this, AndroidContext);
		}

		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);

			_Value.Update(sender, e);
			_Hint.Update(sender, e);
		}
		protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);

			_Value.UpdateParent(sender, e);
			_Hint.UpdateParent(sender, e);
		}


		protected override void EnableCell()
		{
			base.EnableCell();
			_Title.Enable();
			_Value.Enable();
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Hint.Disable();
			_Value.Disable();
		}

		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			_Hint.Update();
			_Value.Update();
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
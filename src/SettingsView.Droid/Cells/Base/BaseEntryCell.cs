using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using EntryCell = Jakar.SettingsView.Shared.Cells.EntryCell;
using Object = Java.Lang.Object;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Base
{
	public abstract class BaseEntryCell : BaseDescriptionCell, ITextWatcher, Android.Views.View.IOnFocusChangeListener, TextView.IOnEditorActionListener
	{
		protected EntryCell _EntryCell => Cell as EntryCell ?? throw new NullReferenceException(nameof(_EntryCell));
		protected HintView _Hint { get; }
		protected AiEditText _Value { get; }
		protected LinearLayout _CellValueStack { get; }
		protected BaseEntryCell( Context context, Cell cell ) : base(context, cell)
		{
			_Hint = BaseTextView.Create<HintView>(ContentView, this, Resource.Id.CellHint);
			_CellValueStack = ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellValueStack));

			LinearLayout accessoryStack = ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellValueStack));
			accessoryStack.RemoveFromParent();

			var textView = new TextView(AndroidContext);
			AddAccessory(_CellValueStack, textView);
			_Value = new AiEditText(AndroidContext);
			_Value.Init(_EntryCell, this);
		}
		protected BaseEntryCell( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)
		{
			_Hint = BaseTextView.Create<HintView>(ContentView, this, Resource.Id.CellHint);
			_CellValueStack = ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellValueStack));

			LinearLayout accessoryStack = ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellValueStack));
			accessoryStack.RemoveFromParent();

			var textView = new TextView(AndroidContext);
			AddAccessory(_CellValueStack, textView);
			_Value = new AiEditText(AndroidContext);
			_Value.Init(_EntryCell, this);
		}


		void ITextWatcher.AfterTextChanged( IEditable? s ) { }
		void ITextWatcher.BeforeTextChanged( ICharSequence? s,
											 int start,
											 int count,
											 int after ) { }
		void ITextWatcher.OnTextChanged( ICharSequence? s,
										 int start,
										 int before,
										 int count )
		{
			if ( string.IsNullOrEmpty(_EntryCell.ValueText) &&
				 s != null &&
				 s.Length() == 0 ) { return; }

			_EntryCell.ValueText = s?.ToString();
		}
		void IOnFocusChangeListener.OnFocusChange( Android.Views.View? v, bool hasFocus )
		{
			if ( hasFocus )
			{
				//show underline when on focus.
				if ( Background != null ) Background.Alpha = 100;
			}
			else
			{
				//hide underline
				if ( Background != null ) Background.Alpha = 0;
				// consider as text input completed.
				_EntryCell.SendCompleted();
			}
		}

		bool TextView.IOnEditorActionListener.OnEditorAction( TextView? v, ImeAction actionId, KeyEvent? e )
		{
			if ( v is null )
				return true;

			if ( actionId != ImeAction.Done &&
				 ( actionId != ImeAction.ImeNull || e != null && e.KeyCode != Keycode.Enter ) )
				return true;
			HideKeyboard(v);
			DoneEdit();

			return true;
		}
		protected internal void DoneEdit()
		{
			_EntryCell.SendCompleted();
			ClearFocus();
		}
		protected void HideKeyboard( Android.Views.View? inputView )
		{
			Object temp = AndroidContext.GetSystemService(Context.InputMethodService) ?? throw new NullReferenceException(nameof(Context.InputMethodService));
			using InputMethodManager inputMethodManager = (InputMethodManager) temp;
			IBinder? windowToken = inputView?.WindowToken;
			if ( windowToken != null ) { inputMethodManager.HideSoftInputFromWindow(windowToken, HideSoftInputFlags.None); }
		}
		protected void ShowKeyboard( Android.Views.View inputView )
		{
			Object temp = AndroidContext.GetSystemService(Context.InputMethodService) ?? throw new NullReferenceException(nameof(Context.InputMethodService));
			using InputMethodManager inputMethodManager = (InputMethodManager) temp;
			inputMethodManager.ShowSoftInput(inputView, ShowFlags.Forced);
			inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
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
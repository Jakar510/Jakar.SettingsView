using System;
using System.ComponentModel;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Jakar.SettingsView.Droid.Cells.Controls;
using Jakar.SettingsView.Droid.Extensions;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AiEntryCell = Jakar.SettingsView.Shared.Cells.EntryCell;
using AObject = Java.Lang.Object;
using AContext = Android.Content.Context;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Base
{
	public abstract class BaseAiEntryCell : BaseValueCell<AiEditText>, ITextWatcher, Android.Views.View.IOnFocusChangeListener, TextView.IOnEditorActionListener
	{
		protected AiEntryCell _EntryCell => Cell as AiEntryCell ?? throw new NullReferenceException(nameof(_EntryCell));


		protected BaseAiEntryCell( AContext context, Cell cell ) : base(context, cell) => _Value.Init(_EntryCell, this);
		protected BaseAiEntryCell( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) => _Value.Init(_EntryCell, this);


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
			AObject temp = AndroidContext.GetSystemService(Context.InputMethodService) ?? throw new NullReferenceException(nameof(Context.InputMethodService));
			using InputMethodManager inputMethodManager = (InputMethodManager) temp;
			IBinder? windowToken = inputView?.WindowToken;
			if ( windowToken != null ) { inputMethodManager.HideSoftInputFromWindow(windowToken, HideSoftInputFlags.None); }
		}
		protected void ShowKeyboard( Android.Views.View inputView )
		{
			AObject temp = AndroidContext.GetSystemService(Context.InputMethodService) ?? throw new NullReferenceException(nameof(Context.InputMethodService));
			using InputMethodManager inputMethodManager = (InputMethodManager) temp;
			inputMethodManager.ShowSoftInput(inputView, ShowFlags.Forced);
			inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
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
			_Value.Enable();
			_Hint.Enable();
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Value.Disable();
			_Hint.Disable();
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
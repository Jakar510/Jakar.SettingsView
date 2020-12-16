using System;
using System.ComponentModel;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Jakar.SettingsView.Droid.Cells.Base;
using Jakar.SettingsView.Shared.Cells;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AiEntryCell = Jakar.SettingsView.Shared.Cells.EntryCell;
using EntryCellRenderer = Jakar.SettingsView.Droid.Cells.EntryCellRenderer;
using Object = Java.Lang.Object;

[assembly: ExportRenderer(typeof(AiEntryCell), typeof(EntryCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)] public class EntryCellRenderer : CellBaseRenderer<EntryCellView> { }

	[Preserve(AllMembers = true)]
	public class EntryCellView : CellBaseView, ITextWatcher, Android.Views.View.IOnFocusChangeListener, TextView.IOnEditorActionListener
	{
		protected AiEntryCell _EntryCell => Cell as AiEntryCell ?? throw new NullReferenceException(nameof(_EntryCell));

		protected internal Android.Views.View ContentView { get; set; }
		protected GridLayout _CellLayout { get; set; }

		protected IconView _Icon { get; set; }
		protected TitleView _Title { get; set; }
		protected DescriptionView _Description { get; set; }
		protected HintView _Hint { get; set; }
		protected AiEditText _Value { get; set; }

		public EntryCellView( Context context, Cell cell ) : base(context, cell)
		{
			ContentView = CreateContentView(Resource.Layout.EntryCellLayout);
			_CellLayout = ContentView.FindViewById<GridLayout>(Resource.Id.EntryCellLayout) ?? throw new NullReferenceException(nameof(_CellLayout));
			_Icon = new IconView(this, ContentView.FindViewById<ImageView>(Resource.Id.EntryCellIcon));
			_Title = new TitleView(this, ContentView.FindViewById<TextView>(Resource.Id.EntryCellTitle));
			_Description = new DescriptionView(this, ContentView.FindViewById<TextView>(Resource.Id.EntryCellDescription));
			_Hint = new HintView(this, ContentView.FindViewById<TextView>(Resource.Id.EntryCellHintText));
			_Value = ContentView.FindViewById<AiEditText>(Resource.Id.EntryCellValue) ?? throw new NullReferenceException(nameof(_Value));
			_Value.Init(_EntryCell, this);

			Click += EntryCellView_Click;
			Click += EditTextOnClick;
			_EntryCell.Focused += EntryCell_Focused;
		}
		public EntryCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)
		{
			ContentView = CreateContentView(Resource.Layout.EntryCellLayout);
			_CellLayout = ContentView.FindViewById<GridLayout>(Resource.Id.EntryCellLayout) ?? throw new NullReferenceException(nameof(_CellLayout));
			_Icon = new IconView(this, ContentView.FindViewById<ImageView>(Resource.Id.EntryCellIcon));
			_Title = new TitleView(this, ContentView.FindViewById<TextView>(Resource.Id.EntryCellTitle));
			_Description = new DescriptionView(this, ContentView.FindViewById<TextView>(Resource.Id.EntryCellDescription));
			_Hint = new HintView(this, ContentView.FindViewById<TextView>(Resource.Id.EntryCellHintText));
			_Value = ContentView.FindViewById<AiEditText>(Resource.Id.EntryCellValue) ?? throw new NullReferenceException(nameof(_Value));
			_Value.Init(_EntryCell, this);

			Click += EntryCellView_Click;
			Click += EditTextOnClick;
			_EntryCell.Focused += EntryCell_Focused;
		}


		/*
		 
		protected override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( Update(sender, e) ) { return; }

			if ( _Title.Update(sender, e) ) { return; }

			if ( _Description.Update(sender, e) ) { return; }

			if ( _Hint.Update(sender, e) ) { return; }

			// if ( e.PropertyName == LabelCell.ValueTextFontSizeProperty.PropertyName ) { UpdateValueTextFontSize(); }
		}
		protected override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( UpdateParent(sender, e) ) { return; }

			if ( _Title.UpdateParent(sender, e) ) { return; }

			if ( _Description.UpdateParent(sender, e) ) { return; }

			if ( _Hint.UpdateParent(sender, e) ) { return; }
		}


		protected override void EnableCell()
		{
			base.EnableCell();
			_Title.Enable();
			_Description.Enable();
			_Hint.Enable();
			Enable();
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Title.Disable();
			_Description.Disable();
			_Hint.Disable();
			Disable();
		}

		 */

		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( _Value.Update(sender, e) ) { return; }

			if ( _Title.Update(sender, e) ) { return; }

			if ( _Description.Update(sender, e) ) { return; }

			if ( _Hint.Update(sender, e) ) { return; }
			
			// if ( e.PropertyName == LabelCell.ValueTextFontSizeProperty.PropertyName ) { UpdateValueTextFontSize(); }
		}
		protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Value.UpdateParent(sender, e) ) { return; }

			if ( _Title.UpdateParent(sender, e) ) { return; }

			if ( _Description.UpdateParent(sender, e) ) { return; }

			if ( _Hint.UpdateParent(sender, e) ) { return; }
		}


		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			_Value.Update();
		}

		bool TextView.IOnEditorActionListener.OnEditorAction( TextView? v, ImeAction actionId, KeyEvent? e )
		{
			if ( v is null ) return true;

			if ( actionId != ImeAction.Done &&
				 ( actionId != ImeAction.ImeNull || e != null && e.KeyCode != Keycode.Enter ) ) return true;
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

		protected void EditTextOnClick( object sender, EventArgs e ) { _Value.PerformSelectAction(); }
		protected void EntryCellView_Click( object sender, EventArgs e )
		{
			RequestFocus();
			_Value.PerformSelectAction();
			ShowKeyboard(_Value); // EntryCellView_Click
		}
		protected void EntryCell_Focused( object sender, EventArgs e )
		{
			RequestFocus();
			ShowKeyboard(_Value);
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
				 s.Length() == 0 ) { return; }

			_EntryCell.ValueText = s?.ToString();
		}
		void IOnFocusChangeListener.OnFocusChange( Android.Views.View? v, bool hasFocus )
		{
			if ( hasFocus )
			{
				//show underline when on focus.
				Background.Alpha = 100;
			}
			else
			{
				//hide underline
				Background.Alpha = 0;
				// consider as text inpute completed.
				_EntryCell.SendCompleted();
			}
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				Click -= EntryCellView_Click;
				_EntryCell.Focused -= EntryCell_Focused;
				_Value.RemoveFromParent();
				_Value.SetOnEditorActionListener(null);
				_Value.RemoveTextChangedListener(this);
				OnFocusChangeListener = null;
				_Value.ClearFocusAction = null;
				_Value.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}
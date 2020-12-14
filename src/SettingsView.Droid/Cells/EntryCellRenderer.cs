using System;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Jakar.SettingsView.Droid.Cells.Base;
using Jakar.SettingsView.Droid.Extensions;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AiEntryCell = Jakar.SettingsView.Shared.Cells.EntryCell;
using Color = Android.Graphics.Color;
using EntryCellRenderer = Jakar.SettingsView.Droid.Cells.EntryCellRenderer;

[assembly: ExportRenderer(typeof(AiEntryCell), typeof(EntryCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)]
	public class EntryCellRenderer : CellBaseRenderer<EntryCellView> { }

	[Preserve(AllMembers = true)]
	public class EntryCellView : CellBaseView, ITextWatcher, Android.Views.View.IOnFocusChangeListener, TextView.IOnEditorActionListener
	{
		protected AiEntryCell _EntryCell => Cell as AiEntryCell ?? throw new NullReferenceException(nameof(_EntryCell));
		protected AiEditText _EditText { get; set; }

		public EntryCellView( Context context, Cell cell ) : base(context, cell)
		{
			_EditText = new AiEditText(context)
						{
							Focusable = true,
							ImeOptions = ImeAction.Done,
							OnFocusChangeListener = this,
							Ellipsize = TextUtils.TruncateAt.End,
						};

			_EditText.SetOnEditorActionListener(this);

			_EditText.SetSingleLine(true);  // TODO: enable multi-line entries

			_EditText.InputType |= InputTypes.TextFlagNoSuggestions;            //disabled spell check
			if ( _EditText.Background != null ) _EditText.Background.Alpha = 0; //hide underline

			_EditText.ClearFocusAction = DoneEdit;

			Click += EntryCellView_Click;
			_EditText.Click += EditTextOnClick;
			_EntryCell.Focused += EntryCell_Focused;

			//remove weight and change width due to fill _EditText.
			if ( TitleLabel.LayoutParameters is LinearLayout.LayoutParams titleParam )
			{
				titleParam.Weight = 0;
				titleParam.Width = ViewGroup.LayoutParams.WrapContent;
			}

			using var layout_params = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1f);
			{
				ContentStack.AddView(_EditText, layout_params);
			}
		}
		public EntryCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }

		protected override void UpdateCell()
		{
			UpdateValueText();
			UpdateValueTextColor();
			UpdateValueTextFontSize();
			UpdateValueTextFont();
			UpdateKeyboard();
			UpdatePlaceholder();
			UpdateAccentColor();
			UpdateTextAlignment();
			UpdateIsPassword();
			base.UpdateCell();
		}

		protected override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == AiEntryCell.ValueTextProperty.PropertyName ) { UpdateValueText(); }
			else if ( e.PropertyName == AiEntryCell.ValueTextFontSizeProperty.PropertyName ) { UpdateWithForceLayout(UpdateValueTextFontSize); }
			else if ( e.PropertyName == AiEntryCell.ValueTextFontFamilyProperty.PropertyName || e.PropertyName == AiEntryCell.ValueTextFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateValueTextFont); }
			else if ( e.PropertyName == AiEntryCell.ValueTextColorProperty.PropertyName ) { UpdateWithForceLayout(UpdateValueTextColor); }
			else if ( e.PropertyName == AiEntryCell.KeyboardProperty.PropertyName ) { UpdateKeyboard(); }
			else if ( e.PropertyName == AiEntryCell.PlaceholderProperty.PropertyName || e.PropertyName == AiEntryCell.PlaceholderColorProperty.PropertyName ) { UpdatePlaceholder(); }
			else if ( e.PropertyName == AiEntryCell.AccentColorProperty.PropertyName ) { UpdateAccentColor(); }
			else if ( e.PropertyName == AiEntryCell.TextAlignmentProperty.PropertyName ) { UpdateTextAlignment(); }
			else if ( e.PropertyName == AiEntryCell.IsPasswordProperty.PropertyName ) { UpdateIsPassword(); }
			else if ( e.PropertyName == AiEntryCell.OnSelectActionProperty.PropertyName ) { UpdateSelectAction(); }
		}
		protected override void ParentPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);
			if ( e.PropertyName == Shared.SettingsView.CellValueTextColorProperty.PropertyName ) { UpdateValueTextColor(); }
			else if ( e.PropertyName == Shared.SettingsView.CellValueTextFontSizeProperty.PropertyName ) { UpdateWithForceLayout(UpdateValueTextFontSize); }
			else if ( e.PropertyName == Shared.SettingsView.CellValueTextFontFamilyProperty.PropertyName || e.PropertyName == Shared.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateValueTextFont); }
			else if ( e.PropertyName == Shared.SettingsView.CellAccentColorProperty.PropertyName ) { UpdateAccentColor(); }
		}




		protected void UpdateValueText()
		{
			_EditText.RemoveTextChangedListener(this);
			if ( _EditText.Text != _EntryCell.ValueText ) { _EditText.Text = _EntryCell.ValueText; }

			_EditText.AddTextChangedListener(this);
		}
		protected void UpdateValueTextFontSize()
		{
			if ( _EntryCell.ValueTextFontSize > 0 ) { _EditText.SetTextSize(Android.Util.ComplexUnitType.Sp, (float) _EntryCell.ValueTextFontSize); }
			else if ( CellParent != null ) { _EditText.SetTextSize(Android.Util.ComplexUnitType.Sp, (float) CellParent.CellValueTextFontSize); }
		}
		protected void UpdateValueTextFont()
		{
			string family = _EntryCell.ValueTextFontFamily ?? CellParent?.CellValueTextFontFamily;
			FontAttributes attr = _EntryCell.ValueTextFontAttributes ?? CellParent.CellValueTextFontAttributes;

			_EditText.Typeface = FontUtility.CreateTypeface(family, attr);
		}
		protected void UpdateValueTextColor()
		{
			if ( _EntryCell.ValueTextColor != Xamarin.Forms.Color.Default ) { _EditText.SetTextColor(_EntryCell.ValueTextColor.ToAndroid()); }
			else if ( CellParent != null && CellParent.CellValueTextColor != Xamarin.Forms.Color.Default ) { _EditText.SetTextColor(CellParent.CellValueTextColor.ToAndroid()); }
		}
		protected void UpdateKeyboard() { _EditText.InputType = _EntryCell.Keyboard.ToInputType() | InputTypes.TextFlagNoSuggestions; }
		protected void UpdateIsPassword() { _EditText.TransformationMethod = _EntryCell.IsPassword ? new PasswordTransformationMethod() : null; }
		protected void UpdatePlaceholder()
		{
			_EditText.Hint = _EntryCell.Placeholder;

			Color placeholderColor = _EntryCell.PlaceholderColor.IsDefault ? Color.Rgb(210, 210, 210) : _EntryCell.PlaceholderColor.ToAndroid();
			_EditText.SetHintTextColor(placeholderColor);
		}
		protected void UpdateTextAlignment() { _EditText.Gravity = _EntryCell.TextAlignment.ToGravityFlags(); }
		protected void UpdateAccentColor()
		{
			if ( _EntryCell.AccentColor != Xamarin.Forms.Color.Default ) { ChangeTextViewBack(_EntryCell.AccentColor.ToAndroid()); }
			else if ( CellParent != null && CellParent.CellAccentColor != Xamarin.Forms.Color.Default ) { ChangeTextViewBack(CellParent.CellAccentColor.ToAndroid()); }
		}

		protected void ChangeTextViewBack( Color accent )
		{
			var colorlist = new ColorStateList(new int[][]
											   {
												   new int[]
												   {
													   Android.Resource.Attribute.StateFocused
												   },
												   new int[]
												   {
													   -Android.Resource.Attribute.StateFocused
												   },
											   }, new int[]
												  {
													  Color.Argb(255, accent.R, accent.G, accent.B),
													  Color.Argb(255, 200, 200, 200)
												  });
			_EditText.Background.SetTintList(colorlist);
		}


		bool TextView.IOnEditorActionListener.OnEditorAction( TextView? v, ImeAction actionId, KeyEvent? e )
		{
			if ( actionId == ImeAction.Done || ( actionId == ImeAction.ImeNull && e.KeyCode == Keycode.Enter ) )
			{
				HideKeyboard(v);
				DoneEdit();
			}

			return true;
		}

		protected void DoneEdit()
		{
			var entryCell = (IEntryCellController) Cell;
			//entryCell.SendCompleted();
			_EditText.ClearFocus();
			ClearFocus();
		}

		protected void HideKeyboard( Android.Views.View inputView )
		{
			using var inputMethodManager = (InputMethodManager) AndroidContext.GetSystemService(Context.InputMethodService);
			IBinder windowToken = inputView.WindowToken;
			if ( windowToken != null ) { inputMethodManager?.HideSoftInputFromWindow(windowToken, HideSoftInputFlags.None); }
		}
		protected void ShowKeyboard( Android.Views.View inputView )
		{
			using var inputMethodManager = (InputMethodManager) AndroidContext.GetSystemService(Context.InputMethodService);
			if ( inputMethodManager is null ) return;
			inputMethodManager.ShowSoftInput(inputView, ShowFlags.Forced);
			inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
		}

		protected void EditTextOnClick( object sender, EventArgs e ) { PerformSelectAction(); }
		protected void EntryCellView_Click( object sender, EventArgs e )
		{
			_EditText.RequestFocus();
			PerformSelectAction();
			ShowKeyboard(_EditText); // EntryCellView_Click
		}
		protected void EntryCell_Focused( object sender, EventArgs e )
		{
			_EditText.RequestFocus();
			ShowKeyboard(_EditText);
		}

		void ITextWatcher.AfterTextChanged( IEditable s ) { }
		void ITextWatcher.BeforeTextChanged( ICharSequence s, int start, int count, int after ) { }
		void ITextWatcher.OnTextChanged( ICharSequence s, int start, int before, int count )
		{
			if ( string.IsNullOrEmpty(_EntryCell.ValueText) && s.Length() == 0 ) { return; }

			_EntryCell.ValueText = s?.ToString();
		}
		void IOnFocusChangeListener.OnFocusChange( Android.Views.View v, bool hasFocus )
		{
			if ( hasFocus )
			{
				//show underline when on focus.
				_EditText.Background.Alpha = 100;
			}
			else
			{
				//hide underline
				_EditText.Background.Alpha = 0;
				// consider as text inpute completed.
				_EntryCell.SendCompleted();
			}
		}


		protected void PerformSelectAction()
		{
			_EditText.Cell = _EntryCell;
			switch ( _EntryCell.OnSelectAction )
			{
				case AiEntryCell.SelectAction.None:
					break;


				case AiEntryCell.SelectAction.Start:
					//Selection.SetSelection(_EditText, 0);
					_EditText.SetSelection(0, 0);

					break;

				case AiEntryCell.SelectAction.End:
					//Selection.SetSelection(_EditText, length - 1);
					_EditText.SetSelection(_EditText.Length(), _EditText.Length());

					break;

				case AiEntryCell.SelectAction.All:
					//Selection.SetSelection(_EditText, 0, length);
					_EditText.SelectAll();

					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		protected void UpdateSelectAction()
		{
			// https://stackoverflow.com/questions/63305827/how-do-i-implement-android-text-ispannable-on-a-android-widget-edittext-inside-o
			// https://stackoverflow.com/questions/20838227/set-cursor-position-in-android-edit-text/20838295
			_EditText.Cell = _EntryCell;
			switch ( _EntryCell.OnSelectAction )
			{
				case AiEntryCell.SelectAction.None:
					break;

				case AiEntryCell.SelectAction.Start:
					break;

				case AiEntryCell.SelectAction.End:
					break;

				case AiEntryCell.SelectAction.All:
					_EditText.SetTextIsSelectable(true);
					_EditText.SetSelectAllOnFocus(true);

					break;

				default:
					throw new ArgumentOutOfRangeException();
			}

			//_EditText.MoveCursorToVisibleOffset();
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				Click -= EntryCellView_Click;
				_EntryCell.Focused -= EntryCell_Focused;
				_EditText.RemoveFromParent();
				_EditText.SetOnEditorActionListener(null);
				_EditText.RemoveTextChangedListener(this);
				_EditText.OnFocusChangeListener = null;
				_EditText.ClearFocusAction = null;
				_EditText.Dispose();
				_EditText = null;
			}

			base.Dispose(disposing);
		}
	}

	[Preserve(AllMembers = true)]
	internal class AiEditText : EditText
	{
		public Action ClearFocusAction { get; set; }
		internal AiEntryCell Cell { get; set; }

		public AiEditText( Context context ) : base(context) { }

		protected override void OnFocusChanged( bool gainFocus, [GeneratedEnum] FocusSearchDirection direction, Android.Graphics.Rect previouslyFocusedRect )
		{
			base.OnFocusChanged(gainFocus, direction, previouslyFocusedRect);
			if ( gainFocus ) { Post(new Runnable(() => { SetSelection(Text.Length); })); }
		}

		public override bool OnKeyPreIme( Keycode keyCode, KeyEvent e )
		{
			if ( keyCode != Keycode.Back || e.Action != KeyEventActions.Up ) return base.OnKeyPreIme(keyCode, e);

			ClearFocus();
			ClearFocusAction?.Invoke();

			return base.OnKeyPreIme(keyCode, e);
		}
	}
}
using System;
using System.ComponentModel;
using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Android.Text;
using Android.Text.Method;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Jakar.SettingsView.Droid.Extensions;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;
using EntryCell = Jakar.SettingsView.Shared.Cells.EntryCell;

namespace Jakar.SettingsView.Droid.Cells.Base
{
	[Preserve(AllMembers = true)]
	public class AiEditText : EditText
	{
		protected internal Color DefaultTextColor { get; }
		protected internal float DefaultFontSize { get; }
		public Action? ClearFocusAction { get; set; }
		protected EntryCellView _Cell { get; private set; }
		protected EntryCell _EntryCell { get; private set; }
		protected internal Shared.SettingsView CellParent => _EntryCell.Parent;


		public AiEditText( Context context ) : base(context)
		{
			DefaultFontSize = TextSize;
			DefaultTextColor = new Color(CurrentTextColor);
		}
		public AiEditText( Context context, EntryCell cell ) : this(context) => _EntryCell = cell;
		internal void Init( EntryCell cell, EntryCellView renderer )
		{
			_EntryCell = cell;
			Focusable = true;
			ImeOptions = ImeAction.Done;
			_Cell = renderer;
			OnFocusChangeListener = _Cell;
			SetOnEditorActionListener(_Cell);
			ClearFocusAction = _Cell.DoneEdit;

			Ellipsize = TextUtils.TruncateAt.End;
			SetSingleLine(true);                     // TODO: enable multi-line entries
			InputType = cell.Keyboard.ToInputType(); //disabled spell check
			if ( Background != null )
				Background.Alpha = 0; //hide underline
		}


		protected override void OnFocusChanged( bool gainFocus, [GeneratedEnum] FocusSearchDirection direction, Android.Graphics.Rect? previouslyFocusedRect )
		{
			base.OnFocusChanged(gainFocus, direction, previouslyFocusedRect);
			if ( gainFocus ) { Post(new Runnable(() => { SetSelection(Text?.Length ?? 0); })); }
		}

		public override bool OnKeyPreIme( Keycode keyCode, KeyEvent? e )
		{
			if ( keyCode != Keycode.Back ||
				 e.Action != KeyEventActions.Up ) return base.OnKeyPreIme(keyCode, e);

			ClearFocus();
			ClearFocusAction?.Invoke();

			return base.OnKeyPreIme(keyCode, e);
		}


		protected internal void PerformSelectAction()
		{
			switch ( _EntryCell.OnSelectAction )
			{
				case EntryCell.SelectAction.None:
					break;


				case EntryCell.SelectAction.Start:
					//Selection.SetSelection(_EditText, 0);
					SetSelection(0, 0);

					break;

				case EntryCell.SelectAction.End:
					//Selection.SetSelection(_EditText, length - 1);
					SetSelection(Length(), Length());

					break;

				case EntryCell.SelectAction.All:
					//Selection.SetSelection(_EditText, 0, length);
					SelectAll();

					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		protected internal bool UpdateSelectAction()
		{
			// https://stackoverflow.com/questions/63305827/how-do-i-implement-android-text-ispannable-on-a-android-widget-edittext-inside-o
			// https://stackoverflow.com/questions/20838227/set-cursor-position-in-android-edit-text/20838295
			switch ( _EntryCell.OnSelectAction )
			{
				case EntryCell.SelectAction.None:
				case EntryCell.SelectAction.Start:
				case EntryCell.SelectAction.End:
					break;

				case EntryCell.SelectAction.All:
					SetTextIsSelectable(true);
					SetSelectAllOnFocus(true);

					break;

				default:
					throw new ArgumentOutOfRangeException();
			}

			//MoveCursorToVisibleOffset();
			return true;
		}
		protected internal bool UpdateText()
		{
			RemoveTextChangedListener(_Cell);
			if ( Text != _EntryCell.ValueText ) { Text = _EntryCell.ValueText; }

			Visibility = string.IsNullOrEmpty(Text) ? ViewStates.Gone : ViewStates.Visible;
			AddTextChangedListener(_Cell);
			return true;
		}
		protected internal bool UpdateTextColor()
		{
			if ( _EntryCell.ValueTextColor != Xamarin.Forms.Color.Default ) { SetTextColor(_EntryCell.ValueTextColor.ToAndroid()); }
			else if ( CellParent.CellValueTextColor != Xamarin.Forms.Color.Default ) { SetTextColor(CellParent.CellValueTextColor.ToAndroid()); }
			else { SetTextColor(DefaultTextColor); }

			return true;
		}
		protected internal bool UpdateKeyboard()
		{
			InputType = _EntryCell.Keyboard.ToInputType();
			return true;
		}
		protected internal bool UpdateIsPassword()
		{
			TransformationMethod = _EntryCell.IsPassword ? new PasswordTransformationMethod() : null;
			return true;
		}
		protected internal bool UpdatePlaceholder()
		{
			Hint = _EntryCell.Placeholder;

			Color placeholderColor = _EntryCell.PlaceholderColor.IsDefault ? Color.Rgb(210, 210, 210) : _EntryCell.PlaceholderColor.ToAndroid();
			SetHintTextColor(placeholderColor);
			return true;
		}
		protected internal bool UpdateTextAlignment()
		{
			Gravity = _EntryCell.TextAlignment.ToGravityFlags();
			return true;
		}
		protected internal bool UpdateAccentColor()
		{
			if ( _EntryCell.AccentColor != Xamarin.Forms.Color.Default ) { return ChangeTextViewBack(_EntryCell.AccentColor.ToAndroid()); }

			if ( CellParent.CellAccentColor != Xamarin.Forms.Color.Default ) { return ChangeTextViewBack(CellParent.CellAccentColor.ToAndroid()); }

			return true;
		}

		protected internal bool ChangeTextViewBack( Color accent )
		{
			var colorList = new ColorStateList(new[]
											   {
												   new[]
												   {
													   Android.Resource.Attribute.StateFocused
												   },
												   new[]
												   {
													   -Android.Resource.Attribute.StateFocused
												   },
											   }, new int[]
												  {
													  Color.Argb(255, accent.R, accent.G, accent.B),
													  Color.Argb(255, 200, 200, 200)
												  });
			Background?.SetTintList(colorList);
			return true;
		}
		protected internal bool UpdateFontSize()
		{
			if ( _EntryCell.ValueTextFontSize > 0 ) { SetTextSize(ComplexUnitType.Sp, (float) _EntryCell.ValueTextFontSize); }
			else { SetTextSize(ComplexUnitType.Sp, (float) CellParent.CellValueTextFontSize); }

			return true;
		}
		protected internal bool UpdateFont()
		{
			string? family = _EntryCell.ValueTextFontFamily ?? CellParent.CellValueTextFontFamily;
			FontAttributes attr = _EntryCell.ValueTextFontAttributes ?? CellParent?.CellValueTextFontAttributes ?? FontAttributes.None;

			Typeface = FontUtility.CreateTypeface(family, attr);

			return true;
		}
		// protected void UpdateValueTextAlignment() { TextAlignment = CellBase.ValueTextTextAlignment; }

		protected internal bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == EntryCell.ValueTextProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == EntryCell.ValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == EntryCell.ValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == EntryCell.ValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == EntryCell.ValueTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == EntryCell.ValueTextFontSizeProperty.PropertyName ) { return _Cell.UpdateWithForceLayout(UpdateFontSize); }

			if ( e.PropertyName == EntryCell.ValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == EntryCell.ValueTextFontAttributesProperty.PropertyName ) { return _Cell.UpdateWithForceLayout(UpdateFont); }

			if ( e.PropertyName == EntryCell.ValueTextColorProperty.PropertyName ) { return _Cell.UpdateWithForceLayout(UpdateTextColor); }

			if ( e.PropertyName == EntryCell.KeyboardProperty.PropertyName ) { return UpdateKeyboard(); }

			if ( e.PropertyName == EntryCell.PlaceholderProperty.PropertyName ||
				 e.PropertyName == EntryCell.PlaceholderColorProperty.PropertyName ) { return UpdatePlaceholder(); }

			if ( e.PropertyName == EntryCell.AccentColorProperty.PropertyName ) { return UpdateAccentColor(); }

			if ( e.PropertyName == EntryCell.TextAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == EntryCell.IsPasswordProperty.PropertyName ) { return UpdateIsPassword(); }

			if ( e.PropertyName == EntryCell.OnSelectActionProperty.PropertyName ) { return UpdateSelectAction(); }

			return false;
		}
		protected internal bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.SettingsView.CellValueTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextFontSizeProperty.PropertyName ) { return _Cell.UpdateWithForceLayout(UpdateFontSize); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { return _Cell.UpdateWithForceLayout(UpdateFont); }

			if ( e.PropertyName == Shared.SettingsView.CellAccentColorProperty.PropertyName ) { return UpdateAccentColor(); }

			return false;
		}
		protected internal void Update()
		{
			UpdateText();
			UpdateText();
			UpdateTextColor();
			UpdateFontSize();
			UpdateFont();
			UpdateKeyboard();
			UpdatePlaceholder();
			UpdateAccentColor();
			UpdateTextAlignment();
			UpdateIsPassword();
			UpdateFontSize();
			UpdateFont();
		}
	}
}
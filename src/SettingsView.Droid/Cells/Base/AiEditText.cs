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
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Cells.Base;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AColor = Android.Graphics.Color;
using AiEntryCell = Jakar.SettingsView.Shared.Cells.EntryCell;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Base
{
	[Preserve(AllMembers = true)]
	public class AiEditText : EditText
	{
		protected internal AColor DefaultTextColor { get; }
		protected internal float DefaultFontSize { get; }
		public Action? ClearFocusAction { get; set; }
		protected BaseAiEntryCell _ViewCell { get; private set; }
		protected AiEntryCell _EntryCell { get; private set; }
		protected internal Shared.SettingsView CellParent => _EntryCell.Parent;


#pragma warning disable 8618
		public AiEditText( Context context ) : base(context)
		{
			DefaultFontSize = TextSize;
			DefaultTextColor = new AColor(CurrentTextColor);
		}
		public AiEditText( Context context, AiEntryCell cell ) : this(context) => _EntryCell = cell;
		public AiEditText( Context context, IAttributeSet attributes ) : base(context, attributes)
		{
			DefaultFontSize = TextSize;
			DefaultTextColor = new AColor(CurrentTextColor);
		}
#pragma warning restore 8618
		internal void Init( AiEntryCell cell, BaseAiEntryCell renderer )
		{
			_EntryCell = cell;
			Focusable = true;
			ImeOptions = ImeAction.Done;
			_ViewCell = renderer;
			OnFocusChangeListener = _ViewCell;
			SetOnEditorActionListener(_ViewCell);
			ClearFocusAction = _ViewCell.DoneEdit;

			Ellipsize = TextUtils.TruncateAt.End;
			SetSingleLine(true);                     // TODO: enable multi-line entries
			InputType = cell.Keyboard.ToInputType(); //disabled spell check
			if ( Background != null )
				Background.Alpha = 0; //hide underline
		}

		protected internal void Enable() { Alpha = CellBaseView.ENABLED_ALPHA; }
		protected internal void Disable() { Alpha = CellBaseView.DISABLED_ALPHA; }

		protected override void OnFocusChanged( bool gainFocus, [GeneratedEnum] FocusSearchDirection direction, Android.Graphics.Rect? previouslyFocusedRect )
		{
			base.OnFocusChanged(gainFocus, direction, previouslyFocusedRect);
			if ( gainFocus ) { Post(new Runnable(() => { SetSelection(Text?.Length ?? 0); })); }
		}

		public override bool OnKeyPreIme( Keycode keyCode, KeyEvent? e )
		{
			if ( keyCode != Keycode.Back ||
				 e != null && e.Action != KeyEventActions.Up ) return base.OnKeyPreIme(keyCode, e);

			ClearFocus();
			ClearFocusAction?.Invoke();

			return base.OnKeyPreIme(keyCode, e);
		}


		protected internal void PerformSelectAction()
		{
			switch ( _EntryCell.OnSelectAction )
			{
				case AiEntryCell.SelectAction.None:
					break;


				case AiEntryCell.SelectAction.Start:
					//Selection.SetSelection(_EditText, 0);
					SetSelection(0, 0);

					break;

				case AiEntryCell.SelectAction.End:
					//Selection.SetSelection(_EditText, length - 1);
					SetSelection(Length(), Length());

					break;

				case AiEntryCell.SelectAction.All:
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
				case AiEntryCell.SelectAction.None:
				case AiEntryCell.SelectAction.Start:
				case AiEntryCell.SelectAction.End:
					break;

				case AiEntryCell.SelectAction.All:
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
			RemoveTextChangedListener(_ViewCell);
			if ( Text != _EntryCell.ValueText ) { Text = _EntryCell.ValueText; }

			Visibility = string.IsNullOrEmpty(Text) ? ViewStates.Gone : ViewStates.Visible;
			AddTextChangedListener(_ViewCell);
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

			AColor placeholderColor = _EntryCell.PlaceholderColor.IsDefault ? AColor.Rgb(210, 210, 210) : _EntryCell.PlaceholderColor.ToAndroid();
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

		protected internal bool ChangeTextViewBack( AColor accent )
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
													  AColor.Argb(255, accent.R, accent.G, accent.B),
													  AColor.Argb(255, 200, 200, 200)
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
			if ( e.PropertyName == CellBaseValueText.ValueTextProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == CellBaseValueText.ValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == CellBaseValueText.ValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == CellBaseValueText.ValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == CellBaseValueText.ValueTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == CellBaseValueText.ValueTextFontSizeProperty.PropertyName ) { return _ViewCell.UpdateWithForceLayout(UpdateFontSize); }

			if ( e.PropertyName == CellBaseValueText.ValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == CellBaseValueText.ValueTextFontAttributesProperty.PropertyName ) { return _ViewCell.UpdateWithForceLayout(UpdateFont); }

			if ( e.PropertyName == CellBaseValueText.ValueTextColorProperty.PropertyName ) { return _ViewCell.UpdateWithForceLayout(UpdateTextColor); }

			if ( e.PropertyName == AiEntryCell.KeyboardProperty.PropertyName ) { return UpdateKeyboard(); }

			if ( e.PropertyName == AiEntryCell.PlaceholderProperty.PropertyName ||
				 e.PropertyName == AiEntryCell.PlaceholderColorProperty.PropertyName ) { return UpdatePlaceholder(); }

			if ( e.PropertyName == AiEntryCell.AccentColorProperty.PropertyName ) { return UpdateAccentColor(); }

			if ( e.PropertyName == AiEntryCell.TextAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == AiEntryCell.IsPasswordProperty.PropertyName ) { return UpdateIsPassword(); }

			if ( e.PropertyName == AiEntryCell.OnSelectActionProperty.PropertyName ) { return UpdateSelectAction(); }

			return false;
		}
		protected internal bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.SettingsView.CellValueTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextFontSizeProperty.PropertyName ) { return _ViewCell.UpdateWithForceLayout(UpdateFontSize); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { return _ViewCell.UpdateWithForceLayout(UpdateFont); }

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
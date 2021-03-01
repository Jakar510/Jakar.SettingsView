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
using Jakar.SettingsView.Droid.Cells.Base;
using Jakar.SettingsView.Droid.Extensions;
using Jakar.SettingsView.Shared.Cells.Base;
using Jakar.SettingsView.Shared.Interfaces;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AColor = Android.Graphics.Color;
using AiEntryCell = Jakar.SettingsView.Shared.Cells.EntryCell;
using BaseCellView = Jakar.SettingsView.Droid.Cells.Base.BaseCellView;
using AContext = Android.Content.Context;
using Switch = Android.Widget.Switch;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Controls
{
	[Preserve(AllMembers = true)]
	public class AiEditText : EditText, IUpdateEntryCell<BaseAiEntryCell, AColor, AiEntryCell>
	{
		public AColor DefaultTextColor { get; }
		public float DefaultFontSize { get; }
		protected BaseAiEntryCell _CellRenderer { get; private set; }
		protected AiEntryCell _EntryCell { get; private set; }
		public Shared.SettingsView CellParent => _EntryCell.Parent;


		public AiEditText( AContext context ) : base(context)
		{
			DefaultFontSize = TextSize;
			DefaultTextColor = new AColor(CurrentTextColor);
			Init();
		}
		public AiEditText( AiEntryCell cell, AContext context ) : this(context) => _EntryCell = cell;
		public AiEditText( BaseCellView cell, AContext context ) : this(context) => _EntryCell = cell.Cell as AiEntryCell ?? throw new NullReferenceException(nameof(cell.Cell));
		public AiEditText( AContext context, IAttributeSet attributes ) : base(context, attributes)
		{
			DefaultFontSize = TextSize;
			DefaultTextColor = new AColor(CurrentTextColor);
			Init();
		}


		public void SetMaxWidth( int width, double factor ) => SetMaxWidth((int) ( width * factor ));
		public void Init()
		{
			Gravity = GravityFlags.Fill;
			
			Ellipsize = null; // TextUtils.TruncateAt.End;
			SetSingleLine(false);
			SetMinLines(1);
			SetMaxLines(10);
			BreakStrategy = BreakStrategy.Balanced;

			Focusable = true;
			ImeOptions = ImeAction.Done;
			if ( Background != null ) { Background.Alpha = 0; } // hide underline
		}
		public void Init( AiEntryCell cell, BaseAiEntryCell renderer )
		{
			SetCell(cell);
			_CellRenderer = renderer;
			OnFocusChangeListener = _CellRenderer;
			SetOnEditorActionListener(_CellRenderer);
			
		}
		public void SetCell( AiEntryCell cell )
		{
			_EntryCell = cell;
			InputType = _EntryCell.Keyboard.ToInputType();
		}

		public static AiEditText Create( Android.Views.View view, AiEntryCell cell, int id )
		{
			AiEditText result = view.FindViewById<AiEditText>(id) ?? throw new NullReferenceException(nameof(id));
			result.SetCell(cell);
			return result;
		}

		public void Enable() { Alpha = BaseCellView.ENABLED_ALPHA; }
		public void Disable() { Alpha = BaseCellView.DISABLED_ALPHA; }

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
			_CellRenderer.DoneEdit();

			return base.OnKeyPreIme(keyCode, e);
		}


		public void PerformSelectAction()
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
		public bool UpdateSelectAction()
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
		public bool UpdateText()
		{
			RemoveTextChangedListener(_CellRenderer);
			if ( Text != _EntryCell.ValueText ) { Text = _EntryCell.ValueText; }

			Visibility = string.IsNullOrEmpty(Text) ? ViewStates.Gone : ViewStates.Visible;
			AddTextChangedListener(_CellRenderer);
			return true;
		}
		public bool UpdateColor() => throw new NotImplementedException();
		public bool UpdateTextColor()
		{
			if ( _EntryCell.ValueTextColor != Xamarin.Forms.Color.Default ) { SetTextColor(_EntryCell.ValueTextColor.ToAndroid()); }
			else if ( CellParent.CellValueTextColor != Xamarin.Forms.Color.Default ) { SetTextColor(CellParent.CellValueTextColor.ToAndroid()); }
			else { SetTextColor(DefaultTextColor); }

			return true;
		}
		public bool UpdateKeyboard()
		{
			InputType = _EntryCell.Keyboard.ToInputType();
			return true;
		}
		public bool UpdateIsPassword()
		{
			TransformationMethod = _EntryCell.IsPassword ? new PasswordTransformationMethod() : null;
			return true;
		}
		public bool UpdatePlaceholder()
		{
			Hint = _EntryCell.Placeholder;

			AColor placeholderColor = _EntryCell.PlaceholderColor.IsDefault ? AColor.Rgb(210, 210, 210) : _EntryCell.PlaceholderColor.ToAndroid();
			SetHintTextColor(placeholderColor);
			return true;
		}
		public bool UpdateTextAlignment()
		{
			Gravity = _EntryCell.TextAlignment.ToGravityFlags();
			return true;
		}
		public bool UpdateAccentColor()
		{
			if ( _EntryCell.AccentColor != Xamarin.Forms.Color.Default ) { return ChangeTextViewBack(_EntryCell.AccentColor.ToAndroid()); }

			if ( CellParent.CellAccentColor != Xamarin.Forms.Color.Default ) { return ChangeTextViewBack(CellParent.CellAccentColor.ToAndroid()); }

			return true;
		}

		public bool ChangeTextViewBack( AColor accent )
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
											   },
											   new int[]
											   {
												   AColor.Argb(255, accent.R, accent.G, accent.B),
												   AColor.Argb(255, 200, 200, 200)
											   }
											  );
			Background?.SetTintList(colorList);
			return true;
		}
		public bool UpdateFontSize()
		{
			if ( _EntryCell.ValueTextFontSize > 0 ) { SetTextSize(ComplexUnitType.Sp, (float) _EntryCell.ValueTextFontSize); }
			else { SetTextSize(ComplexUnitType.Sp, (float) CellParent.CellValueTextFontSize); }

			return true;
		}
		public bool UpdateFont()
		{
			string? family = _EntryCell.ValueTextFontFamily ?? CellParent.CellValueTextFontFamily;
			FontAttributes attr = _EntryCell.ValueTextFontAttributes ?? CellParent?.CellValueTextFontAttributes ?? FontAttributes.None;

			Typeface = FontUtility.CreateTypeface(family, attr);

			return true;
		}

		// protected void UpdateValueTextAlignment() { TextAlignment = CellBase.ValueTextTextAlignment; }

		public bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CellBaseValueText.ValueTextProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == CellBaseValueText.ValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == CellBaseValueText.ValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == CellBaseValueText.ValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == CellBaseValueText.ValueTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == CellBaseValueText.ValueTextFontSizeProperty.PropertyName ) { return _CellRenderer.UpdateWithForceLayout(UpdateFontSize); }

			if ( e.PropertyName == CellBaseValueText.ValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == CellBaseValueText.ValueTextFontAttributesProperty.PropertyName ) { return _CellRenderer.UpdateWithForceLayout(UpdateFont); }

			if ( e.PropertyName == CellBaseValueText.ValueTextColorProperty.PropertyName ) { return _CellRenderer.UpdateWithForceLayout(UpdateTextColor); }

			if ( e.PropertyName == AiEntryCell.KeyboardProperty.PropertyName ) { return UpdateKeyboard(); }

			if ( e.PropertyName == AiEntryCell.PlaceholderProperty.PropertyName ||
				 e.PropertyName == AiEntryCell.PlaceholderColorProperty.PropertyName ) { return UpdatePlaceholder(); }

			if ( e.PropertyName == AiEntryCell.AccentColorProperty.PropertyName ) { return UpdateAccentColor(); }

			if ( e.PropertyName == AiEntryCell.TextAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == AiEntryCell.IsPasswordProperty.PropertyName ) { return UpdateIsPassword(); }

			if ( e.PropertyName == AiEntryCell.OnSelectActionProperty.PropertyName ) { return UpdateSelectAction(); }

			return false;
		}
		public bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.SettingsView.CellValueTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextFontSizeProperty.PropertyName ) { return _CellRenderer.UpdateWithForceLayout(UpdateFontSize); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { return _CellRenderer.UpdateWithForceLayout(UpdateFont); }

			if ( e.PropertyName == Shared.SettingsView.CellAccentColorProperty.PropertyName ) { return UpdateAccentColor(); }

			return false;
		}
		public void Update()
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
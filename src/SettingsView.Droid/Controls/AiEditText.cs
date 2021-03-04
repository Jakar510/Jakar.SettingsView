using System;
using System.ComponentModel;
using Android.Content.Res;
using Android.Runtime;
using Android.Text;
using Android.Text.Method;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Jakar.SettingsView.Droid.Extensions;
using Jakar.SettingsView.Droid.Interfaces;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Interfaces;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AColor = Android.Graphics.Color;
using AiEntryCell = Jakar.SettingsView.Shared.Cells.EntryCell;
using BaseCellView = Jakar.SettingsView.Droid.BaseCell.BaseCellView;
using AContext = Android.Content.Context;
using TextAlignment = Xamarin.Forms.TextAlignment;
using TextChangedEventArgs = Android.Text.TextChangedEventArgs;

#nullable enable
namespace Jakar.SettingsView.Droid.Controls
{
	[Preserve(AllMembers = true)]
	public class AiEditText : EditText, IUpdateEntryCell<IEntryCellRenderer, AColor, AiEntryCell>
	{
		public AColor DefaultTextColor { get; }
		public float DefaultFontSize { get; }
		protected IEntryCellRenderer _CellRenderer { get; private set; }
		protected AiEntryCell _CurrentCell { get; private set; }
		public Shared.sv.SettingsView CellParent => _CurrentCell.Parent;


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public AiEditText( AContext context ) : base(context)
		{
			DefaultFontSize = TextSize;
			DefaultTextColor = new AColor(CurrentTextColor);
			Init();
			TextChanged += OnTextChanged;
		}
		public AiEditText( AiEntryCell cell, AContext context ) : this(context) => _CurrentCell = cell;
		public AiEditText( BaseCellView cell, AContext context ) : this(context) => _CurrentCell = cell.Cell as AiEntryCell ?? throw new NullReferenceException(nameof(cell.Cell));
		public AiEditText( AContext context, IAttributeSet attributes ) : base(context, attributes)
		{
			DefaultFontSize = TextSize;
			DefaultTextColor = new AColor(CurrentTextColor);
			Init();
		}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public static AiEditText Create( Android.Views.View view, AiEntryCell cell, int id )
		{
			AiEditText result = view.FindViewById<AiEditText>(id) ?? throw new NullReferenceException(nameof(id));
			result.SetCell(cell);
			return result;
		}
		protected override void Dispose( bool disposing )
		{
			base.Dispose(disposing);
			TextChanged -= OnTextChanged;
			SetOnEditorActionListener(null);
			RemoveTextChangedListener(_CellRenderer);
		}

		private void OnTextChanged( object sender, TextChangedEventArgs e )
		{
			string s = e.Text is null ? string.Empty : string.Concat(e.Text);
			_CurrentCell.SendTextChanged(Text ?? string.Empty, s);
		}
		public void Init()
		{
			Ellipsize = null; // TextUtils.TruncateAt.End;
			SetSingleLine(false);
			SetMinLines(1);
			SetMaxLines(10);
			BreakStrategy = BreakStrategy.Balanced;

			Focusable = true;
			ImeOptions = ImeAction.Done;
			if ( Background != null ) { Background.Alpha = 0; } // hide underline
		}
		public void Init( AiEntryCell cell, IEntryCellRenderer renderer )
		{
			SetCell(cell);
			_CellRenderer = renderer;
			OnFocusChangeListener = _CellRenderer;
			SetOnEditorActionListener(_CellRenderer);
		}
		public void SetCell( AiEntryCell cell )
		{
			_CurrentCell = cell;
			InputType = _CurrentCell.Keyboard.ToInputType();
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
			switch ( _CurrentCell.OnSelectAction )
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
			switch ( _CurrentCell.OnSelectAction )
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

		// public bool UpdateProportions() => _CellRenderer.UpdateProportions();
		public bool UpdateText()
		{
			RemoveTextChangedListener(_CellRenderer);
			if ( Text != _CurrentCell.ValueText ) { Text = _CurrentCell.ValueText; }

			Visibility = string.IsNullOrEmpty(Text) ? ViewStates.Gone : ViewStates.Visible;
			AddTextChangedListener(_CellRenderer);
			return true;
		}
		public bool UpdateColor() => throw new NotImplementedException();
		public bool UpdateTextColor()
		{
			if ( _CurrentCell.ValueTextColor != Xamarin.Forms.Color.Default ) { SetTextColor(_CurrentCell.ValueTextColor.ToAndroid()); }
			else if ( CellParent.CellValueTextColor != Xamarin.Forms.Color.Default ) { SetTextColor(CellParent.CellValueTextColor.ToAndroid()); }
			else { SetTextColor(DefaultTextColor); }

			return true;
		}
		public bool UpdateKeyboard()
		{
			InputType = _CurrentCell.Keyboard.ToInputType();
			return true;
		}
		public bool UpdateIsPassword()
		{
			TransformationMethod = _CurrentCell.IsPassword ? new PasswordTransformationMethod() : null;
			return true;
		}
		public bool UpdatePlaceholder()
		{
			Hint = _CurrentCell.Placeholder;

			AColor placeholderColor = _CurrentCell.PlaceholderColor.IsDefault ? AColor.Rgb(210, 210, 210) : _CurrentCell.PlaceholderColor.ToAndroid();
			SetHintTextColor(placeholderColor);
			return true;
		}
		public bool UpdateTextAlignment()
		{
			TextAlignment alignment = _CurrentCell.ValueTextAlignment ?? _CurrentCell.Parent.CellValueTextAlignment;
			TextAlignment = alignment.ToAndroidTextAlignment();
			Gravity = alignment.ToGravityFlags();

			return true;
		}
		public bool UpdateAccentColor()
		{
			if ( _CurrentCell.AccentColor != Xamarin.Forms.Color.Default ) { return ChangeTextViewBack(_CurrentCell.AccentColor.ToAndroid()); }

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
			if ( _CurrentCell.ValueTextFontSize > 0 ) { SetTextSize(ComplexUnitType.Sp, (float) _CurrentCell.ValueTextFontSize); }
			else { SetTextSize(ComplexUnitType.Sp, (float) CellParent.CellValueTextFontSize); }

			return true;
		}
		public bool UpdateFont()
		{
			string? family = _CurrentCell.ValueTextFontFamily ?? CellParent.CellValueTextFontFamily;
			FontAttributes attr = _CurrentCell.ValueTextFontAttributes ?? CellParent?.CellValueTextFontAttributes ?? FontAttributes.None;

			Typeface = FontUtility.CreateTypeface(family, attr);

			return true;
		}

		// protected void UpdateValueTextAlignment() { TextAlignment = CellBase.ValueTextTextAlignment; }

		public bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == ValueTextCellBase.ValueTextProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == ValueCellBase.ValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == ValueCellBase.ValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == ValueCellBase.ValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == ValueCellBase.ValueTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == ValueCellBase.ValueTextFontSizeProperty.PropertyName ) { return _CellRenderer.UpdateWithForceLayout(UpdateFontSize); }

			if ( e.PropertyName == ValueCellBase.ValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == ValueCellBase.ValueTextFontAttributesProperty.PropertyName ) { return _CellRenderer.UpdateWithForceLayout(UpdateFont); }

			if ( e.PropertyName == ValueCellBase.ValueTextColorProperty.PropertyName ) { return _CellRenderer.UpdateWithForceLayout(UpdateTextColor); }

			if ( e.PropertyName == ValueCellBase.ValueTextAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			
			if ( e.PropertyName == AiEntryCell.KeyboardProperty.PropertyName ) { return UpdateKeyboard(); }

			if ( e.PropertyName == AiEntryCell.PlaceholderProperty.PropertyName ||
				 e.PropertyName == AiEntryCell.PlaceholderColorProperty.PropertyName ) { return UpdatePlaceholder(); }

			if ( e.PropertyName == AiEntryCell.AccentColorProperty.PropertyName ) { return UpdateAccentColor(); }

			if ( e.PropertyName == AiEntryCell.IsPasswordProperty.PropertyName ) { return UpdateIsPassword(); }

			if ( e.PropertyName == AiEntryCell.OnSelectActionProperty.PropertyName ) { return UpdateSelectAction(); }

			return false;
		}
		public bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.sv.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontSizeProperty.PropertyName ) { return _CellRenderer.UpdateWithForceLayout(UpdateFontSize); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.sv.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { return _CellRenderer.UpdateWithForceLayout(UpdateFont); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellAccentColorProperty.PropertyName ) { return UpdateAccentColor(); }

			return false;
		}
		public void Update()
		{
			UpdateText();
			UpdateTextColor();
			UpdateFontSize();
			UpdateFont();
			UpdateKeyboard();
			UpdatePlaceholder();
			UpdateAccentColor();
			UpdateTextAlignment();
			UpdateIsPassword();
		}
	}
}
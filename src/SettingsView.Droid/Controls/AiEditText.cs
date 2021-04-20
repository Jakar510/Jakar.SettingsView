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
using Jakar.Api.Droid.Extensions;
using Jakar.SettingsView.Droid.Interfaces;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Enumerations;
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
	public class AiEditText : EditText, IUpdateEntryCell<AColor>, IUpdateCell<AiEntryCell>, IDefaultColors<AColor>, ISetCell<AiEntryCell, IEntryCellRenderer>
	{
		public AColor                 DefaultTextColor       { get; }
		public AColor                 DefaultBackgroundColor { get; }
		public float                  DefaultFontSize        { get; }
		public IEntryCellRenderer     CellRenderer           { get; private set; }
		public AiEntryCell            Cell                   { get; private set; }
		public Shared.sv.SettingsView CellParent             => Cell.Parent;


		#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public AiEditText( AContext context ) : base(context)
		{
			DefaultFontSize        = TextSize;
			DefaultTextColor       = new AColor(CurrentTextColor);
			DefaultBackgroundColor = Color.Default.ToAndroid();
			Initialize();
			TextChanged += OnTextChanged;
		}

		public AiEditText( AiEntryCell  cell, AContext context ) : this(context) => Cell = cell;
		public AiEditText( BaseCellView cell, AContext context ) : this(context) => Cell = cell.Cell as AiEntryCell ?? throw new NullReferenceException(nameof(cell.Cell));
		public AiEditText( AContext context, IAttributeSet attributes ) : base(context, attributes)
		{
			DefaultFontSize  = TextSize;
			DefaultTextColor = new AColor(CurrentTextColor);
			Initialize();
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
			RemoveTextChangedListener(CellRenderer);
		}

		private void OnTextChanged( object sender, TextChangedEventArgs e )
		{
			string s = e.Text is null
						   ? string.Empty
						   : string.Concat(e.Text);

			Cell.SendTextChanged(Text ?? string.Empty, s);
		}

		public void Initialize()
		{
			SetSingleLine(false);
			Ellipsize = null; // TextUtils.TruncateAt.End;
			SetMinLines(1);
			SetMaxLines(10);
			BreakStrategy = BreakStrategy.Balanced;

			Focusable  = true;
			ImeOptions = ImeAction.Done;
			SetBackgroundColor(Color.Transparent.ToAndroid());
			if ( Background != null ) { Background.Alpha = 0; } // hide underline
		}

		public void Init( AiEntryCell cell, IEntryCellRenderer renderer )
		{
			SetCell(cell);
			CellRenderer         = renderer;
			OnFocusChangeListener = CellRenderer;
			SetOnEditorActionListener(CellRenderer);
		}

		public void SetCell( AiEntryCell cell )
		{
			Cell      = cell;
			InputType = Cell.Keyboard.ToInputType();
		}


		public void Enable() { Alpha  = SvConstants.Cell.ENABLED_ALPHA; }
		public void Disable() { Alpha = SvConstants.Cell.DISABLED_ALPHA; }

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
			CellRenderer.DoneEdit();

			return base.OnKeyPreIme(keyCode, e);
		}


		public void PerformSelectAction()
		{
			switch ( Cell.OnSelectAction )
			{
				case SelectAction.None:
					break;


				case SelectAction.Start:
					//Selection.SetSelection(_EditText, 0);
					SetSelection(0, 0);

					break;

				case SelectAction.End:
					//Selection.SetSelection(_EditText, length - 1);
					SetSelection(Length(), Length());

					break;

				case SelectAction.All:
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
			switch ( Cell.OnSelectAction )
			{
				case SelectAction.None:
				case SelectAction.Start:
				case SelectAction.End:
					break;

				case SelectAction.All:
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
			RemoveTextChangedListener(CellRenderer);

			if ( Text != Cell.ValueText )
			{
				if ( string.IsNullOrWhiteSpace(Text) ) { Text = Cell.ValueText; }
				else { Cell.ValueText                         = Text; }
			}

			// Visibility = string.IsNullOrEmpty(Text) ? ViewStates.Invisible : ViewStates.Visible;
			AddTextChangedListener(CellRenderer);
			return true;
		}

		public bool UpdateFontSize()
		{
			SetTextSize(ComplexUnitType.Sp, (float) Cell.ValueTextConfig.FontSize);

			// SetTextSize(ComplexUnitType.Sp, DefaultFontSize);

			return true;
		}

		public bool UpdateTextColor()
		{
			SetTextColor(Cell.ValueTextConfig.Color.ToAndroid());

			// SetTextColor(DefaultTextColor);

			return true;
		}

		public bool UpdateFont()
		{
			string?        family = Cell.HintConfig.FontFamily;
			FontAttributes attr   = Cell.ValueTextConfig.FontAttributes;

			Typeface = FontUtility.CreateTypeface(family, attr);

			return true;
		}

		public bool UpdateTextAlignment()
		{
			TextAlignment alignment = Cell.ValueTextConfig.TextAlignment;
			TextAlignment = alignment.ToAndroidTextAlignment();
			Gravity       = alignment.ToGravityFlags();

			return true;
		}

		public bool UpdateKeyboard()
		{
			InputType = Cell.Keyboard.ToInputType();
			return true;
		}

		public bool UpdateIsPassword()
		{
			TransformationMethod = Cell.IsPassword
									   ? new PasswordTransformationMethod()
									   : null;

			return true;
		}

		public bool UpdatePlaceholder()
		{
			Hint = Cell.Placeholder;

			AColor placeholderColor = Cell.GetPlaceholderColor().ToAndroid();
			SetHintTextColor(placeholderColor);
			return true;
		}

		public bool UpdateAccentColor() => ChangeTextViewBack(Cell.GetAccentColor().ToAndroid());

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
											   new[]
											   {
												   accent.ToArgb(),
												   CellParent.SelectedColor.ToAndroid() // AColor.Argb(255, 200, 200, 200)
											   }
											  );

			Background?.SetTintList(colorList);
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

			if ( e.PropertyName == ValueCellBase.ValueTextFontSizeProperty.PropertyName ) { return CellRenderer.UpdateWithForceLayout(UpdateFontSize); }

			if ( e.PropertyName == ValueCellBase.ValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == ValueCellBase.ValueTextFontAttributesProperty.PropertyName ) { return CellRenderer.UpdateWithForceLayout(UpdateFont); }

			if ( e.PropertyName == ValueCellBase.ValueTextColorProperty.PropertyName ) { return CellRenderer.UpdateWithForceLayout(UpdateTextColor); }

			if ( e.PropertyName == ValueCellBase.ValueTextAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }


			if ( e.PropertyName == AiEntryCell.KeyboardProperty.PropertyName ) { return UpdateKeyboard(); }

			if ( e.PropertyName == AiEntryCell.PlaceholderProperty.PropertyName ||
				 e.PropertyName == AiEntryCell.PlaceholderColorProperty.PropertyName ) { return UpdatePlaceholder(); }

			if ( e.PropertyName == AiEntryCell.AccentColorProperty.PropertyName ) { return UpdateAccentColor(); }

			if ( e.PropertyName == AiEntryCell.IsPasswordProperty.PropertyName ) { return UpdateIsPassword(); }

			if ( e.PropertyName == AiEntryCell.OnSelectActionProperty.PropertyName ) { return UpdateSelectAction(); }

			// if ( e.PropertyName == CellBase.BackgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }

			return false;
		}

		public bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.sv.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontSizeProperty.PropertyName ) { return CellRenderer.UpdateWithForceLayout(UpdateFontSize); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.sv.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { return CellRenderer.UpdateWithForceLayout(UpdateFont); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellAccentColorProperty.PropertyName ) { return UpdateAccentColor(); }

			// if ( e.PropertyName == Shared.sv.SettingsView.CellBackgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }

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

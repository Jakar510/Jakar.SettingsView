using System;
using System.ComponentModel;
using Foundation;
using Jakar.SettingsView.iOS.Extensions;
using Jakar.SettingsView.iOS.Interfaces;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Enumerations;
using Jakar.SettingsView.Shared.Interfaces;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.PlatformConfiguration;
using AiEntryCell = Jakar.SettingsView.Shared.Cells.EntryCell;
using BaseCellView = Jakar.SettingsView.iOS.BaseCell.BaseCellView;
using TextAlignment = Xamarin.Forms.TextAlignment;

#nullable enable
namespace Jakar.SettingsView.iOS.Controls
{
	[Preserve(AllMembers = true)]
	public class AiEditText : UITextField, IUpdateEntryCell<IEntryCellRenderer, Color, AiEntryCell>
	{
		protected bool _HasFocus { get; set; }

		public Color DefaultTextColor { get; }
		public float DefaultFontSize { get; }
		protected IEntryCellRenderer _CellRenderer { get; private set; }
		protected AiEntryCell _CurrentCell { get; private set; }
		public Shared.sv.SettingsView CellParent => _CurrentCell.Parent;


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public AiEditText() : base()
		{
			// DefaultFontSize = TextSize;
			// DefaultTextColor = new AColor(CurrentTextColor);
			Initialize();
			TouchUpInside += ValueFieldOnTouchUpInside;
			EditingChanged += TextField_EditingChanged;
			EditingDidBegin += ValueField_EditingDidBegin;
			EditingDidEnd += ValueField_EditingDidEnd;
			ShouldReturn = OnShouldReturn;
		}
		public AiEditText( AiEntryCell cell ) : this()
		{
			_CurrentCell = cell;
			_CurrentCell.Focused += EntryCell_Focused;
		}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		// public static AiEditText Create( View view, AiEntryCell cell, int id )
		// {
		// 	AiEditText result = view.FindViewById<AiEditText>(id) ?? throw new NullReferenceException(nameof(id));
		// 	result.SetCell(cell);
		// 	return result;
		// }
		protected override void Dispose( bool disposing )
		{
			base.Dispose(disposing);
			TouchUpInside -= ValueFieldOnTouchUpInside;
			EditingChanged -= TextField_EditingChanged;
			EditingDidBegin -= ValueField_EditingDidBegin;
			EditingDidEnd -= ValueField_EditingDidEnd;
			_CurrentCell.Focused -= EntryCell_Focused;
			ShouldReturn = null;
		}

		public void SetEnabledAppearance( bool isEnabled )
		{
			Alpha = isEnabled
						? SVConstants.Cell.ENABLED_ALPHA
						: SVConstants.Cell.DISABLED_ALPHA;
		}
		public void Initialize()
		{
			SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Horizontal);
			SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);

			BorderStyle = UITextBorderStyle.None;
			AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			ReturnKeyType = UIReturnKeyType.Done;

			// SetSingleLine(false);
			// Ellipsize = null; // TextUtils.TruncateAt.End;
			// SetMinLines(1);
			// SetMaxLines(10);
			// BreakStrategy = BreakStrategy.Balanced;
			//
			// Focusable = true;
			// ImeOptions = ImeAction.Done;
			// SetBackgroundColor(Color.Transparent.ToUIColor());
			// if ( Background != null ) { Background.Alpha = 0; } // hide underline
		}
		public void Init( AiEntryCell cell, IEntryCellRenderer renderer )
		{
			SetCell(cell);
			_CellRenderer = renderer;
			// OnFocusChangeListener = _CellRenderer; 
			// SetOnEditorActionListener(_CellRenderer);
		}
		public void SetCell( AiEntryCell cell )
		{
			_CurrentCell = cell;
			// InputType = _CurrentCell.Keyboard.ToInputType();
		}


		public void Enable() { Alpha = SVConstants.Cell.ENABLED_ALPHA; }
		public void Disable() { Alpha = SVConstants.Cell.DISABLED_ALPHA; }

		// protected override void OnFocusChanged( bool gainFocus, [GeneratedEnum] FocusSearchDirection direction, Android.Graphics.Rect? previouslyFocusedRect )
		// {
		// 	base.OnFocusChanged(gainFocus, direction, previouslyFocusedRect);
		// 	if ( gainFocus ) { Post(new Runnable(() => { SetSelection(Text?.Length ?? 0); })); }
		// }
		// public override bool OnKeyPreIme( Keycode keyCode, KeyEvent? e )
		// {
		// 	if ( keyCode != Keycode.Back ||
		// 		 e != null && e.Action != KeyEventActions.Up ) return base.OnKeyPreIme(keyCode, e);
		//
		// 	ClearFocus();
		// 	_CellRenderer.DoneEdit();
		//
		// 	return base.OnKeyPreIme(keyCode, e);
		// }


		public void PerformSelectAction()
		{
			// https://stackoverflow.com/questions/34922331/getting-and-setting-cursor-position-of-uitextfield-and-uitextview-in-swift

			UITextPosition start = BeginningOfDocument;
			UITextPosition end = EndOfDocument;
			switch ( _CurrentCell.OnSelectAction )
			{
				case SelectAction.None:
					break;

				case SelectAction.Start:
					SelectedTextRange = GetTextRange(start, start);
					Select(this);

					break;

				case SelectAction.End:
					SelectedTextRange = GetTextRange(end, end);
					Select(this);

					break;

				case SelectAction.All:
					SelectedTextRange = GetTextRange(start, end);
					Select(this);
					//SelectAll(ValueField);

					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		public bool UpdateSelectAction()
		{
			// https://stackoverflow.com/questions/34922331/getting-and-setting-cursor-position-of-uitextfield-and-uitextview-in-swift

			ClearsOnBeginEditing = _CurrentCell.OnSelectAction switch
								   {
									   SelectAction.None => false,
									   SelectAction.Start => false,
									   SelectAction.End => false,
									   SelectAction.All => true,
									   _ => throw new ArgumentOutOfRangeException()
								   };
			return true;
		}

		public bool UpdateText() => UpdateText(_CurrentCell.ValueText);
		public bool UpdateText( string? text )
		{
			Text = text;
			Hidden = string.IsNullOrEmpty(Text);

			return true;
		}
		public bool UpdateFontSize()
		{
			ContentScaleFactor = _CurrentCell.ValueTextConfig.FontSize.ToNFloat();

			return true;
		}
		public bool UpdateTextColor()
		{
			TextColor = _CurrentCell.ValueTextConfig.Color.ToUIColor();

			return true;
		}
		public bool UpdateFont()
		{
			string? family = _CurrentCell.ValueTextConfig.FontFamily;
			FontAttributes attr = _CurrentCell.ValueTextConfig.FontAttributes;
			var size = (float) _CurrentCell.ValueTextConfig.FontSize;

			Font = FontUtility.CreateNativeFont(family, size, attr);

			return UpdatePlaceholder();
		}
		public bool UpdateTextAlignment()
		{
			TextAlignment alignment = _CurrentCell.ValueTextConfig.TextAlignment;
			TextAlignment = alignment.ToUITextAlignment();

			return true;
		}

		public bool UpdateKeyboard()
		{
			this.ApplyKeyboard(_CurrentCell.Keyboard);
			return true;
		}
		public bool UpdateIsPassword()
		{
			// https://stackoverflow.com/a/6578848/9530917
			SecureTextEntry = _CurrentCell.IsPassword;
			return true;
		}
		public bool UpdatePlaceholder()
		{
			// https://stackoverflow.com/a/23610570/9530917
			UIColor placeholderColor = _CurrentCell.GetPlaceholderColor().ToUIColor();
			AttributedPlaceholder = new NSAttributedString(_CurrentCell.Placeholder, Font, placeholderColor);
			return true;
		}
		public bool UpdateAccentColor() =>
			ChangeTextViewBack(_CurrentCell.GetAccentColor());
		public bool ChangeTextViewBack( Color accent )
		{
			TintColor = accent.ToUIColor();
			return true;
		}
		protected void ValueFieldOnTouchUpInside( object sender, EventArgs e ) { PerformSelectAction(); }
		protected void TextField_EditingChanged( object sender, EventArgs e ) { _CurrentCell.ValueText = Text; }
		protected void ValueField_EditingDidBegin( object sender, EventArgs e ) { _HasFocus = true; }

		protected void ValueField_EditingDidEnd( object sender, EventArgs e )
		{
			if ( !_HasFocus ) { return; }

			ResignFirstResponder();
			_CurrentCell.SendCompleted();
			_HasFocus = false;
		}
		protected void EntryCell_Focused( object sender, EventArgs e ) { BecomeFirstResponder(); }
		protected bool OnShouldReturn( UITextField view )
		{
			_HasFocus = false;
			ResignFirstResponder();
			_CurrentCell.SendCompleted();

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

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontSizeProperty.PropertyName ) { return _CellRenderer.UpdateWithForceLayout(UpdateFontSize); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.sv.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { return _CellRenderer.UpdateWithForceLayout(UpdateFont); }

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
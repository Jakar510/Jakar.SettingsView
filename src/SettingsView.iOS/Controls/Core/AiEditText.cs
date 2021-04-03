using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using Jakar.Api.Extensions;
using Jakar.Api.iOS.Extensions;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Controls.Manager;
using Jakar.SettingsView.iOS.Interfaces;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Enumerations;
using Jakar.SettingsView.Shared.Interfaces;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using AiEntryCell = Jakar.SettingsView.Shared.Cells.EntryCell;
using TextAlignment = Xamarin.Forms.TextAlignment;

#nullable enable
namespace Jakar.SettingsView.iOS.Controls.Core
{
	[Preserve(AllMembers = true)]
	public class AiEditText : BaseViewManager<UITextField, AiEntryCell>, IUpdateEntryCell<Color>, IInitializeControl, IRenderValue
	{
		protected bool _HasFocus { get; set; }

		private readonly EntryCellView _renderer;
		protected IEntryCellRenderer _CellRenderer => _renderer;
		protected override IUseConfiguration _Config => _Cell.ValueTextConfig;


		public AiEditText( EntryCellView renderer ) : this(new UITextField(), renderer) { }
		public AiEditText( UITextField control, EntryCellView renderer ) : base(control,
																				renderer.EntryCell,
																				control.TextColor,
																				control.BackgroundColor,
																				control.MinimumFontSize
																			   )
		{
			_renderer = renderer;
			_Cell.Focused += EntryCell_Focused;

			Initialize();
			Control.TouchUpInside += ValueFieldOnTouchUpInside;
			Control.EditingChanged += TextField_EditingChanged;
			Control.EditingDidBegin += ValueField_EditingDidBegin;
			Control.EditingDidEnd += ValueField_EditingDidEnd;
			Control.ShouldReturn = OnShouldReturn;
		}


		public override void Initialize( Stack parent )
		{
			parent.BringSubviewToFront(Control);
			Control.UpdateConstraintsIfNeeded();
			Control.LayoutIfNeeded();
		}


		public void SetEnabledAppearance( bool isEnabled )
		{
			Control.Alpha = isEnabled
							   ? SvConstants.Cell.ENABLED_ALPHA
							   : SvConstants.Cell.DISABLED_ALPHA;
		}
		public override void Initialize()
		{
			Control.SetContentHuggingPriority(SvConstants.Layout.Priority.Minimum, UILayoutConstraintAxis.Horizontal);
			Control.SetContentCompressionResistancePriority(SvConstants.Layout.Priority.Highest, UILayoutConstraintAxis.Horizontal);

			Control.BorderStyle = UITextBorderStyle.None;
			Control.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			Control.ReturnKeyType = UIReturnKeyType.Done;

			Control.BackgroundColor = UIColor.Clear;

			// Control.SetSingleLine(false);
			// Control.Ellipsize = null; // TextUtils.TruncateAt.End;
			// Control.SetMinLines(1);
			// Control.SetMaxLines(10);
			// Control.BreakStrategy = BreakStrategy.Balanced;
			//
			// Control.Focusable = true;
			// Control.ImeOptions = ImeAction.Done;
			// Control.SetBackgroundColor(Color.Transparent.ToUIColor());
			// if ( Control.Background != null ) { Control.Background.Alpha = 0; } // hide underline
		}


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

			UITextPosition start = Control.BeginningOfDocument;
			UITextPosition end = Control.EndOfDocument;
			switch ( _Cell.OnSelectAction )
			{
				case SelectAction.None:
					break;

				case SelectAction.Start:
					Control.SelectedTextRange = Control.GetTextRange(start, start);
					Control.Select(Control);

					break;

				case SelectAction.End:
					Control.SelectedTextRange = Control.GetTextRange(end, end);
					Control.Select(Control);

					break;

				case SelectAction.All:
					Control.SelectedTextRange = Control.GetTextRange(start, end);
					Control.Select(Control);
					//SelectAll(ValueField);

					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		public bool UpdateSelectAction()
		{
			// https://stackoverflow.com/questions/34922331/getting-and-setting-cursor-position-of-uitextfield-and-uitextview-in-swift
			if ( _Cell is null ) throw new NullReferenceException(nameof(_Cell));

			Control.ClearsOnBeginEditing = _Cell.OnSelectAction switch
										   {
											   SelectAction.None => false,
											   SelectAction.Start => false,
											   SelectAction.End => false,
											   SelectAction.All => true,
											   _ => throw new ArgumentOutOfRangeException()
										   };
			return true;
		}

		public override bool UpdateText() => UpdateText(_Cell.ValueText);
		public override bool UpdateText( string? text )
		{
			Control.Text = text;
			Control.Hidden = string.IsNullOrEmpty(Control.Text);

			return true;
		}
		public override bool UpdateFontSize()
		{
			if ( _Cell is null ) throw new NullReferenceException(nameof(_Cell));
			Control.ContentScaleFactor = _Cell.ValueTextConfig.FontSize.ToNFloat();

			return true;
		}
		public override bool UpdateTextColor()
		{
			if ( _Cell is null ) throw new NullReferenceException(nameof(_Cell));
			Control.TextColor = _Cell.ValueTextConfig.Color.ToUIColor();

			return true;
		}
		public override bool UpdateFont()
		{
			if ( _Cell is null ) throw new NullReferenceException(nameof(_Cell));
			string? family = _Cell.ValueTextConfig.FontFamily;
			FontAttributes attr = _Cell.ValueTextConfig.FontAttributes;
			var size = (float) _Cell.ValueTextConfig.FontSize;

			Control.Font = FontUtility.CreateNativeFont(family, size, attr);

			// make the view height fit font size
			nfloat contentH = Control.IntrinsicContentSize.Height;
			CGRect bounds = Control.Bounds;
			Control.Bounds = new CGRect(0, 0, bounds.Width, contentH);
			return UpdatePlaceholder();
		}
		public override bool UpdateTextAlignment()
		{
			if ( _Cell is null ) throw new NullReferenceException(nameof(_Cell));
			TextAlignment alignment = _Cell.ValueTextConfig.TextAlignment;
			Control.TextAlignment = alignment.ToUITextAlignment();

			return true;
		}

		public bool UpdateKeyboard()
		{
			if ( _Cell is null ) throw new NullReferenceException(nameof(_Cell));
			Control.ApplyKeyboard(_Cell.Keyboard);
			return true;
		}
		public bool UpdateIsPassword()
		{
			// https://stackoverflow.com/a/6578848/9530917
			if ( _Cell is null ) throw new NullReferenceException(nameof(_Cell));
			Control.SecureTextEntry = _Cell.IsPassword;
			return true;
		}
		public bool UpdatePlaceholder()
		{
			// https://stackoverflow.com/a/23610570/9530917
			if ( _Cell is null ) throw new NullReferenceException(nameof(_Cell));
			UIColor placeholderColor = _Cell.GetPlaceholderColor().ToUIColor();
			Control.AttributedPlaceholder = new NSAttributedString(_Cell.Placeholder ?? string.Empty, Control.Font, placeholderColor);
			return true;
		}


		public bool UpdateAccentColor() => ChangeTextViewBack(_Cell.GetAccentColor());
		public bool ChangeTextViewBack( Color accent )
		{
			Control.TintColor = accent.ToUIColor();
			return true;
		}


		protected void ValueFieldOnTouchUpInside( object sender, EventArgs e ) { PerformSelectAction(); }
		protected void TextField_EditingChanged( object sender, EventArgs e ) { _Cell.ValueText = Control.Text; }
		protected void ValueField_EditingDidBegin( object sender, EventArgs e ) { _HasFocus = true; }
		protected void ValueField_EditingDidEnd( object sender, EventArgs e )
		{
			if ( !_HasFocus ) { return; }

			if ( _Cell is null ) throw new NullReferenceException(nameof(_Cell));

			Control.ResignFirstResponder();
			_CellRenderer.DoneEdit();
			_HasFocus = false;
		}
		protected void EntryCell_Focused( object sender, EventArgs e ) { Control.BecomeFirstResponder(); }
		protected bool OnShouldReturn( UITextField view )
		{
			if ( _Cell is null ) throw new NullReferenceException(nameof(_Cell));
			_HasFocus = false;
			Control.ResignFirstResponder();
			_Cell.SendCompleted();

			return true;
		}

		// protected void UpdateValueTextAlignment() { TextAlignment = CellBase.ValueTextTextAlignment; }

		public override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(ValueTextCellBase.ValueTextProperty) ) { return UpdateText(); }

			if ( e.IsEqual(ValueCellBase.ValueTextFontSizeProperty) ) { return UpdateFontSize(); }

			if ( e.IsOneOf(ValueCellBase.ValueTextFontFamilyProperty, ValueCellBase.ValueTextFontAttributesProperty) ) { return UpdateFont(); }

			if ( e.IsEqual(ValueCellBase.ValueTextFontSizeProperty) ) { return _CellRenderer.UpdateWithForceLayout(UpdateFontSize); }

			if ( e.IsOneOf(ValueCellBase.ValueTextFontFamilyProperty, ValueCellBase.ValueTextFontAttributesProperty) ) { return _CellRenderer.UpdateWithForceLayout(UpdateFont); }

			if ( e.IsEqual(ValueCellBase.ValueTextColorProperty) ) { return _CellRenderer.UpdateWithForceLayout(UpdateTextColor); }

			if ( e.IsEqual(ValueCellBase.ValueTextAlignmentProperty) ) { return UpdateTextAlignment(); }


			if ( e.IsEqual(AiEntryCell.KeyboardProperty) ) { return UpdateKeyboard(); }

			if ( e.IsOneOf(AiEntryCell.PlaceholderProperty, AiEntryCell.PlaceholderColorProperty) ) { return UpdatePlaceholder(); }

			if ( e.IsEqual(AiEntryCell.AccentColorProperty) ) { return _CellRenderer.UpdateWithForceLayout(UpdateAccentColor); }

			if ( e.IsEqual(AiEntryCell.IsPasswordProperty) ) { return UpdateIsPassword(); }

			if ( e.IsEqual(AiEntryCell.OnSelectActionProperty) ) { return UpdateSelectAction(); }

			// if ( e.IsEqual(CellBase.BackgroundColorProperty) ) { UpdateBackgroundColor(); }

			return false;
		}
		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(Shared.sv.SettingsView.CellValueTextColorProperty) ) { return UpdateTextColor(); }

			if ( e.IsEqual(Shared.sv.SettingsView.CellValueTextFontSizeProperty) ) { return UpdateFontSize(); }

			if ( e.IsOneOf(Shared.sv.SettingsView.CellValueTextFontFamilyProperty, Shared.sv.SettingsView.CellValueTextFontAttributesProperty) ) { return UpdateFont(); }

			if ( e.IsEqual(Shared.sv.SettingsView.CellValueTextColorProperty) ) { return UpdateTextColor(); }

			if ( e.IsEqual(Shared.sv.SettingsView.CellValueTextFontSizeProperty) ) { return _CellRenderer.UpdateWithForceLayout(UpdateFontSize); }

			if ( e.IsOneOf(Shared.sv.SettingsView.CellValueTextFontFamilyProperty, Shared.sv.SettingsView.CellValueTextFontAttributesProperty) ) { return _CellRenderer.UpdateWithForceLayout(UpdateFont); }

			if ( e.IsEqual(Shared.sv.SettingsView.CellAccentColorProperty) ) { return UpdateAccentColor(); }

			// if ( e.PropertyName == Shared.sv.SettingsView.CellBackgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }

			return false;
		}
		public override void Update()
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


		protected override void Dispose( bool disposing )
		{
			base.Dispose(disposing);

			Control.TouchUpInside -= ValueFieldOnTouchUpInside;
			Control.EditingChanged -= TextField_EditingChanged;
			Control.EditingDidBegin -= ValueField_EditingDidBegin;
			Control.EditingDidEnd -= ValueField_EditingDidEnd;
			_Cell.Focused -= EntryCell_Focused;
			Control.ShouldReturn = null;
		}
	}
}
using System;
using CoreGraphics;
using Foundation;
using Jakar.SettingsView.iOS.Extensions;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using AiEntryCell = Jakar.SettingsView.Shared.Cells.EntryCell;
using EntryCellRenderer = Jakar.SettingsView.iOS.Cells.EntryCellRenderer;

[assembly: ExportRenderer(typeof(AiEntryCell), typeof(EntryCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)]
	public class EntryCellRenderer : CellBaseRenderer<EntryCellView> { }

	[Preserve(AllMembers = true)]
	public class EntryCellView : CellBaseView
	{
		private AiEntryCell _EntryCell => Cell as AiEntryCell;
		internal UITextField ValueField { get; set; }
		private UIView _FieldWrapper { get; set; }
		private bool _HasFocus { get; set; }

		public EntryCellView( Cell formsCell ) : base(formsCell)
		{
			ValueField = new UITextField // TODO: enable multi-line entries
			{
							 BorderStyle = UITextBorderStyle.None,
							 AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight,
							 ReturnKeyType = UIReturnKeyType.Done,
						 };

			ValueField.TouchUpInside += ValueFieldOnTouchUpInside;
			ValueField.EditingChanged += TextField_EditingChanged;
			ValueField.EditingDidBegin += ValueField_EditingDidBegin;
			ValueField.EditingDidEnd += ValueField_EditingDidEnd;
			ValueField.ShouldReturn = OnShouldReturn;

			_EntryCell.Focused += EntryCell_Focused;


			_FieldWrapper = new UIView
							{
								AutosizesSubviews = true
							};
			_FieldWrapper.SetContentHuggingPriority(100f, UILayoutConstraintAxis.Horizontal);
			_FieldWrapper.SetContentCompressionResistancePriority(100f, UILayoutConstraintAxis.Horizontal);

			_FieldWrapper.AddSubview(ValueField);
			ContentStack.AddArrangedSubview(_FieldWrapper);
		}

		public override void UpdateCell( UITableView tableView = null )
		{
			base.UpdateCell(tableView);

			if ( ValueField is null ) ; // For HotReload

			UpdateValueText();
			UpdateValueTextColor();
			UpdateValueTextFont();
			UpdatePlaceholder();
			UpdateKeyboard();
			UpdateIsPassword();
			UpdateTextAlignment();
		}

		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == AiEntryCell.ValueTextProperty.PropertyName ) { UpdateValueText(); }
			else if ( e.PropertyName == AiEntryCell.ValueTextFontSizeProperty.PropertyName || e.PropertyName == AiEntryCell.ValueTextFontFamilyProperty.PropertyName || e.PropertyName == AiEntryCell.ValueTextFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateValueTextFont); }
			else if ( e.PropertyName == AiEntryCell.ValueTextColorProperty.PropertyName ) { UpdateValueTextColor(); }
			else if ( e.PropertyName == AiEntryCell.KeyboardProperty.PropertyName ) { UpdateKeyboard(); }
			else if ( e.PropertyName == AiEntryCell.PlaceholderProperty.PropertyName || e.PropertyName == AiEntryCell.PlaceholderColorProperty.PropertyName ) { UpdatePlaceholder(); }
			else if ( e.PropertyName == AiEntryCell.TextAlignmentProperty.PropertyName ) { UpdateTextAlignment(); }
			else if ( e.PropertyName == AiEntryCell.IsPasswordProperty.PropertyName ) { UpdateIsPassword(); }
			else if ( e.PropertyName == AiEntryCell.OnSelectActionProperty.PropertyName ) { UpdateSelectAction(); }
		}

		public override void ParentPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);
			if ( e.PropertyName == Shared.SettingsView.CellValueTextColorProperty.PropertyName )
			{
				UpdateValueTextColor();
				ValueField.SetNeedsLayout(); // immediately reflect
			}
			else if ( e.PropertyName == Shared.SettingsView.CellValueTextFontSizeProperty.PropertyName || e.PropertyName == Shared.SettingsView.CellValueTextFontFamilyProperty.PropertyName || e.PropertyName == Shared.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateValueTextFont); }
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath ) { ValueField.BecomeFirstResponder(); }

		protected override void SetEnabledAppearance( bool isEnabled )
		{
			ValueField.Alpha = isEnabled ? 1.0f : 0.3f;

			base.SetEnabledAppearance(isEnabled);
		}


		private void UpdateValueText()
		{
			//Without this judging, TextField don't correctly work when inputting Japanese (maybe other 2byte languages either).
			if ( ValueField.Text != _EntryCell.ValueText ) { ValueField.Text = _EntryCell.ValueText; }
		}
		private void UpdateValueTextFont()
		{
			string family = _EntryCell.ValueTextFontFamily ?? CellParent.CellValueTextFontFamily;
			FontAttributes attr = _EntryCell.ValueTextFontAttributes ?? CellParent.CellValueTextFontAttributes;

			if ( _EntryCell.ValueTextFontSize > 0 ) { ValueField.Font = FontUtility.CreateNativeFont(family, (float) _EntryCell.ValueTextFontSize, attr); }
			else if ( CellParent != null ) { ValueField.Font = FontUtility.CreateNativeFont(family, (float) CellParent.CellValueTextFontSize, attr); }

			//make the view height fit font size
			nfloat contentH = ValueField.IntrinsicContentSize.Height;
			CGRect bounds = ValueField.Bounds;
			ValueField.Bounds = new CGRect(0, 0, bounds.Width, contentH);

			_FieldWrapper.Bounds = new CGRect(0, 0, _FieldWrapper.Bounds.Width, contentH);
		}
		private void UpdateValueTextColor()
		{
			if ( _EntryCell.ValueTextColor != Color.Default ) { ValueField.TextColor = _EntryCell.ValueTextColor.ToUIColor(); }
			else if ( CellParent != null && CellParent.CellValueTextColor != Color.Default ) { ValueField.TextColor = CellParent.CellValueTextColor.ToUIColor(); }

			ValueField.SetNeedsLayout();
		}
		private void UpdateKeyboard() { ValueField.ApplyKeyboard(_EntryCell.Keyboard); }
		private void UpdatePlaceholder()
		{
			if ( !_EntryCell.PlaceholderColor.IsDefault )
			{
				ValueField.Placeholder = null;
				ValueField.AttributedPlaceholder = new NSAttributedString(_EntryCell.Placeholder, ValueField.Font, _EntryCell.PlaceholderColor.ToUIColor());
			}
			else
			{
				ValueField.AttributedPlaceholder = null;
				ValueField.Placeholder = _EntryCell.Placeholder;
			}
		}
		private void UpdateTextAlignment()
		{
			ValueField.TextAlignment = _EntryCell.TextAlignment.ToUITextAlignment();
			ValueField.SetNeedsLayout();
		}
		private void UpdateIsPassword() { ValueField.SecureTextEntry = _EntryCell.IsPassword; }


		private void ValueFieldOnTouchUpInside( object sender, EventArgs e ) { PerformSelectAction(); }
		private void TextField_EditingChanged( object sender, EventArgs e ) { _EntryCell.ValueText = ValueField.Text; }
		private void ValueField_EditingDidBegin( object sender, EventArgs e ) { _HasFocus = true; }
		private void ValueField_EditingDidEnd( object sender, EventArgs e )
		{
			if ( !_HasFocus ) { return; }

			ValueField.ResignFirstResponder();
			_EntryCell.SendCompleted();
			_HasFocus = false;
		}
		private void EntryCell_Focused( object sender, EventArgs e ) { ValueField.BecomeFirstResponder(); }
		private bool OnShouldReturn( UITextField view )
		{
			_HasFocus = false;
			ValueField.ResignFirstResponder();
			_EntryCell.SendCompleted();

			return true;
		}
		protected void PerformSelectAction()
		{
			// https://stackoverflow.com/questions/34922331/getting-and-setting-cursor-position-of-uitextfield-and-uitextview-in-swift

			UITextPosition start = ValueField.BeginningOfDocument;
			UITextPosition end = ValueField.EndOfDocument;
			switch ( _EntryCell.OnSelectAction )
			{
				case AiEntryCell.SelectAction.None:
					break;

				case AiEntryCell.SelectAction.Start:
					ValueField.SelectedTextRange = ValueField.GetTextRange(start, start);
					ValueField.Select(ValueField);

					break;

				case AiEntryCell.SelectAction.End:
					ValueField.SelectedTextRange = ValueField.GetTextRange(end, end);
					ValueField.Select(ValueField);

					break;

				case AiEntryCell.SelectAction.All:
					ValueField.SelectedTextRange = ValueField.GetTextRange(start, end);
					ValueField.Select(ValueField);
					//ValueField.SelectAll(ValueField);

					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		protected void UpdateSelectAction()
		{
			// https://stackoverflow.com/questions/34922331/getting-and-setting-cursor-position-of-uitextfield-and-uitextview-in-swift

			ValueField.ClearsOnBeginEditing = _EntryCell.OnSelectAction switch
											  {
												  AiEntryCell.SelectAction.None => false,
												  AiEntryCell.SelectAction.Start => false,
												  AiEntryCell.SelectAction.End => false,
												  AiEntryCell.SelectAction.All => true,
												  _ => throw new ArgumentOutOfRangeException()
											  };
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				ValueField.TouchUpInside -= ValueFieldOnTouchUpInside;
				ValueField.EditingChanged -= TextField_EditingChanged;
				ValueField.EditingDidBegin -= ValueField_EditingDidBegin;
				ValueField.EditingDidEnd -= ValueField_EditingDidEnd;
				_EntryCell.Focused -= EntryCell_Focused;
				ValueField.ShouldReturn = null;
				ValueField.RemoveFromSuperview();
				ValueField.Dispose();
				ValueField = null;

				ContentStack.RemoveArrangedSubview(_FieldWrapper);
				_FieldWrapper.Dispose();
				_FieldWrapper = null;
			}

			base.Dispose(disposing);
		}
	}
}
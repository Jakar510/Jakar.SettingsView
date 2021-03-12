using System;
using System.Windows.Input;
using CoreGraphics;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Cells.Sources;
using Jakar.SettingsView.iOS.OLD_Cells;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Config;
using UIKit;
using Xamarin.Forms;

// [assembly: ExportRenderer(typeof(NumberPickerCell), typeof(NumberPickerCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.OLD_Cells
{
	// [Foundation.Preserve(AllMembers = true)] public class NumberPickerCellRenderer : CellBaseRenderer<NumberPickerCellView> { }

	[Foundation.Preserve(AllMembers = true)]
	public class NumberPickerCellView : LabelCellView
	{
		public UITextField DummyField { get; set; }

		protected NumberPickerSource _Model { get; set; }
		protected UILabel _PopupTitle { get; set; }
		protected UIPickerView _Dialog { get; set; }
		protected ICommand? _Command { get; set; }

		protected NumberPickerCell _NumberPickerCell => Cell as NumberPickerCell ?? throw new NullReferenceException(nameof(_NumberPickerCell));


		public NumberPickerCellView( Cell formsCell ) : base(formsCell)
		{
			DummyField = new NoCaretField
						 {
							 BorderStyle = UITextBorderStyle.None,
							 BackgroundColor = UIColor.Clear
						 };
			ContentView.AddSubview(DummyField);
			ContentView.SendSubviewToBack(DummyField);

			SelectionStyle = UITableViewCellSelectionStyle.Default;


			_Dialog = new UIPickerView();

			nfloat width = UIScreen.MainScreen.Bounds.Width;

			_PopupTitle = new UILabel
					 {
						 TextAlignment = UITextAlignment.Center
					 };

			var toolbar = new UIToolbar(new CGRect(0, 0, (float) width, 44))
						  {
							  BarStyle = UIBarStyle.Default,
							  Translucent = true
						  };
			var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel,
												   ( o, e ) =>
												   {
													   DummyField.ResignFirstResponder();
													   Select(_Model.PreSelectedItem);
												   }
												  );

			var labelButton = new UIBarButtonItem(_PopupTitle);
			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done,
												 ( o, a ) =>
												 {
													 _Model.OnUpdatePickerFormModel();
													 DummyField.ResignFirstResponder();
													 _Command?.Execute(_Model.SelectedItem);
												 }
												);

			toolbar.SetItems(new[]
							 {
								 cancelButton,
								 spacer,
								 labelButton,
								 spacer,
								 doneButton
							 },
							 false
							);

			DummyField.InputView = _Dialog;
			DummyField.InputAccessoryView = toolbar;

			_Model = new NumberPickerSource();
			_Dialog.Model = _Model;

			_Model.UpdatePickerFromModel += Model_UpdatePickerFromModel;
		}

		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == NumberPickerCell.MinProperty.PropertyName ||
				 e.PropertyName == NumberPickerCell.MaxProperty.PropertyName ) { UpdateNumberList(); }
			else if ( e.PropertyName == NumberPickerCell.NumberProperty.PropertyName ) { UpdateNumber(); }
			else if ( e.PropertyName == PopupConfig.TitleProperty.PropertyName ) { UpdatePopupTitle(); }
			else if ( e.PropertyName == NumberPickerCell.SelectedCommandProperty.PropertyName ) { UpdateCommand(); }
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			tableView.DeselectRow(indexPath, true);
			DummyField.BecomeFirstResponder();
		}

		public override void UpdateCell( UITableView tableView )
		{
			base.UpdateCell(tableView);

			UpdateNumberList();
			UpdateNumber();
			UpdatePopupTitle();
			UpdateCommand();
		}

		protected void UpdateNumber()
		{
			Select(_NumberPickerCell.Number);
			ValueLabel.Text = _NumberPickerCell.Number.ToString();
		}
		protected void UpdateNumberList()
		{
			_Model.SetNumbers(_NumberPickerCell.Min, _NumberPickerCell.Max);
			Select(_NumberPickerCell.Number);
		}
		protected void UpdatePopupTitle()
		{
			_PopupTitle.Text = _NumberPickerCell.Title;
			_PopupTitle.SizeToFit();
			_PopupTitle.Frame = new CGRect(0, 0, 160, 44);
		}

		protected void UpdateCommand() { _Command = _NumberPickerCell.SelectedCommand; }
		protected void Model_UpdatePickerFromModel( object sender, EventArgs e )
		{
			_NumberPickerCell.Number = _Model.SelectedItem;
			ValueLabel.Text = _Model.SelectedItem.ToString();
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			DummyField.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
		}

		protected void Select( int number )
		{
			int idx = _Model.Items.IndexOf(number);
			if ( idx == -1 )
			{
				number = _Model.Items[0];
				idx = 0;
			}

			_Dialog.Select(idx, 0, false);
			_Model.SelectedItem = number;
			_Model.SelectedIndex = idx;
			_Model.PreSelectedItem = number;
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_Model.UpdatePickerFromModel -= Model_UpdatePickerFromModel;
				DummyField.RemoveFromSuperview();
				DummyField?.Dispose();
				_PopupTitle?.Dispose();
				_Model?.Dispose();
				_Dialog?.Dispose();
				_Command = null;
			}

			base.Dispose(disposing);
		}
	}
}
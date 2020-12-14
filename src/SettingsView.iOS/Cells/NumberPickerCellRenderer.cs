using System;
using System.Windows.Input;
using CoreGraphics;
using Foundation;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.iOS.Cells;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(NumberPickerCell), typeof(NumberPickerCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)] public class NumberPickerCellRenderer : CellBaseRenderer<NumberPickerCellView> { }

	[Preserve(AllMembers = true)]
	public class NumberPickerCellView : LabelCellView
	{
		public UITextField DummyField { get; set; }

		private NumberPickerSource _Model { get; set; }
		private UILabel _Title { get; set; }
		private UIPickerView _Picker { get; set; }
		private ICommand? _Command { get; set; }

		private NumberPickerCell _NumberPickerCell => Cell as NumberPickerCell ?? throw new NullReferenceException(nameof(_NumberPickerCell));


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


			_Picker = new UIPickerView();

			nfloat width = UIScreen.MainScreen.Bounds.Width;

			_Title = new UILabel
					 {
						 TextAlignment = UITextAlignment.Center
					 };

			var toolbar = new UIToolbar(new CGRect(0, 0, (float) width, 44))
						  {
							  BarStyle = UIBarStyle.Default,
							  Translucent = true
						  };
			var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, ( o, e ) =>
																				 {
																					 DummyField.ResignFirstResponder();
																					 Select(_Model.PreSelectedItem);
																				 });

			var labelButton = new UIBarButtonItem(_Title);
			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, ( o, a ) =>
																			 {
																				 _Model.OnUpdatePickerFormModel();
																				 DummyField.ResignFirstResponder();
																				 _Command?.Execute(_Model.SelectedItem);
																			 });

			toolbar.SetItems(new[]
							 {
								 cancelButton,
								 spacer,
								 labelButton,
								 spacer,
								 doneButton
							 }, false);

			DummyField.InputView = _Picker;
			DummyField.InputAccessoryView = toolbar;

			_Model = new NumberPickerSource();
			_Picker.Model = _Model;

			_Model.UpdatePickerFromModel += Model_UpdatePickerFromModel;
		}

		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == NumberPickerCell.MinProperty.PropertyName ||
				 e.PropertyName == NumberPickerCell.MaxProperty.PropertyName ) { UpdateNumberList(); }
			else if ( e.PropertyName == NumberPickerCell.NumberProperty.PropertyName ) { UpdateNumber(); }
			else if ( e.PropertyName == NumberPickerCell.PickerTitleProperty.PropertyName ) { UpdateTitle(); }
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
			if ( DummyField is null )
				return; // For HotReload

			UpdateNumberList();
			UpdateNumber();
			UpdateTitle();
			UpdateCommand();
		}

		private void UpdateNumber()
		{
			Select(_NumberPickerCell.Number);
			ValueLabel.Text = _NumberPickerCell.Number.ToString();
		}
		private void UpdateNumberList()
		{
			_Model.SetNumbers(_NumberPickerCell.Min, _NumberPickerCell.Max);
			Select(_NumberPickerCell.Number);
		}
		private void UpdateTitle()
		{
			_Title.Text = _NumberPickerCell.PickerTitle;
			_Title.SizeToFit();
			_Title.Frame = new CGRect(0, 0, 160, 44);
		}

		private void UpdateCommand() { _Command = _NumberPickerCell.SelectedCommand; }
		private void Model_UpdatePickerFromModel( object sender, EventArgs e )
		{
			_NumberPickerCell.Number = _Model.SelectedItem;
			ValueLabel.Text = _Model.SelectedItem.ToString();
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			if ( DummyField is null )
				return; // For HotReload

			DummyField.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
		}

		private void Select( int number )
		{
			int idx = _Model.Items.IndexOf(number);
			if ( idx == -1 )
			{
				number = _Model.Items[0];
				idx = 0;
			}

			_Picker.Select(idx, 0, false);
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
				_Title?.Dispose();
				_Model?.Dispose();
				_Picker?.Dispose();
				_Command = null;
			}

			base.Dispose(disposing);
		}
	}
}
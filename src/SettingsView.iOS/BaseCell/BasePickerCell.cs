// unset

using System.Diagnostics.CodeAnalysis;
using Foundation;
using Jakar.SettingsView.iOS.Controls;
using UIKit;
using Xamarin.Forms;

namespace Jakar.SettingsView.iOS.BaseCell
{
	[Preserve(AllMembers = true)]
	public abstract class BasePickerCell : BaseAiValueCell
	{
		protected NoCaretField _DummyField { get; set; }


		[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
		protected BasePickerCell( Cell cell ) : base(cell)
		{
			// InputView
			_DummyField = new NoCaretField();
			_ContentView.AddSubview(_DummyField);
			_ContentView.SendSubviewToBack(_DummyField);

			SelectionStyle = UITableViewCellSelectionStyle.Default;

			SetUp();
		}

		protected internal override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			tableView.DeselectRow(indexPath, true);
			ShowPopup();
		}

		protected virtual void ShowPopup() { _DummyField.BecomeFirstResponder(); }
		protected virtual void SetUp() { }
		protected virtual void SetUp( UITableView tableView, NSIndexPath indexPath ) { }


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_DummyField.RemoveFromSuperview();
				_DummyField.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}
// unset

using System.Diagnostics.CodeAnalysis;
using Foundation;
using Jakar.SettingsView.iOS.OLD_Cells;
using UIKit;
using Xamarin.Forms;

namespace Jakar.SettingsView.iOS.BaseCell
{
	[Foundation.Preserve(AllMembers = true)]
	public abstract class BasePickerCell : BaseAiValueCell
	{
		protected NoCaretField _DummyField { get; set; }


		[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
		protected BasePickerCell( Cell cell ) : base(cell)
		{
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
		protected abstract void SetUp();


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
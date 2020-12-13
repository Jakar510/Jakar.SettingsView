using CoreGraphics;
using UIKit;

namespace Jakar.SettingsView.iOS.Cells
{
	/// <summary>
	/// No caret field.
	/// </summary>
	[Foundation.Preserve(AllMembers = true)]
	public class NoCaretField : UITextField
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.iOS.Cells.NoCaretField"/> class.
		/// </summary>
		public NoCaretField() : base(new CGRect()) { }
		/// <summary>
		/// Gets the caret rect for position.
		/// </summary>
		/// <returns>The caret rect for position.</returns>
		/// <param name="position">Position.</param>
		public override CGRect GetCaretRectForPosition( UITextPosition position ) => new CGRect();
	}
}
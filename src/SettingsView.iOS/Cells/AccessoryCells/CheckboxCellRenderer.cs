using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Controls;
using Jakar.SettingsView.Shared.Cells;
using UIKit;
using Xamarin.Forms;


#nullable enable
[assembly: ExportRenderer(typeof(CheckboxCell), typeof(CheckboxCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Foundation.Preserve(AllMembers = true)]
	public class CheckboxCellRenderer : CellBaseRenderer<CheckboxCellView> { }

	[Foundation.Preserve(AllMembers = true)]
	public class CheckboxCellView : BaseAccessoryCell<CheckboxCell, SimpleCheck>
	{
		public CheckboxCellView( CheckboxCell formsCell ) : base(formsCell) { }
		
		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			_Accessory.Toggle();

			tableView.DeselectRow(indexPath, true);
		}
	}

	// public class CheckBox : UIButton, IRenderAccessory
	// {
	// 	public BaseAccessoryCell<CheckableCellBase, CheckBox> Renderer { get; }
	//
	// 	public UIEdgeInsets Inset { get; set; } = new(20, 20, 20, 20);
	//
	// 	public CGColor? FillColor { get; set; }
	//
	// 	public Action<UIButton>? CheckChanged { get; set; }
	//
	// 	public UITapGestureRecognizer Recognizer { get; set; }
	//
	// 	public CheckBox( BaseAccessoryCell<CheckableCellBase, CheckBox> renderer ) : base()
	// 	{
	// 		Renderer = renderer;
	// 		Recognizer = new UITapGestureRecognizer(OnClick);
	// 		AddGestureRecognizer(Recognizer);
	// 	}
	//
	//
	// 	private void OnClick( UITapGestureRecognizer obj )
	// 	{
	// 		Selected = !Selected;
	// 		CheckChanged?.Invoke(this);
	// 	}
	//
	// 	public override void Draw( CGRect rect )
	// 	{
	// 		base.Draw(rect);
	//
	// 		var lineWidth = rect.Size.Width / 10;
	//
	// 		// Draw check mark
	// 		if ( Selected )
	// 		{
	// 			Layer.BackgroundColor = FillColor;
	//
	// 			var checkMark = new UIBezierPath();
	// 			var size = rect.Size;
	// 			checkMark.MoveTo(new CGPoint(x: 22f / 100f * size.Width, y: 52f / 100f * size.Height));
	// 			checkMark.AddLineTo(new CGPoint(x: 38f / 100f * size.Width, y: 68f / 100f * size.Height));
	// 			checkMark.AddLineTo(new CGPoint(x: 76f / 100f * size.Width, y: 30f / 100f * size.Height));
	//
	// 			checkMark.LineWidth = lineWidth;
	// 			UIColor.White.SetStroke();
	// 			checkMark.Stroke();
	// 		}
	//
	// 		else { Layer.BackgroundColor = new CGColor(0, 0, 0, 0); }
	// 	}
	//
	// 	public override bool PointInside( CGPoint point, UIEvent? uiEvent )
	// 	{
	// 		CGRect rect = Bounds;
	// 		rect.X -= Inset.Left;
	// 		rect.Y -= Inset.Top;
	// 		rect.Width += Inset.Left + Inset.Right;
	// 		rect.Height += Inset.Top + Inset.Bottom;
	//
	// 		return rect.Contains(point);
	// 	}
	//
	// 	public void Initialize( Stack parent ) { }
	// 	public void Update() { }
	// 	public bool Update( object sender, PropertyChangedEventArgs e ) => throw new NotImplementedException();
	// 	public bool UpdateParent( object sender, PropertyChangedEventArgs e ) => throw new NotImplementedException();
	// 	public void Enable() { }
	// 	public void Disable() { }
	//
	// 	protected override void Dispose( bool disposing )
	// 	{
	// 		if ( disposing )
	// 		{
	// 			RemoveGestureRecognizer(Recognizer);
	// 			Recognizer.Dispose();
	// 			CheckChanged = null;
	// 		}
	//
	// 		base.Dispose(disposing);
	// 	}
	// }
}
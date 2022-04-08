[assembly: ExportRenderer(typeof(ButtonCell), typeof(ButtonCellRenderer))]


namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)] public class ButtonCellRenderer : CellBaseRenderer<ButtonCellView> { }

	[Preserve(AllMembers = true)]
	public class ButtonCellView : BaseCellView
	{
		protected internal ButtonCell ButtonCell => Cell as ButtonCell ?? throw new NullReferenceException(nameof(ButtonCell));

		protected ButtonView _Button { get; set; }

		public ButtonCellView( Cell formsCell ) : base(formsCell)
		{
			SelectionStyle = UITableViewCellSelectionStyle.None;

			_Button = new ButtonView(this);
			_Button.Initialize(_MainStack);


			this.SetContent(_MainStack);
			
			if ( !string.IsNullOrEmpty(Cell.AutomationId) ) { _MainStack.AccessibilityIdentifier = Cell.AutomationId; }

			SetNeedsLayout();
		}


		public override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Button.Update(sender, e) ) { return; }

			base.CellPropertyChanged(sender, e);
		}


		public override bool RowLongPressed( UITableView tableView, NSIndexPath indexPath )
		{
			_Button.RunLong();
			return true;
			// return base.RowLongPressed(tableView, indexPath);
		}
		public override void RowSelected( UITableView adapter, NSIndexPath position ) { _Button.Run(); }


		protected void EnableCell() { _Button.SetEnabledAppearance(true); }
		protected void DisableCell() { _Button.SetEnabledAppearance(false); }

		public override void UpdateCell( UITableView tableView )
		{
			_Button.Update();
			base.UpdateCell(tableView);
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing ) { _Button.Dispose(); }

			base.Dispose(disposing);
		}
	}
}
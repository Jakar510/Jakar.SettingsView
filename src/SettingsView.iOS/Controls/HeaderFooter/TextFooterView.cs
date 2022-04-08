namespace Jakar.SettingsView.iOS.Controls.HeaderFooter
{
	public class TextFooterView : UITableViewHeaderFooterView
	{
		public PaddingLabel Label { get; set; }
		private readonly List<NSLayoutConstraint> _constraints = new List<NSLayoutConstraint>();

		public TextFooterView( IntPtr handle ) : base(handle)
		{
			Label = new PaddingLabel
					{
						Lines = 0,
						LineBreakMode = UILineBreakMode.CharacterWrap,
						TranslatesAutoresizingMaskIntoConstraints = false
					};

			ContentView.AddSubview(Label);

			_constraints.Add(Label.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor, 0));
			_constraints.Add(Label.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor, 0));
			_constraints.Add(Label.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor, 0));
			_constraints.Add(Label.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor, 0));

			_constraints.ForEach(c =>
								 {
									 c.Priority = 999f; // fix warning-log:Unable to simultaneously satisfy constraints.
									 c.Active = true;
								 }
								);

			BackgroundView = new UIView();
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_constraints.ForEach(c => c.Dispose());
				Label.Dispose();
				BackgroundView?.Dispose();
				BackgroundView = null;
			}

			base.Dispose(disposing);
		}
	}
}
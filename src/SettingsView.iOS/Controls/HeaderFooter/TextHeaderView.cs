using System;
using System.Collections.Generic;
using UIKit;
using Xamarin.Forms;

namespace Jakar.SettingsView.iOS.Controls.HeaderFooter
{
	public class TextHeaderView : UITableViewHeaderFooterView
	{
		public PaddingLabel Label { get; set; }
		private readonly List<NSLayoutConstraint> _constraints = new();
		private LayoutAlignment _curAlignment;
		private bool _isInitialized;

		public TextHeaderView( IntPtr handle ) : base(handle)
		{
			Label = new PaddingLabel();
			Label.Lines = 0;
			Label.LineBreakMode = UILineBreakMode.CharacterWrap;
			Label.TranslatesAutoresizingMaskIntoConstraints = false;

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


		public void SetVerticalAlignment( LayoutAlignment align )
		{
			if ( _isInitialized && align == _curAlignment ) { return; }

			foreach ( NSLayoutConstraint c in _constraints )
			{
				c.Active = false;
				c.Dispose();
			}

			_constraints.Clear();

			_constraints.Add(Label.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor, 0));
			_constraints.Add(Label.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor, 0));

			switch (align)
			{
				case LayoutAlignment.Start:
					_constraints.Add(Label.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor, 0));
					break;

				case LayoutAlignment.End:
					_constraints.Add(Label.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor, 0));
					break;

				// case LayoutAlignment.Center: 
				// case LayoutAlignment.Fill: 
				default:
					_constraints.Add(Label.CenterYAnchor.ConstraintEqualTo(ContentView.CenterYAnchor, 0));
					break;
			}

			_constraints.ForEach(c =>
								 {
									 c.Priority = 999f; // fix warning-log:Unable to simultaneously satisfy constraints.
									 c.Active = true;
								 }
								);

			_curAlignment = align;
			_isInitialized = true;
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_constraints.ForEach(c => c.Dispose());
				Label?.Dispose();
				Label = null;
				BackgroundView?.Dispose();
				BackgroundView = null;
			}

			base.Dispose(disposing);
		}
	}
}
﻿using System;
using System.Collections.Generic;
using UIKit;
using Xamarin.Forms;

namespace Jakar.SettingsView.iOS
{
	public class TextHeaderView : UITableViewHeaderFooterView
	{
		public PaddingLabel Label { get; set; }
		private List<NSLayoutConstraint> _constraints = new List<NSLayoutConstraint>();
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
								 });


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

			if ( align == LayoutAlignment.Start ) { _constraints.Add(Label.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor, 0)); }
			else if ( align == LayoutAlignment.End ) { _constraints.Add(Label.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor, 0)); }
			else { _constraints.Add(Label.CenterYAnchor.ConstraintEqualTo(ContentView.CenterYAnchor, 0)); }

			_constraints.ForEach(c =>
								 {
									 c.Priority = 999f; // fix warning-log:Unable to simultaneously satisfy constraints.
									 c.Active = true;
								 });

			_curAlignment = align;
			_isInitialized = true;
		}

		protected override void Dispose( bool disposing )
		{
			base.Dispose(disposing);
			if ( disposing )
			{
				_constraints.ForEach(c => c.Dispose());
				Label?.Dispose();
				Label = null;
				BackgroundView?.Dispose();
				BackgroundView = null;
			}
		}
	}
}
namespace Jakar.SettingsView.iOS.Controls
{
	[Foundation.Preserve(AllMembers = true)]
	public class SimpleCheck : FormsCheckBox, IRenderAccessory // AView
	{
		protected BaseAccessoryCell<CheckboxCell, SimpleCheck> _Renderer { get; }
		public SimpleCheck( BaseAccessoryCell<CheckboxCell, SimpleCheck> renderer ) : base()
		{
			_Renderer = renderer;
			CheckedChanged += OnCheckedChanged;
		}

		public void Update()
		{
			UpdateChecked();
			UpdateAccentColor();
		}

		public bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(CheckableCellBase.accentColorProperty) ) { return UpdateAccentColor(); }

			if ( e.IsEqual(CheckableCellBase.checkedProperty) ) { return UpdateChecked(); }

			return false;
		}
		public bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(Shared.sv.SettingsView.cellAccentColorProperty) ) { return UpdateAccentColor(); }

			return false;
		}

		public void Enable()
		{
			IsEnabled = true;
			Alpha = SvConstants.Cell.ENABLED_ALPHA;
		}
		public void Disable()
		{
			IsEnabled = true;
			Alpha = SvConstants.Cell.DISABLED_ALPHA;
		}


		protected void OnCheckedChanged( object sender, EventArgs e )
		{
			if ( sender is SimpleCheck check ) { CheckChanged(check); }
		}
		public void CheckChanged( SimpleCheck check )
		{
			_Renderer.Cell.Checked = check.Selected;
			_Renderer.Cell.ValueChangedHandler.SendValueChanged(check.Selected);
		}


		public void Toggle()
		{
			if ( IsChecked ) { Deselect(); }
			else { Select(); }
		}
		public void Select() { IsChecked = true; }
		public void Deselect() { IsChecked = false; }


		protected bool UpdateAccentColor()
		{
			CheckBoxTintColor = _Renderer.Cell.CheckableConfig.AccentColor;
			return true;
		}
		protected bool UpdateChecked()
		{
			IsChecked = _Renderer.Cell.Checked;
			return true;
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing ) { CheckedChanged -= OnCheckedChanged; }

			base.Dispose(disposing);
		}

		// from CheckBoxRenderer 
		// 
		//
		// public override CGSize SizeThatFits( CGSize size )
		// {
		// 	var result = base.SizeThatFits(size);
		// 	var height = Math.Max(MinimumSize, result.Height);
		// 	var width = Math.Max(MinimumSize, result.Width);
		// 	var final = Math.Min(width, height);
		// 	return new CGSize(final, final);
		// }
		//
		// public override SizeRequest GetDesiredSize( double widthConstraint, double heightConstraint )
		// {
		// 	var sizeConstraint = base.GetDesiredSize(widthConstraint, heightConstraint);
		//
		// 	var set = false;
		//
		// 	var width = widthConstraint;
		// 	var height = heightConstraint;
		// 	if ( sizeConstraint.Request.Width == 0 )
		// 	{
		// 		if ( widthConstraint <= 0 || double.IsInfinity(widthConstraint) )
		// 		{
		// 			width = MinimumSize;
		// 			set = true;
		// 		}
		// 	}
		//
		// 	if ( sizeConstraint.Request.Height == 0 )
		// 	{
		// 		if ( heightConstraint <= 0 || double.IsInfinity(heightConstraint) )
		// 		{
		// 			height = MinimumSize;
		// 			set = true;
		// 		}
		// 	}
		//
		// 	if ( set )
		// 	{
		// 		sizeConstraint = new SizeRequest(new Size(width, height), new Size(MinimumSize, MinimumSize));
		// 	}
		//
		// 	return sizeConstraint;
		// }
	}
}
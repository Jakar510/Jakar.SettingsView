// unset



namespace Jakar.SettingsView.iOS.Controls
{
	public class AiSwitch : UISwitch, IRenderAccessory
	{
		protected BaseAccessoryCell<SwitchCell, AiSwitch> _Renderer { get; }

		public AiSwitch( BaseAccessoryCell<SwitchCell, AiSwitch> renderer ) : base()
		{
			_Renderer     =  renderer;
			ValueChanged  += OnValueChanged;
			TouchUpInside += OnTouchUpInside;
		}

		protected void OnTouchUpInside( object sender, EventArgs e ) => Toggle();

		protected void OnValueChanged( object sender, EventArgs e )
		{
			_Renderer.Cell.Checked = On;
			_Renderer.Cell.ValueChangedHandler.SendValueChanged(On);
		}

		public void Initialize( UIStackView parent ) { parent.AddArrangedSubview(this); }

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

		protected bool UpdateAccentColor()
		{
			TintColor = _Renderer.Cell.CheckableConfig.AccentColor.ToUIColor();
			return true;
		}

		protected bool UpdateChecked()
		{
			On = _Renderer.Cell.Checked;
			return true;
		}

		public void Enable()
		{
			Enabled = true;
			Alpha   = SvConstants.Cell.ENABLED_ALPHA;
		}

		public void Disable()
		{
			Enabled = true;
			Alpha   = SvConstants.Cell.DISABLED_ALPHA;
		}

		public void Toggle()
		{
			if ( On ) { Deselect(); }
			else { Select(); }
		}

		public void Select() { On   = true; }
		public void Deselect() { On = false; }


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				ValueChanged  -= OnValueChanged;
				TouchUpInside -= OnTouchUpInside;
			}

			base.Dispose(disposing);
		}
	}
}

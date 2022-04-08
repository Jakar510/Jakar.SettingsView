namespace Jakar.SettingsView.iOS.Controls.Manager
{
	public abstract class BaseViewManager<TView, TCell> : IDefaultColors<UIColor?>, IInitializeControl, IDisposable where TCell : CellBase
																													where TView : UIView
	{
		public BaseCellView Renderer               { get; private set; }
		public float        DefaultFontSize        { get; }
		public UIColor?     DefaultTextColor       { get; }
		public UIColor?     DefaultBackgroundColor { get; }


		protected abstract IUseConfiguration _Config { get; }
		protected          TCell             _Cell   { get; private set; }
		public             TView             Control { get; }


		protected BaseViewManager( BaseCellView renderer,
								   TCell        cell,
								   TView        control,
								   UIColor?     textColor,
								   UIColor?     backgroundColor,
								   nfloat       fontSize
		)
		{
			Renderer = renderer ?? throw new NullReferenceException(nameof(renderer));
			Control  = control ?? throw new NullReferenceException(nameof(control));
			_Cell    = cell ?? throw new NullReferenceException(nameof(cell));

			DefaultBackgroundColor = backgroundColor;
			DefaultTextColor       = textColor;
			DefaultFontSize        = fontSize.ToFloat();
		}

		public void SetRenderer( BaseCellView  renderer ) { Renderer = renderer ?? throw new NullReferenceException(nameof(renderer)); }
		public void SetCell( TCell             cell ) => _Cell = cell;
		public abstract void Initialize( UIStackView parent );


		public virtual void Update() { UpdateIsEnabled(); }

		public abstract bool Update( object       sender, PropertyChangedEventArgs e );
		public abstract bool UpdateParent( object sender, PropertyChangedEventArgs e );


		protected virtual void UpdateIsEnabled() => SetEnabledAppearance(_Cell.IsEnabled);

		public void SetEnabledAppearance( in bool isEnabled )
		{
			if ( isEnabled ) { Enable(); }
			else { Disable(); }

			Control.UserInteractionEnabled = isEnabled;
		}

		public virtual void Enable() { Control.Alpha  = SvConstants.Cell.ENABLED_ALPHA; }
		public virtual void Disable() { Control.Alpha = SvConstants.Cell.DISABLED_ALPHA; }


		private bool _disposed;
		public void Dispose() => Dispose(true);

		protected virtual void Dispose( bool disposing )
		{
			if ( !disposing ) { return; }

			if ( _disposed ) { return; }

			_disposed = true;
			Control.RemoveFromSuperview();
			Control.Dispose();

			DefaultTextColor?.Dispose();

			DefaultBackgroundColor?.Dispose();
		}
	}
}

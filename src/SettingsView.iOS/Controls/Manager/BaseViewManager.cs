using System;
using System.ComponentModel;
using Jakar.Api.iOS.Extensions;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Interfaces;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Interfaces;
using UIKit;


#nullable enable
namespace Jakar.SettingsView.iOS.Controls.Manager
{
	public abstract class BaseViewManager<TView, TCell> : IUpdateCell<TCell>, IDefaultColors<UIColor?>, IInitializeControl, IDisposable where TCell : CellBase
																																		where TView : UIView
	{
		public float    DefaultFontSize        { get; }
		public UIColor? DefaultTextColor       { get; }
		public UIColor? DefaultBackgroundColor { get; }


		protected abstract IUseConfiguration _Config { get; }
		protected          TCell             _Cell   { get; private set; }
		public             TView             Control { get; protected set; }


		protected BaseViewManager( TView    control,
								   TCell    cell,
								   UIColor? textColor,
								   UIColor? backgroundColor,
								   nfloat   fontSize
		)
		{
			Control = control;
			_Cell = cell;

			DefaultBackgroundColor = backgroundColor;
			DefaultTextColor = textColor;
			DefaultFontSize = fontSize.ToFloat();
		}

		public void SetCell( TCell             cell ) => _Cell = cell;
		public abstract void Initialize( Stack parent );
		public virtual void Initialize() { }


		public virtual void Update()
		{
			UpdateText();
			UpdateTextColor();
			UpdateTextAlignment();
			UpdateFont();
			UpdateFontSize();
			UpdateIsEnabled();
		}

		public abstract bool Update( object       sender, PropertyChangedEventArgs e );
		public abstract bool UpdateParent( object sender, PropertyChangedEventArgs e );


		public abstract bool UpdateFont();
		public abstract bool UpdateTextColor();
		public abstract bool UpdateFontSize();
		public abstract bool UpdateTextAlignment();


		public abstract bool UpdateText();
		public abstract bool UpdateText( string? s );


		protected virtual void UpdateIsEnabled() => SetEnabledAppearance(_Cell.IsEnabled);

		public void SetEnabledAppearance( in bool isEnabled )
		{
			if ( isEnabled ) { Enable(); }
			else { Disable(); }

			Control.UserInteractionEnabled = isEnabled;
		}

		public virtual void Enable() { Control.Alpha = SvConstants.Cell.ENABLED_ALPHA; }
		public virtual void Disable() { Control.Alpha = SvConstants.Cell.DISABLED_ALPHA; }


		private bool _disposed;
		public void Dispose() => Dispose(true);

		protected virtual void Dispose( bool disposing )
		{
			if ( !disposing ) { return; }

			if ( _disposed ) { return; }

			_disposed = true;
			DefaultTextColor?.Dispose();

			DefaultBackgroundColor?.Dispose();
		}
	}
}

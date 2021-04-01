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
	public abstract class BaseViewManager<TView, TCell> : IUpdateCell<UIColor?, TCell>, IInitializeControl, IDisposable where TCell : CellBase
																													   where TView : UIView
	{
		public float DefaultFontSize { get; }
		public UIColor? DefaultTextColor { get; }
		public UIColor? DefaultBackgroundColor { get; }


		protected TCell _Cell { get; private set; }
		public TView Control { get; protected set; }


		protected BaseViewManager( TView control,
								   TCell cell,
								   UIColor? textColor,
								   UIColor? backgroundColor,
								   nfloat fontSize )
		{
			Control = control;
			_Cell = cell;

			DefaultBackgroundColor = backgroundColor;
			DefaultTextColor = textColor;
			DefaultFontSize = fontSize.ToFloat();
		}
		public void SetCell( TCell cell ) => _Cell = cell;
		public abstract void Initialize( Stack parent );
		public virtual void Initialize() { }


		public virtual void Update()
		{
			UpdateText();
			UpdateTextColor();
			UpdateAlignment();
			UpdateFont();
			UpdateFontSize();
			UpdateIsEnabled();
		}
		public abstract bool Update( object sender, PropertyChangedEventArgs e );
		public abstract bool UpdateParent( object sender, PropertyChangedEventArgs e );


		public abstract bool UpdateFont();
		public abstract bool UpdateTextColor();
		public abstract bool UpdateFontSize();
		protected abstract bool UpdateAlignment();


		public abstract bool UpdateText();
		public abstract bool UpdateText( string? s );


		protected virtual void UpdateIsEnabled() => SetEnabledAppearance(_Cell.IsEnabled);
		public void SetEnabledAppearance( in bool isEnabled )
		{
			if ( isEnabled ) { Enable(); }
			else { Disable(); }

			Control.UserInteractionEnabled = isEnabled;
		}
		public virtual void Enable() { Control.Alpha = SVConstants.Cell.ENABLED_ALPHA; }
		public virtual void Disable() { Control.Alpha = SVConstants.Cell.DISABLED_ALPHA; }


		public void Dispose() => Dispose(true);
		protected virtual void Dispose( bool disposing )
		{
			if ( !disposing ) { return; }

			DefaultTextColor?.Dispose();
			DefaultBackgroundColor?.Dispose();
		}
	}

	public static class LayoutExtensions
	{
		
		public static void AddFull( this UIView Control, UIStackView parent )
		{
			parent.AddArrangedSubview(Control);

			Control.LeftAnchor.ConstraintEqualTo(parent.LeftAnchor).Active = true;
			Control.RightAnchor.ConstraintEqualTo(parent.RightAnchor).Active = true;

			Control.BottomAnchor.ConstraintEqualTo(parent.BottomAnchor).Active = true;
			Control.TopAnchor.ConstraintEqualTo(parent.TopAnchor).Active = true;

			Control.UpdateConstraintsIfNeeded();
		}
		public static void AddFull( this UIView Control, UIView parent )
		{
			parent.AddSubview(Control);

			Control.LeftAnchor.ConstraintEqualTo(parent.LeftAnchor).Active = true;
			Control.RightAnchor.ConstraintEqualTo(parent.RightAnchor).Active = true;

			Control.BottomAnchor.ConstraintEqualTo(parent.BottomAnchor).Active = true;
			Control.TopAnchor.ConstraintEqualTo(parent.TopAnchor).Active = true;

			Control.UpdateConstraintsIfNeeded();
		}
	}
}
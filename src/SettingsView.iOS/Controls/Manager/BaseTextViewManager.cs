using System;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Interfaces;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Interfaces;
using UIKit;


#nullable enable
namespace Jakar.SettingsView.iOS.Controls.Manager
{
	public abstract class BaseTextViewManager<TView, TCell> : BaseViewManager<TView, TCell>, IUpdateCell<TCell>, IInitializeControl, IDisposable where TCell : CellBase
																																				 where TView : UIView
	{
		protected BaseTextViewManager( BaseCellView renderer,
									   TCell        cell,
									   TView        control,
									   UIColor?     textColor,
									   UIColor?     backgroundColor,
									   nfloat       fontSize
		) : base(renderer,
				 cell,
				 control,
				 textColor,
				 backgroundColor,
				 fontSize) { }

		public virtual void Initialize() { }

		public override void Update()
		{
			UpdateText();
			UpdateTextColor();
			UpdateTextAlignment();
			UpdateFont();
			UpdateFontSize();
			UpdateIsEnabled();

			base.Update();
		}

		public abstract bool UpdateFont();
		public abstract bool UpdateTextColor();
		public abstract bool UpdateFontSize();
		public abstract bool UpdateTextAlignment();


		public abstract bool UpdateText();
		// ReSharper disable once UnusedMemberInSuper.Global
		public abstract bool UpdateText( string? s );
	}
}

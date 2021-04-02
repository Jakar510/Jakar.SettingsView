using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using CoreGraphics;
using Jakar.Api.iOS.Extensions;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Controls.Manager;
using Jakar.SettingsView.iOS.Interfaces;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Interfaces;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#nullable enable
namespace Jakar.SettingsView.iOS.Controls.Core
{
	[Foundation.Preserve(AllMembers = true)]
	public abstract class BaseTextView<TCell, TCellRenderer> : BaseViewManager<UITextView, TCell>, IRenderValue, IUpdateCell<TCell>, IDefaultColors<UIColor?>, IInitializeControl, IDisposable where TCell : TitleCellBase
																																															   where TCellRenderer : BaseCellView
	{
		protected TCellRenderer _Renderer { get; set; }
		protected bool _IsAvailable { get; private set; } = true;


		protected BaseTextView( TCellRenderer renderer ) : this(new UITextView(), renderer) { }
		protected BaseTextView( UITextView control, TCellRenderer renderer ) : base(control,
																					renderer.Cell as TCell ?? throw new NullReferenceException(nameof(renderer.Cell)),
																					control.TextColor,
																					control.BackgroundColor,
																					control.MinimumZoomScale
																				   )
		{
			Initialize();
			_Renderer = renderer;
		}
		// protected BaseTextView( BaseCellView renderer ) : base()
		// {
		// 	_Renderer = renderer ?? throw new NullReferenceException(nameof(renderer));
		// 	Initialize();
		// 	SetUsed(renderer.Cell);
		// }
		public override void Initialize( Stack parent )
		{
			Control.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.DefaultHigh, UILayoutConstraintAxis.Horizontal);
			Control.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.DefaultHigh, UILayoutConstraintAxis.Vertical);

			Control.UpdateConstraintsIfNeeded();
		}


		public void SetUsed( bool used ) { _IsAvailable = used; }

		public override void Initialize()
		{
			Control.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Horizontal);
			Control.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);

			// Control.LineBreakMode = UILineBreakMode.WordWrap;
			// Control.Lines = 10;
			// Control.BaselineAdjustment = UIBaselineAdjustment.AlignCenters;
			// Control.AdjustsLetterSpacingToFitWidth = true;
			// Control.AdjustsFontSizeToFitWidth = true;
			Control.TintAdjustmentMode = UIViewTintAdjustmentMode.Automatic;

			Control.BackgroundColor = UIColor.Clear;
		}

		public override void Enable() { Control.Alpha = SVConstants.Cell.ENABLED_ALPHA; }
		public override void Disable() { Control.Alpha = SVConstants.Cell.DISABLED_ALPHA; }


		public override bool UpdateText( string? text )
		{
			Control.Text = text;
			Control.Hidden = string.IsNullOrEmpty(Control.Text);

			return true;
		}
		public override bool UpdateFontSize()
		{
			Control.ContentScaleFactor = _Config.FontSize.ToNFloat();

			return true;
		}
		public override bool UpdateTextColor()
		{
			Control.TextColor = _Config.Color.ToUIColor();

			return true;
		}
		public override bool UpdateFont()
		{
			IUseConfiguration config = _Config;
			string? family = config.FontFamily;
			FontAttributes attr = config.FontAttributes;
			var size = (float) config.FontSize;

			Control.Font = FontUtility.CreateNativeFont(family, size, attr);

			// make the view height fit font size
			nfloat contentH = Control.IntrinsicContentSize.Height;
			CGRect bounds = Control.Bounds;
			Control.Bounds = new CGRect(0, 0, bounds.Width, contentH);

			return true;
		}
		public override bool UpdateTextAlignment()
		{
			Control.TextAlignment = _Config.TextAlignment.ToUITextAlignment();

			return true;
		}

		public void SetEnabledAppearance( bool isEnabled )
		{
			Control.Alpha = isEnabled
								? SVConstants.Cell.ENABLED_ALPHA
								: SVConstants.Cell.DISABLED_ALPHA;
		}


		public override void Update()
		{
			// UpdateBackgroundColor();
			UpdateText();
			UpdateTextColor();
			UpdateFontSize();
			UpdateFont();
			UpdateTextAlignment();
		}


		public override bool Update( object sender, PropertyChangedEventArgs e ) =>
			// if ( e.PropertyName == CellBase.BackgroundColorProperty.PropertyName )
			// {
			// 	UpdateBackgroundColor();
			// 	return true;
			// }
			false;

		public override bool UpdateParent( object sender, PropertyChangedEventArgs e ) =>
			// if ( e.PropertyName == Shared.sv.SettingsView.CellBackgroundColorProperty.PropertyName )
			// {
			// 	UpdateBackgroundColor();
			// 	return true;
			// }
			false;

		protected override void Dispose( bool disposing )
		{
			if ( disposing ) { Control.RemoveFromSuperview(); }

			base.Dispose(disposing);
		}
	}
}
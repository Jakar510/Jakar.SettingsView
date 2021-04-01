using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Jakar.Api.iOS.Extensions;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Interfaces;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Interfaces;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#nullable enable
namespace Jakar.SettingsView.iOS.Controls.Core
{
	[Foundation.Preserve(AllMembers = true)]
	public abstract class BaseTextView : UILabel, IUpdateCell<Color, BaseCellView>, IInitializeControl, IDisposable
	{
		public Color DefaultTextColor { get; }
		public Color DefaultBackgroundColor { get; }
		public float DefaultFontSize { get; }
		protected BaseCellView _Renderer { get; set; }
		protected bool _IsAvailable { get; private set; } = true;


		[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
		protected BaseTextView( BaseCellView renderer ) : base()
		{
			DefaultBackgroundColor = BackgroundColor.ToColor();
			DefaultTextColor = TextColor.ToColor();
			DefaultFontSize = ContentScaleFactor.ToFloat();

			_Renderer = renderer ?? throw new NullReferenceException(nameof(renderer));
			Initialize();
			SetUsed(renderer.Cell);
		}
		public virtual void Initialize( Stack parent )
		{
			SetContentCompressionResistancePriority(SVConstants.Layout.Priority.DefaultHigh, UILayoutConstraintAxis.Horizontal);
			SetContentCompressionResistancePriority(SVConstants.Layout.Priority.DefaultHigh, UILayoutConstraintAxis.Vertical);

			UpdateConstraintsIfNeeded();
		}


		public void SetUsed( bool used ) { _IsAvailable = used; }
		public abstract void SetUsed( Cell cell );
		public void SetCell( BaseCellView cell ) { _Renderer = cell ?? throw new NullReferenceException(nameof(cell)); }

		public virtual void Initialize()
		{
			SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Horizontal);
			SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);

			LineBreakMode = UILineBreakMode.WordWrap;
			Lines = 10;
			BaselineAdjustment = UIBaselineAdjustment.AlignCenters;
			AdjustsLetterSpacingToFitWidth = true;
			AdjustsFontSizeToFitWidth = true;
			TintAdjustmentMode = UIViewTintAdjustmentMode.Automatic;

			BackgroundColor = UIColor.Clear;
		}

		public void Enable() { Alpha = SVConstants.Cell.ENABLED_ALPHA; }
		public void Disable() { Alpha = SVConstants.Cell.DISABLED_ALPHA; }


		public void SetEnabledAppearance( bool isEnabled )
		{
			Alpha = isEnabled
						? SVConstants.Cell.ENABLED_ALPHA
						: SVConstants.Cell.DISABLED_ALPHA;
		}
		public abstract bool UpdateText();
		public abstract bool UpdateTextColor();
		public abstract bool UpdateFontSize();
		public abstract bool UpdateTextAlignment();
		public abstract bool UpdateFont();


		public virtual void Update()
		{
			// UpdateBackgroundColor();
			UpdateText();
			UpdateTextColor();
			UpdateFontSize();
			UpdateFont();
			UpdateTextAlignment();
		}


		public virtual bool Update( object sender, PropertyChangedEventArgs e ) =>
			// if ( e.PropertyName == CellBase.BackgroundColorProperty.PropertyName )
			// {
			// 	UpdateBackgroundColor();
			// 	return true;
			// }
			false;

		public virtual bool UpdateParent( object sender, PropertyChangedEventArgs e ) =>
			// if ( e.PropertyName == Shared.sv.SettingsView.CellBackgroundColorProperty.PropertyName )
			// {
			// 	UpdateBackgroundColor();
			// 	return true;
			// }
			false;

		protected override void Dispose( bool disposing )
		{
			if ( disposing ) { RemoveFromSuperview(); }

			base.Dispose(disposing);
		}
	}
}
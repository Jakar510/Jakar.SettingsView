using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Jakar.SettingsView.iOS.Extensions;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Interfaces;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#nullable enable
namespace Jakar.SettingsView.iOS.Controls.Core
{
	[Foundation.Preserve(AllMembers = true)]
	public abstract class BaseTextView<TCell> : UILabel, IUpdateCell<Color, TCell>
	{
		public Color DefaultTextColor { get; }
		public float DefaultFontSize { get; }
		protected TCell _Renderer { get; set; }


		[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
		protected BaseTextView( TCell renderer ) : base()
		{
			DefaultTextColor = TextColor.ToColor();
			DefaultFontSize = ContentScaleFactor.ToFloat();
			_Renderer = renderer ?? throw new NullReferenceException(nameof(renderer));
			Initialize();
		}
		// protected BaseTextView( BaseCellView cell, AContext context ) : this(context) => SetCell(cell);
		// protected BaseTextView( AContext context, IAttributeSet attributes ) : base(context, attributes)
		// {
		// 	DefaultFontSize = TextSize;
		// 	DefaultTextColor = new AColor(CurrentTextColor);
		// 	Init();
		// }


		public void SetCell( TCell cell ) { _Renderer = cell ?? throw new NullReferenceException(nameof(cell)); }
		// public static TCell Create<TCell>( View view, BaseCellView cell, int id ) where TCell : BaseTextView
		// {
		// 	TCell result = view.FindViewById<TCell>(id) ?? throw new NullReferenceException(nameof(id));
		// 	result.SetCell(cell);
		// 	return result;
		// }

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


		[SuppressMessage("ReSharper", "InvertIf")]
		public virtual bool Update( object sender, PropertyChangedEventArgs e )
		{
			// if ( e.PropertyName == CellBase.BackgroundColorProperty.PropertyName )
			// {
			// 	UpdateBackgroundColor();
			// 	return true;
			// }

			return false;
		}

		[SuppressMessage("ReSharper", "InvertIf")]
		public virtual bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			// if ( e.PropertyName == Shared.sv.SettingsView.CellBackgroundColorProperty.PropertyName )
			// {
			// 	UpdateBackgroundColor();
			// 	return true;
			// }

			return false;
		}
	}
}
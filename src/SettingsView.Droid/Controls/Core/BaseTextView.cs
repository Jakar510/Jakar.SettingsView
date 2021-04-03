using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Android.Util;
using Android.Widget;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AColor = Android.Graphics.Color;
using AContext = Android.Content.Context;
using AView = Android.Views.View;
using BaseCellView = Jakar.SettingsView.Droid.BaseCell.BaseCellView;

#nullable enable
namespace Jakar.SettingsView.Droid.Controls.Core
{
	[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
	[Android.Runtime.Preserve(AllMembers = true)]
	public abstract class BaseTextView : TextView, IUpdateCell<BaseCellView>, IDefaultColors<AColor>, IUpdateCell
	{
		public AColor DefaultTextColor { get; }
		public AColor DefaultBackgroundColor { get; }
		public float DefaultFontSize { get; }
		protected BaseCellView _Cell { get; set; }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		protected BaseTextView( AContext context ) : base(context)
		{
			DefaultFontSize = TextSize;
			DefaultBackgroundColor = Color.Default.ToAndroid();
			DefaultTextColor = new AColor(CurrentTextColor);
			Initialize();
		}
		protected BaseTextView( BaseCellView cell, AContext context ) : this(context) => SetCell(cell);
		protected BaseTextView( AContext context, IAttributeSet attributes ) : base(context, attributes)
		{
			DefaultFontSize = TextSize;
			DefaultTextColor = new AColor(CurrentTextColor);
			Initialize();
		}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


		public void SetCell( BaseCellView cell ) { _Cell = cell ?? throw new NullReferenceException(nameof(cell)); }
		public static TCell Create<TCell>( AView view, BaseCellView cell, int id ) where TCell : BaseTextView
		{
			TCell result = view.FindViewById<TCell>(id) ?? throw new NullReferenceException(nameof(id));
			result.SetCell(cell);
			return result;
		}

		public void SetMaxWidth( int width, double factor ) => SetMaxWidth((int) ( width * factor ));
		public virtual void Initialize()
		{
			// SetSingleLine(false);
			// Ellipsize = null;
			// SetMinLines(1);
			// SetMaxLines(10);
			// BreakStrategy = BreakStrategy.Balanced; // BreakKind.Word

			// CanScrollHorizontally(0); // Negative to check scrolling up, positive to check scrolling down.
			// CanScrollVertically(0);
			// SetScroller(new Scroller(_Cell.AndroidContext));

			SetBackgroundColor(Color.Transparent.ToAndroid());
			if ( Background != null ) { Background.Alpha = 0; } // hide underline
		}


		public void Enable() { Alpha = SvConstants.Cell.ENABLED_ALPHA; }
		public void Disable() { Alpha = SvConstants.Cell.DISABLED_ALPHA; }


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
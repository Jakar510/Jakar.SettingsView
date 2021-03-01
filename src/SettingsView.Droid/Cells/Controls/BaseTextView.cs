using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Android.Text;
using Android.Text.Method;
using Android.Util;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Droid.Cells.Base;
using Jakar.SettingsView.Shared.Interfaces;
using BreakStrategy = Android.Text.BreakStrategy;
using AColor = Android.Graphics.Color;
using AContext = Android.Content.Context;
using AView = Android.Views.View;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Controls
{
	[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
	public abstract class BaseTextView : TextView, IUpdateCell<AColor, BaseCellView>
	{
		public AColor DefaultTextColor { get; }
		public float DefaultFontSize { get; }

		protected BaseCellView _Cell { get; set; }

		// public TextView Label { get; protected set; }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		protected BaseTextView( AContext context ) : base(context)
		{
			DefaultFontSize = TextSize;
			DefaultTextColor = new AColor(CurrentTextColor);
			Init();
		}
		protected BaseTextView( BaseCellView cell, AContext context ) : this(context) => SetCell(cell);
		protected BaseTextView( AContext context, IAttributeSet attributes ) : base(context, attributes)
		{
			DefaultFontSize = TextSize;
			DefaultTextColor = new AColor(CurrentTextColor);
			Init();
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
		public virtual void Init()
		{
			// Ellipsize = null;
			// SetSingleLine(false);
			// SetMinLines(1);
			// SetMaxLines(10);
			// BreakStrategy = BreakStrategy.Balanced; // BreakKind.Word

			// CanScrollHorizontally(0); // Negative to check scrolling up, positive to check scrolling down.
			// CanScrollVertically(0);
			// SetScroller(new Scroller(_Cell.AndroidContext));

			if ( Background != null ) { Background.Alpha = 0; } // hide underline
		}


		public void Enable() { Alpha = BaseCellView.ENABLED_ALPHA; }
		public void Disable() { Alpha = BaseCellView.DISABLED_ALPHA; }


		public abstract bool UpdateText();
		public abstract bool UpdateColor();
		public abstract bool UpdateFontSize();
		public abstract bool UpdateFont();
		// public abstract bool UpdateAlignment();

		public abstract bool Update( object sender, PropertyChangedEventArgs e );
		public abstract void Update();
		public abstract bool UpdateParent( object sender, PropertyChangedEventArgs e );

	}
}
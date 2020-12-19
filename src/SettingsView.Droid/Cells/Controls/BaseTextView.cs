using System;
using System.ComponentModel;
using Android.Util;
using Android.Widget;
using Jakar.SettingsView.Droid.Cells.Base;
using BreakStrategy = Android.Text.BreakStrategy;
using AColor = Android.Graphics.Color;
using AContext = Android.Content.Context;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Controls
{
	public abstract class BaseTextView : TextView
	{
		protected internal AColor DefaultTextColor { get; }
		protected internal float DefaultFontSize { get; }

		protected BaseCellView _Cell { get; set; }
		// protected internal TextView Label { get; protected set; }

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
		protected internal void SetCell( BaseCellView cell ) { _Cell = cell ?? throw new NullReferenceException(nameof(cell)); }
		public static TCell Create<TCell>( Android.Views.View view, BaseCellView cell, int id ) where TCell : BaseTextView
		{
			TCell hint = view.FindViewById<TCell>(id) ?? throw new NullReferenceException(nameof(id));
			hint.SetCell(cell);
			return hint;
		}
		protected internal virtual void Init()
		{
			SetSingleLine(false);
			SetMinLines(1);
			SetMaxLines(10);

			// BreakKind.Word
			BreakStrategy = BreakStrategy.Simple;

			CanScrollHorizontally(0);
		}


		protected internal void Enable() { Alpha = BaseCellView.ENABLED_ALPHA; }
		protected internal void Disable() { Alpha = BaseCellView.DISABLED_ALPHA; }

		protected internal abstract bool UpdateText();
		protected internal abstract bool UpdateColor();
		protected internal abstract bool UpdateFontSize();
		protected internal abstract bool UpdateFont();
		// protected internal abstract bool UpdateAlignment();

		protected internal abstract bool Update( object sender, PropertyChangedEventArgs e );
		protected internal abstract void Update();
		protected internal abstract bool UpdateParent( object sender, PropertyChangedEventArgs e );
	}
}
using System;
using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Widget;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Base
{
	public abstract class BaseTextView : TextView
	{
		protected internal Color DefaultTextColor { get; }
		protected internal float DefaultFontSize { get; }

		protected CellBaseView _Cell { get; set; }
		// protected internal TextView Label { get; protected set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		protected BaseTextView( Context context ) : base(context)
		{
			DefaultFontSize = TextSize;
			DefaultTextColor = new Color(CurrentTextColor);
		}
		protected BaseTextView( CellBaseView cell, Context context ) : this(context) => SetCell(cell);
		protected BaseTextView( Context context, IAttributeSet attributes ) : base(context, attributes)
		{
			DefaultFontSize = TextSize;
			DefaultTextColor = new Color(CurrentTextColor);
		}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		protected internal void SetCell( CellBaseView cell ) { _Cell = cell ?? throw new NullReferenceException(nameof(cell)); }
		public static TCell Create<TCell>( Android.Views.View view, CellBaseView cell, int id ) where TCell : BaseTextView
		{
			TCell hint = view.FindViewById<TCell>(id) ?? throw new NullReferenceException(nameof(id));
			hint.SetCell(cell);
			return hint;
		}


		protected internal void Enable() { Alpha = CellBaseView.ENABLED_ALPHA; }
		protected internal void Disable() { Alpha = CellBaseView.DISABLED_ALPHA; }

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
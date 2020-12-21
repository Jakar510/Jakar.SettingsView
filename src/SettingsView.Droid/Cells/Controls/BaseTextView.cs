using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Android.Util;
using Android.Widget;
using Jakar.SettingsView.Droid.Cells.Base;
using Jakar.SettingsView.Shared.Interfaces;
using BreakStrategy = Android.Text.BreakStrategy;
using AColor = Android.Graphics.Color;
using AContext = Android.Content.Context;

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
		public static TCell Create<TCell>( Android.Views.View view, BaseCellView cell, int id ) where TCell : BaseTextView
		{
			TCell result = view.FindViewById<TCell>(id) ?? throw new NullReferenceException(nameof(id));
			result.SetCell(cell);
			return result;
		}
		public virtual void Init()
		{
			SetSingleLine(false);
			SetMinLines(1);
			SetMaxLines(10);

			// BreakKind.Word
			BreakStrategy = BreakStrategy.Simple;

			CanScrollHorizontally(0);
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
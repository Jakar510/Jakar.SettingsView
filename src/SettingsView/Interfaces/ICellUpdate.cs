// unset

using System;
using System.ComponentModel;
using System.Threading;
using Xamarin.Forms;
using EntryCell = Jakar.SettingsView.Shared.Cells.EntryCell;

#nullable enable
namespace Jakar.SettingsView.Shared.Interfaces
{
	public interface IUpdateCell<out TColor, in TCell>
	{
		public TColor DefaultTextColor { get; }
		public float DefaultFontSize { get; }

		public void SetCell( TCell cell );
		public void Init();

		public void Enable();
		public void Disable();

		public bool UpdateText();
		public bool UpdateColor();
		public bool UpdateFontSize();
		public bool UpdateFont();
		// public abstract bool UpdateAlignment();

		public bool Update( object sender, PropertyChangedEventArgs e );
		public void Update();
		public bool UpdateParent( object sender, PropertyChangedEventArgs e );
	}


	public interface IUpdateEntryCell<in TRenderer, TColor, in TCell> : IUpdateCell<TColor, TCell>
	{
		public Action? ClearFocusAction { get; set; }
		public void Init( EntryCell cell, TRenderer renderer );

		public void PerformSelectAction();
		public bool UpdateSelectAction();
		public bool UpdateTextColor();
		public bool UpdateKeyboard();
		public bool UpdateIsPassword();
		public bool UpdatePlaceholder();
		public bool UpdateTextAlignment();
		public bool UpdateAccentColor();
		public bool ChangeTextViewBack( TColor accent );
	}


	public interface IUpdateICon<in TCell, TImage, in Thandler> : IDisposable
	{
		public bool UpdateIconRadius();
		public bool UpdateIconSize();

		public void SetCell( TCell cell );
		public Size GetIconSize();
		public bool Refresh( bool forceLoad = false );
		public void LoadIconImage( Thandler handler, ImageSource source, CancellationToken token );
		public TImage CreateRoundImage( TImage image );
		public bool UpdateParent( object sender, PropertyChangedEventArgs e );
		public bool Update( object sender, PropertyChangedEventArgs e );
		public bool Update();
	}
}
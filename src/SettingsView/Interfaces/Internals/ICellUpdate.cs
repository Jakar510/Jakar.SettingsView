// unset

using System;
using System.ComponentModel;

#nullable enable
namespace Jakar.SettingsView.Shared.Interfaces
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public interface IUpdateCell : IDisposable
	{
		public float DefaultFontSize { get; }

		public void Initialize();

		public void Enable();
		public void Disable();

		// public bool UpdateProportions();
		public bool UpdateText();
		public bool UpdateTextColor();
		public bool UpdateFontSize();
		public bool UpdateFont();
		// public abstract bool UpdateAlignment();


		public bool Update( object sender, PropertyChangedEventArgs e );
		public void Update();
		public bool UpdateParent( object sender, PropertyChangedEventArgs e );
	}
}
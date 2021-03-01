using System;
using Android.Text;
using Android.Widget;

namespace Jakar.SettingsView.Droid.Interfaces
{
	public interface IEntryCellRenderer : ITextWatcher, Android.Views.View.IOnFocusChangeListener, TextView.IOnEditorActionListener
	{
		public void UpdateWithForceLayout( Action updateAction );
		public bool UpdateWithForceLayout( Func<bool> updateAction );
		public void DoneEdit();
	}
}
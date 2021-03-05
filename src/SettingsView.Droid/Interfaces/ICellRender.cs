using System;
using Android.Text;
using Android.Widget;

namespace Jakar.SettingsView.Droid.Interfaces
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public interface IEntryCellRenderer : ITextWatcher, Android.Views.View.IOnFocusChangeListener, TextView.IOnEditorActionListener
	{
		public void UpdateWithForceLayout( Action updateAction );
		public bool UpdateWithForceLayout( Func<bool> updateAction );
		// public bool UpdateProportions();
		public void DoneEdit();
	}
}
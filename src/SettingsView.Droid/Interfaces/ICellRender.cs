namespace Jakar.SettingsView.Droid.Interfaces;

[Preserve(AllMembers = true)]
public interface IEntryCellRenderer : ITextWatcher, Android.Views.View.IOnFocusChangeListener, TextView.IOnEditorActionListener
{
    public void UpdateWithForceLayout( Action     updateAction );
    public bool UpdateWithForceLayout( Func<bool> updateAction );
    public void DoneEdit();
}
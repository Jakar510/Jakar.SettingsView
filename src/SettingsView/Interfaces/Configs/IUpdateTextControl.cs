namespace Jakar.SettingsView.Shared.Interfaces;

[Xamarin.Forms.Internals.Preserve(true, false)]
public interface IUpdateTextControl
{
    public void Update( IUseConfiguration configuration );
    public void SetText( string?          text );
}



[Xamarin.Forms.Internals.Preserve(true, false)]
public interface IUpdateIconControl
{
    public void Update( IUseIconConfiguration configuration );
}



[Xamarin.Forms.Internals.Preserve(true, false)]
public interface IUpdateAccessoryControl
{
    public bool Checked { get; set; }

    public void Update( IUseCheckableConfiguration configuration );
		
    public void Select();
    public void Deselect();
}
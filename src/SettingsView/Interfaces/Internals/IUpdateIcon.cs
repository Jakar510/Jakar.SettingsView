// unset

namespace Jakar.SettingsView.Shared.Interfaces;

public interface IUpdateIcon<TRenderer, TImage, in THandler> : IDisposable
{
    public TRenderer Renderer { get; }

    public void SetRenderer( TRenderer cell );

    public void Enable();
    public void Disable();

    public bool UpdateIconRadius();
    public bool UpdateIconSize();

    public Size GetIconSize();
    public bool Refresh( bool              forceLoad = false );
    public void LoadIconImage( THandler    handler, ImageSource source, CancellationToken token );
    public TImage CreateRoundImage( TImage image );

    public bool UpdateParent( object sender, PropertyChangedEventArgs e );
    public bool Update( object       sender, PropertyChangedEventArgs e );
    public void Update();
}
// unset

namespace Jakar.SettingsView.Shared.Interfaces;

[Xamarin.Forms.Internals.Preserve(true, false)]
public interface ISectionFooterHeader : IVisualElementController, INotifyPropertyChanged
{
    // https://github.com/muak/AiForms.SettingsView/issues/118

    public View View { get; }

    internal Section? Section { get; set; }

    public void SetText( string?     text );
    public void SetTextColor( Color  color );
    public void SetBackground( Color value );
    public void SetTextFont( double  fontSize, string family, FontAttributes attributes );


    // The following are provided by any Xamarin.Forms.View but are required (no explicit interface available).
    public object?   BindingContext  { get; set; }
    public Element   Parent          { get; set; }
    public double    HeightRequest   { get; set; }
    public Thickness Padding         { get; set; }
    public Color     BackgroundColor { get; set; }
    public bool      IsEnabled       { get; set; }
    public bool      IsVisible       { get; set; }
}
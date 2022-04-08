using Jakar.SettingsView.Shared.Events;


namespace Jakar.SettingsView.Shared.CellBase;

[Xamarin.Forms.Internals.Preserve(true, false)]
public abstract class CellBase : Cell, IVisibleCell
{
    public new event EventHandler<CellTappedEventArgs>? Tapped;

    internal new void OnTapped() { Tapped?.Invoke(this, new CellTappedEventArgs(this)); }


    public static readonly BindableProperty isVisibleProperty = BindableProperty.Create(nameof(IsVisible),
                                                                                        typeof(bool),
                                                                                        typeof(CellBase),
                                                                                        SvConstants.Cell.VISIBLE,
                                                                                        defaultBindingMode: BindingMode.OneWay
                                                                                       );

    public static readonly BindableProperty backgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(CellBase), SvConstants.Cell.color);


    public bool IsVisible
    {
        get => (bool) GetValue(isVisibleProperty);
        set => SetValue(isVisibleProperty, value);
    }

    public Color BackgroundColor
    {
        get => (Color) GetValue(backgroundColorProperty);
        set => SetValue(backgroundColorProperty, value);
    }

    public Section? Section { get; set; }

    public new sv.SettingsView Parent
    {
        get => base.Parent as sv.SettingsView ?? throw new NullReferenceException(nameof(Parent));
        set => base.Parent = value;
    }

    internal Color GetBackground() =>
        BackgroundColor == SvConstants.Cell.color
            ? Parent.CellBackgroundColor
            : BackgroundColor;

    public virtual void Reload()
    {
        if ( Section is null ) { return; }

        int index = Section.IndexOf(this);
        if ( index < 0 ) { return; }

        // raise replace event manually.
        Cell temp = Section[index];
        Section[index] = temp;
    }
}
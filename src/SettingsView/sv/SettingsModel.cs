using Xamarin.Forms.Internals;


namespace Jakar.SettingsView.Shared.sv;

[Preserve(true, false)]
public class SettingsModel : TableModel
{
    // ReSharper disable once InconsistentNaming
    protected static readonly BindableProperty PathProperty = BindableProperty.Create("Path", typeof(Tuple<int, int>), typeof(Cell));

    protected SettingsRoot _Root { get; }


    public SettingsModel( SettingsRoot settingsRoot ) => _Root = settingsRoot;
		

    public override Cell GetCell( int section, int row )
    {
        var cell = (Cell) GetItem(section, row);
        SetPath(cell, new Tuple<int, int>(section, row));
        return cell;
    }
    public override object GetItem( int  section, int row ) => _Root.ElementAt(section)[row] ?? throw new NullReferenceException($"Can't get section number {section} and row number {row}");
    public override int GetRowCount( int section ) => _Root.ElementAt(section).Count;
    public override int GetSectionCount() => _Root.Count();

    public virtual Section? GetSection( int          section ) => _Root.ElementAtOrDefault(section);
    public virtual Section? GetSectionFromCell( Cell cell ) { return _Root.FirstOrDefault(x => x.Contains(cell)); }
    public virtual (int section, int cell) GetIndexesFromCell( Cell cell )
    {
        Section? section = _Root.FirstOrDefault(x => x.Contains(cell));
        return section is null
                   ? ( -1, -1 )
                   : ( GetSectionIndex(section), section.IndexOf(cell) );
    }
    public virtual int GetSectionIndex( Section section ) => _Root.IndexOf(section);

    public override string? GetSectionTitle( int section ) => _Root.ElementAt(section).Title;


    public virtual ISectionHeader GetSectionHeaderView( int section ) => _Root.ElementAt(section).HeaderView;
    public virtual string? GetFooterText( int               section ) => _Root.ElementAt(section).FooterText;
    public virtual ISectionFooter GetSectionFooterView( int section ) => _Root.ElementAt(section).FooterView;

    protected override void OnRowSelected( object item )
    {
        base.OnRowSelected(item);

        ( item as CellBase.CellBase )?.OnTapped();
    }

    // public virtual double GetHeaderHeight( int section ) => _root.ElementAt(section).HeaderHeight;
    // public virtual double GetHeaderHeight( int section ) => _root.ElementAt(section).HeaderView.Height;
    public virtual double GetHeaderHeight( int section ) => _Root.ElementAt(section).HeaderView.HeightRequest;


    // this method no longer uses except for iOS.CellBaseRenderer.
    internal static Tuple<int, int> GetPath( Cell item )
    {
        if ( item is null ) throw new ArgumentNullException(nameof(item));

        return (Tuple<int, int>) item.GetValue(PathProperty);
    }

    // ReSharper disable once SuggestBaseTypeForParameter
    protected static void SetPath( Cell item, Tuple<int, int> index ) { item.SetValue(PathProperty, index); }
}
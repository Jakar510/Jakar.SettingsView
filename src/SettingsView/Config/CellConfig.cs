using Jakar.SettingsView.Shared.Interfaces;

#nullable enable
namespace Jakar.SettingsView.Shared.Config
{
	public class CellConfig : BaseConfig, IParent<CellBase.CellBase>
	{
		public CellBase.CellBase? Parent { get; set; }

		// public CellBase.CellBase? Parent
		// {
		// 	get => _parent;
		// 	set
		// 	{
		// 		if ( _parent is not null ) { PropertyChanged -= _parent.ParentOnPropertyChanged; }
		//
		// 		_parent = value;
		// 		if ( _parent is not null ) { PropertyChanged += _parent.ParentOnPropertyChanged; }
		// 	}
		// }
	}

	// public class CellConfig
	// {
	// 	public Color BackgroundColor { get; init; }
	// 	public Color AccentColor { get; init; }
	//
	// 	public ItemConfig? Title { get; init; }
	// 	public ItemConfig? Description { get; init; }
	// 	public ItemConfig? Hint { get; init; }
	// 	public ItemConfig? Value { get; init; }
	//
	// 	public PopupConfig? Popup { get; init; }
	// 	public CellConfig() { }
	// }
}
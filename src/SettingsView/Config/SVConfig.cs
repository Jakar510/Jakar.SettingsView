// unset

using Jakar.SettingsView.Shared.Interfaces;

#nullable enable
namespace Jakar.SettingsView.Shared.Config
{
	public class SVConfig : BaseConfig, IParent<sv.SettingsView>
	{
		private sv.SettingsView? _parent;

		public sv.SettingsView? Parent
		{
			get => _parent;
			set
			{
				if ( _parent is not null ) { PropertyChanged -= _parent.ParentOnPropertyChanged; }

				_parent = value;
				if ( _parent is not null ) { PropertyChanged += _parent.ParentOnPropertyChanged; }
			}
		}
	}
}
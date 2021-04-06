// unset

using Jakar.SettingsView.Shared.Interfaces;

#nullable enable
namespace Jakar.SettingsView.Shared.Config
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public class SvConfig : BaseConfig, IParent<sv.SettingsView>
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
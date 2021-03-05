// unset

using System.ComponentModel;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.Config
{
	public class BaseConfig : BindableObject
	{
		protected void ParentOnPropertyChanged( object sender, PropertyChangedEventArgs e ) { base.OnPropertyChanged(e.PropertyName); }

	}
}
// unset

using System;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Interfaces;

#nullable enable
namespace Jakar.SettingsView.Shared.sv
{
	public abstract class FooterView : BaseHeaderFooterView, ISectionFooter
	{
		protected FooterView() => BackgroundColor = SVConstants.FOOTER_BACKGROUND_COLOR;
	}
}
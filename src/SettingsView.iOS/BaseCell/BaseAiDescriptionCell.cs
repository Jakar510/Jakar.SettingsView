using System;
using System.ComponentModel;
using Jakar.SettingsView.iOS.Controls;
using UIKit;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.iOS.BaseCell
{
	[Foundation.Preserve(AllMembers = true)]
	public abstract class BaseAiDescriptionCell : BaseAiTitledCell
	{
		protected IconView _Icon { get; }
		protected DescriptionView _Description { get; }
		protected UIStackView ValueStack { get; }

		protected BaseAiDescriptionCell( Cell cell ) : base(cell)
		{
			_Description = new DescriptionView(this);
			_Icon = new IconView(this);
		}

		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Icon.Update(sender, e) ) return;
			if ( _Description.Update(sender, e) ) return;
			base.CellPropertyChanged(sender, e);
		}
		protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Description.UpdateParent(sender, e) ) return;
			if ( _Icon.UpdateParent(sender, e) ) return;
			base.ParentPropertyChanged(sender, e);
		}

		protected override void EnableCell()
		{
			base.EnableCell();
			_Description.Enable();
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Description.Disable();
		}

		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			_Icon.Update();
			_Description.Update();
		}

		protected override void Dispose( bool disposing )
		{
			base.Dispose(disposing);
			_Icon.Dispose();
			_Description.Dispose();
		}
	}
}
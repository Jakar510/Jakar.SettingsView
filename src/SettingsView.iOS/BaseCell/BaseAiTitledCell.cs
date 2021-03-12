using System;
using System.ComponentModel;
using Jakar.SettingsView.iOS.Controls;
using Jakar.SettingsView.iOS.Controls.Core;
using UIKit;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.iOS.BaseCell
{
	[Foundation.Preserve(AllMembers = true)]
	public abstract class BaseAiTitledCell : BaseCellView
	{
		protected TitleView _Title { get; }

		protected BaseAiTitledCell( Cell cell ) : base(cell) => _Title = new TitleView(this);


		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Title.Update(sender, e) ) return;
			base.CellPropertyChanged(sender, e);
		}
		protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Title.UpdateParent(sender, e) ) return;
			base.ParentPropertyChanged(sender, e);
		}

		protected override void EnableCell()
		{
			base.EnableCell();
			_Title.Enable();
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Title.Disable();
		}

		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			_Title.Update();
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_Title.RemoveFromSuperview();
				_Title.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}
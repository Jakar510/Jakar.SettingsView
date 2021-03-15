using System;
using System.ComponentModel;
using Jakar.SettingsView.iOS.Controls;
using Jakar.SettingsView.iOS.Controls.Core;
using Jakar.SettingsView.Shared.Config;
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
		protected UIStackView _TitleStack { get; }

		protected BaseAiDescriptionCell( Cell cell ) : base(cell)
		{
			// https://stackoverflow.com/a/60832786/9530917
			// _________________________________________________________________________________


			_Icon = new IconView(this);
			_ContentView.AddArrangedSubview(_Icon);

			_Icon.WidthAnchor.ConstraintEqualTo(_ContentView.WidthAnchor, SVConstants.Layout.ColumnFactors.Icon).Active = true;

			_Icon.HeightAnchor.ConstraintEqualTo(_ContentView.HeightAnchor).Active = true;
			_Icon.LeadingAnchor.ConstraintEqualTo(_ContentView.LeadingAnchor).Active = true;

			_Icon.CenterYAnchor.ConstraintEqualTo(_ContentView.CenterYAnchor).Active = true;

			// -----------------------------------------------------------------------------------

			_TitleStack = CreateStackView(UILayoutConstraintAxis.Vertical);
			_ContentView.AddArrangedSubview(_TitleStack);

			_TitleStack.HeightAnchor.ConstraintEqualTo(_ContentView.HeightAnchor).Active = true;

			NSLayoutConstraint width = _TitleStack.WidthAnchor.ConstraintGreaterThanOrEqualTo(_ContentView.WidthAnchor, SVConstants.Layout.ColumnFactors.TitleStack);
			width.Active = true;
			width.Priority = SVConstants.Layout.Priority.HIGH;
			_ContentView.AddConstraint(width);

			_TitleStack.AddArrangedSubview(_Title);
			_Title.WidthAnchor.ConstraintEqualTo(_TitleStack.WidthAnchor).Active = true;

			// -----------------------------------------------------------------------------------
			_Description = new DescriptionView(this);
			_TitleStack.AddArrangedSubview(_Description);
			_Description.WidthAnchor.ConstraintEqualTo(_TitleStack.WidthAnchor).Active = true;

			// -----------------------------------------------------------------------------------
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
			if ( disposing )
			{
				_Icon.RemoveFromSuperview();
				_Icon.Dispose();

				_Description.RemoveFromSuperview();
				_Description.Dispose();

				_TitleStack.RemoveFromSuperview();
				_TitleStack.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}
using System;
using System.ComponentModel;
using Foundation;
using Jakar.SettingsView.iOS.Extensions;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Misc;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


#nullable enable
namespace Jakar.SettingsView.iOS.BaseCell
{
	[Preserve(AllMembers = true)]
	public abstract class BaseCellView : CellTableViewCell
	{
		private UIStackView? _rootView;
		private UIStackView? _contentView;

		protected UIStackView _RootView
		{
			get => _rootView ?? throw new NullReferenceException(nameof(_rootView));
			set
			{
				if ( _rootView is not null )
				{
					foreach ( NSLayoutConstraint constraint in _rootView.Constraints )
					{
						constraint.Active = false;
						constraint.Dispose();
					}

					_rootView.RemoveFromSuperview();
				}
				
				_rootView = value;
				ContentView.AddSubview(_rootView);

				// _rootView.WidthAnchor.ConstraintEqualTo(ContentView.WidthAnchor).Active = true;
				// _rootView.LeadingAnchor.ConstraintEqualTo(ContentView.LeadingAnchor).Active = true;
				// _rootView.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor).Active = true;

				_rootView.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor).Active = true;
				_rootView.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor).Active = true;

				_rootView.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
				_rootView.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;

				_rootView.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);
				_rootView.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Vertical);

				_rootView.SetContentHuggingPriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);
				_rootView.SetContentHuggingPriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Vertical);

				NSLayoutConstraint height = _RootView.HeightAnchor.ConstraintGreaterThanOrEqualTo(SVConstants.Defaults.MIN_ROW_HEIGHT.ToNFloat());
				height.Active = true;
				height.Priority = SVConstants.Layout.Priority.HIGH;
			}
		}

		protected UIStackView _ContentView
		{
			get => _contentView ?? throw new NullReferenceException(nameof(_contentView));
			set
			{
				if ( _contentView is not null )
				{
					foreach ( NSLayoutConstraint constraint in _contentView.Constraints )
					{
						constraint.Active = false;
						constraint.Dispose();
					}

					_contentView.RemoveFromSuperview();
				}

				_contentView = value;
				_RootView.AddArrangedSubview(_contentView);
				_contentView.WidthAnchor.ConstraintEqualTo(_RootView.WidthAnchor).Active = true;
			}
		}

		protected internal CellBase? CellBase => Cell as CellBase;                                      // ?? throw new NullReferenceException(nameof(CellBase));
		protected internal Shared.sv.SettingsView? CellParent => Cell.Parent as Shared.sv.SettingsView; // ?? throw new NullReferenceException(nameof(CellParent));

		protected internal static UIStackView CreateStackView( UILayoutConstraintAxis orientation ) =>
			new()
			{
				Alignment = orientation == UILayoutConstraintAxis.Vertical
								? UIStackViewAlignment.Center
								: UIStackViewAlignment.Fill,
				Distribution = UIStackViewDistribution.Fill,
				Axis = orientation,
				Spacing = (nfloat) ( orientation == UILayoutConstraintAxis.Vertical
										 ? SVConstants.Cell.PADDING.Top
										 : SVConstants.Cell.PADDING.Left ),
				BackgroundColor = Color.Transparent.ToUIColor()
			};
		protected BaseCellView( Cell cell ) : base(UITableViewCellStyle.Default, cell.GetType().FullName)
		{
			Cell = cell;
			_RootView = CreateStackView(UILayoutConstraintAxis.Vertical);
			_ContentView = CreateStackView(UILayoutConstraintAxis.Horizontal);
		}
		private void UpdateSelectedColor()
		{
			if ( CellParent is null ||
				 CellParent.SelectedColor == Color.Default ) return;

			if ( SelectedBackgroundView is not null ) { SelectedBackgroundView.BackgroundColor = CellParent.SelectedColor.ToUIColor(); }
			else
			{
				SelectedBackgroundView = new UIView
										 {
											 BackgroundColor = CellParent.SelectedColor.ToUIColor()
										 };
			}
		}

		protected internal virtual void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(CellBase.IsVisibleProperty) ) { UpdateIsVisible(); }
			else if ( e.IsEqual(Cell.IsEnabledProperty) ) { UpdateIsEnabled(); }
			else if ( e.IsEqual(CellBase.BackgroundColorProperty) ) { UpdateBackgroundColor(); }
		}
		protected internal virtual void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(Shared.sv.SettingsView.SelectedColorProperty) ) { UpdateSelectedColor(); }
		}
		protected internal virtual void SectionPropertyChanged( object sender, PropertyChangedEventArgs e ) { }

		protected void UpdateIsVisible() { CellBase?.Section?.ShowVisibleCells(); }


		public void UpdateWithForceLayout( Action updateAction )
		{
			updateAction();
			SetNeedsLayout();
		}
		public bool UpdateWithForceLayout( Func<bool> updateAction )
		{
			bool result = updateAction();
			SetNeedsLayout();
			return result;
		}


		protected internal virtual void RowSelected( UITableView tableView, NSIndexPath indexPath ) { }
		protected internal virtual bool RowLongPressed( UITableView tableView, NSIndexPath indexPath ) => false;


		protected internal virtual void UpdateCell() => UpdateCell(null);
		protected internal virtual void UpdateCell( UITableView? parent )
		{
			UpdateBackgroundColor();
			UpdateIsEnabled();
			SetNeedsLayout();
			UpdateSelectedColor();
		}


		protected virtual void UpdateIsEnabled() { SetEnabledAppearance(Cell.IsEnabled); }
		protected void SetEnabledAppearance( bool isEnabled )
		{
			if ( isEnabled ) { EnableCell(); }
			else { DisableCell(); }
		}
		protected virtual void EnableCell()
		{
			// not to invoke a ripple effect and not to selected
			UserInteractionEnabled = false;
		}
		protected virtual void DisableCell()
		{
			// not to invoke a ripple effect and not to selected
			UserInteractionEnabled = true;
		}


		protected void SetRightMarginZero()
		{
			if ( !UIDevice.CurrentDevice.CheckSystemVersion(11, 0) ) return;
			//StackH.LayoutMargins = new UIEdgeInsets(6, 16, 6, 0);
			UIEdgeInsets margins = _RootView.LayoutMargins;
			margins.Left = 0;
			_RootView.LayoutMargins = margins;
		}
		protected virtual void UpdateBackgroundColor() { BackgroundColor = ( CellBase?.BackgroundColor ?? SVConstants.Cell.COLOR ).ToUIColor(); }


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( CellBase != null )
				{
					CellBase.PropertyChanged -= CellPropertyChanged;
					if ( CellParent != null ) CellParent.PropertyChanged -= ParentPropertyChanged;

					if ( CellBase.Section != null )
					{
						CellBase.Section.PropertyChanged -= SectionPropertyChanged;
						CellBase.Section = null;
					}
				}
			}

			base.Dispose(disposing);
		}
	}
}
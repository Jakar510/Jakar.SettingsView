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
		// internal NSLayoutConstraint LeftMarginConstraint { get; private set; }
		// internal NSLayoutConstraint RightMarginConstraint { get; private set; }
		// internal NSLayoutConstraint TopMarginConstraint { get; private set; }
		// internal NSLayoutConstraint BottomMarginConstraint { get; private set; }
		// internal NSLayoutConstraint WidthConstraint { get; set; }
		// internal NSLayoutConstraint MinHeightConstraint { get; set; }

		// RightMarginConstraint = NSLayoutConstraint.Create(this,
		// 												  NSLayoutAttribute.TrailingMargin,
		// 												  NSLayoutRelation.Equal,
		// 												  _ContentView,
		// 												  NSLayoutAttribute.TrailingMargin,
		// 												  Factor.One,
		// 												  (float) SVConstants.Cell.PADDING.Left
		// 												 );
		// LeftMarginConstraint = NSLayoutConstraint.Create(this,
		// 												 NSLayoutAttribute.LeadingMargin,
		// 												 NSLayoutRelation.Equal,
		// 												 _ContentView,
		// 												 NSLayoutAttribute.LeadingMargin,
		// 												 Factor.One,
		// 												 (float) SVConstants.Cell.PADDING.Right
		// 												);
		//
		// TopMarginConstraint = NSLayoutConstraint.Create(this,
		// 												NSLayoutAttribute.TopMargin,
		// 												NSLayoutRelation.Equal,
		// 												_ContentView,
		// 												NSLayoutAttribute.TopMargin,
		// 												Factor.One,
		// 												(float) SVConstants.Cell.PADDING.Top
		// 											   );
		// BottomMarginConstraint = NSLayoutConstraint.Create(this,
		// 												   NSLayoutAttribute.BottomMargin,
		// 												   NSLayoutRelation.Equal,
		// 												   _ContentView,
		// 												   NSLayoutAttribute.BottomMargin,
		// 												   Factor.One,
		// 												   (float) SVConstants.Cell.PADDING.Bottom
		// 												  );
		//
		// WidthConstraint = NSLayoutConstraint.Create(this,
		// 											NSLayoutAttribute.Width,
		// 											NSLayoutRelation.GreaterThanOrEqual,
		// 											_ContentView,
		// 											NSLayoutAttribute.Width,
		// 											Factor.One,
		// 											Factor.Zero
		// 										   );
		// MinHeightConstraint = NSLayoutConstraint.Create(this,
		// 												NSLayoutAttribute.Height,
		// 												NSLayoutRelation.GreaterThanOrEqual,
		// 												_ContentView,
		// 												NSLayoutAttribute.Height,
		// 												Factor.One,
		// 												(float) SVConstants.Defaults.MIN_ROW_HEIGHT
		// 											   );
		// _ContentView.AddConstraint(RightMarginConstraint);
		// _ContentView.AddConstraint(LeftMarginConstraint);
		// _ContentView.AddConstraint(BottomMarginConstraint);
		// _ContentView.AddConstraint(TopMarginConstraint);
		// _ContentView.AddConstraint(WidthConstraint);
		// _ContentView.AddConstraint(MinHeightConstraint);

		private UIStackView? _rootView;

		protected UIStackView _RootView
		{
			get => _rootView ?? throw new NullReferenceException(nameof(_rootView));
			set
			{
				_rootView?.RemoveFromSuperview();
				_rootView = value;
				ContentView.AddSubview(_rootView);
			}
		}

		protected UIStackView _ContentView { get; set; }

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
			_RootView.WidthAnchor.ConstraintEqualTo(ContentView.WidthAnchor).Active = true;

			_RootView.LeadingAnchor.ConstraintEqualTo(ContentView.LeadingAnchor).Active = true;
			_RootView.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor).Active = true;

			_RootView.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
			_RootView.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;

			_RootView.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.DefaultHigh, UILayoutConstraintAxis.Horizontal);
			_RootView.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.DefaultHigh, UILayoutConstraintAxis.Vertical);

			_RootView.SetContentHuggingPriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);
			_RootView.SetContentHuggingPriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Vertical);

			NSLayoutConstraint height = _RootView.HeightAnchor.ConstraintGreaterThanOrEqualTo(SVConstants.Defaults.MIN_ROW_HEIGHT.ToNFloat());
			height.Active = true;
			height.Priority = SVConstants.Layout.Priority.Required;


			_ContentView = CreateStackView(UILayoutConstraintAxis.Horizontal);
			_ContentView.WidthAnchor.ConstraintEqualTo(_RootView.WidthAnchor).Active = true;
			_RootView.AddArrangedSubview(_ContentView);
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
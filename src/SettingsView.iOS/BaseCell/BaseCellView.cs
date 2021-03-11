using System;
using System.ComponentModel;
using Foundation;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


#nullable enable
namespace Jakar.SettingsView.iOS.BaseCell
{
	[Foundation.Preserve(AllMembers = true)]
	public abstract class BaseCellView : CellTableViewCell
	{
		public static class Factor
		{
			public const float Zero = 0;
			public const float One = 1;
		}

		public static class Priority
		{
			public const float Zero = 0;                                                                       // 0
			public const float LOW = 1;                                                                        // 1
			public const float FittingSizeLevel = (float) UILayoutPriority.FittingSizeLevel;                   // 50
			public const float DefaultLow = (float) UILayoutPriority.DefaultLow;                               // 250
			public const float DragThatCannotResizeScene = (float) UILayoutPriority.DragThatCannotResizeScene; // 490
			public const float SceneSizeStayPut = (float) UILayoutPriority.SceneSizeStayPut;                   // 500
			public const float DragThatCanResizeScene = (float) UILayoutPriority.DragThatCanResizeScene;       // 510
			public const float DefaultHigh = (float) UILayoutPriority.DefaultHigh;                             // 750
			public const float HIGH = 999;                                                                     // 999
			public const float Required = (float) UILayoutPriority.Required;                                   // 1000
		}

		internal NSLayoutConstraint LeftMarginConstraint { get; private set; }
		internal NSLayoutConstraint RightMarginConstraint { get; private set; }
		internal NSLayoutConstraint TopMarginConstraint { get; private set; }
		internal NSLayoutConstraint BottomMarginConstraint { get; private set; }
		internal NSLayoutConstraint WidthConstraint { get; set; }
		internal NSLayoutConstraint MinHeightConstraint { get; set; }

		private UIStackView? _contentView;

		protected UIStackView _ContentView
		{
			get => _contentView ?? throw new NullReferenceException(nameof(_contentView));
			set
			{
				_contentView?.RemoveFromSuperview();
				_contentView = value;
				Add(_contentView);
			}
		}

		protected internal CellBase? CellBase => Cell as CellBase;                                      // ?? throw new NullReferenceException(nameof(CellBase));
		protected internal Shared.sv.SettingsView? CellParent => Cell.Parent as Shared.sv.SettingsView; // ?? throw new NullReferenceException(nameof(CellParent));

		protected internal static UIStackView CreateStackView( UILayoutConstraintAxis orientation ) =>
			new()
			{
				Alignment = UIStackViewAlignment.Fill,
				Axis = orientation,
				BackgroundColor = Color.Transparent.ToUIColor()
			};
		protected BaseCellView( Cell cell ) : base(UITableViewCellStyle.Default, cell.GetType().FullName)
		{
			// UIKit.UIStackView
			// UIKit.UICollectionView
			// UIKit.UICollectionViewCell
			// UIKit.UITableView

			Cell = cell;
			_ContentView = CreateStackView(UILayoutConstraintAxis.Horizontal);

			// _ContentView.DataSource = new UICollectionViewController();
			_ContentView.SetContentCompressionResistancePriority(Priority.DefaultHigh, UILayoutConstraintAxis.Horizontal);
			_ContentView.SetContentCompressionResistancePriority(Priority.DefaultHigh, UILayoutConstraintAxis.Vertical);

			_ContentView.SetContentHuggingPriority((float) UILayoutPriority.Required, UILayoutConstraintAxis.Horizontal);
			_ContentView.SetContentHuggingPriority((float) UILayoutPriority.Required, UILayoutConstraintAxis.Vertical);

			RightMarginConstraint = NSLayoutConstraint.Create(this,
															  NSLayoutAttribute.TrailingMargin,
															  NSLayoutRelation.Equal,
															  _ContentView,
															  NSLayoutAttribute.TrailingMargin,
															  Factor.One,
															  (float) SVConstants.Cell.PADDING.Left
															 );
			LeftMarginConstraint = NSLayoutConstraint.Create(this,
															 NSLayoutAttribute.LeadingMargin,
															 NSLayoutRelation.Equal,
															 _ContentView,
															 NSLayoutAttribute.LeadingMargin,
															 Factor.One,
															 (float) SVConstants.Cell.PADDING.Right
															);

			TopMarginConstraint = NSLayoutConstraint.Create(this,
															NSLayoutAttribute.TopMargin,
															NSLayoutRelation.Equal,
															_ContentView,
															NSLayoutAttribute.TopMargin,
															Factor.One,
															(float) SVConstants.Cell.PADDING.Top
														   );
			BottomMarginConstraint = NSLayoutConstraint.Create(this,
															   NSLayoutAttribute.BottomMargin,
															   NSLayoutRelation.Equal,
															   _ContentView,
															   NSLayoutAttribute.BottomMargin,
															   Factor.One,
															   (float) SVConstants.Cell.PADDING.Bottom
															  );

			WidthConstraint = NSLayoutConstraint.Create(this,
														NSLayoutAttribute.Width,
														NSLayoutRelation.GreaterThanOrEqual,
														_ContentView,
														NSLayoutAttribute.Width,
														Factor.One,
														Factor.Zero
													   );
			MinHeightConstraint = NSLayoutConstraint.Create(this,
															NSLayoutAttribute.Height,
															NSLayoutRelation.GreaterThanOrEqual,
															_ContentView,
															NSLayoutAttribute.Height,
															Factor.One,
															(float) SVConstants.Defaults.MIN_ROW_HEIGHT
														   );
			_ContentView.AddConstraint(RightMarginConstraint);
			_ContentView.AddConstraint(LeftMarginConstraint);
			_ContentView.AddConstraint(BottomMarginConstraint);
			_ContentView.AddConstraint(TopMarginConstraint);
			_ContentView.AddConstraint(WidthConstraint);
			_ContentView.AddConstraint(MinHeightConstraint);

			ContentView.AddSubview(_ContentView);
		}
		private void UpdateSelectedColor()
		{
			if ( CellParent == null ||
				 CellParent.SelectedColor == Color.Default ) return;

			if ( SelectedBackgroundView != null ) { SelectedBackgroundView.BackgroundColor = CellParent.SelectedColor.ToUIColor(); }
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
			if ( e.PropertyName == CellBase.IsVisibleProperty.PropertyName ) { UpdateIsVisible(); }
			else if ( e.PropertyName == Cell.IsEnabledProperty.PropertyName ) { UpdateIsEnabled(); }
			else if ( e.PropertyName == CellBase.BackgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }
		}
		protected internal virtual void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.sv.SettingsView.SelectedColorProperty.PropertyName ) { UpdateSelectedColor(); }
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


		protected virtual void UpdateBackgroundColor() { }


		protected internal void HideKeyboard( UIView inputView ) { }
		protected internal void ShowKeyboard( UIView inputView ) { }

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
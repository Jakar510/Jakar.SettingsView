using System;
using System.ComponentModel;
using CoreGraphics;
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
		// hide unused native controls
#pragma warning disable IDE1006 // Naming Styles
		private new UIImageView ImageView => base.ImageView;
		private new UILabel TextLabel => base.TextLabel;
#pragma warning restore IDE1006 // Naming Styles


		private NSLayoutConstraint? _minHeightConstraint;
		private UIStackView? _rootView;

		protected UIStackView _RootView
		{
			get => _rootView ?? throw new NullReferenceException(nameof(_rootView));
			set
			{
				if ( _minHeightConstraint is not null )
				{
					_minHeightConstraint.Active = false;
					_minHeightConstraint.Dispose();
					_minHeightConstraint = null;
				}

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
				if ( _rootView is null ) return;
				ContentView.AddSubview(_rootView);

				_rootView.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);
				_rootView.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Vertical);

				_rootView.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Horizontal);
				_rootView.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Vertical);

				_rootView.TranslatesAutoresizingMaskIntoConstraints = false;
				_rootView.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor).Active = true;
				_rootView.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor).Active = true;
				_rootView.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
				_rootView.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;

				double minHeight = Math.Max(CellParent?.RowHeight ?? -1, SVConstants.Defaults.MIN_ROW_HEIGHT);
				_minHeightConstraint = _rootView.HeightAnchor.ConstraintGreaterThanOrEqualTo(minHeight.ToNFloat());
				_minHeightConstraint.Priority = SVConstants.Layout.Priority.HIGH; // this is superior to any other view.
				_minHeightConstraint.Active = true;


				// _rootView.WidthAnchor.ConstraintEqualTo(ContentView.WidthAnchor).Active = true;
				// _rootView.LeadingAnchor.ConstraintEqualTo(ContentView.LeadingAnchor).Active = true;
				// _rootView.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor).Active = true;


				// NSLayoutConstraint height = _rootView.HeightAnchor.ConstraintGreaterThanOrEqualTo(SVConstants.Defaults.MIN_ROW_HEIGHT.ToNFloat());
				// height.Active = true;
				// height.Priority = SVConstants.Layout.Priority.HIGH;
				// ContentView.AddConstraint(height);


				// foreach ( NSLayoutConstraint constraint in _rootView.Constraints ) { Console.WriteLine(constraint); }
				// foreach ( NSLayoutConstraint constraint in ContentView.Constraints ) { Console.WriteLine(constraint); }
			}
		}


		private UIStackView? _contentView;

		protected UIStackView? _ContentView
		{
			get => _contentView;
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
				if ( _contentView is null ) return;
				if ( _RootView is null ) throw new NullReferenceException(nameof(_RootView));

				_RootView.AddArrangedSubview(_contentView);
				_contentView.WidthAnchor.ConstraintEqualTo(_RootView.WidthAnchor).Active = true;
			}
		}


		protected internal CellBase? CellBase => Cell as CellBase;                                      // ?? throw new NullReferenceException(nameof(CellBase));
		protected internal Shared.sv.SettingsView? CellParent => Cell.Parent as Shared.sv.SettingsView; // ?? throw new NullReferenceException(nameof(CellParent));


		protected BaseCellView( Cell cell ) : base(UITableViewCellStyle.Default, cell.GetType().FullName)
		{
			// remove existing native controls/views 
			ImageView.RemoveFromSuperview();
			TextLabel.RemoveFromSuperview();
			ImageView.Hidden = true;
			TextLabel.Hidden = true;

			// this.ContentView
			// this.BackgroundView
			// this.AccessoryView
			// this.ImageView

			// this.InputView
			// this.InputViewController

			Cell = cell;
			_RootView = CreateStackView(UILayoutConstraintAxis.Vertical);
		}


		protected internal static UIStackView CreateStackView( UILayoutConstraintAxis orientation ) =>
			new()
			{
				Alignment = orientation == UILayoutConstraintAxis.Vertical
								? UIStackViewAlignment.Center
								: UIStackViewAlignment.Fill,
				Distribution = orientation == UILayoutConstraintAxis.Vertical
								   ? UIStackViewDistribution.Fill
								   : UIStackViewDistribution.FillProportionally,
				Axis = orientation,
				Spacing = ( orientation == UILayoutConstraintAxis.Vertical
								? SVConstants.Cell.PADDING.Top
								: SVConstants.Cell.PADDING.Left ).ToNFloat(),
				BackgroundColor = Color.Transparent.ToUIColor(),
				LayoutMargins = SVConstants.Cell.PADDING.ToUIEdgeInsets(),
				LayoutMarginsRelativeArrangement = true
			};
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


		protected virtual void UpdateBackgroundColor() { BackgroundColor = ( CellBase?.GetBackground() ?? SVConstants.Cell.COLOR ).ToUIColor(); }


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

				_RootView.Dispose();
				_rootView = null;

				_ContentView?.Dispose();
				_ContentView = null;

				if ( _minHeightConstraint is not null )
				{
					_minHeightConstraint.Active = false;
					_minHeightConstraint.Dispose();
					_minHeightConstraint = null;
				}
			}

			base.Dispose(disposing);
		}
	}


	// public class CustomVegeCell : UITableViewCell
	// {
	// 	// https://docs.microsoft.com/en-us/xamarin/ios/user-interface/controls/tables/customizing-table-appearance
	// 	private readonly UILabel headingLabel;
	// 	private readonly UILabel subheadingLabel;
	// 	private readonly UIImageView imageView;
	// 	public CustomVegeCell( NSString cellId ) : base(UITableViewCellStyle.Default, cellId)
	// 	{
	// 		SelectionStyle = UITableViewCellSelectionStyle.Gray;
	// 		ContentView.BackgroundColor = UIColor.FromRGB(218, 255, 127);
	// 		imageView = new UIImageView();
	// 		headingLabel = new UILabel()
	// 					   {
	// 						   Font = UIFont.FromName("Cochin-BoldItalic", 22f),
	// 						   TextColor = UIColor.FromRGB(127, 51, 0),
	// 						   BackgroundColor = UIColor.Clear
	// 					   };
	// 		subheadingLabel = new UILabel()
	// 						  {
	// 							  Font = UIFont.FromName("AmericanTypewriter", 12f),
	// 							  TextColor = UIColor.FromRGB(38, 127, 0),
	// 							  TextAlignment = UITextAlignment.Center,
	// 							  BackgroundColor = UIColor.Clear
	// 						  };
	// 		ContentView.AddSubviews(headingLabel, subheadingLabel, imageView);
	// 	}
	// 	public void UpdateCell( string caption, string subtitle, UIImage image )
	// 	{
	// 		imageView.Image = image;
	// 		headingLabel.Text = caption;
	// 		subheadingLabel.Text = subtitle;
	// 	}
	// 	public override void LayoutSubviews()
	// 	{
	// 		// Rectangle rect = ContentView.Bounds.ToRectangle();
	// 		base.LayoutSubviews();
	// 		imageView.Frame = new CGRect(ContentView.Bounds.Width - 63, 5, 33, 33);
	// 		headingLabel.Frame = new CGRect(5, 4, ContentView.Bounds.Width - 63, 25);
	// 		subheadingLabel.Frame = new CGRect(100, 18, 100, 20);
	// 	}
	// }
}
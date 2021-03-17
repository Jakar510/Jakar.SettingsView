using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using CoreFoundation;
using Foundation;
using Jakar.SettingsView.iOS.Extensions;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Misc;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#nullable enable
namespace Jakar.SettingsView.iOS.BaseCell
{
	/// <summary>
	/// Cell base view.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class BaseCellView : CellTableViewCell
	{
		public UILabel? HintLabel { get; set; }
		public UILabel? TitleLabel { get; set; }
		public UILabel? DescriptionLabel { get; set; }

		public UIImageView? IconView { get; set; }
		protected UIStackView? _ContentStack { get; set; }

		protected UIStackView? _StackH { get; set; }
		protected UIStackView? _StackV { get; set; }

		protected CellBase? _CellBase => Cell as CellBase;
		public Shared.sv.SettingsView? CellParent => Cell.Parent as Shared.sv.SettingsView;


		protected IconCellBase? _IconCell => Cell as IconCellBase;
		protected TitleCellBase? _TitleCell => Cell as TitleCellBase;
		protected DescriptionCellBase? _DescriptionCell => Cell as DescriptionCellBase;
		protected HintTextCellBase? _HintCell => Cell as HintTextCellBase;
		protected ValueCellBase? _ValueCell => Cell as ValueCellBase;
		protected ValueTextCellBase? _ValueTextCell => Cell as ValueTextCellBase;


		protected Size _IconSize { get; set; }
		protected NSLayoutConstraint? _IconConstraintHeight { get; set; }
		protected NSLayoutConstraint? _IconConstraintWidth { get; set; }
		protected NSLayoutConstraint? _MinHeightConstraint { get; set; }
		protected CancellationTokenSource? _IconTokenSource { get; set; }

		public BaseCellView( Cell formsCell ) : base(UITableViewCellStyle.Default, formsCell.GetType().FullName)
		{
			SelectionStyle = UITableViewCellSelectionStyle.None;
			Cell = formsCell;
			SetUpContentView();
		}

		public virtual void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == TitleCellBase.TitleProperty.PropertyName ) { UpdateTitleText(); }
			else if ( e.PropertyName == TitleCellBase.TitleColorProperty.PropertyName ) { UpdateTitleColor(); }
			else if ( e.PropertyName == TitleCellBase.TitleFontSizeProperty.PropertyName ||
					  e.PropertyName == TitleCellBase.TitleFontFamilyProperty.PropertyName ||
					  e.PropertyName == TitleCellBase.TitleFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateTitleFont); }

			else if ( e.PropertyName == DescriptionCellBase.DescriptionProperty.PropertyName ) { UpdateWithForceLayout(UpdateDescriptionText); }
			else if ( e.PropertyName == DescriptionCellBase.DescriptionFontSizeProperty.PropertyName ||
					  e.PropertyName == DescriptionCellBase.DescriptionFontFamilyProperty.PropertyName ||
					  e.PropertyName == DescriptionCellBase.DescriptionFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateDescriptionFont); }
			else if ( e.PropertyName == DescriptionCellBase.DescriptionColorProperty.PropertyName ) { UpdateDescriptionColor(); }

			else if ( e.PropertyName == CellBase.BackgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }
			else if ( e.PropertyName == Cell.IsEnabledProperty.PropertyName ) { UpdateIsEnabled(); }

			else if ( e.PropertyName == HintTextCellBase.HintProperty.PropertyName ) { UpdateWithForceLayout(UpdateHintText); }
			else if ( e.PropertyName == HintTextCellBase.HintColorProperty.PropertyName ) { UpdateHintTextColor(); }
			else if ( e.PropertyName == HintTextCellBase.HintFontSizeProperty.PropertyName ||
					  e.PropertyName == HintTextCellBase.HintFontFamilyProperty.PropertyName ||
					  e.PropertyName == HintTextCellBase.HintFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateHintFont); }

			else if ( e.PropertyName == IconCellBase.IconSizeProperty.PropertyName ) { UpdateWithForceLayout(UpdateIconSize); }
			else if ( e.PropertyName == IconCellBase.IconRadiusProperty.PropertyName ) { UpdateWithForceLayout(UpdateIconRadius); }
			else if ( e.PropertyName == IconCellBase.IconSourceProperty.PropertyName ) { UpdateIcon(); }
		}

		public virtual void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.sv.SettingsView.CellTitleColorProperty.PropertyName ) { UpdateTitleColor(); }
			else if ( e.PropertyName == Shared.sv.SettingsView.CellTitleFontSizeProperty.PropertyName ||
					  e.PropertyName == Shared.sv.SettingsView.CellTitleFontFamilyProperty.PropertyName ||
					  e.PropertyName == Shared.sv.SettingsView.CellTitleFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateTitleFont); }

			else if ( e.PropertyName == Shared.sv.SettingsView.CellDescriptionColorProperty.PropertyName ) { UpdateDescriptionColor(); }
			else if ( e.PropertyName == Shared.sv.SettingsView.CellDescriptionFontSizeProperty.PropertyName ||
					  e.PropertyName == Shared.sv.SettingsView.CellDescriptionFontFamilyProperty.PropertyName ||
					  e.PropertyName == Shared.sv.SettingsView.CellDescriptionFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateDescriptionFont); }

			else if ( e.PropertyName == Shared.sv.SettingsView.CellBackgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }

			else if ( e.PropertyName == Shared.sv.SettingsView.CellHintTextColorProperty.PropertyName ) { UpdateHintTextColor(); }
			else if ( e.PropertyName == Shared.sv.SettingsView.CellHintFontSizeProperty.PropertyName ||
					  e.PropertyName == Shared.sv.SettingsView.CellHintFontFamilyProperty.PropertyName ||
					  e.PropertyName == Shared.sv.SettingsView.CellHintFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateHintFont); }

			else if ( e.PropertyName == Shared.sv.SettingsView.CellIconSizeProperty.PropertyName ) { UpdateWithForceLayout(UpdateIconSize); }
			else if ( e.PropertyName == Shared.sv.SettingsView.CellIconRadiusProperty.PropertyName ) { UpdateWithForceLayout(UpdateIconRadius); }

			else if ( e.PropertyName == Shared.sv.SettingsView.SelectedColorProperty.PropertyName ) { UpdateSelectedColor(); }
			else if ( e.PropertyName == TableView.RowHeightProperty.PropertyName ) { UpdateMinRowHeight(); }
		}

		public virtual void SectionPropertyChanged( object sender, PropertyChangedEventArgs e ) { }

		public virtual void RowSelected( UITableView tableView, NSIndexPath indexPath ) { }

		public virtual bool RowLongPressed( UITableView tableView, NSIndexPath indexPath ) => false;

		protected void UpdateWithForceLayout( Action updateAction )
		{
			updateAction();
			SetNeedsLayout();
		}

		private void UpdateSelectedColor()
		{
			if ( CellParent is not null &&
				 CellParent.SelectedColor != Color.Default )
			{
				if ( SelectedBackgroundView is not null ) { SelectedBackgroundView.BackgroundColor = CellParent.SelectedColor.ToUIColor(); }
				else
				{
					SelectedBackgroundView = new UIView
											 {
												 BackgroundColor = CellParent.SelectedColor.ToUIColor()
											 };
				}
			}
		}
		private void UpdateBackgroundColor()
		{
			if ( _CellBase is null ) return; // for HotReload
			BackgroundColor = _CellBase.GetBackground().ToUIColor();
		}

		private void UpdateHintText()
		{
			if ( HintLabel is null ) return; // for HotReload
			if ( _HintCell is null ) return;

			HintLabel.Text = _HintCell.Hint;
			HintLabel.Hidden = string.IsNullOrEmpty(_HintCell.Hint);
		}
		private void UpdateHintTextColor()
		{
			if ( HintLabel is null ) return; // for HotReload
			if ( _HintCell is null ) return;

			HintLabel.TextColor = _HintCell.HintConfig.Color.ToUIColor();
		}
		private void UpdateHintFont()
		{
			if ( HintLabel is null ) return; // for HotReload
			if ( _HintCell is null ) return;

			HintTextCellBase.HintConfiguration config = _HintCell.HintConfig;
			HintLabel.Font = FontUtility.CreateNativeFont(config.FontFamily, config.FontSize.ToFloat(), config.FontAttributes);
		}

		private void UpdateTitleText()
		{
			if ( TitleLabel is null ) return; // for HotReload
			if ( _TitleCell is null ) return;

			TitleLabel.Text = _TitleCell.Title;
			//Since Layout breaks when text empty, prevent Label height from resizing 0.
			if ( string.IsNullOrEmpty(TitleLabel.Text) )
			{
				TitleLabel.Hidden = true;
				TitleLabel.Text = " ";
			}
			else { TitleLabel.Hidden = false; }
		}
		private void UpdateTitleColor()
		{
			if ( TitleLabel is null ) return; // for HotReload
			if ( _TitleCell is null ) return;

			TitleLabel.TextColor = _TitleCell.TitleConfig.Color.ToUIColor();
		}
		private void UpdateTitleFont()
		{
			if ( TitleLabel is null ) return; // for HotReload
			if ( _TitleCell is null ) return;

			var config = _TitleCell.TitleConfig;
			TitleLabel.Font = FontUtility.CreateNativeFont(config.FontFamily, config.FontSize.ToFloat(), config.FontAttributes);
		}

		private void UpdateDescriptionText()
		{
			if ( DescriptionLabel is null ) return; // for HotReload
			if ( _DescriptionCell is null ) return;

			DescriptionLabel.Text = _DescriptionCell.Description;
			DescriptionLabel.Hidden = string.IsNullOrEmpty(DescriptionLabel.Text); // layout break because of StackView spacing; DescriptionLabel hidden to fix it. 
		}
		private void UpdateDescriptionFont()
		{
			if ( DescriptionLabel is null ) return; // for HotReload
			if ( _DescriptionCell is null ) return;

			var config = _DescriptionCell.DescriptionConfig;
			DescriptionLabel.Font = FontUtility.CreateNativeFont(config.FontFamily, config.FontSize.ToFloat(), config.FontAttributes);
		}
		private void UpdateDescriptionColor()
		{
			if ( DescriptionLabel is null ) return; // for HotReload
			if ( _DescriptionCell is null ) return;

			DescriptionLabel.TextColor = _DescriptionCell.DescriptionConfig.Color.ToUIColor();
		}

		protected virtual void UpdateIsEnabled() { SetEnabledAppearance(Cell.IsEnabled); }

		protected virtual void SetEnabledAppearance( bool isEnabled )
		{
			if ( isEnabled )
			{
				UserInteractionEnabled = true;
				if ( TitleLabel != null ) TitleLabel.Alpha = SVConstants.Cell.ENABLED_ALPHA;
				if ( DescriptionLabel != null ) DescriptionLabel.Alpha = SVConstants.Cell.ENABLED_ALPHA;
				if ( IconView != null ) IconView.Alpha = SVConstants.Cell.ENABLED_ALPHA;
			}
			else
			{
				UserInteractionEnabled = false;
				if ( TitleLabel != null ) TitleLabel.Alpha = SVConstants.Cell.DISABLED_ALPHA;
				if ( DescriptionLabel != null ) DescriptionLabel.Alpha = SVConstants.Cell.DISABLED_ALPHA;
				if ( IconView != null ) IconView.Alpha = SVConstants.Cell.DISABLED_ALPHA;
			}
		}

		private void UpdateIconSize()
		{
			if ( IconView is null ) return;
			if ( _IconCell is null ) return;

			Size size = _IconCell.GetIconSize(); // size = new Size(32, 32); 

			//do nothing when current size is previous size
			if ( size == _IconSize ) { return; }

			if ( _IconSize != default )
			{
				// remove previous constraint
				if ( _IconConstraintHeight != null )
				{
					_IconConstraintHeight.Active = false;
					_IconConstraintHeight?.Dispose();
				}

				if ( _IconConstraintWidth != null )
				{
					_IconConstraintWidth.Active = false;
					_IconConstraintWidth?.Dispose();
				}
			}

			_IconConstraintHeight = IconView.HeightAnchor.ConstraintEqualTo((nfloat) size.Height);
			_IconConstraintWidth = IconView.WidthAnchor.ConstraintEqualTo((nfloat) size.Width);

			_IconConstraintHeight.Priority = SVConstants.Layout.Priority.HIGH; // fix warning-log:Unable to simultaneously satisfy constraints.
			_IconConstraintHeight.Active = true;
			_IconConstraintWidth.Active = true;

			IconView.UpdateConstraints();

			_IconSize = size;
		}
		private void UpdateIconRadius()
		{
			if ( IconView is null ) return; // for HotReload
			if ( _IconCell is null ) return;

			IconView.Layer.CornerRadius = _IconCell.GetIconRadius().ToNFloat();
		}
		private void UpdateIcon()
		{
			if ( IconView is null ) return; // for HotReload
			if ( _IconCell is null ) return;

			if ( _IconTokenSource is not null &&
				 !_IconTokenSource.IsCancellationRequested ) { _IconTokenSource.Cancel(); }

			UpdateIconSize();

			if ( IconView.Image is not null ) { IconView.Image = null; }

			if ( _IconCell.IconSource is not null )
			{
				// hide IconView because UIStackView Distribution won't work when a image isn't set.
				IconView.Hidden = false;

				if ( ImageCacheController.Instance.ObjectForKey(FromObject(_IconCell.IconSource.GetHashCode())) is UIImage cache )
				{
					IconView.Image = cache;
					return;
				}

				var handler = Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IImageSourceHandler>(_IconCell.IconSource.GetType());
				LoadIconImage(handler, _IconCell.IconSource);
			}
			else { IconView.Hidden = true; }
		}
		private void LoadIconImage( IImageSourceHandler handler, ImageSource source )
		{
			if ( IconView is null ) return; // for HotReload
			if ( _IconCell is null ) return;

			_IconTokenSource = new CancellationTokenSource();
			CancellationToken token = _IconTokenSource.Token;
			UIImage? image = null;

			var scale = (float) UIScreen.MainScreen.Scale;
			Task.Run(async () =>
					 {
						 if ( source is FontImageSource ) { DispatchQueue.MainQueue.DispatchSync(async () => { image = await handler.LoadImageAsync(source, token, scale: scale); }); }
						 else { image = await handler.LoadImageAsync(source, token, scale: scale); }

						 token.ThrowIfCancellationRequested();
					 },
					 token
					)
				.ContinueWith(t =>
							  {
								  if ( !t.IsCompleted ) return;
								  if ( _IconCell.IconSource is not null &&
									   image is not null )
									  ImageCacheController.Instance.SetObjectforKey(image, FromObject(_IconCell.IconSource.GetHashCode()));
								  BeginInvokeOnMainThread(() =>
														  {
															  IconView.Image = image;
															  SetNeedsLayout();
														  }
														 );
							  },
							  token
							 );
		}


		private void UpdateMinRowHeight()
		{
			if ( CellParent is null ) throw new NullReferenceException(nameof(CellParent));
			if ( _StackH is null ) throw new NullReferenceException(nameof(_StackH));

			if ( _MinHeightConstraint is not null )
			{
				_MinHeightConstraint.Active = false;
				_MinHeightConstraint.Dispose();
				_MinHeightConstraint = null;
			}

			if ( CellParent.HasUnevenRows )
			{
				_MinHeightConstraint = _StackH.HeightAnchor.ConstraintGreaterThanOrEqualTo(CellParent.RowHeight);
				_MinHeightConstraint.Priority = SVConstants.Layout.Priority.HIGH;
				_MinHeightConstraint.Active = true;
			}

			_StackH.UpdateConstraints();
		}

		public virtual void UpdateCell( UITableView tableView )
		{
			if ( TitleLabel is null ) return; // For HotReload

			UpdateBackgroundColor();
			UpdateTitleText();
			UpdateTitleColor();
			UpdateTitleFont();
			UpdateDescriptionText();
			UpdateDescriptionColor();
			UpdateDescriptionFont();
			UpdateHintText();
			UpdateHintTextColor();
			UpdateHintFont();

			UpdateIcon();
			UpdateIconRadius();

			UpdateIsEnabled();

			SetNeedsLayout();
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _CellBase != null )
				{
					_CellBase.PropertyChanged -= CellPropertyChanged;

					if ( _CellBase.Section is not null )
					{
						_CellBase.Section.PropertyChanged -= SectionPropertyChanged;
						_CellBase.Section = null;
					}
				}

				if ( CellParent != null ) CellParent.PropertyChanged -= ParentPropertyChanged;


				SelectedBackgroundView?.Dispose();
				SelectedBackgroundView = null;

				Device.BeginInvokeOnMainThread(() =>
											   {
												   HintLabel?.RemoveFromSuperview();
												   HintLabel?.Dispose();
												   HintLabel = null;
												   TitleLabel?.Dispose();
												   TitleLabel = null;
												   DescriptionLabel?.Dispose();
												   DescriptionLabel = null;
												   IconView?.RemoveFromSuperview();
												   IconView?.Image?.Dispose();
												   IconView?.Dispose();
												   IconView = null;
												   _IconTokenSource?.Dispose();
												   _IconTokenSource = null;
												   _IconConstraintWidth?.Dispose();
												   _IconConstraintHeight?.Dispose();
												   _IconConstraintHeight = null;
												   _IconConstraintWidth = null;
												   _ContentStack?.RemoveFromSuperview();
												   _ContentStack?.Dispose();
												   _ContentStack = null;
												   Cell = null;

												   _StackV?.RemoveFromSuperview();
												   _StackV?.Dispose();
												   _StackV = null;

												   _StackH?.RemoveFromSuperview();
												   _StackH?.Dispose();
												   _StackH = null;
											   }
											  );
			}

			base.Dispose(disposing);
		}

		private void SetUpHintLabel()
		{
			HintLabel = new UILabel();
			HintLabel.LineBreakMode = UILineBreakMode.Clip;
			HintLabel.Lines = 0;
			HintLabel.TintAdjustmentMode = UIViewTintAdjustmentMode.Automatic;
			HintLabel.AdjustsFontSizeToFitWidth = true;
			HintLabel.BaselineAdjustment = UIBaselineAdjustment.AlignCenters;
			HintLabel.TextAlignment = UITextAlignment.Right;
			HintLabel.AdjustsLetterSpacingToFitWidth = true;

			AddSubview(HintLabel);

			HintLabel.TranslatesAutoresizingMaskIntoConstraints = false;
			HintLabel.TopAnchor.ConstraintEqualTo(TopAnchor, 2).Active = true;
			HintLabel.LeftAnchor.ConstraintEqualTo(LeftAnchor, 16).Active = true;
			HintLabel.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor, -10).Active = true;
			HintLabel.BottomAnchor.ConstraintLessThanOrEqualTo(BottomAnchor, -12).Active = true;

			HintLabel.SizeToFit();
			BringSubviewToFront(HintLabel);
		}

		protected void SetRightMarginZero()
		{
			if ( _StackH is null ) throw new NullReferenceException(nameof(_StackH));
			if ( UIDevice.CurrentDevice.CheckSystemVersion(11, 0) ) { _StackH.LayoutMargins = new UIEdgeInsets(6, 16, 6, 0); }
		}


		protected virtual void SetUpContentView()
		{
			SetUpHintLabel();

			//remove existing views 
			ImageView.RemoveFromSuperview();
			TextLabel.RemoveFromSuperview();
			ImageView.Hidden = true;
			TextLabel.Hidden = true;

			//Outer HorizontalStackView
			_StackH = new UIStackView
					  {
						  Axis = UILayoutConstraintAxis.Horizontal,
						  Alignment = UIStackViewAlignment.Center,
						  Spacing = 16,
						  Distribution = UIStackViewDistribution.Fill,
						  LayoutMargins = new UIEdgeInsets(6, 16, 6, 16),
						  LayoutMarginsRelativeArrangement = true
					  };
			//set margin

			IconView = new UIImageView();

			//round corners
			IconView.ClipsToBounds = true;

			_StackH.AddArrangedSubview(IconView);

			UpdateIconSize();

			//VerticalStackView that is arranged at right. ( put main parts and Description ) 
			_StackV = new UIStackView
					  {
						  Axis = UILayoutConstraintAxis.Vertical,
						  Alignment = UIStackViewAlignment.Fill,
						  Spacing = 4,
						  Distribution = UIStackViewDistribution.Fill,
					  };

			//HorizontalStackView that is arranged at upper in right.( put LabelText and ValueText)
			_ContentStack = new UIStackView
							{
								Axis = UILayoutConstraintAxis.Horizontal,
								Alignment = UIStackViewAlignment.Fill,
								Spacing = 6,
								Distribution = UIStackViewDistribution.Fill,
							};

			TitleLabel = new UILabel();
			DescriptionLabel = new UILabel
							   {
								   Lines = 0,
								   LineBreakMode = UILineBreakMode.WordWrap
							   };


			_ContentStack.AddArrangedSubview(TitleLabel);

			_StackV.AddArrangedSubview(_ContentStack);
			_StackV.AddArrangedSubview(DescriptionLabel);

			_StackH.AddArrangedSubview(_StackV);

			IconView.SetContentHuggingPriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal); //if possible, not to expand.
			_StackV.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Horizontal);
			_ContentStack.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Horizontal);
			TitleLabel.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Horizontal);
			DescriptionLabel.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Horizontal);


			IconView.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal); //if possible, not to shrink. 
			_StackV.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);
			_ContentStack.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);
			TitleLabel.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);
			DescriptionLabel.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);

			IconView.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Vertical);
			IconView.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Vertical);
			_StackV.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Vertical);
			_StackV.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Vertical);

			ContentView.AddSubview(_StackH);

			_StackH.TranslatesAutoresizingMaskIntoConstraints = false;
			_StackH.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
			_StackH.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor).Active = true;
			_StackH.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;
			_StackH.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor).Active = true;


			double minHeight = Math.Max(CellParent?.RowHeight ?? -1, SVConstants.Defaults.MIN_ROW_HEIGHT);
			_MinHeightConstraint = _StackH.HeightAnchor.ConstraintGreaterThanOrEqualTo(minHeight.ToNFloat());
			// fix warning-log:Unable to simultaneously satisfy constraints.
			_MinHeightConstraint.Priority = SVConstants.Layout.Priority.HIGH; // this is superior to any other view.
			_MinHeightConstraint.Active = true;

			if ( !string.IsNullOrEmpty(Cell.AutomationId) ) { _ContentStack.AccessibilityIdentifier = Cell.AutomationId; }

			UpdateSelectedColor();
		}
	}
}
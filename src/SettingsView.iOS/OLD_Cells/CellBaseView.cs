using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using CoreFoundation;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Extensions;
using Jakar.SettingsView.iOS.OLD_Cells;
using Jakar.SettingsView.Shared.Cells;
using UIKit;
using Xamarin.Forms;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Misc;
using Xamarin.Forms.Platform.iOS;

namespace Jakar.SettingsView.iOS.OLD_Cells
{
	[Obsolete("old api")]
	[Foundation.Preserve(AllMembers = true)]
	public class CellBaseView : CellTableViewCell
	{
		public UILabel HintLabel { get; private set; }
		public UILabel TitleLabel { get; private set; }
		public UILabel DescriptionLabel { get; private set; }
		public UIImageView IconView { get; private set; }
		protected CellBase _CellBase => Cell as CellBase;

		public Shared.sv.SettingsView CellParent => Cell.Parent as Shared.sv.SettingsView;
		protected UIStackView _ContentStack { get; private set; }

		protected UIStackView _StackH { get; private set; }
		protected UIStackView _StackV { get; private set; }

		// private Size _iconSize;
		private NSLayoutConstraint _IconConstraintHeight { get; set; }
		private NSLayoutConstraint _IconConstraintWidth { get; set; }
		private NSLayoutConstraint _MinHeightConstraint { get; set; }
		private CancellationTokenSource _IconTokenSource { get; set; }

		public CellBaseView( Cell formsCell ) : base(UITableViewCellStyle.Default, formsCell.GetType().FullName)
		{
			Cell = formsCell;

			SelectionStyle = UITableViewCellSelectionStyle.None;
			SetUpHintLabel();
			SetUpContentView();

			UpdateSelectedColor();
		}

		public virtual void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			// if ( e.PropertyName == _CellBase.TitleProperty.PropertyName ) { UpdateTitleText(); }
			// else if ( e.PropertyName == _CellBase.TitleColorProperty.PropertyName ) { UpdateTitleColor(); }
			// else if ( e.PropertyName == _CellBase.TitleFontSizeProperty.PropertyName ||
			// 		  e.PropertyName == _CellBase.TitleFontFamilyProperty.PropertyName ||
			// 		  e.PropertyName == _CellBase.TitleFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateTitleFont); }
			// else if ( e.PropertyName == _CellBase.DescriptionProperty.PropertyName ) { UpdateWithForceLayout(UpdateDescriptionText); }
			// else if ( e.PropertyName == _CellBase.DescriptionFontSizeProperty.PropertyName ||
			// 		  e.PropertyName == _CellBase.DescriptionFontFamilyProperty.PropertyName ||
			// 		  e.PropertyName == _CellBase.DescriptionFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateDescriptionFont); }
			// else if ( e.PropertyName == _CellBase.DescriptionColorProperty.PropertyName ) { UpdateDescriptionColor(); }
			// else if ( e.PropertyName == _CellBase.IconSourceProperty.PropertyName ) { UpdateIcon(); }
			// else if ( e.PropertyName == CellBase.BackgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }
			// else if ( e.PropertyName == _CellBase.HintTextProperty.PropertyName ) { UpdateWithForceLayout(UpdateHintText); }
			// else if ( e.PropertyName == _CellBase.HintTextColorProperty.PropertyName ) { UpdateHintTextColor(); }
			// else if ( e.PropertyName == _CellBase.HintFontSizeProperty.PropertyName ||
			// 		  e.PropertyName == _CellBase.HintFontFamilyProperty.PropertyName ||
			// 		  e.PropertyName == _CellBase.HintFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateHintFont); }
			// else if ( e.PropertyName == _CellBase.IconSizeProperty.PropertyName ) { UpdateWithForceLayout(UpdateIconSize); }
			// else if ( e.PropertyName == _CellBase.IconRadiusProperty.PropertyName ) { UpdateWithForceLayout(UpdateIconRadius); }
			if ( e.PropertyName == Cell.IsEnabledProperty.PropertyName ) { UpdateIsEnabled(); }
		}

		public virtual void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			// if ( e.PropertyName == Shared.sv.SettingsView.CellTitleColorProperty.PropertyName ) { UpdateTitleColor(); }
			// else if ( e.PropertyName == Shared.sv.SettingsView.CellTitleFontSizeProperty.PropertyName ||
			// 		  e.PropertyName == Shared.sv.SettingsView.CellTitleFontFamilyProperty.PropertyName ||
			// 		  e.PropertyName == Shared.sv.SettingsView.CellTitleFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateTitleFont); }
			// else if ( e.PropertyName == Shared.sv.SettingsView.CellDescriptionColorProperty.PropertyName ) { UpdateDescriptionColor(); }
			// else if ( e.PropertyName == Shared.sv.SettingsView.CellDescriptionFontSizeProperty.PropertyName ||
			// 		  e.PropertyName == Shared.sv.SettingsView.CellDescriptionFontFamilyProperty.PropertyName ||
			// 		  e.PropertyName == Shared.sv.SettingsView.CellDescriptionFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateDescriptionFont); }
			if ( e.PropertyName == Shared.sv.SettingsView.CellBackgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }
			// else if ( e.PropertyName == Shared.sv.SettingsView.CellHintTextColorProperty.PropertyName ) { UpdateHintTextColor(); }
			// else if ( e.PropertyName == Shared.sv.SettingsView.CellHintFontSizeProperty.PropertyName ||
			// 		  e.PropertyName == Shared.sv.SettingsView.CellHintFontFamilyProperty.PropertyName ||
			// 		  e.PropertyName == Shared.sv.SettingsView.CellHintFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateHintFont); }
			// else if ( e.PropertyName == Shared.sv.SettingsView.CellIconSizeProperty.PropertyName ) { UpdateWithForceLayout(UpdateIconSize); }
			// else if ( e.PropertyName == Shared.sv.SettingsView.CellIconRadiusProperty.PropertyName ) { UpdateWithForceLayout(UpdateIconRadius); }
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
			if ( CellParent != null &&
				 CellParent.SelectedColor != Color.Default )
			{
				if ( SelectedBackgroundView != null ) { SelectedBackgroundView.BackgroundColor = CellParent.SelectedColor.ToUIColor(); }
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
			if ( _CellBase.BackgroundColor != Color.Default ) { BackgroundColor = _CellBase.BackgroundColor.ToUIColor(); }
			else if ( CellParent != null &&
					  CellParent.CellBackgroundColor != Color.Default ) { BackgroundColor = CellParent.CellBackgroundColor.ToUIColor(); }
		}
		
		// private void UpdateHintText()
		// {
		// 	if ( HintLabel is null )
		// 		return; // for HotReload
		//
		// 	HintLabel.Text = _CellBase.HintText;
		// 	HintLabel.Hidden = string.IsNullOrEmpty(_CellBase.HintText);
		// }
		// private void UpdateHintTextColor()
		// {
		// 	if ( HintLabel is null )
		// 		return; // for HotReload
		//
		// 	if ( _CellBase.HintTextColor != Color.Default ) { HintLabel.TextColor = _CellBase.HintTextColor.ToUIColor(); }
		// 	else if ( CellParent != null &&
		// 			  CellParent.CellHintTextColor != Color.Default ) { HintLabel.TextColor = CellParent.CellHintTextColor.ToUIColor(); }
		// }
		// private void UpdateHintFont()
		// {
		// 	if ( HintLabel is null ) return; // for HotReload
		//
		// 	string family = _CellBase.HintFontFamily ?? CellParent.CellHintFontFamily;
		// 	FontAttributes attr = _CellBase.HintFontAttributes ?? CellParent.CellHintFontAttributes;
		//
		// 	if ( _CellBase.HintFontSize > 0 ) { HintLabel.Font = FontUtility.CreateNativeFont(family, (float) _CellBase.HintFontSize, attr); }
		// 	else if ( CellParent != null ) { HintLabel.Font = FontUtility.CreateNativeFont(family, (float) CellParent.CellHintFontSize, attr); }
		// }
		// private void UpdateTitleText()
		// {
		// 	if ( TitleLabel is null )
		// 		return; // for HotReload
		//
		// 	TitleLabel.Text = _CellBase.Title;
		// 	//Since Layout breaks when text empty, prevent Label height from resizing 0.
		// 	if ( string.IsNullOrEmpty(TitleLabel.Text) )
		// 	{
		// 		TitleLabel.Hidden = true;
		// 		TitleLabel.Text = " ";
		// 	}
		// 	else { TitleLabel.Hidden = false; }
		// }
		//
		// private void UpdateTitleColor()
		// {
		// 	if ( TitleLabel is null )
		// 		return; // for HotReload
		//
		// 	if ( _CellBase.TitleColor != Color.Default ) { TitleLabel.TextColor = _CellBase.TitleColor.ToUIColor(); }
		// 	else if ( CellParent != null &&
		// 			  CellParent.CellTitleColor != Color.Default ) { TitleLabel.TextColor = CellParent.CellTitleColor.ToUIColor(); }
		// }
		// private void UpdateTitleFont()
		// {
		// 	if ( TitleLabel is null )
		// 		return; // for HotReload
		//
		// 	string family = _CellBase.TitleFontFamily ?? CellParent.CellTitleFontFamily;
		// 	FontAttributes attr = _CellBase.TitleFontAttributes ?? CellParent.CellTitleFontAttributes;
		//
		// 	if ( _CellBase.TitleFontSize > 0 ) { TitleLabel.Font = FontUtility.CreateNativeFont(family, (float) _CellBase.TitleFontSize, attr); }
		// 	else if ( CellParent != null ) { TitleLabel.Font = FontUtility.CreateNativeFont(family, (float) CellParent.CellTitleFontSize, attr); }
		// }
		//
		// private void UpdateDescriptionText()
		// {
		// 	if ( DescriptionLabel is null )
		// 		return; // for HotReload
		//
		// 	DescriptionLabel.Text = _CellBase.Description;
		// 	//layout break because of StackView spacing.DescriptionLabel hidden to fix it. 
		// 	DescriptionLabel.Hidden = string.IsNullOrEmpty(DescriptionLabel.Text);
		// }
		// private void UpdateDescriptionFont()
		// {
		// 	if ( DescriptionLabel is null )
		// 		return; // for HotReload
		//
		// 	string family = _CellBase.DescriptionFontFamily ?? CellParent.CellDescriptionFontFamily;
		// 	FontAttributes attr = _CellBase.DescriptionFontAttributes ?? CellParent.CellDescriptionFontAttributes;
		//
		// 	if ( _CellBase.DescriptionFontSize > 0 ) { DescriptionLabel.Font = FontUtility.CreateNativeFont(family, (float) _CellBase.DescriptionFontSize, attr); }
		// 	else if ( CellParent != null ) { DescriptionLabel.Font = FontUtility.CreateNativeFont(family, (float) CellParent.CellDescriptionFontSize, attr); }
		// }
		// private void UpdateDescriptionColor()
		// {
		// 	if ( DescriptionLabel is null )
		// 		return; // for HotReload
		//
		// 	if ( _CellBase.DescriptionColor != Color.Default ) { DescriptionLabel.TextColor = _CellBase.DescriptionColor.ToUIColor(); }
		// 	else if ( CellParent != null &&
		// 			  CellParent.CellDescriptionColor != Color.Default ) { DescriptionLabel.TextColor = CellParent.CellDescriptionColor.ToUIColor(); }
		// }

		protected virtual void UpdateIsEnabled() { SetEnabledAppearance(_CellBase.IsEnabled); }

		protected virtual void SetEnabledAppearance( bool isEnabled )
		{
			if ( TitleLabel is null ) return; // for HotReload

			if ( isEnabled )
			{
				UserInteractionEnabled = true;
				TitleLabel.Alpha = 1f;
				DescriptionLabel.Alpha = 1f;
				IconView.Alpha = 1f;
			}
			else
			{
				UserInteractionEnabled = false;
				TitleLabel.Alpha = 0.3f;
				DescriptionLabel.Alpha = 0.3f;
				IconView.Alpha = 0.3f;
			}
		}

		// private void UpdateIconSize()
		// {
		// 	Size size;
		// 	if ( _CellBase.IconSize != default(Size) ) { size = _CellBase.IconSize; }
		// 	else if ( CellParent != null &&
		// 			  CellParent.CellIconSize != default(Size) ) { size = CellParent.CellIconSize; }
		// 	else { size = new Size(32, 32); }
		//
		// 	//do nothing when current size is previous size
		// 	if ( size == _iconSize ) { return; }
		//
		// 	if ( _iconSize != default(Size) )
		// 	{
		// 		//remove previous constraint
		// 		_IconConstraintHeight.Active = false;
		// 		_IconConstraintWidth.Active = false;
		// 		_IconConstraintHeight?.Dispose();
		// 		_IconConstraintWidth?.Dispose();
		// 	}
		//
		// 	_IconConstraintHeight = IconView.HeightAnchor.ConstraintEqualTo((nfloat) size.Height);
		// 	_IconConstraintWidth = IconView.WidthAnchor.ConstraintEqualTo((nfloat) size.Width);
		//
		// 	_IconConstraintHeight.Priority = 999f; // fix warning-log:Unable to simultaneously satisfy constraints.
		// 	_IconConstraintHeight.Active = true;
		// 	_IconConstraintWidth.Active = true;
		//
		// 	IconView.UpdateConstraints();
		//
		// 	_iconSize = size;
		// }
		// private void UpdateIconRadius()
		// {
		// 	if ( IconView is null )
		// 		return; // for HotReload
		//
		// 	if ( _CellBase.IconRadius >= 0 ) { IconView.Layer.CornerRadius = (float) _CellBase.IconRadius; }
		// 	else if ( CellParent != null ) { IconView.Layer.CornerRadius = (float) CellParent.CellIconRadius; }
		// }
		// private void UpdateIcon()
		// {
		// 	if ( IconView is null )
		// 		return; // for HotReload
		//
		// 	if ( _iconTokenSource != null &&
		// 		 !_iconTokenSource.IsCancellationRequested ) { _iconTokenSource.Cancel(); }
		//
		// 	UpdateIconSize();
		//
		// 	if ( IconView.Image != null ) { IconView.Image = null; }
		//
		// 	if ( _CellBase.IconSource != null )
		// 	{
		// 		//image未設定の時はhiddenにしないとstackviewのDistributionが機能しなくなる
		// 		//hide IconView because UIStackView Distribution won't work when a image isn't set.
		// 		IconView.Hidden = false;
		//
		// 		var cache = ImageCacheController.Instance.ObjectForKey(FromObject(_CellBase.IconSource.GetHashCode())) as UIImage;
		// 		if ( cache != null )
		// 		{
		// 			IconView.Image = cache;
		// 			return;
		// 		}
		//
		// 		var handler = Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IImageSourceHandler>(_CellBase.IconSource.GetType());
		// 		LoadIconImage(handler, _CellBase.IconSource);
		// 	}
		// 	else { IconView.Hidden = true; }
		// }
		// private void LoadIconImage( IImageSourceHandler handler, ImageSource source )
		// {
		// 	_iconTokenSource = new CancellationTokenSource();
		// 	CancellationToken token = _iconTokenSource.Token;
		// 	UIImage image = null;
		//
		// 	var scale = (float) UIScreen.MainScreen.Scale;
		// 	Task.Run(async () =>
		// 			 {
		// 				 if ( source is FontImageSource ) { DispatchQueue.MainQueue.DispatchSync(async () => { image = await handler.LoadImageAsync(source, token, scale: scale); }); }
		// 				 else { image = await handler.LoadImageAsync(source, token, scale: scale); }
		//
		// 				 token.ThrowIfCancellationRequested();
		// 			 },
		// 			 token
		// 			)
		// 		.ContinueWith(t =>
		// 					  {
		// 						  if ( !t.IsCompleted ) return;
		// 						  ImageCacheController.Instance.SetObjectforKey(image, FromObject(_CellBase.IconSource.GetHashCode()));
		// 						  BeginInvokeOnMainThread(() =>
		// 												  {
		// 													  IconView.Image = image;
		// 													  SetNeedsLayout();
		// 												  }
		// 												 );
		// 					  },
		// 					  token
		// 					 );
		// }

		private void UpdateMinRowHeight()
		{
			if ( _MinHeightConstraint is not null )
			{
				_MinHeightConstraint.Active = false;
				_MinHeightConstraint.Dispose();
				_MinHeightConstraint = null;
			}

			if ( CellParent.HasUnevenRows )
			{
				_MinHeightConstraint = _StackH.HeightAnchor.ConstraintGreaterThanOrEqualTo(CellParent.RowHeight);
				_MinHeightConstraint.Priority = 999f;
				_MinHeightConstraint.Active = true;
			}

			_StackH.UpdateConstraints();
		}

		public virtual void UpdateCell( UITableView parent = null )
		{
			if ( TitleLabel is null ) return; // For HotReload

			UpdateBackgroundColor();
			// UpdateTitleText();
			// UpdateTitleColor();
			// UpdateTitleFont();
			// UpdateDescriptionText();
			// UpdateDescriptionColor();
			// UpdateDescriptionFont();
			// UpdateHintText();
			// UpdateHintTextColor();
			// UpdateHintFont();
			//
			// UpdateIcon();
			// UpdateIconRadius();

			UpdateIsEnabled();

			SetNeedsLayout();
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_CellBase.PropertyChanged -= CellPropertyChanged;
				CellParent.PropertyChanged -= ParentPropertyChanged;

				if ( _CellBase.Section != null )
				{
					_CellBase.Section.PropertyChanged -= SectionPropertyChanged;
					_CellBase.Section = null;
				}


				SelectedBackgroundView?.Dispose();
				SelectedBackgroundView = null;

				Device.BeginInvokeOnMainThread(() =>
											   {
												   HintLabel.RemoveFromSuperview();
												   HintLabel.Dispose();
												   HintLabel = null;
												   TitleLabel?.Dispose();
												   TitleLabel = null;
												   DescriptionLabel?.Dispose();
												   DescriptionLabel = null;
												   IconView.RemoveFromSuperview();
												   IconView.Image?.Dispose();
												   IconView.Dispose();
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

												   _StackH.RemoveFromSuperview();
												   _StackH.Dispose();
												   _StackH = null;
											   }
											  );
			}

			base.Dispose(disposing);
		}

		private void SetUpHintLabel()
		{
			HintLabel = new UILabel
						{
							LineBreakMode = UILineBreakMode.WordWrap,
							Lines = 0,
							TintAdjustmentMode = UIViewTintAdjustmentMode.Automatic,
							AdjustsFontSizeToFitWidth = true,
							BaselineAdjustment = UIBaselineAdjustment.AlignCenters,
							TextAlignment = UITextAlignment.Right,
							AdjustsLetterSpacingToFitWidth = true
						};

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
			if ( UIDevice.CurrentDevice.CheckSystemVersion(11, 0) ) { _StackH.LayoutMargins = new UIEdgeInsets(6, 16, 6, 0); }
		}


		protected virtual void SetUpContentView()
		{
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

			IconView = new UIImageView
					   {
						   ClipsToBounds = true // round corners
					   };

			
			_StackH.AddArrangedSubview(IconView);

			// UpdateIconSize();

			//VerticalStackView that is arranged at right. ( put main parts and Description ) 
			_StackV = new UIStackView
					 {
						 Axis = UILayoutConstraintAxis.Vertical,
						 Alignment = UIStackViewAlignment.Fill,
						 Spacing = 4,
						 Distribution = UIStackViewDistribution.Fill,
					 };

			//HorizontalStackView that is arranged at upper in right.(put LabelText and ValueText)
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

			IconView.SetContentHuggingPriority(999f, UILayoutConstraintAxis.Horizontal); // if possible, not to expand. 
			_StackV.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Horizontal);
			_ContentStack.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Horizontal);
			TitleLabel.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Horizontal);
			DescriptionLabel.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Horizontal);


			IconView.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal); // if possible, not to shrink. 
			_StackV.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal);
			_ContentStack.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal);
			TitleLabel.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal);
			DescriptionLabel.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal);

			IconView.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Vertical);
			IconView.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Vertical);
			_StackV.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Vertical);
			_StackV.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Vertical);

			ContentView.AddSubview(_StackH);

			_StackH.TranslatesAutoresizingMaskIntoConstraints = false;
			_StackH.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
			_StackH.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor).Active = true;
			_StackH.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;
			_StackH.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor).Active = true;


			double minHeight = Math.Max(CellParent?.RowHeight ?? -1, SVConstants.Defaults.MIN_ROW_HEIGHT);
			_MinHeightConstraint = _StackH.HeightAnchor.ConstraintGreaterThanOrEqualTo(minHeight.ToNFloat());
			// fix warning-log:Unable to simultaneously satisfy constraints.
			_MinHeightConstraint.Priority = 999f; // this is superior to any other view.
			_MinHeightConstraint.Active = true;
		}
	}
}
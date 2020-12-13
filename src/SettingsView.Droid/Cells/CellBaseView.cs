using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Droid.Interfaces;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ARelativeLayout = Android.Widget.RelativeLayout;
using AGridLayout = Android.Widget.GridLayout;
using AColor = Android.Graphics.Color;
using AView = Android.Views.View;

namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)]
	public abstract class CellBaseView : ARelativeLayout, INativeElementView, IBaseCell
	{
		public Cell Cell { get; set; }
		public Element Element => Cell;
		protected CellBase _CellBase => Cell as CellBase;
		public Shared.SettingsView CellParent => Cell.Parent as Shared.SettingsView;

		public ImageView IconView { get; set; }

		// public LinearLayout ContentStack { get; set; }
		public LinearLayout AccessoryStack { get; set; }

		public Context AndroidContext { get; set; }
		public CancellationTokenSource IconTokenSource { get; set; }
		public AColor DefaultTextColor { get; set; }
		public ColorDrawable BackgroundColor { get; set; }
		public ColorDrawable SelectedColor { get; set; }
		public RippleDrawable Ripple { get; set; }
		public float DefaultFontSize { get; set; }
		public float IconRadius { get; set; }


		protected CellBaseView( Context context, Cell cell ) : base(context)
		{
			AndroidContext = context;
			Cell = cell;

			// ReSharper disable once VirtualMemberCallInConstructor
			CreateContentView();
		}
		protected CellBaseView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) => AndroidContext = SettingsViewInit.Current;

		protected abstract void CreateContentView();
		// {
		// 	var layoutInflater = (LayoutInflater) AndroidContext.GetSystemService(Context.LayoutInflaterService);
		// 	AView contentView = layoutInflater?.Inflate(Resource.Layout.CellBaseView, this, true);
		//
		// 	if ( contentView != null )
		// 	{
		// 		contentView.LayoutParameters = new ViewGroup.LayoutParams(-1, -1);
		//
		// 		IconView = contentView.FindViewById<ImageView>(Resource.Id.CellIcon);
		// 		// TitleLabel = contentView.FindViewById<TextView>(Resource.Id.CellTitle);
		// 		// DescriptionLabel = contentView.FindViewById<TextView>(Resource.Id.CellDescription);
		// 		// HintLabel = contentView.FindViewById<TextView>(Resource.Id.CellHintText);
		// 		// ContentStack = contentView.FindViewById<LinearLayout>(Resource.Id.CellContentStack);
		// 		// CellValueStack = contentView.FindViewById<LinearLayout>(Resource.Id.CellValueView);
		// 		AccessoryStack = contentView.FindViewById<LinearLayout>(Resource.Id.CellAccessoryView);
		// 	}
		//
		// 	BackgroundColor = new ColorDrawable();
		// 	SelectedColor = new ColorDrawable(AColor.Argb(125, 180, 180, 180));
		//
		// 	var sel = new StateListDrawable();
		//
		// 	sel.AddState(new[]
		// 				 {
		// 					 Android.Resource.Attribute.StateSelected
		// 				 }, SelectedColor);
		// 	sel.AddState(new[]
		// 				 {
		// 					 -Android.Resource.Attribute.StateSelected
		// 				 }, BackgroundColor);
		// 	sel.SetExitFadeDuration(250);
		// 	sel.SetEnterFadeDuration(250);
		//
		// 	AColor rippleColor = AColor.Rgb(180, 180, 180);
		// 	if ( CellParent.SelectedColor != Xamarin.Forms.Color.Default ) { rippleColor = CellParent.SelectedColor.ToAndroid(); }
		//
		// 	Ripple = DrawableUtility.CreateRipple(rippleColor, sel);
		//
		// 	Background = Ripple;
		//
		// 	// if ( TitleLabel is null ) return;
		// 	// DefaultTextColor = new AColor(TitleLabel.CurrentTextColor);
		// 	// DefaultFontSize = TitleLabel.TextSize;
		// }

		public abstract void CellPropertyChanged( object sender, PropertyChangedEventArgs e );
		// {
		// 	if ( e.PropertyName == CellBase.TitleProperty.PropertyName ) { UpdateTitleText(); }
		// 	else if ( e.PropertyName == CellBase.TitleColorProperty.PropertyName ) { UpdateTitleColor(); }
		// 	else if ( e.PropertyName == CellBase.TitleFontSizeProperty.PropertyName ) { UpdateTitleFontSize(); }
		// 	else if ( e.PropertyName == CellBase.TitleFontFamilyProperty.PropertyName || e.PropertyName == CellBase.TitleFontAttributesProperty.PropertyName ) { UpdateTitleFont(); }
		// 	else if ( e.PropertyName == CellBase.DescriptionProperty.PropertyName ) { UpdateDescriptionText(); }
		// 	else if ( e.PropertyName == CellBase.DescriptionFontSizeProperty.PropertyName ) { UpdateDescriptionFontSize(); }
		// 	else if ( e.PropertyName == CellBase.DescriptionFontFamilyProperty.PropertyName || e.PropertyName == CellBase.DescriptionFontAttributesProperty.PropertyName ) { UpdateDescriptionFont(); }
		// 	else if ( e.PropertyName == CellBase.DescriptionColorProperty.PropertyName ) { UpdateDescriptionColor(); }
		// 	else if ( e.PropertyName == CellBase.IconSourceProperty.PropertyName ) { UpdateIcon(); }
		// 	else if ( e.PropertyName == CellBase.BackgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }
		// 	else if ( e.PropertyName == CellBase.HintTextProperty.PropertyName ) { UpdateWithForceLayout(UpdateHintText); }
		// 	else if ( e.PropertyName == CellBase.HintTextColorProperty.PropertyName ) { UpdateHintTextColor(); }
		// 	else if ( e.PropertyName == CellBase.HintFontSizeProperty.PropertyName ) { UpdateWithForceLayout(UpdateHintFontSize); }
		// 	else if ( e.PropertyName == CellBase.HintFontFamilyProperty.PropertyName || e.PropertyName == CellBase.HintFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateHintFont); }
		// 	else if ( e.PropertyName == CellBase.IconSizeProperty.PropertyName ) { UpdateIcon(); }
		// 	else if ( e.PropertyName == CellBase.IconRadiusProperty.PropertyName )
		// 	{
		// 		UpdateIconRadius();
		// 		UpdateIcon(true);
		// 	}
		// 	else if ( e.PropertyName == Cell.IsEnabledProperty.PropertyName ) { UpdateIsEnabled(); }
		// }
		public abstract void ParentPropertyChanged( object sender, PropertyChangedEventArgs e );
		// {
		// 	avoid running the vain process when popping a page.
		// 	if ( ( sender as BindableObject )?.BindingContext == null ) { return; }
		//
		// 	if ( e.PropertyName == Shared.SettingsView.CellTitleColorProperty.PropertyName ) { UpdateTitleColor(); }
		// 	else if ( e.PropertyName == Shared.SettingsView.CellTitleFontSizeProperty.PropertyName ) { UpdateWithForceLayout(UpdateTitleFontSize); }
		// 	else if ( e.PropertyName == Shared.SettingsView.CellTitleFontFamilyProperty.PropertyName || e.PropertyName == Shared.SettingsView.CellTitleFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateTitleFont); }
		// 	else if ( e.PropertyName == Shared.SettingsView.CellDescriptionColorProperty.PropertyName ) { UpdateDescriptionColor(); }
		// 	else if ( e.PropertyName == Shared.SettingsView.CellDescriptionFontSizeProperty.PropertyName ) { UpdateWithForceLayout(UpdateDescriptionFontSize); }
		// 	else if ( e.PropertyName == Shared.SettingsView.CellDescriptionFontFamilyProperty.PropertyName || e.PropertyName == Shared.SettingsView.CellDescriptionFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateDescriptionFont); }
		// 	else if ( e.PropertyName == Shared.SettingsView.CellBackgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }
		// 	else if ( e.PropertyName == Shared.SettingsView.CellHintTextColorProperty.PropertyName ) { UpdateHintTextColor(); }
		// 	else if ( e.PropertyName == Shared.SettingsView.CellHintFontSizeProperty.PropertyName ) { UpdateWithForceLayout(UpdateHintFontSize); }
		// 	else if ( e.PropertyName == Shared.SettingsView.CellHintFontFamilyProperty.PropertyName || e.PropertyName == Shared.SettingsView.CellHintFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateHintFont); }
		// 	else if ( e.PropertyName == Shared.SettingsView.CellIconSizeProperty.PropertyName ) { UpdateIcon(); }
		// 	else if ( e.PropertyName == Shared.SettingsView.CellIconRadiusProperty.PropertyName )
		// 	{
		// 		UpdateIconRadius();
		// 		UpdateIcon(true);
		// 	}
		// 	else if ( e.PropertyName == Shared.SettingsView.SelectedColorProperty.PropertyName ) { UpdateWithForceLayout(UpdateSelectedColor); }
		// }
		public virtual void SectionPropertyChanged( object sender, PropertyChangedEventArgs e ) { }


		public virtual void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { }
		public virtual bool RowLongPressed( SettingsViewRecyclerAdapter adapter, int position ) => false;


		public virtual void UpdateCell()
		{
			Invalidate();
		}


		// public void UpdateHint( object sender, PropertyChangedEventArgs e )
		// {
		// 	if ( e.PropertyName == CellBase.TitleProperty.PropertyName )
		// 	{ UpdateTitleText(); }
		// 	else if ( e.PropertyName == CellBase.TitleColorProperty.PropertyName )
		// 	{ UpdateTitleColor(); }
		// 	else if ( e.PropertyName == CellBase.TitleFontSizeProperty.PropertyName )
		// 	{ UpdateTitleFontSize(); }
		// 	else if ( e.PropertyName == CellBase.TitleFontFamilyProperty.PropertyName || e.PropertyName == CellBase.TitleFontAttributesProperty.PropertyName )
		// 	{ UpdateTitleFont(); }
		// 	else if ( e.PropertyName == CellBase.DescriptionProperty.PropertyName )
		// 	{ UpdateDescriptionText(); }
		// 	else if ( e.PropertyName == CellBase.DescriptionFontSizeProperty.PropertyName )
		// 	{ UpdateDescriptionFontSize(); }
		// 	else if ( e.PropertyName == CellBase.DescriptionFontFamilyProperty.PropertyName || e.PropertyName == CellBase.DescriptionFontAttributesProperty.PropertyName )
		// 	{ UpdateDescriptionFont(); }
		// 	else if ( e.PropertyName == CellBase.DescriptionColorProperty.PropertyName )
		// 	{ UpdateDescriptionColor(); }
		// 	else if ( e.PropertyName == CellBase.IconSourceProperty.PropertyName )
		// 	{ UpdateIcon(); }
		// 	else if ( e.PropertyName == CellBase.BackgroundColorProperty.PropertyName )
		// 	{ UpdateBackgroundColor(); }
		// 	else if ( e.PropertyName == CellBase.HintTextProperty.PropertyName )
		// 	{ UpdateWithForceLayout(UpdateHintText); }
		// 	else if ( e.PropertyName == CellBase.HintTextColorProperty.PropertyName )
		// 	{ UpdateHintTextColor(); }
		// 	else if ( e.PropertyName == CellBase.HintFontSizeProperty.PropertyName )
		// 	{ UpdateWithForceLayout(UpdateHintFontSize); }
		// 	else if ( e.PropertyName == CellBase.HintFontFamilyProperty.PropertyName || e.PropertyName == CellBase.HintFontAttributesProperty.PropertyName )
		// 	{ UpdateWithForceLayout(UpdateHintFont); }
		// 	else if ( e.PropertyName == CellBase.IconSizeProperty.PropertyName )
		// 	{ UpdateIcon(); }
		// 	else if ( e.PropertyName == CellBase.IconRadiusProperty.PropertyName )
		// 	{
		// 		UpdateIconRadius();
		// 		UpdateIcon(true);
		// 	}
		// 	else if ( e.PropertyName == Cell.IsEnabledProperty.PropertyName )
		// 	{ UpdateIsEnabled(); }
		// }






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

				IconView?.SetImageDrawable(null);
				IconView?.SetImageBitmap(null);
				IconView?.Dispose();
				IconView = null;
				// ContentStack?.Dispose();
				// ContentStack = null;
				AccessoryStack?.Dispose();
				AccessoryStack = null;
				Cell = null;

				IconTokenSource?.Dispose();
				IconTokenSource = null;
				AndroidContext = null;

				BackgroundColor?.Dispose();
				BackgroundColor = null;
				SelectedColor?.Dispose();
				SelectedColor = null;
				Ripple?.Dispose();
				Ripple = null;

				Background?.Dispose();
				Background = null;
			}

			base.Dispose(disposing);
		}
	}
}
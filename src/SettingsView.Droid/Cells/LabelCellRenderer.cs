using System;
using System.ComponentModel;
using Android.Content;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Droid.Cells;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LabelCell), typeof(LabelCellRenderer))]

namespace Jakar.SettingsView.Droid.Cells
{
	/// <summary>
	/// Label cell renderer.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class LabelCellRenderer : CellBaseRenderer<LabelCellView> { }

	/// <summary>
	/// Label cell view.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class LabelCellView : CellBaseView
	{
		private LabelCell _LabelCell => Cell as LabelCell;

		public TextView ValueLabel { get; set; }
		public TextView VValueLabel { get; set; }


		public LabelCellView( Context context, Cell cell ) : base(context, cell)
		{
			ValueLabel = new TextView(context)
						 {
							 Ellipsize = TextUtils.TruncateAt.End,
							 Gravity = GravityFlags.Right
						 };
			ValueLabel.SetSingleLine(true);

			using var textParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
			{
				ContentStack.AddView(ValueLabel, textParams);
			}
		}
		public LabelCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }

		public override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);

			if ( e.PropertyName == LabelCell.ValueTextProperty.PropertyName ) { UpdateValueText(); }
			else if ( e.PropertyName == LabelCell.ValueTextFontSizeProperty.PropertyName ) { UpdateValueTextFontSize(); }
			else if ( e.PropertyName == LabelCell.ValueTextFontFamilyProperty.PropertyName || e.PropertyName == LabelCell.ValueTextFontAttributesProperty.PropertyName ) { UpdateValueTextFont(); }
			else if ( e.PropertyName == LabelCell.ValueTextColorProperty.PropertyName ) { UpdateValueTextColor(); }
			else if ( e.PropertyName == LabelCell.IgnoreUseDescriptionAsValueProperty.PropertyName ) { UpdateUseDescriptionAsValue(); }
		}
		public override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);

			if ( e.PropertyName == Shared.SettingsView.CellValueTextColorProperty.PropertyName ) { UpdateValueTextColor(); }
			else if ( e.PropertyName == Shared.SettingsView.CellValueTextFontSizeProperty.PropertyName ) { UpdateValueTextFontSize(); }
			else if ( e.PropertyName == Shared.SettingsView.CellValueTextFontFamilyProperty.PropertyName || e.PropertyName == Shared.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { UpdateValueTextFont(); }
		}


		protected override void SetEnabledAppearance( bool isEnabled )
		{
			if ( isEnabled ) { ValueLabel.Alpha = 1f; }
			else { ValueLabel.Alpha = 0.3f; }

			base.SetEnabledAppearance(isEnabled);
		}


		public override void UpdateCell()
		{
			base.UpdateCell();
			UpdateUseDescriptionAsValue(); //at first after base
			UpdateValueText();
			UpdateValueTextColor();
			UpdateValueTextFontSize();
			UpdateValueTextFont();
		}
		private void UpdateUseDescriptionAsValue()
		{
			if ( !_LabelCell.IgnoreUseDescriptionAsValue && CellParent != null && CellParent.UseDescriptionAsValue )
			{
				VValueLabel = DescriptionLabel;
				DescriptionLabel.Visibility = ViewStates.Visible;
				ValueLabel.Visibility = ViewStates.Gone;
			}
			else
			{
				VValueLabel = ValueLabel;
				ValueLabel.Visibility = ViewStates.Visible;
			}
		}
		protected void UpdateValueText() { VValueLabel.Text = _LabelCell.ValueText; }
		private void UpdateValueTextFontSize()
		{
			if ( _LabelCell.ValueTextFontSize > 0 ) { ValueLabel.SetTextSize(Android.Util.ComplexUnitType.Sp, (float) _LabelCell.ValueTextFontSize); }
			else if ( CellParent != null ) { ValueLabel.SetTextSize(Android.Util.ComplexUnitType.Sp, (float) CellParent.CellValueTextFontSize); }

			Invalidate();
		}
		private void UpdateValueTextFont()
		{
			string family = _LabelCell.ValueTextFontFamily ?? CellParent?.CellValueTextFontFamily;
			FontAttributes attr = _LabelCell.ValueTextFontAttributes ?? CellParent.CellValueTextFontAttributes;

			ValueLabel.Typeface = FontUtility.CreateTypeface(family, attr);
			Invalidate();
		}
		private void UpdateValueTextColor()
		{
			if ( _LabelCell.ValueTextColor != Color.Default ) { ValueLabel.SetTextColor(_LabelCell.ValueTextColor.ToAndroid()); }
			else if ( CellParent != null && CellParent.CellValueTextColor != Color.Default ) { ValueLabel.SetTextColor(CellParent.CellValueTextColor.ToAndroid()); }
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				ValueLabel?.Dispose();
				ValueLabel = null;
				VValueLabel = null;
			}

			base.Dispose(disposing);
		}
	}
}
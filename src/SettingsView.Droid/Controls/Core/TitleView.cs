using System;
using System.ComponentModel;
using Android.Content;
using Android.Util;
using Jakar.SettingsView.Droid.Extensions;
using Jakar.SettingsView.Shared.CellBase;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using BaseCellView = Jakar.SettingsView.Droid.BaseCell.BaseCellView;

#nullable enable
namespace Jakar.SettingsView.Droid.Controls.Core
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public class TitleView : BaseTextView
	{
		private TitleCellBase _CurrentCell => _Cell.Cell as TitleCellBase ?? throw new NullReferenceException(nameof(_CurrentCell));

		public TitleView( Context context ) : base(context) { }
		public TitleView( BaseCellView baseView, Context context ) : base(baseView, context) { }
		public TitleView( Context context, IAttributeSet attributes ) : base(context, attributes) { }


		public override bool UpdateText()
		{
			Text = _CurrentCell.Title;
			//hide TextView right padding when TextView.Text empty.
			// Visibility = string.IsNullOrEmpty(Text) ? ViewStates.Gone : ViewStates.Visible;

			return true;
		}
		public override bool UpdateTextColor()
		{
			SetTextColor(_CurrentCell.TitleConfig.Color.ToAndroid());
			// SetTextColor(DefaultTextColor);

			return true;
		}
		public override bool UpdateFontSize()
		{
			SetTextSize(ComplexUnitType.Sp, (float) _CurrentCell.TitleConfig.FontSize);
			// SetTextSize(ComplexUnitType.Sp, DefaultFontSize);

			return true;
		}
		public override bool UpdateFont()
		{
			string? family = _CurrentCell.TitleConfig.FontFamily;
			FontAttributes attr = _CurrentCell.TitleConfig.FontAttributes;

			Typeface = FontUtility.CreateTypeface(family, attr);

			return true;
		}
		public override bool UpdateTextAlignment()
		{
			TextAlignment alignment = _CurrentCell.TitleConfig.TextAlignment;
			TextAlignment = alignment.ToAndroidTextAlignment();
			Gravity = alignment.ToGravityFlags();

			return true;
		}

		public override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == TitleCellBase.TitleProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == TitleCellBase.TitleColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == TitleCellBase.TitleFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == TitleCellBase.TitleFontFamilyProperty.PropertyName ||
				 e.PropertyName == TitleCellBase.TitleFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == TitleCellBase.TitleAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			return base.Update(sender, e);
		}
		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.sv.SettingsView.CellTitleColorProperty.PropertyName ) { return UpdateBackgroundColor(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellTitleFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellTitleAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellTitleFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.sv.SettingsView.CellTitleFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			return base.UpdateParent(sender, e);
		}
	}
}
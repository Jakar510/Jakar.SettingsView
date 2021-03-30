using System;
using System.ComponentModel;
using Android.Content;
using Android.Util;
using Jakar.Api.Droid.Extensions;
using Jakar.Api.Extensions;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Misc;
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
			if ( e.IsEqual(TitleCellBase.TitleProperty) ) { return UpdateText(); }

			if ( e.IsEqual(TitleCellBase.TitleColorProperty) ) { return UpdateTextColor(); }

			if ( e.IsEqual(TitleCellBase.TitleFontSizeProperty) ) { return UpdateFontSize(); }

			if ( e.IsOneOf(TitleCellBase.TitleFontFamilyProperty, TitleCellBase.TitleFontAttributesProperty) ) { return UpdateFont(); }

			if ( e.IsEqual(TitleCellBase.TitleAlignmentProperty) ) { return UpdateTextAlignment(); }

			return base.Update(sender, e);
		}
		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(Shared.sv.SettingsView.CellTitleColorProperty) ) { return UpdateTextColor(); }

			if ( e.IsEqual(Shared.sv.SettingsView.CellTitleFontSizeProperty) ) { return UpdateFontSize(); }

			if ( e.IsEqual(Shared.sv.SettingsView.CellTitleAlignmentProperty) ) { return UpdateTextAlignment(); }

			if ( e.IsOneOf(Shared.sv.SettingsView.CellTitleFontFamilyProperty, Shared.sv.SettingsView.CellTitleFontAttributesProperty) ) { return UpdateFont(); }

			return base.UpdateParent(sender, e);
		}
	}
}
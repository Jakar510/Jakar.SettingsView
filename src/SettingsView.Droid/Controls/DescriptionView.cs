using System;
using System.ComponentModel;
using Android.Util;
using Android.Views;
using Jakar.SettingsView.Droid.Extensions;
using Jakar.SettingsView.Shared.Cells.Base;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using BaseCellView = Jakar.SettingsView.Droid.BaseCell.BaseCellView;
using AContext = Android.Content.Context;
using TextAlignment = Xamarin.Forms.TextAlignment;

#nullable enable
namespace Jakar.SettingsView.Droid.Controls
{
	public class DescriptionView : BaseTextView
	{
		private CellBaseDescription _CurrentCell => _Cell.Cell as CellBaseDescription ?? throw new NullReferenceException(nameof(_CurrentCell));

		public DescriptionView( AContext context ) : base(context) { }
		public DescriptionView( BaseCellView baseView, AContext context ) : base(baseView, context) { }
		public DescriptionView( AContext context, IAttributeSet attributes ) : base(context, attributes) { }


		public override bool UpdateText()
		{
			Text = _CurrentCell.Description;
			Visibility = string.IsNullOrEmpty(Text) ? ViewStates.Gone : ViewStates.Visible;

			return true;
		}
		public override bool UpdateFontSize()
		{
			if ( _CurrentCell.DescriptionFontSize > 0 ) { SetTextSize(ComplexUnitType.Sp, (float) _CurrentCell.DescriptionFontSize); }
			else if ( _Cell.CellParent != null ) { SetTextSize(ComplexUnitType.Sp, (float) _Cell.CellParent.CellDescriptionFontSize); }
			else { SetTextSize(ComplexUnitType.Sp, DefaultFontSize); }

			return true;
		}
		public override bool UpdateColor()
		{
			if ( _CurrentCell.DescriptionColor != Color.Default ) { SetTextColor(_CurrentCell.DescriptionColor.ToAndroid()); }
			else if ( _Cell.CellParent != null &&
					  _Cell.CellParent.CellDescriptionColor != Color.Default ) { SetTextColor(_Cell.CellParent.CellDescriptionColor.ToAndroid()); }
			else { SetTextColor(DefaultTextColor); }

			return true;
		}
		public override bool UpdateFont()
		{
			string? family = _CurrentCell.DescriptionFontFamily ?? _Cell.CellParent?.CellDescriptionFontFamily;
			FontAttributes attr = _CurrentCell.DescriptionFontAttributes ?? _Cell.CellParent?.CellDescriptionFontAttributes ?? FontAttributes.None;

			Typeface = FontUtility.CreateTypeface(family, attr);

			return true;
		}
		public bool UpdateTextAlignment()
		{
			TextAlignment alignment = _CurrentCell.DescriptionAlignment ?? _CurrentCell.Parent.CellDescriptionAlignment;
			TextAlignment = alignment.ToAndroidTextAlignment();
			Gravity = alignment.ToGravityFlags();

			return true;
		}

		public override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CellBaseDescription.DescriptionProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == CellBaseDescription.DescriptionFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == CellBaseDescription.DescriptionFontFamilyProperty.PropertyName ||
				 e.PropertyName == CellBaseDescription.DescriptionFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == CellBaseDescription.DescriptionColorProperty.PropertyName ) { return UpdateColor(); }

			if ( e.PropertyName == CellBaseDescription.DescriptionAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			return false;
		}
		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.SettingsView.CellDescriptionColorProperty.PropertyName ) { return UpdateColor(); }

			if ( e.PropertyName == Shared.SettingsView.CellDescriptionFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.SettingsView.CellDescriptionAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == Shared.SettingsView.CellDescriptionFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.SettingsView.CellDescriptionFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			return false;
		}
		public override void Update()
		{
			UpdateText();
			UpdateColor();
			UpdateFontSize();
			UpdateFont();
			UpdateTextAlignment();
		}
	}
}
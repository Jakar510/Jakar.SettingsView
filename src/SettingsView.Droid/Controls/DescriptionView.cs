using System;
using System.ComponentModel;
using Android.Util;
using Android.Views;
using Jakar.SettingsView.Droid.Extensions;
using Jakar.SettingsView.Shared.CellBase;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using BaseCellView = Jakar.SettingsView.Droid.BaseCell.BaseCellView;
using AContext = Android.Content.Context;
using TextAlignment = Xamarin.Forms.TextAlignment;

#nullable enable
namespace Jakar.SettingsView.Droid.Controls
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public class DescriptionView : BaseTextView
	{
		private DescriptionCellBase _CurrentCell => _Cell.Cell as DescriptionCellBase ?? throw new NullReferenceException(nameof(_CurrentCell));

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
			if ( e.PropertyName == DescriptionCellBase.DescriptionProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == DescriptionCellBase.DescriptionFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == DescriptionCellBase.DescriptionFontFamilyProperty.PropertyName ||
				 e.PropertyName == DescriptionCellBase.DescriptionFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == DescriptionCellBase.DescriptionColorProperty.PropertyName ) { return UpdateColor(); }

			if ( e.PropertyName == DescriptionCellBase.DescriptionAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			return false;
		}
		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.sv.SettingsView.CellDescriptionColorProperty.PropertyName ) { return UpdateColor(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellDescriptionFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellDescriptionAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellDescriptionFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.sv.SettingsView.CellDescriptionFontAttributesProperty.PropertyName ) { return UpdateFont(); }

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
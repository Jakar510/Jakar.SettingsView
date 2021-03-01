using System;
using System.ComponentModel;
using Android.Content;
using Android.Util;
using Android.Views;
using Jakar.SettingsView.Shared.Cells.Base;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using BaseCellView = Jakar.SettingsView.Droid.Cells.Base.BaseCellView;
using AContext = Android.Content.Context;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Controls
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

		// public override bool UpdateAlignment() { _TextAlignment = _CellBase.DescriptionTextAlignment; }

		public override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CellBaseDescription.DescriptionProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == CellBaseDescription.DescriptionFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == CellBaseDescription.DescriptionFontFamilyProperty.PropertyName ||
				 e.PropertyName == CellBaseDescription.DescriptionFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == CellBaseDescription.DescriptionColorProperty.PropertyName ) { return UpdateColor(); }

			return false;
		}
		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.SettingsView.CellDescriptionColorProperty.PropertyName ) { return UpdateColor(); }

			if ( e.PropertyName == Shared.SettingsView.CellDescriptionFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

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
		}
	}
}
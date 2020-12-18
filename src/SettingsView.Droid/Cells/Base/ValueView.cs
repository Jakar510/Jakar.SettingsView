using System;
using System.ComponentModel;
using Android.Content;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Cells.Base;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Base
{
	public class ValueView : BaseTextView
	{
		private CellBaseValue _CurrentCell => _Cell.Cell as CellBaseValue ?? throw new NullReferenceException(nameof(_CurrentCell));
		private CellBaseValueText? _CurrentTextCell => _CurrentCell as CellBaseValueText;


		public ValueView( Context context ) : base(context)
		{
			Ellipsize = TextUtils.TruncateAt.End;
			Gravity = GravityFlags.Right;
		}
		public ValueView( CellBaseView baseView, Context context ) : base(baseView, context)
		{
			Ellipsize = TextUtils.TruncateAt.End;
			Gravity = GravityFlags.Right;
		}
		public ValueView( Context context, IAttributeSet attributes ) : base(context, attributes) { }


		protected internal override bool UpdateText()
		{
			if ( _CurrentTextCell is null ) return false;
			Text = _CurrentTextCell.ValueText;
			Visibility = string.IsNullOrEmpty(Text) ? ViewStates.Gone : ViewStates.Visible;

			return true;
		}
		protected internal override bool UpdateFontSize()
		{
			if ( _CurrentCell.ValueTextFontSize > 0 ) { SetTextSize(ComplexUnitType.Sp, (float) _CurrentCell.ValueTextFontSize); }
			else if ( _Cell.CellParent != null ) { SetTextSize(ComplexUnitType.Sp, (float) _Cell.CellParent.CellValueTextFontSize); }
			else { SetTextSize(ComplexUnitType.Sp, DefaultFontSize); }

			return true;
		}
		protected internal override bool UpdateColor()
		{
			if ( _CurrentCell.ValueTextColor != Color.Default ) { SetTextColor(_CurrentCell.ValueTextColor.ToAndroid()); }
			else if ( _Cell.CellParent != null &&
					  _Cell.CellParent.CellValueTextColor != Color.Default ) { SetTextColor(_Cell.CellParent.CellValueTextColor.ToAndroid()); }
			else { SetTextColor(DefaultTextColor); }

			return true;
		}
		protected internal override bool UpdateFont()
		{
			string? family = _CurrentCell.ValueTextFontFamily ?? _Cell.CellParent?.CellValueTextFontFamily;
			FontAttributes attr = _CurrentCell.ValueTextFontAttributes ?? _Cell.CellParent?.CellValueTextFontAttributes ?? FontAttributes.None;

			Typeface = FontUtility.CreateTypeface(family, attr);

			return true;
		}
		// protected internal override bool UpdateAlignment() { _TextAlignment = _CellBase.ValueTextTextAlignment; }

		protected internal override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CellBaseValueText.ValueTextProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == CellBaseValueText.ValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == CellBaseValueText.ValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == CellBaseValueText.ValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == CellBaseValueText.ValueTextColorProperty.PropertyName ) { return UpdateColor(); }

			return false;
		}
		protected internal override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.SettingsView.CellValueTextColorProperty.PropertyName ) { return UpdateColor(); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			return false;
		}
		protected internal override void Update()
		{
			UpdateText();
			UpdateColor();
			UpdateFontSize();
			UpdateFont();
		}
	}
}
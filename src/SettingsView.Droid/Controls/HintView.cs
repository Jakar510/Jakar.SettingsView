using System;
using System.ComponentModel;
using Android.Content;
using Android.Util;
using Android.Views;
using Jakar.SettingsView.Droid.Extensions;
using Jakar.SettingsView.Shared.CellBase;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using BaseCellView = Jakar.SettingsView.Droid.BaseCell.BaseCellView;
using TextAlignment = Xamarin.Forms.TextAlignment;

#nullable enable
namespace Jakar.SettingsView.Droid.Controls
{
	public class HintView : BaseTextView
	{
		[Android.Runtime.Preserve(AllMembers = true)]
		private HintTextCellBase _CurrentCell => _Cell.Cell as HintTextCellBase ?? throw new NullReferenceException(nameof(_CurrentCell));

		public HintView( Context context ) : base(context) { }
		public HintView( BaseCellView baseView, Context context ) : base(baseView, context) { }
		public HintView( Context context, IAttributeSet attributes ) : base(context, attributes) { }


		public override bool UpdateText()
		{
			Text = _CurrentCell.Hint;
			Visibility = string.IsNullOrEmpty(Text) ? ViewStates.Gone : ViewStates.Visible;

			return true;
		}
		public override bool UpdateFontSize()
		{
			if ( _CurrentCell.HintFontSize > 0 ) { SetTextSize(ComplexUnitType.Sp, (float) _CurrentCell.HintFontSize); }
			else if ( _Cell.CellParent != null ) { SetTextSize(ComplexUnitType.Sp, (float) _Cell.CellParent.CellHintFontSize); }
			else { SetTextSize(ComplexUnitType.Sp, DefaultFontSize); }

			return true;
		}
		public override bool UpdateColor()
		{
			if ( _CurrentCell.HintColor != Color.Default ) { SetTextColor(_CurrentCell.HintColor.ToAndroid()); }
			else if ( _Cell.CellParent != null &&
					  _Cell.CellParent.CellHintTextColor != Color.Default ) { SetTextColor(_Cell.CellParent.CellHintTextColor.ToAndroid()); }
			else { SetTextColor(DefaultTextColor); }

			return true;
		}
		public override bool UpdateFont()
		{
			string? family = _CurrentCell.HintFontFamily ?? _Cell.CellParent?.CellHintFontFamily;
			FontAttributes attr = _CurrentCell.HintFontAttributes ?? _Cell.CellParent?.CellHintFontAttributes ?? FontAttributes.None;

			Typeface = FontUtility.CreateTypeface(family, attr);

			return true;
		}
		public bool UpdateTextAlignment()
		{
			TextAlignment alignment = _CurrentCell.HintAlignment ?? _CurrentCell.Parent.CellHintAlignment;
			TextAlignment = alignment.ToAndroidTextAlignment();
			Gravity = alignment.ToGravityFlags();

			return true;
		}

		public override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == HintTextCellBase.HintProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == HintTextCellBase.HintFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == HintTextCellBase.HintFontFamilyProperty.PropertyName ||
				 e.PropertyName == HintTextCellBase.HintFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == HintTextCellBase.HintColorProperty.PropertyName ) { return UpdateColor(); }

			if ( e.PropertyName == HintTextCellBase.HintAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			return false;
		}
		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.sv.SettingsView.CellHintTextColorProperty.PropertyName ) { return UpdateColor(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellHintAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellHintFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellHintTextColorProperty.PropertyName ||
				 e.PropertyName == Shared.sv.SettingsView.CellHintFontAttributesProperty.PropertyName ) { return UpdateFont(); }

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
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.Content;
using Android.Util;
using Android.Views;
using Jakar.SettingsView.Droid.Extensions;
using Jakar.SettingsView.Shared.Cells.Base;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using BaseCellView = Jakar.SettingsView.Droid.BaseCell.BaseCellView;
using TextAlignment = Xamarin.Forms.TextAlignment;

#nullable enable
namespace Jakar.SettingsView.Droid.Controls
{
	public class ValueView : BaseTextView
	{
		private CellBaseValue _CurrentCell => _Cell.Cell as CellBaseValue ?? throw new NullReferenceException(nameof(_CurrentCell));
		private CellBaseValueText? _CurrentTextCell => _CurrentCell as CellBaseValueText;


		public ValueView( Context context ) : base(context) => Init();
		public ValueView( BaseCellView baseView, Context context ) : base(baseView, context) => Init();
		public ValueView( Context context, IAttributeSet attributes ) : base(context, attributes) => Init();


		public override bool UpdateText()
		{
			if ( _CurrentTextCell is null ) return false;
			Text = _CurrentTextCell.ValueText;
			Visibility = string.IsNullOrEmpty(Text) ? ViewStates.Gone : ViewStates.Visible;

			return true;
		}
		public override bool UpdateFontSize()
		{
			if ( _CurrentCell.ValueTextFontSize > 0 ) { SetTextSize(ComplexUnitType.Sp, (float) _CurrentCell.ValueTextFontSize); }
			else if ( _Cell.CellParent != null ) { SetTextSize(ComplexUnitType.Sp, (float) _Cell.CellParent.CellValueTextFontSize); }
			else { SetTextSize(ComplexUnitType.Sp, DefaultFontSize); }

			return true;
		}
		public override bool UpdateColor()
		{
			if ( _CurrentCell.ValueTextColor != Color.Default ) { SetTextColor(_CurrentCell.ValueTextColor.ToAndroid()); }
			else if ( _Cell.CellParent != null &&
					  _Cell.CellParent.CellValueTextColor != Color.Default ) { SetTextColor(_Cell.CellParent.CellValueTextColor.ToAndroid()); }
			else { SetTextColor(DefaultTextColor); }

			return true;
		}
		public override bool UpdateFont()
		{
			string? family = _CurrentCell.ValueTextFontFamily ?? _Cell.CellParent?.CellValueTextFontFamily;
			FontAttributes attr = _CurrentCell.ValueTextFontAttributes ?? _Cell.CellParent?.CellValueTextFontAttributes ?? FontAttributes.None;

			Typeface = FontUtility.CreateTypeface(family, attr);

			return true;
		}
		public bool UpdateTextAlignment()
		{
			if ( _CurrentTextCell == null ) return true;
			TextAlignment alignment = _CurrentTextCell.ValueTextAlignment ?? _CurrentCell.Parent.CellValueTextAlignment;
			Console.WriteLine($"\"{GetType().FullName}\"    _____________alignment_____________: TextAlignment.{alignment}\n\n");
			TextAlignment = alignment.ToAndroidTextAlignment();
			Gravity = alignment.ToGravityFlags();

			return true;
		}

		public override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CellBaseValueText.ValueTextProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == CellBaseValue.ValueTextAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == CellBaseValue.ValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == CellBaseValue.ValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == CellBaseValue.ValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == CellBaseValue.ValueTextColorProperty.PropertyName ) { return UpdateColor(); }

			return false;
		}
		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.SettingsView.CellValueTextColorProperty.PropertyName ) { return UpdateColor(); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

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
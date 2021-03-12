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
namespace Jakar.SettingsView.Droid.Controls.Core
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public class ValueView : BaseTextView
	{
		private ValueCellBase _CurrentCell => _Cell.Cell as ValueCellBase ?? throw new NullReferenceException(nameof(_CurrentCell));
		private ValueTextCellBase? _CurrentTextCell => _CurrentCell as ValueTextCellBase;


		public ValueView( Context context ) : base(context) => Initialize();
		public ValueView( BaseCellView baseView, Context context ) : base(baseView, context) => Initialize();
		public ValueView( Context context, IAttributeSet attributes ) : base(context, attributes) => Initialize();


		public override bool UpdateText() => _CurrentTextCell is not null && UpdateText(_CurrentTextCell.ValueText);
		public bool UpdateText( string? text )
		{
			Text = text;
			Visibility = string.IsNullOrEmpty(Text)
							 ? ViewStates.Gone
							 : ViewStates.Visible;

			return true;
		}
		public override bool UpdateFontSize()
		{
			SetTextSize(ComplexUnitType.Sp, (float) _CurrentCell.ValueTextConfig.FontSize);
			// SetTextSize(ComplexUnitType.Sp, DefaultFontSize);

			return true;
		}
		public override bool UpdateTextColor()
		{
			SetTextColor(_CurrentCell.ValueTextConfig.Color.ToAndroid());
			SetTextColor(DefaultTextColor);

			return true;
		}
		public override bool UpdateFont()
		{
			string? family = _CurrentCell.HintConfig.FontFamily;
			FontAttributes attr = _CurrentCell.ValueTextConfig.FontAttributes;

			Typeface = FontUtility.CreateTypeface(family, attr);

			return true;
		}
		public override bool UpdateTextAlignment()
		{
			TextAlignment alignment = _CurrentCell.ValueTextConfig.TextAlignment;
			TextAlignment = alignment.ToAndroidTextAlignment();
			Gravity = alignment.ToGravityFlags();

			return true;
		}

		public override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == ValueTextCellBase.ValueTextProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == ValueCellBase.ValueTextAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == ValueCellBase.ValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == ValueCellBase.ValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == ValueCellBase.ValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == ValueCellBase.ValueTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			// if ( e.PropertyName == CellBase.BackgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }

			return base.Update(sender, e);
		}
		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextColorProperty.PropertyName ) { return UpdateBackgroundColor(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.sv.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			return base.UpdateParent(sender, e);
		}
	}
}
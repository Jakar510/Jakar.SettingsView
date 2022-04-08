

namespace Jakar.SettingsView.Droid.Controls.Core;

public class HintView : BaseTextView
{
    [Preserve(AllMembers = true)]
    private HintTextCellBase _CurrentCell => _Cell.Cell as HintTextCellBase ?? throw new NullReferenceException(nameof(_CurrentCell));

    public HintView( Context      context ) : base(context) { }
    public HintView( BaseCellView baseView, Context       context ) : base(baseView, context) { }
    public HintView( Context      context,  IAttributeSet attributes ) : base(context, attributes) { }


    public override bool UpdateText()
    {
        Text = _CurrentCell.Hint;
        Visibility = string.IsNullOrEmpty(Text)
                         ? ViewStates.Gone
                         : ViewStates.Visible;

        return true;
    }
    public override bool UpdateFontSize()
    {
        SetTextSize(ComplexUnitType.Sp, (float) _CurrentCell.HintConfig.FontSize);
        // SetTextSize(ComplexUnitType.Sp, DefaultFontSize);

        return true;
    }
    public override bool UpdateTextColor()
    {
        SetTextColor(_CurrentCell.HintConfig.Color.ToAndroid());
        SetTextColor(DefaultTextColor);

        return true;
    }
    public override bool UpdateFont()
    {
        string?        family = _CurrentCell.HintConfig.FontFamily;
        FontAttributes attr   = _CurrentCell.DescriptionConfig.FontAttributes;

        Typeface = FontUtility.CreateTypeface(family, attr);

        return true;
    }
    public override bool UpdateTextAlignment()
    {
        TextAlignment alignment = _CurrentCell.HintConfig.TextAlignment;
        TextAlignment = alignment.ToAndroidTextAlignment();
        Gravity       = alignment.ToGravityFlags();

        return true;
    }

    public override bool Update( object sender, PropertyChangedEventArgs e )
    {
        if ( e.IsEqual(HintTextCellBase.hintProperty) ) { return UpdateText(); }

        if ( e.IsEqual(HintTextCellBase.hintFontSizeProperty) ) { return UpdateFontSize(); }

        if ( e.IsOneOf(HintTextCellBase.hintFontFamilyProperty, HintTextCellBase.hintFontAttributesProperty) ) { return UpdateFont(); }

        if ( e.IsEqual(HintTextCellBase.hintColorProperty) ) { return UpdateTextColor(); }

        if ( e.IsEqual(HintTextCellBase.hintAlignmentProperty) ) { return UpdateTextAlignment(); }
			
        return base.Update(sender, e);
    }
    public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
    {
        if ( e.IsEqual(Shared.sv.SettingsView.cellHintTextColorProperty) ) { return UpdateTextColor(); }

        if ( e.IsEqual(Shared.sv.SettingsView.cellHintAlignmentProperty) ) { return UpdateTextAlignment(); }

        if ( e.IsEqual(Shared.sv.SettingsView.cellHintFontSizeProperty) ) { return UpdateFontSize(); }

        if ( e.IsOneOf(Shared.sv.SettingsView.cellHintTextColorProperty, Shared.sv.SettingsView.cellHintFontAttributesProperty) ) { return UpdateFont(); }

        return base.UpdateParent(sender, e);
    }
}
namespace Jakar.SettingsView.Droid.Controls.Core;

[Preserve(AllMembers = true)]
public class ValueView : BaseTextView
{
    private ValueCellBase      _CurrentCell     => _Cell.Cell as ValueCellBase ?? throw new NullReferenceException(nameof(_CurrentCell));
    private ValueTextCellBase? _CurrentTextCell => _CurrentCell as ValueTextCellBase;


    public ValueView( Context      context ) : base(context) => Initialize();
    public ValueView( BaseCellView baseView, Context       context ) : base(baseView, context) => Initialize();
    public ValueView( Context      context,  IAttributeSet attributes ) : base(context, attributes) => Initialize();


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
        SetTextSize(ComplexUnitType.Sp, (float)_CurrentCell.ValueTextConfig.FontSize);

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
        string?        family = _CurrentCell.HintConfig.FontFamily;
        FontAttributes attr   = _CurrentCell.ValueTextConfig.FontAttributes;

        Typeface = FontUtility.CreateTypeface(family, attr);

        return true;
    }

    public override bool UpdateTextAlignment()
    {
        TextAlignment alignment = _CurrentCell.ValueTextConfig.TextAlignment;
        TextAlignment = alignment.ToAndroidTextAlignment();
        Gravity       = alignment.ToGravityFlags();

        return true;
    }

    public override bool Update( object sender, PropertyChangedEventArgs e )
    {
        if ( e.IsEqual(ValueTextCellBase.valueTextProperty) ) { return UpdateText(); }

        if ( e.IsEqual(ValueCellBase.valueTextAlignmentProperty) ) { return UpdateTextAlignment(); }

        if ( e.IsEqual(ValueCellBase.valueTextFontSizeProperty) ) { return UpdateFontSize(); }

        if ( e.IsOneOf(ValueCellBase.valueTextFontFamilyProperty, ValueCellBase.valueTextFontAttributesProperty) ) { return UpdateFont(); }

        if ( e.IsEqual(ValueCellBase.valueTextColorProperty) ) { return UpdateTextColor(); }

        return base.Update(sender, e);
    }

    public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
    {
        if ( e.IsEqual(Shared.sv.SettingsView.cellValueTextColorProperty) ) { return UpdateTextColor(); }

        if ( e.IsEqual(Shared.sv.SettingsView.cellValueTextAlignmentProperty) ) { return UpdateTextAlignment(); }

        if ( e.IsEqual(Shared.sv.SettingsView.cellValueTextFontSizeProperty) ) { return UpdateFontSize(); }

        if ( e.IsOneOf(Shared.sv.SettingsView.cellValueTextFontFamilyProperty, Shared.sv.SettingsView.cellValueTextFontAttributesProperty) ) { return UpdateFont(); }

        return base.UpdateParent(sender, e);
    }
}

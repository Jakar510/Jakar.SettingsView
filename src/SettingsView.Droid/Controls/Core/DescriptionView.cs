

namespace Jakar.SettingsView.Droid.Controls.Core;

[Preserve(AllMembers = true)]
public class DescriptionView : BaseTextView
{
    private DescriptionCellBase _CurrentCell => _Cell.Cell as DescriptionCellBase ?? throw new NullReferenceException(nameof(_CurrentCell));

    public DescriptionView( AContext     context ) : base(context) { }
    public DescriptionView( BaseCellView baseView, AContext      context ) : base(baseView, context) { }
    public DescriptionView( AContext     context,  IAttributeSet attributes ) : base(context, attributes) { }


    public override bool UpdateText()
    {
        Text = _CurrentCell.Description;
        Visibility = string.IsNullOrEmpty(Text)
                         ? ViewStates.Gone
                         : ViewStates.Visible;

        return true;
    }
    public override bool UpdateFontSize()
    {
        SetTextSize(ComplexUnitType.Sp, (float) _CurrentCell.DescriptionConfig.FontSize);
        // SetTextSize(ComplexUnitType.Sp, DefaultFontSize);

        return true;
    }
    public override bool UpdateTextColor()
    {
        SetTextColor(_CurrentCell.DescriptionConfig.Color.ToAndroid());

        return true;
    }
    public override bool UpdateFont()
    {
        string?        family = _CurrentCell.DescriptionConfig.FontFamily;
        FontAttributes attr   = _CurrentCell.DescriptionConfig.FontAttributes;

        Typeface = FontUtility.CreateTypeface(family, attr);

        return true;
    }
    public override bool UpdateTextAlignment()
    {
        TextAlignment alignment = _CurrentCell.DescriptionConfig.TextAlignment;
        TextAlignment = alignment.ToAndroidTextAlignment();
        Gravity       = alignment.ToGravityFlags();

        return true;
    }

    public override bool Update( object sender, PropertyChangedEventArgs e )
    {
        if ( e.PropertyName == DescriptionCellBase.descriptionProperty.PropertyName ) { return UpdateText(); }

        if ( e.PropertyName == DescriptionCellBase.descriptionFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

        if ( e.PropertyName == DescriptionCellBase.descriptionFontFamilyProperty.PropertyName ||
             e.PropertyName == DescriptionCellBase.descriptionFontAttributesProperty.PropertyName ) { return UpdateFont(); }

        if ( e.PropertyName == DescriptionCellBase.descriptionColorProperty.PropertyName ) { return UpdateTextColor(); }

        if ( e.PropertyName == DescriptionCellBase.descriptionAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

        // if ( e.PropertyName == CellBase.BackgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }

        return base.Update(sender, e);
    }
    public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
    {
        if ( e.PropertyName == Shared.sv.SettingsView.cellDescriptionColorProperty.PropertyName ) { return UpdateTextColor(); }

        if ( e.PropertyName == Shared.sv.SettingsView.cellDescriptionFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

        if ( e.PropertyName == Shared.sv.SettingsView.cellDescriptionAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

        if ( e.PropertyName == Shared.sv.SettingsView.cellDescriptionFontFamilyProperty.PropertyName ||
             e.PropertyName == Shared.sv.SettingsView.cellDescriptionFontAttributesProperty.PropertyName ) { return UpdateFont(); }

        return base.UpdateParent(sender, e);
    }
}
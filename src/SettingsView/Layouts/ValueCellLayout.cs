namespace Jakar.SettingsView.Shared.Layouts;

public class BaseValueCellLayout<TValue> : BaseCellTitleLayout where TValue : View, IUpdateTextControl, new()
{
    public Hint   Hint  { get; protected set; }
    public TValue Value { get; protected set; }


    public BaseValueCellLayout()
    {
        Hint = new Hint();

        Value = new TValue();
        SetRow(Value, 0);
        SetColumn(Value, 2);

        RowDefinitions    = Definitions.Rows();
        ColumnDefinitions = Definitions.Columns.Value();

        Children.Add(Icon);
        Children.Add(Title);
        Children.Add(Description);
        Children.Add(Hint);
        Children.Add(Value);
    }
}



public class EntryCellLayout : BaseValueCellLayout<EntryValue> { }



public class ValueCellLayout : BaseValueCellLayout<Value> { }
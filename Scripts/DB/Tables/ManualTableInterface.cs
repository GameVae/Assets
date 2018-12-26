

namespace ManualTable.Interface
{
    public interface IManualRow
    {
        int FieldCount { get; }
        string ValuesSequence { get; }
        string KeyValuePairs { get; }
    }

    public interface ITable { }
}
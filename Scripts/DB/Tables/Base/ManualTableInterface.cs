

using Json;
using Json.Interface;
using System.Collections.Generic;

namespace ManualTable.Interface
{
    public interface IManualRow
    {
        int FieldCount { get; }
        string ValuesSequence { get; }
        string KeyValuePairs { get; }
    }

    public interface ITable
    {
        System.Type RowType { get; }
        int Count { get; }
        IJSON this[int rowIndex] { get; set; }
    }
}
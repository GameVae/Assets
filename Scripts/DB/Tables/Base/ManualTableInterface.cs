

using Json;
using Json.Interface;
using System.Collections.Generic;

namespace ManualTable.Interface
{
    public interface IManualRow { }

    public interface ITable
    {
        System.Type RowType { get; }
        int Count { get; }
        IManualRow this[int rowIndex] { get; set; }
    }
}
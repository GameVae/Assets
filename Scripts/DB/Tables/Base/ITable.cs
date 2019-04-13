using Json;
using System.Collections.Generic;

namespace DataTable
{
    public interface ITableData { }

    public interface ISQLiteData : ITableData { }

    public interface ITable
    {
        int Count { get; }
        System.Type RowType { get; }
        ITableData this[int rowIndex] { get; set; }
    }
}
using System;

namespace DataTable
{
    public interface ISearchByFakeCompare<T>
        where T : ITableData
    {
        T GetSearchObject(object obj);
    }
    public interface ITableData: IComparable { }

    public interface ISQLiteData : ITableData { }

    public interface ITable
    {
        int Count { get; }
        Type RowType { get; }
        ITableData this[int rowIndex] { get; set; }

        void Clear();
    }

    public interface IJSONTable : ITable
    {
        void LoadTable(JSONObject jsonObj);
        void UpdateTable(JSONObject jsonObj);
    }

    public interface ISQLiteTable : ITable
    {
        void LoadTable();
    }
}
using DataTable.Row;
using UnityEngine;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New RSS Table", menuName = "DataTable/JsonTable/JSONTable", order = 7)]
    public sealed class JSONTable_RSSPosition : JSONTable<RSS_PositionRow>
    {
        public RSS_PositionRow GetRssAt(Vector3Int serPosition)
        {
            int count = Count;
            string position = serPosition.ToPositionString();

            for (int i = 0; i < count; i++)
            {
                if (Rows[i].Position.CompareTo(position) == 0)
                    return Rows[i];
            }
            return null;
        }
    }
}
using ManualTable.Row;
using System.Collections;
using UnityEngine;

namespace ManualTable
{
    [CreateAssetMenu(fileName = "New Unit Table", menuName = "SQLiteTable/Unit JSONTable", order = 12)]
    public sealed class UnitJSONTable : JSONTable<UnitRow>
    {
        private void Sort()
        {
            Rows?.BinarySort_L();
        }

        public UnitRow GetUnit(int id)
        {
            int index = Rows.BinarySearch_L(0, Rows.Count - 1, id);
            return index >= 0 ? Rows[index] : null;
        }

        public override void LoadTable(JSONObject data, bool clearPre = true)
        {
            base.LoadTable(data, clearPre);
            Sort();
        }
    }
}
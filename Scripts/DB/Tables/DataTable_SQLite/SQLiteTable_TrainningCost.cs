using EnumCollect;
using DataTable.Row;
using System.Linq;
using UnityEngine;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New TranningCost Table", menuName = "DataTable/SQLiteTable/TranningCost", order = 3)]
    public class SQLiteTable_TrainningCost : SQLiteTable<TrainningCostRow>
    {
        public TrainningCostRow this[ListUpgrade type]
        {
            get
            {
                return Rows.FirstOrDefault(r => r.ID_Unit == type);
            }
        }
    }
}
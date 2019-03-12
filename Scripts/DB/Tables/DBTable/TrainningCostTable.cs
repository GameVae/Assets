using EnumCollect;
using ManualTable.Row;
using System.Linq;
using UnityEngine;

namespace ManualTable
{
    [CreateAssetMenu(fileName = "New TranningCost Table", menuName = "SQLiteTable/TranningCost", order = 12)]
    public class TrainningCostTable : ManualTableBase<TrainningCostRow>
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
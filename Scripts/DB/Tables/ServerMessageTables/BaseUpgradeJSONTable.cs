using ManualTable.Row;
using UnityEngine;


namespace ManualTable
{
    [CreateAssetMenu(fileName = "New BaseUpg Table", menuName = "SQLiteTable/BaseUpg JSONTable", order = 6)]
    public sealed class BaseUpgradeJSONTable : JSONTable<BaseUpgradeRow>
    {
        public BaseUpgradeRow this[EnumCollect.ListUpgrade type]
        {
            get
            {
                try
                {
                    return Rows[(int)type - 1];
                }
                catch { return null; }
            }
        }
    }
}


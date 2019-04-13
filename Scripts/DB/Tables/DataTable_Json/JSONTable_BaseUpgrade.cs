using DataTable.Row;
using UnityEngine;


namespace DataTable
{
    [CreateAssetMenu(fileName = "New BaseUpg Table", menuName = "DataTable/JsonTable/BaseUpg JSONTable", order = 5)]
    public sealed class JSONTable_BaseUpgrade : JSONTable<BaseUpgradeRow>
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


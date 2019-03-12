
using ManualTable.Row;
using UnityEngine;

namespace ManualTable
{
    [CreateAssetMenu(fileName = "New Base Player Table", menuName = "SQLiteTable/BasePlayer JSONTable", order = 15)]
    public sealed class BasePlayerJSONTable : ManualTableJSON<BasePlayerRow>
    {
    }
}


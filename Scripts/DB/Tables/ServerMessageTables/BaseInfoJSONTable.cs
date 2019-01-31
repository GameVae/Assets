using System;
using ManualTable.Row;
using UnityEngine;

namespace ManualTable
{
    [CreateAssetMenu(fileName = "New BaseInfo Table", menuName = "SQLiteTable/BaseInfo JSONTable", order = 9)]
    public sealed class BaseInfoJSONTable : ManualTableJSON<BaseInfoRow>
    {
    }
}


using System;
using ManualTable.Row;
using UnityEngine;

namespace ManualTable
{
    [CreateAssetMenu(fileName = "New UserInfo Table", menuName = "SQLiteTable/UserInfo JSONTable", order = 10)]
    public sealed class UserInfoJSONTable : ManualTableJSON<UserInfoRow> { }
}


using System;
using ManualTable.Row;
using UnityEngine;

namespace ManualTable
{
    [CreateAssetMenu(fileName = "New UserInfo Table", menuName = "SQLiteTable/UserInfo JSONTable", order = 10)]
    public sealed class UserInfoJSONTable : ManualTableJSON<UserInfoRow>
    {
        private void Sort()
        {
            Rows?.BinarySort();
        }

        public UserInfoRow GetUser(int id)
        {
            int index = Rows.BinarySearch(0, Rows.Count - 1, id);
            return index >= 0 ? Rows[index] : null;
        }

        public override void LoadTable(JSONObject data, bool clearPre = true)
        {
            base.LoadTable(data, clearPre);
            Sort();
        }
    }
}


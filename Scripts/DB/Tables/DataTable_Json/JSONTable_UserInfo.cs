using DataTable.Row;
using UnityEngine;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New UserInfo Table", menuName = "DataTable/JsonTable/UserInfo JSONTable", order = 9)]
    public sealed class JSONTable_UserInfo : JSONTable<UserInfoRow>
    {
        private void Sort()
        {
            Rows?.BinarySort_L();
        }

        public UserInfoRow GetUser(int id)
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


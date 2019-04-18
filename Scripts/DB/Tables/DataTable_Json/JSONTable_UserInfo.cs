using DataTable.Row;
using UnityEngine;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New UserInfo Table", menuName = "DataTable/JsonTable/UserInfo JSONTable", order = 9)]
    public sealed class JSONTable_UserInfo : JSONTable<UserInfoRow>
    {
        private void Sort()
        {
            Rows.BinarySort_L();
        }

        public UserInfoRow GetUser(int id)
        {
            int index = Rows.BinarySearch_L(0, Rows.Count, id);
            return index < Count && Rows[index].ID_User == id ? Rows[index] : null;
        }

        public override void LoadTable(JSONObject data, bool clearPre = true)
        {
            base.LoadTable(data, clearPre);
            Sort();
        }
    }
}


using DataTable.Row;
using UnityEngine;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New UserInfo Table", menuName = "DataTable/JsonTable/UserInfo JSONTable", order = 9)]
    public sealed class JSONTable_UserInfo : JSONTable<UserInfoRow>, ISearchByObjectCompare<UserInfoRow>
    {
        private UserInfoRow searchObject;
        private UserInfoRow SearchObject
        {
            get
            {
                return searchObject ?? (searchObject = new UserInfoRow());
            }
        }

        private void Sort()
        {
            Rows.BinarySort_R();
        }

        public UserInfoRow GetUserById(int id)
        {          
            int index = Rows.BinarySearch_R(GetSearchObject(id));

            return Rows[index].ID_User == id ? Rows[index] : null;
        }

        public override void LoadTable(JSONObject data)
        {
            base.LoadTable(data);
            Sort();
        }

        public UserInfoRow GetSearchObject(object obj)
        {
            SearchObject.ID_User = (int)obj;
            return SearchObject;
        }
    }
}


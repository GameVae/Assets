using DataTable.Row;
using UnityEngine;
using Extensions.BinarySearch;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New UserInfo Table", menuName = "DataTable/JsonTable/UserInfo JSONTable", order = 9)]
    public sealed class JSONTable_UserInfo : JSONTable<UserInfoRow>,
        ISearchByFakeCompare<UserInfoRow>
    {
        private UserInfoRow searchObject;
        private UserInfoRow SearchObject
        {
            get
            {
                return searchObject ?? (searchObject = new UserInfoRow());
            }
        }

        public UserInfoRow GetUserById(int id)
        {
            return Rows.FirstOrDefault_R(GetSearchObject(id));
        }

        public UserInfoRow GetUserByName(string userName)
        {
            int count = Count;
            for (int i = 0; i < count; i++)
            {
                if (Rows[i].NameInGame.Equals(userName))
                    return Rows[i];
            }
            return null;
        }

        /// <summary>
        /// Compare by user id
        /// </summary>
        /// <param name="obj">User Id</param>
        /// <returns></returns>
        public UserInfoRow GetSearchObject(object obj)
        {
            SearchObject.ID_User = (int)obj;
            return SearchObject;
        }

        public override void LoadTable(JSONObject data)
        {
            base.LoadTable(data);
            Sort();
        }

    }
}


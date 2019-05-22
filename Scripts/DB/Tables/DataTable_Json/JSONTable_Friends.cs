using System;
using DataTable.Row;
using Extensions.BinarySearch;
using UnityEngine;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New Friend Table", menuName = "DataTable/JsonTable/Friend JSONTable", order = 10)]
    public class JSONTable_Friends : JSONTable<FriendRow>
    {
        private FriendRow searchObject;
        private FriendRow SearchObject
        {
            get
            {
                return searchObject ?? (searchObject = new FriendRow());
            }
        }

        public FriendRow GetFriendInfoById(int id)
        {
            return Rows.FirstOrDefault_R(GetSearchObject(id));
        }

        public void RemoveById(int id)
        {
            int index = Rows.BinarySearch_R(GetSearchObject(id));
            if(Rows[index].ID_Player == id)
            {
                Rows.RemoveAt(index);
            }
        }

        public void Add(FriendRow item)
        {
            int index = Rows.BinarySearch_R(GetSearchObject(item.ID_Player));
            if(Rows[index].ID_Player != item.ID_Player)
            {
                base.Insert(item);
            }
        }

        public FriendRow GetSearchObject(object obj)
        {
            SearchObject.ID_Player = (int)obj;
            return SearchObject;
        }

        public void UpdateFriendInfo(FriendRow info)
        {
            base.UpdateOrInsert(info);
        }
    }
}
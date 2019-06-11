using DataTable.Row;
using Generic.Contants;
using Generic.Singleton;
using MultiThread;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New RSS Table", menuName = "DataTable/JsonTable/JSONTable", order = 7)]
    public sealed class JSONTable_RSSPosition : JSONTable<RSS_PositionRow>
    {
        private const int SER_ROW = 512;
        private const int SER_COL = 512;

        private MultiThreadHelper threadHelper;
        private Dictionary<int, int> positionDict;

        private MultiThreadHelper ThreadHelper
        {
            get
            {
                return threadHelper ?? (threadHelper = Singleton.Instance<MultiThreadHelper>());
            }
        }
        private Dictionary<int, int> PositionDict
        {
            get
            {
                return positionDict ?? (positionDict = new Dictionary<int, int>());
            }
        }

        public override int Insert(RSS_PositionRow obj)
        {
            int result = base.Insert(obj);
            if(result != -1)
            {
                int key = UniqueId(obj.Position.Parse3Int());
                PositionDict[key] = obj.ID;
                //Debugger.Log("Added key " + key + " count " + PositionDict.Count );
            }
            return result;
        }
        //private void GeneratePositionDict()
        //{
        //    lock (Locker)
        //    {
        //        PositionDict.Clear();
        //        int count = Count;
        //        for (int i = 0; i < count; i++)
        //        {
        //            int key = UniqueId(Rows[i].Position.Parse3Int());
        //            PositionDict[key] = Rows[i].ID;
        //        }
        //    }
        //}

        private int UniqueId(Vector3Int serPosition)
        {
            return serPosition.y * SER_COL + serPosition.x;
        }

        private Vector3Int IdTo3Int(int uniqueId)
        {
            if (uniqueId >= 0)
            {
                return new Vector3Int(uniqueId % SER_COL, uniqueId / SER_COL, 0);
            }
            return Constants.InvalidPosition;
        }

        //private void WaitForAsyncLoadComplete(object obj)
        //{
        //    while (!Operation.IsDone)
        //    {
        //        Thread.Sleep(20);
        //    }
        //    ThreadHelper.MainThreadInvoke(GeneratePositionDict);
        //}

        //public override void LoadTable(JSONObject jsonObj)
        //{
        //    base.LoadTable(jsonObj);
        //    GeneratePositionDict();
        //}

        //public override void AsyncLoadTable(JSONObject jsonObj)
        //{
        //    base.AsyncLoadTable(jsonObj);
        //    MultiThreadHelper.ThreadInvoke(WaitForAsyncLoadComplete);
        //}

        public RSS_PositionRow GetRssAt(Vector3Int serPosition)
        {
            int key = UniqueId(serPosition);
            PositionDict.TryGetValue(key, out int id);
            if(id > 0 && id < Rows.Count)
            {
                return Rows[id - 1];
            }
            return null;           
        }

        protected override bool UpdateOrInsert(RSS_PositionRow updateData)
        {
            bool isUpdate = base.UpdateOrInsert(updateData);

            if (!isUpdate)
            {
                int key = UniqueId(updateData.Position.Parse3Int());
                PositionDict.Add(key, updateData.ID);
            }
            return isUpdate;
        }
    }
}
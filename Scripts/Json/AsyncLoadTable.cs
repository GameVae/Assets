using System;
using UnityEngine;
using System.Threading;
using Generic.Singleton;
using DataTable;

namespace Json
{
    public class AsyncTableLoader<T> : ISingleton, IAsyncHandler
        where T : ITableData
    {
        public struct ParseInfo
        {
            public JSONObject Obj;
            public Action<T> ResultHandler;
            public AJPHelper.Operation Operation;
        }

        private AsyncTableLoader() { }

        private void Parse(JSONObject obj, Action<T> resultHanlder, AJPHelper.Operation oper)
        {
            // TODO: 
            // Debug.Log(Thread.CurrentThread.Name + " started");

            string json;
            int i = 0;
            int capacity = obj.Count;
            oper.SpentTime = System.DateTime.Now.Millisecond;

            while (i < capacity)
            {
                json = obj[i].ToString();
                resultHanlder(JsonUtility.FromJson<T>(json));

                i++;
                oper.Progress = (float)Math.Round(i * 1.0f / capacity, 3);
            }

            if (!oper.IsDone)
            {
                oper.IsDone = true;
                oper.SpentTime = System.DateTime.Now.Millisecond - oper.SpentTime;

                // TODO:
                // Debugger.Log("async handled: " + capacity);
            }
        }

        private void Callback(object obj)
        {
            ParseInfo info = (ParseInfo)obj;
            Parse(info.Obj, info.ResultHandler, info.Operation);
        }

        private AJPHelper.Operation Start(ParseInfo info)
        {
            ThreadPool.QueueUserWorkItem(Callback, info);
            return info.Operation;
        }

        public AJPHelper.Operation Start(object info)
        {
            return Start((ParseInfo)info);
        }
    }
}
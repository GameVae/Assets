using System;
using UnityEngine;
using System.Threading;
using Generic.Singleton;

namespace Json
{
    public class AsyncLoadTable<T> : ISingleton
    {
        public class ParseOperation
        {
            public bool IsDone;
            public int SpentTime;
            public float Progress;
        }

        public struct ParseInfo
        {
            public JSONObject Obj;
            public Action<T> ResultHandler;
            public ParseOperation Operation;
        }

        private AsyncLoadTable() { }

        private void Parse(JSONObject obj, Action<T> resultHanlder, ParseOperation oper)
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

                // Thread.Sleep(10);
            }

            if (!oper.IsDone)
            {
                oper.IsDone = true;
                oper.SpentTime = System.DateTime.Now.Millisecond - oper.SpentTime;

                // TODO:
                //Debug.Log(Thread.CurrentThread.ManagedThreadId + " elapsed time: " + oper.SpentTime);
            }
        }

        private void Callback(object obj)
        {
            ParseInfo info = (ParseInfo)obj;
            Parse(info.Obj, info.ResultHandler, info.Operation);
        }

        public void Start(ParseInfo info)
        {
            ThreadPool.QueueUserWorkItem(Callback, info);
        }
    }
}
using DataTable;
using Generic.Singleton;
using UnityEngine;

namespace Json
{
    // ASYNC JSON PARSE HELPER
    // Pattern: Factory
    public sealed class AJPHelper : ISingleton
    {
        public class Operation
        {
            /// <summary>
            /// Milisecond
            /// </summary>
            public int SpentTime;
            public bool IsDone;
            public float Progress;
        }

        private AJPHelper() { }

        public IAsyncHandler GetParser<T>()
            where T : ITableData
        {
            return Singleton.Instance<AsyncTableLoader<T>>();
        }

        //public static TResult ParseJson<TResult>(string json)
        //{
        //    try
        //    {
        //        return JsonUtility.FromJson<TResult>(json);
        //    }
        //    catch (System.Exception e)
        //    {
        //        Debugger.Log(e.ToString());
        //        return default(TResult);
        //    }

        //}
    }
}
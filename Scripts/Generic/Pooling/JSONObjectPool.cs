using Generic.Singleton;
using System.Collections.Generic;

namespace Generic.Pooling
{
    public class JSONObjectPool : ISingleton
    {
        private Queue<JSONObject> pool;

        private JSONObjectPool()
        {
            pool = new Queue<JSONObject>();
        }
     
        public JSONObject Get()
        {
            if (pool.Count > 0)
                return pool.Dequeue();
            else
                return new JSONObject();
        }

        public void Return(JSONObject obj)
        {
            obj.Clear();
            if (!pool.Contains(obj))
                pool.Enqueue(obj);
        }
    }
}
using Generic.Singleton;
using System.Collections.Generic;

namespace Generic.Pool
{
    public class PoolJSONObject : ISingleton
    {
        private Queue<JSONObject> pool;
        private PoolJSONObject()
        {
            pool = new Queue<JSONObject>();
        }

        public JSONObject Get(JSONObject.Type type)
        {
            if (pool.Count <= 0)
            {
                return JSONObject.Create(type);
            }
            else
            {
                JSONObject result = pool.Dequeue();
                result.type = type;
                return result;
            }
        }

        public void Return(JSONObject value)
        {
            if (value == null)
                return;
            value.Clear();
            pool.Enqueue(value);
        }

        public void Return(List<JSONObject> values)
        {
            for (int i = 0; i < values?.Count; i++)
            {
                Return(values[i]);
            }
            values?.Clear();
        }
    }
}
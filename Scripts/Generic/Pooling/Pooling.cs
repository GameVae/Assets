using Generic.Singleton;
using System.Collections.Generic;

namespace Generic.Pooling
{
    public class Pooling<T> : MonoSingle<Pooling<T>>
        where T : IPoolable
    {
        private int instanceID = int.MinValue;
        private System.Func<int, T> itemCreator;
        private Queue<T> pooling;
        private Dictionary<int, T> activedItem;

        public int ActiveCount { get { return activedItem.Count; } }
        public void Initalize(System.Func<int, T> creator, int initalizeSize = 0)
        {
            pooling = new Queue<T>();
            activedItem = new Dictionary<int, T>();
            itemCreator = creator;
            for (int i = 0; i < initalizeSize; i++)
            {
                pooling.Enqueue(itemCreator(instanceID++));
            }
        }

        public T GetItem()
        {
            T item = default(T);
            if (pooling.Count > 0)
                item = pooling.Dequeue();
            else
                item = itemCreator(instanceID++);

            activedItem.Add(item.ManagedId,item);
            return item;
        }

        public void Release(T item)
        {
            item.Dispose();
            activedItem.Remove(item.ManagedId);
            pooling.Enqueue(item);
        }
    }
}
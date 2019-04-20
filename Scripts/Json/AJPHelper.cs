using DataTable;
using Generic.Singleton;

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
    }
}
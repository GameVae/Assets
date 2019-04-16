using Generic.Singleton;

namespace Json
{
    // ASYNC JSON PARSE HELPER
    // Pattern: Factory
    public sealed class AJPHelper : ISingleton
    {
        private AJPHelper() { }

        public AsyncLoadTable<T> GetParser<T>()
        {
            return Singleton.Instance<AsyncLoadTable<T>>();
        }

        public void Parse<T>(AsyncLoadTable<T>.ParseInfo info)
        {
            GetParser<T>().Start(info);
        }
    }
}
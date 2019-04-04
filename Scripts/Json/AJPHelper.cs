using Generic.Singleton;

namespace Json
{
    // ASYNC JSON PARSE HELPER
    // Pattern: Factory
    public class AJPHelper : ISingleton
    {
        private AJPHelper() { }

        public AsyncJsonParser<T> GetParser<T>()
        {
            return Singleton.Instance<AsyncJsonParser<T>>();
        }

        public void Parse<T>(AsyncJsonParser<T>.ParseInfo info)
        {
            GetParser<T>().Start(info);
        }
    }
}
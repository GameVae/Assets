
public static class Extension
{
    public static T TryGet<T>(this object[] data, int index)
    {
        if (data?.Length <= index) return default(T);
        return (T)data[index];
    }
}
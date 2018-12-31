
public static class Extension
{
    public static T TryGet<T>(this object[] data, int index)
    {

        try
        {
            return (T)data[index];
        }
        catch
        {
            return default(T);
        }
    }
}
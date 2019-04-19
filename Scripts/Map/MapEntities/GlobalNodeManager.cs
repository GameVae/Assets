using Generic.Singleton;

public class GlobalNodeManager : ISingleton
{
    private static int Id = 0;
    public static int ID()
    {
        return Id++;
    }

    private GlobalNodeManager()
    {

    }
}

using Generic.Singleton;
using ManualTable;
using ManualTable.Row;

public sealed class Player : MonoSingle<Player>
{
    private UserInfoRow userInfo;

    public UserInfoRow Info
    {
        get { return userInfo; }
        set { userInfo = value; }
    }
}

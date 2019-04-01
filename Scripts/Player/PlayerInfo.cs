using Generic.Singleton;
using ManualTable;
using ManualTable.Row;

public sealed class PlayerInfo : MonoSingle<PlayerInfo>
{
    private UserInfoRow userInfo;
    private BaseInfoRow baseInfo;

    public UserInfoRow Info
    {
        get { return userInfo; }
        set { userInfo = value; }
    }

    public BaseInfoRow BaseInfo
    {
        get { return baseInfo; }
        set { baseInfo = value; }
    }

}

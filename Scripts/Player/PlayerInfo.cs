using Generic.Singleton;
using DataTable;
using DataTable.Row;
using UnityEngine;

public sealed class PlayerInfo : MonoSingle<PlayerInfo>
{
    [SerializeField] private UserInfoRow userInfo;
    [SerializeField] private BaseInfoRow baseInfo;

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

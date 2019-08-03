using DataTable;
using DataTable.Row;
using Generic.Singleton;
using UnityEngine;

public sealed class FriendSys : MonoSingle<FriendSys>
{
    [SerializeField] private JSONTable_Friends friends;

    public bool IsMyFriend(int id)
    {
        FriendRow f = friends.GetFriendInfoById(id);
        return f != null;
    }

    public FriendRow GetFriend(int id)
    {
        return friends.GetFriendInfoById(id);
    }
}

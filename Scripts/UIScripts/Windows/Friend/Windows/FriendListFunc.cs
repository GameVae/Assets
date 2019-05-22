using DataTable;
using DataTable.Row;

public class FriendListFunc : BaseFriendFunc
{
    public override void Close()
    {
        base.Close();
        Release();
    }

    public override void Load(params object[] input)
    {
        JSONTable_Friends friends = SyncData.FriendTable;
        JSONTable_UserInfo users = SyncData.UserInfos;

        UserInfoRow user = null;

        for (int i = 0; i < friends.Count; i++)
        {
            FriendRow friendInfo = friends.ReadOnlyRows[i];

            if (IsAlreadyFriend(friendInfo))
            {
                user = users.GetUserById(friendInfo.ID_Player);

                FriendTag tag = GetFriendTag(friendInfo);

                tag.UserName.text = user.NameInGame;

                tag.RemoveButton.OnClickEvents += delegate
                {
                    OnUnfriendButton(friendInfo);
                    ReleaseTag(tag);
                };

                tag.gameObject.SetActive(true);
            }
        }
    }

    protected override void Init() { }
}

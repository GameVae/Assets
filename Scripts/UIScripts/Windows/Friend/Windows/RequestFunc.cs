using DataTable.Row;
using DataTable;

public class RequestFunc : BaseFriendFunc
{
    protected override void Init() { }

    public override void Close()
    {
        base.Close();
        Release();
    }

    public override void Load(params object[] input)
    {
        JSONTable_Friends friends = SyncData.FriendTable;
        JSONTable_UserInfo users = SyncData.UserInfos;

        for (int i = 0; i < friends.Count; i++)
        {
            FriendRow info = friends.ReadOnlyRows[i];
            if (info.AcceptTime != 0.0f)
            {
                UserInfoRow user = users.GetUserById(info.ID_Player);
                AddFriendTag(info, user);
            }
        }
    }

    private void AddFriendTag(FriendRow info,UserInfoRow user)
    {
        FriendTag tag = GetFriendTag(info);

        tag.UserName.text = user.NameInGame;
        tag.Describe.text = "Waiting " + info.AcceptTimeString();

        tag.RemoveButton.OnClickEvents += delegate 
        {
            OnRejectButton(info);
            ReleaseTag(tag);
        };

        if(!info.RequestBool)
        {
            //tag.Add_AcceptButon.gameObject.SetActive(true);           
            tag.Add_AcceptButon.OnClickEvents += delegate 
            {                
                OnAcceptButton(info);
                ReleaseTag(tag);
            };
        }
        tag.gameObject.SetActive(true);
    }
}

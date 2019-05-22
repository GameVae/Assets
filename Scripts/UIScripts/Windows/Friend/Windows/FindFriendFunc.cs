using DataTable;
using DataTable.Row;
using UI.Composites;

public class FindFriendFunc : BaseFriendFunc
{
    public PlaceholderComp NotFoundMessage;

    public SelectableComp FindButton;
    public CustomInputField UserNameInput;

    public override void Load(params object[] input) { }

    protected override void Init()
    {
        NotFoundMessage.gameObject.SetActive(false);
        FindButton.OnClickEvents += OnFindFriendButton;
    }

    private void ReleaseFriendTags()
    {
        NotFoundMessage.gameObject.SetActive(false);
        Release();
    }

    private void OnFindFriendButton()
    {
        ReleaseFriendTags();
        string userName = UserNameInput.Text;
        if (string.IsNullOrEmpty(userName))
        {
            Debugger.Log("player name is empty");
            return;
        }

        JSONTable_UserInfo users = SyncData.UserInfos;
        UserInfoRow foundUser = users.GetUserByName(userName);

        if(foundUser != null)
        {
            FriendRow friendInfo = FriendTable.GetFriendInfoById(foundUser.ID_User);
            if (friendInfo == null)
            {
                if (foundUser.ID_User != SyncData.User_ID)
                {
                    CreateFriendTag
                        (
                        new FriendRow()
                        {
                            ID_Player = foundUser.ID_User,
                            RequestBool = true,
                        },
                        foundUser,
                        false
                        );
                }
            }
            else
            {
                CreateFriendTag
                   (
                   friendInfo,
                   foundUser,
                   true
                   );
            }
        }
        else
        {
            NotFoundMessage.gameObject.SetActive(true);
        }
    } 

    private void CreateFriendTag(FriendRow friendInfo, UserInfoRow userInfo,bool isExisted)
    {
        FriendTag tag = GetFriendTag(friendInfo);
        tag.UserName.text = userInfo.NameInGame;

        if (isExisted)
        {
            bool alreadyFriend = IsAlreadyFriend(friendInfo);
            bool waitingAccept = IsWaitingAccept(friendInfo);
            bool waitingRemove = IsWaitingUnFriend(friendInfo);

            tag.UserName.text = userInfo.NameInGame;

           
            if(alreadyFriend) 
            {
                tag.RemoveButton.OnClickEvents += delegate
                {
                    OnUnfriendButton(friendInfo);
                    ReleaseTag(tag);
                };
            }
            else if (waitingAccept)
            {
                tag.RemoveButton.OnClickEvents += delegate
                {
                    OnRejectButton(friendInfo);
                    ReleaseTag(tag);
                };
                tag.Add_AcceptButon.OnClickEvents += delegate
                {
                    OnAcceptButton(friendInfo);
                    ReleaseTag(tag);
                };
            }
            //else if(waitingRemove)
            //{
            //    tag.RemoveButton.gameObject.SetActive(false);
            //    tag.Add_AcceptButon.gameObject.SetActive(false);
            //}

            string acceptTime = friendInfo.AcceptTime != 0.0f ? "Waiting " + friendInfo.AcceptTimeString() : null;
            string removeTime = friendInfo.RemoveTime != 0.0f ? "Removing " + friendInfo.RemoveTimeString() : null;

            tag.Describe.text = acceptTime + removeTime;
        }
        else
        {
            // doesn't friend
            tag.RemoveButton.gameObject.SetActive(false);
            tag.Add_AcceptButon.gameObject.SetActive(true);

            tag.Add_AcceptButon.OnClickEvents += delegate
            {
                OnAddFriendButton(friendInfo);
                ReleaseTag(tag);
            };
        }
        tag.gameObject.SetActive(true);

    }

}

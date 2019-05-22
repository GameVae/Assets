using DataTable;
using UI.Widget;
using DataTable.Row;
using Generic.Pooling;
using System.Collections.Generic;

public abstract class BaseFriendFunc : ToggleWindow
{
    private Queue<FriendTag> tags;
    private Pooling<FriendTag> tagsPooling;
    private SIO_FriendListener friendListeners;

    public FriendTag FriendTagPrefab;
    public GUIScrollView ScrollView;

    private Queue<FriendTag> Tags
    {
        get
        {
            return tags ?? (tags = new Queue<FriendTag>());
        }
    }
    private Pooling<FriendTag> TagsPooling
    {
        get
        {
            return tagsPooling ?? (tagsPooling = new Pooling<FriendTag>(TagCreator));
        }
    }

    protected SIO_FriendListener FriendListeners
    {
        get
        {
            return friendListeners ?? (friendListeners = FindObjectOfType<SIO_FriendListener>());
        }
    }
    protected JSONTable_Friends FriendTable
    {
        get
        {
            return SyncData.FriendTable;
        }
    }

    public override void Open()
    {
        base.Open();
        Load();
    }

    protected void Release()
    {
        while (Tags.Count > 0)
        {
            TagsPooling.Release(tags.Dequeue());
        }
    }

    protected void SetFocusFriendInfo(FriendRow info)
    {
        FriendListeners.SetFocusFriendInfo(info);
    }

    protected FriendTag GetFriendTag(FriendRow info)
    {
        FriendTag item = TagsPooling.GetItem();
        Tags.Enqueue(item);

        if (IsAlreadyFriend(info))
        {
            item.RemoveButton.gameObject.SetActive(true);
            item.Add_AcceptButon.gameObject.SetActive(false);
        }
        else
        {
            if (IsWaitingAccept(info))
            {
                item.RemoveButton.gameObject.SetActive(true);
                if (!info.RequestBool)
                {
                    item.Add_AcceptButon.gameObject.SetActive(true);
                }
                else
                {
                    item.Add_AcceptButon.gameObject.SetActive(false);
                }
            }
            else if (IsWaitingUnFriend(info))
            {
                item.RemoveButton.gameObject.SetActive(false);
                item.Add_AcceptButon.gameObject.SetActive(false);
            }
        }

        return item;
    }

    protected void ReleaseTag(FriendTag tag)
    {
        TagsPooling.Release(tag);
        tag.gameObject.SetActive(false);
    }

    private FriendTag TagCreator(int insId)
    {
        FriendTag tag = Instantiate(FriendTagPrefab, ScrollView.Content);
        tag.FirstSetup(insId);
        return tag;
    }

    protected void OnAcceptButton(FriendRow info)
    {
        SetFocusFriendInfo(info);

        info.AcceptTime = 0.0f;
        info.RemoveTime = 0.0f;

        Debugger.Log(SyncData.UserInfos.GetUserById(info.ID_Player).NameInGame + " " + info.ID_Player);
        FriendListeners.Emit("S_ACCEPT_FRIEND");

        SyncData.FriendTable.UpdateFriendInfo(info);
    }

    protected void OnAddFriendButton(FriendRow info)
    {
        SetFocusFriendInfo(info);
        info.AcceptTime = 24.0f * 3600; // 24h
        info.RemoveTime = 0.0f;
        Debugger.Log(SyncData.UserInfos.GetUserById(info.ID_Player).NameInGame + " " + info.ID_Player);
        FriendListeners.Emit("S_ADD_FRIEND");

        SyncData.FriendTable.UpdateFriendInfo(info);
    }

    protected void OnUnfriendButton(FriendRow info)
    {
        SetFocusFriendInfo(info);

        info.AcceptTime = 0.0f;
        info.RemoveTime = 1800.0f; // 30'
        Debugger.Log(SyncData.UserInfos.GetUserById(info.ID_Player).NameInGame + " " + info.ID_Player);

        FriendListeners.Emit("S_UNFRIEND");

        SyncData.FriendTable.UpdateFriendInfo(info);
    }

    protected void OnRejectButton(FriendRow info)
    {
        SetFocusFriendInfo(info);
        Debugger.Log(SyncData.UserInfos.GetUserById(info.ID_Player).NameInGame + " " + info.ID_Player);

        FriendListeners.Emit("S_REJECT_FRIEND");

        SyncData.FriendTable.RemoveById(info.ID_Player);
    }

    protected bool IsAlreadyFriend(FriendRow info)
    {
        return info.AcceptTime == 0.0f && info.RemoveTime == 0.0f;
    }

    protected bool IsWaitingAccept(FriendRow info)
    {
        return info.AcceptTime != 0.0f;
    }

    protected bool IsWaitingUnFriend(FriendRow info)
    {
        return info.RemoveTime != 0.0f;
    }
}

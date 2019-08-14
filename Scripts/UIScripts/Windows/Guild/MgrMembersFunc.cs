using DataTable.Row;
using EnumCollect;
using Generic.Pooling;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UI.Widget;
using UnityEngine;

public class MgrMembersFunc : ToggleWindow
{
    [SerializeField] GuildSys guildSys;
    [SerializeField] MemberTag memberTagPrefab;
    [SerializeField] GUIScrollView scrollView;

    private Queue<MemberTag> catchingTags;
    private Queue<MemberTag> CatchingTags
    {
        get
        {
            return catchingTags ?? (catchingTags = new Queue<MemberTag>());
        }
    }

    private Pooling<MemberTag> poolTag;
    private Pooling<MemberTag> PoolTag
    {
        get
        {
            return poolTag ?? (poolTag = new Pooling<MemberTag>(CreateTag));
        }
    }

    public override void Load(params object[] input)
    {
        GuildMemberRow own = Own();
        if (own != null)
        {
            LoadAllMemeber(own);
        }
    }

    protected override void Init()
    {

    }

    public override void Close()
    {
        Release();
        base.Close();
    }
    private MemberTag CreateTag(int insId)
    {
        MemberTag tag = Instantiate(memberTagPrefab, scrollView.Content);
        tag.FirstSetup(insId);
        return tag;
    }

    private void Release()
    {
        while (CatchingTags.Count > 0)
        {
            PoolTag.Release(CatchingTags.Dequeue());
        }
    }

    private GuildMemberRow Own()
    {
        int myId = guildSys.PlayerInfo.Info.ID_User;
        return guildSys.GuildTable.FindMember(myId);
    }

    private void LoadAllMemeber(GuildMemberRow own)
    {
        ReadOnlyCollection<GuildMemberRow> members = guildSys.GuildTable.ReadOnlyRows;
        int count = members.Count;
        for (int i = 0; i < count; i++)
        {
            int capture = i;
            MemberTag tag = PoolTag.GetItem();

            if (ChangableGradePermission(own, members[capture]))
            {
                tag.IncreaseGradeBtn.OnClickEvents += () => S_PROMOTE(members[capture]);
                tag.DecreaseGradeBtn.OnClickEvents += () => S_DECREASE_GRADE(members[capture]);
            }
            else
            {
                tag.IncreaseGradeBtn.gameObject.SetActive(false);
                tag.DecreaseGradeBtn.gameObject.SetActive(false);
            }
            if (KickPermission(own, members[capture]))
            {
                tag.KickBtn.OnClickEvents += () => S_KICKOUT_GUILD(members[capture]);
            }
            else
            {
                tag.KickBtn.gameObject.SetActive(false);
            }

            CatchingTags.Enqueue(tag);
            tag.gameObject.SetActive(true);
        }
    }

    private bool KickPermission(GuildMemberRow own, GuildMemberRow other)
    {
        return own.GuildPosition >= GuildPosition.Admin && own.GuildPosition > other.GuildPosition;
    }

    private bool ChangableGradePermission(GuildMemberRow own, GuildMemberRow other)
    {
        return own.GuildPosition > other.GuildPosition;
    }

    private void S_KICKOUT_GUILD(GuildMemberRow member)
    {
        Dictionary<string, string> info = new Dictionary<string, string>()
        {

        };

        JSONObject data = new JSONObject(info);
        guildSys.EventController.Emit("S_KICKOUT_GUILD", data);
    }

    private void S_PROMOTE(GuildMemberRow member)
    {
        Dictionary<string, string> info = new Dictionary<string, string>()
        {

        };

        JSONObject data = new JSONObject(info);
        guildSys.EventController.Emit("S_PROMOTE", data);
    }

    private void S_DECREASE_GRADE(GuildMemberRow member)
    {
        Dictionary<string, string> info = new Dictionary<string, string>()
        {

        };

        JSONObject data = new JSONObject(info);
        guildSys.EventController.Emit("S_DECREASE_GRADE", data);
    }
}

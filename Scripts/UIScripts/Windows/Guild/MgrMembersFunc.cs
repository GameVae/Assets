using Generic.Pooling;
using System.Collections.Generic;
using UI.Widget;
using UnityEngine;

public class MgrMembersFunc : ToggleWindow
{
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

    private void S_KICKOUT_GUILD(MemberTag tag)
    {

    }

    private void S_PROMOTE(MemberTag tag)
    {

    }

    private void S_INCREASE_GRADE(MemberTag tag)
    {

    }
}

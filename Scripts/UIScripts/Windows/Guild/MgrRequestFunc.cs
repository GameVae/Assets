using Generic.Pooling;
using System.Collections;
using System.Collections.Generic;
using UI.Widget;
using UnityEngine;

public class MgrRequestFunc : ToggleWindow
{
    [SerializeField] private GuildSys guildSys;
    [SerializeField] private GUIScrollView ScrollView;
    [SerializeField] private ApplyTag applyTagPrefab;
    
    private Pooling<ApplyTag> poolTag;
    private Pooling<ApplyTag> PoolTag
    {
        get
        {
            return poolTag ?? (poolTag = new Pooling<ApplyTag>(CreateTag));
        }
    }

    private Queue<ApplyTag> catchingTags;
    private Queue<ApplyTag> CatchingTags
    {
        get
        {
            return catchingTags ?? (catchingTags = new Queue<ApplyTag>());
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

    private void Release()
    {
        while(CatchingTags.Count > 0)
        {
            PoolTag.Release(CatchingTags.Dequeue());
        }
    }

    private ApplyTag CreateTag(int insId)
    {
        ApplyTag tag = Instantiate(applyTagPrefab, ScrollView.Content);
        tag.FirstSetup(insId);

        return tag;
    }

    private void S_ACCEPT_APPLY(ApplyTag tag)
    {
        Dictionary<string, string> acceptInfo = new Dictionary<string, string>()
        {

        };
        guildSys.EventController.Emit("S_ACCEPT_APPLY", new JSONObject(acceptInfo));
    }

    private void S_REJECT_APPLY(ApplyTag tag)
    {
        Dictionary<string, string> rejectInfo = new Dictionary<string, string>()
        {

        };
        guildSys.EventController.Emit("S_REJECT_APPLY", new JSONObject(rejectInfo));
    }
}

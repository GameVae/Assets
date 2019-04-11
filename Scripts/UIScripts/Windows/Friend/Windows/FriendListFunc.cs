using Generic.Pooling;
using Generic.Singleton;
using System.Collections.Generic;
using UI.Widget;

public class FriendListFunc : ToggleWindow
{
    public FriendTag FriendTagPrefab;
    public GUIScrollView ScrollView;

    private Queue<FriendTag> tags;
    private Pooling<FriendTag> tagsPooling;

    public override void Load(params object[] input)
    {
        for (int i = 0; i < 20; i++)
        {
            FriendTag tag = tagsPooling.GetItem();
            tag.UserName.text = ("User Test" + i);
            tags.Enqueue(tag);
        }
    }

    protected override void Init()
    {
        tags = new Queue<FriendTag>();
        tagsPooling = Singleton.Instance<Pooling<FriendTag>>();
        tagsPooling.Initalize(TagCreator);
    }

    public override void Close()
    {
        base.Close();

        while (tags.Count > 0)
        {
            tagsPooling.Release(tags.Dequeue());
        }
    }

    private FriendTag TagCreator(int insId)
    {
        FriendTag tag = Instantiate(FriendTagPrefab, ScrollView.Content);
        tag.FirstSetup(insId);
        return tag;
    }
}

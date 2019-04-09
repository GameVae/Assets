using System.Collections;
using System.Collections.Generic;
using UI.Widget;
using UnityEngine;

public class FriendWindow : BaseWindow
{
    public GUIInteractableIcon OpenButton;
    public GUIInteractableIcon CloseButton;

    public FriendTag FriendTagPrefab;
    public GUIScrollView ScrollView;

    private RectTransform scrollViewContent;

    protected override void Awake()
    {
        base.Awake();
        OpenButton.OnClickEvents += Open;
        CloseButton.OnClickEvents += Close;

        scrollViewContent = ScrollView.Content;
        
    }

    public override void Load(params object[] input)
    {

    }

    protected override void Init()
    {
        for (int i = 0; i < 10; i++)
        {
            var f = Instantiate(FriendTagPrefab, ScrollView.Content);
            f.gameObject.SetActive(true);
        }
    }

    public override void Open()
    {
        base.Open();
        Load();
    }
}

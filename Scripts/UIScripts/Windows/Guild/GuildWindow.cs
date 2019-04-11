using System.Collections;
using System.Collections.Generic;
using UI.Composites;
using UnityEngine;

public class GuildWindow : WindowToggleGroup
{
    [Header("Test")]
    public GameObject[] Windows;
    private GameObject testActiveWindow;

    protected override void Init()
    {
        base.Init();
        toggles.ToggleSelectedEvt += OnTogglesSelected;
    }
    private void OnTogglesSelected(ToggleComp toggle)
    {
        int index = toggles.Toggles.IndexOf(toggle);
        testActiveWindow?.SetActive(false);
        testActiveWindow = Windows[index];
        testActiveWindow.SetActive(true);
    }
}

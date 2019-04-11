using System.Collections;
using System.Collections.Generic;
using UI.Composites;
using UnityEngine;

public class GuildWindow : WindowToggleGroup
{
    [Header("Test")]
    public GameObject[] Windows;
    private GameObject activeWindow;

    protected override void Init()
    {
        base.Init();
        toggles.ToggleSelectedEvt += OnTogglesSelected;
    }
    private void OnTogglesSelected(ToggleComp toggle)
    {
        int index = toggles.Toggles.IndexOf(toggle);
        activeWindow?.SetActive(false);
        activeWindow = Windows[index];
        activeWindow.SetActive(true);
    }
}

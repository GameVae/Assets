using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupWindow : MonoBehaviour, IWindow
{
    private bool inited;
    private UpgradeResearchManager manager;

    public GUIProgressSlider ProgressBar;
    public GUIInteractableIcon Mainbase;
    public GUIInteractableIcon Resource;
    public GUIInteractableIcon Defense;
    public GUIInteractableIcon Army;
    public GUIInteractableIcon Trade;

    private void Awake()
    {
        if (!inited)
        {
            Init();
        }
    }

    private void Start()
    {
       
    }

    public void LoadData(params object[] input)
    {
        
    }


    private void Init()
    {
        manager = GetComponentInParent<UpgradeResearchManager>();
        Mainbase.OnClickEvents += delegate
        {
            manager.Open(UpgradeResearchManager.Window.UpgradeResearch);
        };
        Resource.OnClickEvents += delegate
        {
            manager.Open(UpgradeResearchManager.Window.Resource);
        };
        Defense.OnClickEvents += delegate
        {
            manager.Open(UpgradeResearchManager.Window.Defense);
        };
        Army.OnClickEvents += delegate
        {
            manager.Open(UpgradeResearchManager.Window.Army);
        };
        Trade.OnClickEvents += delegate
        {
            manager.Open(UpgradeResearchManager.Window.Trade);
        };
    }
}

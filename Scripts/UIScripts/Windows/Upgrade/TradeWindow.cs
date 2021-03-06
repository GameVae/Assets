﻿using UnityEngine;
using UI.Widget;
using static WindowManager;
using EnumCollect;

public class TradeWindow : BaseWindow, IWindowGroup
{
    [Header("Constructs"), Space]
    public ArmyWindow.Element Market;
    public ArmyWindow.Element PhoHienShip;
    public ListUpgrade[] UpgradeTypes;

    [Header("Research"), Space]
    public Transform[] MarketResearchs;
    public Transform[] PHShiptResearchs;

    public ListUpgrade[] MarketResearchTypes;
    public ListUpgrade[] PHShiptResearchTypes;

    private ArmyWindow.Element[] marketResearchElements;
    private ArmyWindow.Element[] PHShipResearchElements;

    public WindowGroup Group
    {
        get { return WDOCtrl[GroupType]; }
    }

    public WindowGroupType GroupType
    {
        get { return WindowGroupType.UpgradeResearchGroup; }
    }

    private void SetupMarketResearch()
    {
        int count = MarketResearchs.Length;
        marketResearchElements = new ArmyWindow.Element[count];
        for (int i = 0; i < count; i++)
        {
            int captureIndex = i;
            marketResearchElements[i] = new ArmyWindow.Element()
            {
                Icon = MarketResearchs[i].GetComponentInChildren<GUIInteractableIcon>(),
                LevelBar = MarketResearchs[i].GetComponentInChildren<GUIProgressSlider>(),
            };
            marketResearchElements[i].Icon.OnClickEvents
                += delegate
                {
                    Group.Open(WindowType.UpgradeResearch);
                    Group[WindowType.UpgradeResearch].Load(MarketResearchTypes[captureIndex]);
                };
        }
    }

    private void SetupHPShipResearch()
    {
        int count = PHShiptResearchs.Length;
        PHShipResearchElements = new ArmyWindow.Element[count];
        for (int i = 0; i < count; i++)
        {
            int captureIndex = i;
            PHShipResearchElements[i] = new ArmyWindow.Element()
            {
                Icon = PHShiptResearchs[i].GetComponentInChildren<GUIInteractableIcon>(),
                LevelBar = PHShiptResearchs[i].GetComponentInChildren<GUIProgressSlider>(),
            };
            PHShipResearchElements[i].Icon.OnClickEvents
                += delegate
                {
                    Group.Open(WindowType.UpgradeResearch);
                    Group[WindowType.UpgradeResearch].Load(PHShiptResearchTypes[captureIndex]);
                };
        }
    }

    protected override void Init()
    {
        SetupMarketResearch();
        SetupHPShipResearch();
    }

    public override void Load(params object[] input)
    {
        throw new System.NotImplementedException();
    }
}

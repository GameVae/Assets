﻿using EnumCollect;
using UI.Widget;
using UnityEngine;
using static UpgradeResearchManager;

public class ResourceWindow : MonoBehaviour, IWindow
{
    private UpgradeResearchManager manager;

    [Header("Constructs"), Space]
    public Transform[] Constructs;
    private ArmyWindow.Element[] constructElements;

    [Header("Research")]
    public Transform[] Researchs;
    private ArmyWindow.Element[] researchElements;

    public ListUpgrade[] ResearchTypes;
    public ListUpgrade[] ConstructTypes;

    private void Awake()
    {
        manager = GetComponentInParent<UpgradeResearchManager>();
        SetupConstructElement();
        SetupResearchElements();
    }


    private void SetupConstructElement()
    {
        int count = Constructs.Length;
        constructElements = new ArmyWindow.Element[count];
        for (int i = 0; i < count; i++)
        {
            int captureIndex = i;
            constructElements[i] = new ArmyWindow.Element()
            {
                Icon = Constructs[i].GetComponentInChildren<GUIInteractableIcon>(),
                LevelBar = Constructs[i].GetComponentInChildren<GUIProgressSlider>(),
            };
            constructElements[i].Icon.OnClickEvents +=
                delegate 
                {
                    manager.Open(Window.UpgradeResearch);
                    manager[Window.UpgradeResearch]
                    .Load(ConstructTypes[captureIndex]);
                };
        }
    }

    private void SetupResearchElements()
    {
        int count = Researchs.Length;
        researchElements = new ArmyWindow.Element[count];
        for (int i = 0; i < count; i++)
        {
            int captureIndex = i;
            researchElements[i] = new ArmyWindow.Element()
            {
                Icon = Researchs[i].GetComponentInChildren<GUIInteractableIcon>(),
                LevelBar = Researchs[i].GetComponentInChildren<GUIProgressSlider>(),
            };
            researchElements[i].Icon.OnClickEvents +=
               delegate 
               {
                   manager.Open(Window.UpgradeResearch);
                   manager[Window.UpgradeResearch].
                   Load(ResearchTypes[captureIndex]);
               };
        }
    }

    public void Load(params object[] input)
    {
    }

    public void Open()
    {
        gameObject.SetActive(true);
        Load();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}

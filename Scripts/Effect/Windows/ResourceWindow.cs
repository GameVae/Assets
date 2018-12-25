using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceWindow : MonoBehaviour, IWindow
{

    private bool inited;
    private UpgradeResearchManager manager;

    [Header("Constructs"), Space]
    public Transform[] Constructs;
    private ArmyWindow.Element[] constructElements;

    [Header("Research")]
    public Transform[] Researchs;
    private ArmyWindow.Element[] researchElements;

    private void Awake()
    {
        if (!inited)
        {
            manager = GetComponentInParent<UpgradeResearchManager>();
            SetupConstructElement();
            SetupResearchElements();
            inited = true;
        }
    }


    private void SetupConstructElement()
    {
        int count = Constructs.Length;
        constructElements = new ArmyWindow.Element[count];
        for (int i = 0; i < count; i++)
        {
            constructElements[i] = new ArmyWindow.Element()
            {
                Icon = Constructs[i].GetComponentInChildren<GUIInteractableIcon>(),
                LevelBar = Constructs[i].GetComponentInChildren<GUIProgressSlider>(),
            };
            constructElements[i].Icon.OnClickEvents +=
                delegate { manager.Open(UpgradeResearchManager.Window.UpgradeResearch); };
        }
    }

    private void SetupResearchElements()
    {
        int count = Researchs.Length;
        researchElements = new ArmyWindow.Element[count];
        for (int i = 0; i < count; i++)
        {
            researchElements[i] = new ArmyWindow.Element()
            {
                Icon = Researchs[i].GetComponentInChildren<GUIInteractableIcon>(),
                LevelBar = Researchs[i].GetComponentInChildren<GUIProgressSlider>(),
            };
            researchElements[i].Icon.OnClickEvents +=
               delegate { manager.Open(UpgradeResearchManager.Window.UpgradeResearch); };
        }
    }

    public void LoadData(params object[] input)
    {
        throw new System.NotImplementedException();
    }
}

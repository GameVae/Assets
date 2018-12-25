using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseWindow : MonoBehaviour ,IWindow
{

    private bool inited;
    private UpgradeResearchManager manager;

    [Header("Constructs"), Space]
    public Transform[] Constructs;
    private ArmyWindow.Element[] constructElements;

    private void Awake()
    {
        if (!inited)
        {
            manager = GetComponentInParent<UpgradeResearchManager>();
            SetupConstructElement();
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

    public void LoadData(params object[] input)
    {
        throw new System.NotImplementedException();
    }
}

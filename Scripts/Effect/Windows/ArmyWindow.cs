using System;
using TMPro;
using UnityEngine;

public class ArmyWindow : MonoBehaviour,IWindow
{
    [Serializable]
    public struct Element
    {
        public GUIInteractableIcon Icon;
        public GUIProgressSlider LevelBar;
    }

    private bool inited;
    private UpgradeResearchManager manager;
    
    [Header("Toggle Group")]
    public GUIToggle Toggle;

    [Header("Illustration Group")]
    private TextMeshProUGUI typeName;
    private GUIInteractableIcon illusIcon;
    private GUIInteractableIcon upgradeIcon;
    private GUIProgressSlider levelBar;
    public Transform IllustrationGroup;

    [Header("Element Group")]
    private Element[] elements;

    public Transform[] OrderElements;

    private void Awake()
    {
        if (!inited)
        {
            manager = GetComponentInParent<UpgradeResearchManager>();
            SetupIllustrationGroup();
            SetupOrderElements();
            inited = true;
        }
    }

    private void Start()
    {
        // elements[0].Icon.OnOffSwitch.SwitchOn();
    }

    private void SetupIllustrationGroup()
    {
        typeName = IllustrationGroup.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        illusIcon = IllustrationGroup.GetChild(0).GetComponentInChildren<GUIInteractableIcon>();

        upgradeIcon = IllustrationGroup.GetChild(1).GetComponent<GUIInteractableIcon>();

        levelBar = IllustrationGroup.GetChild(2).GetComponent<GUIProgressSlider>();

    }

    private void SetupOrderElements()
    {
        int elementCount = OrderElements.Length;
        elements = new Element[elementCount];

        for (int i = 0; i < elementCount; i++)
        {
            elements[i] = new Element()
            {
                Icon = OrderElements[i].GetComponentInChildren<GUIInteractableIcon>(),
                LevelBar = OrderElements[i].GetComponentInChildren<GUIProgressSlider>(),
            };
            elements[i].Icon.OnClickEvents +=
                delegate { manager.Open(UpgradeResearchManager.Window.UpgradeResearch); };
        }
    }

    public void LoadData(params object[] input)
    {
        throw new NotImplementedException();
    }
}

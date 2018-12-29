using System;
using TMPro;
using UnityEngine;
using UI.Widget;
using System.Collections.Generic;
using Network.Sync;

public class ArmyWindow : MonoBehaviour, IWindow
{
    [Serializable]
    public struct Element
    {
        public GUIInteractableIcon Icon;
        public GUIProgressSlider LevelBar;
    }

    [Serializable]
    public struct ArmyType
    {
        public string[] Types;
    }

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

    [Header("Army Type's Name")]
    public ArmyType Infantry;
    public ArmyType Ranged;
    public ArmyType Mounted;
    public ArmyType SeigeEngine;
    private Dictionary<string, ArmyType> typeDict;

    private void Awake()
    {
        manager = GetComponentInParent<UpgradeResearchManager>();

        typeDict = new Dictionary<string, ArmyType>()
        {
            {"Infantry" ,Infantry},
            {"Ranged" ,Ranged},
            {"Mounted" ,Mounted},
            {"Siege Engine" ,SeigeEngine},
        };

        SetupIllustrationGroup();
        SetupOrderElements();
        Toggle.CheckMarkEvents += delegate
        {
            typeName.text = Toggle.ActiveMark.Placeholder.text;
            Load(typeName.text);
        };
    }

    private void Start()
    {
        Toggle.ActiveToggle(0);
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
            int captureIndex = i;
            elements[i] = new Element()
            {
                Icon = OrderElements[i].GetComponentInChildren<GUIInteractableIcon>(),
                LevelBar = OrderElements[i].GetComponentInChildren<GUIProgressSlider>(),
            };
            elements[i].Icon.OnClickEvents +=
                delegate
                {
                    manager.Open(UpgradeResearchManager.Window.UpgradeResearch);
                    manager.UpgradeResearchWindow.Load(typeDict[typeName.text].Types[captureIndex]);
                };
        }

    }

    public void Load(params object[] input)
    {
        int mainLevel = Sync.Instance.MainBaseLevel;
        int curLevel = input.TryGet<int>(1);

        ArmyType armyType = typeDict[(string)input[0]];

        upgradeIcon.InteractableChange(mainLevel > curLevel);
        levelBar.Value = curLevel;

        for (int i = 0; i < armyType.Types.Length; i++)
        {
            elements[i].Icon.Placeholder.text = armyType.Types[i];
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}

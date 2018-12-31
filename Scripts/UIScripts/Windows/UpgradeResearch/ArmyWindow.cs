using System;
using TMPro;
using UnityEngine;
using UI.Widget;
using System.Collections.Generic;
using Network.Sync;
using static UpgradeResearchManager;
using ManualTable;
using ManualTable.Row;
using System.Linq;

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
        public SoldierTable[] AgentDatabase;
        public Database ConstructType;
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
            {"Siege Engine",SeigeEngine},
        };

        SetupIllustrationGroup();
        SetupOrderElements();
        Toggle.CheckMarkEvents += delegate
        {
            typeName.text = Toggle.ActiveMark.Placeholder.text;
            Load(typeName.text);
        };
        upgradeIcon.OnClickEvents +=
            delegate { OnUpgradeBtn(typeName.text); };
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
                    manager.Open(Window.UpgradeResearch);
                    OnElementBtn(typeName.text,captureIndex);
                };
        }

    }

    private void OnElementBtn(string type, int index)
    {
        SoldierTable table = typeDict[type].AgentDatabase[index];
        string name = typeDict[typeName.text].Types[index];

       // server data
        int level = 4;
        int[] current = new int[] { 0, 1, 2, 3 };
        int[] need;
        SoldierRow row = table.Rows.FirstOrDefault(x => x.Level == level);

        if (row != null)
            need = new int[] { row.Food, row.Wood, row.Stone, row.Metal };
        else need = new int[4];

        manager[Window.UpgradeResearch].Load(
            name,
            level,
            current,
            need,
            row?.MightBonus,
            row?.ResearchTime,
            row?.TimeInt,
            6
            );
    }

    private void OnUpgradeBtn(string datatype)
    {
        // server data 
        int level = 4;
        int[] current = new int[] { 10000, 1, 2, 3 };

        ArmyType info = typeDict[datatype];
        manager.Open(Window.UpgradeResearch);
        MainBaseTable table = manager[info.ConstructType] as MainBaseTable;

        int[] need;
        MainBaseRow row = table.Rows.FirstOrDefault(x => x.Level == level);
        if (row != null)
        {
            need = new int[] { row.FoodCost, row.WoodCost, row.StoneCost, row.MetalCost };
        }
        else need = new int[4];

        manager[Window.UpgradeResearch].Load
            (typeName.text,
            level,
            current,
            need,
            row?.MightBonus,
            row?.TimeMin,
            row?.TimeInt,
            row?.Required,
            6 // research require
            );
    }

    public void Load(params object[] input)
    {
        // data for test
        int mainLevel = Sync.Instance.MainBaseLevel;
        int curLevel = Sync.Instance.InfantryLevel;

        ArmyType armyType = typeDict[input.TryGet<string>(0)];
        MainBaseTable table = manager[armyType.ConstructType] as MainBaseTable;

        for (int i = 0, j = 0; i < table.Rows.Count && j < elements.Length; i++)
        {
            if (table.Rows[i].Unlock != null &&
                table.Rows[i].Unlock != "")
            {
                elements[j].Icon.InteractableChange(curLevel >= table.Rows[i].Level);
                j++;
            }
        }

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

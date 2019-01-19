using System;
using TMPro;
using UnityEngine;
using UI.Widget;
using System.Collections.Generic;
using static UpgResWdoCtrl;
using ManualTable;
using ManualTable.Row;
using System.Linq;
using EnumCollect;
using DB;

public class ArmyWindow : BaseWindow
{
    [Serializable]
    public struct Element
    {
        public GUIInteractableIcon Icon;
        public GUIProgressSlider LevelBar;
    }

    [Serializable]
    public struct ElementTypeInfo
    {
        public ListUpgrade BaseType;
        public string[] Titles;
        public ListUpgrade[] Types;
    }

    [Header("Toggle Group")]
    public GUIToggle Toggle;

    [Header("Illustration Group")]
    private TextMeshProUGUI typeName;
    private GUIInteractableIcon illusImg;
    private GUIInteractableIcon upgradeBtn;
    private GUIProgressSlider levelBar;
    public Transform IllustrationGroup;

    [Header("Element Group")]
    private Element[] elements;

    public Transform[] OrderElements;

    [Header("Army Type's Name")]
    public ElementTypeInfo[] ArmyTypes;

    protected override void Start()
    {
        Toggle.ActiveToggle(0);
    }

    protected override void Init()
    {
        SetupIllustrationGroup();
        SetupOrderElements();
        Toggle.CheckMarkEvents += delegate
        {
            Load();
        };

        upgradeBtn.OnClickEvents +=
            delegate { OnUpgradeBtn(); };
    }

    private void SetupIllustrationGroup()
    {
        typeName = IllustrationGroup.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        illusImg = IllustrationGroup.GetChild(0).GetComponentInChildren<GUIInteractableIcon>();

        upgradeBtn = IllustrationGroup.GetChild(1).GetComponent<GUIInteractableIcon>();

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
                    WDOCtrl.Open(UgrResWindow.UpgradeResearch);
                    OnElementBtn(ArmyTypes[Toggle.ActiveIndex].Types[captureIndex]);
                };
        }

    }

    private void OnElementBtn(ListUpgrade type)
    {
        MilitaryTable table = DBReference.Instance[type] as MilitaryTable;

        int[] need;
        MilitaryRow row = table.Rows.FirstOrDefault(x => x.Level == SyncData.CurrentUpgrade.Level);

        if (row != null)
            need = new int[] { row.Food, row.Wood, row.Stone, row.Metal };
        else need = new int[4];

        WDOCtrl[UgrResWindow.UpgradeResearch].Load(
            type,
            need,
            row?.MightBonus,
            row?.ResearchTime,
            row?.TimeInt
            );
    }

    private void OnUpgradeBtn()
    {
        ElementTypeInfo armyType = ArmyTypes[Toggle.ActiveIndex];
        ListUpgrade type = armyType.BaseType;

        // open
        WDOCtrl.Open(UgrResWindow.UpgradeResearch);
        // server data 
        int level = SyncData.BaseUpgrade[type].Level;
  
        MainBaseTable table = WDOCtrl[type] as MainBaseTable;

        int[] need;
        MainBaseRow row = table.Rows.FirstOrDefault(x => x.Level == level);
        if (row != null)
            need = new int[] { row.FoodCost, row.WoodCost, row.StoneCost, row.MetalCost };
        else need = new int[4];

        /// <summary>
        /// 0: type - ListUpgrade
        /// 1: need material - int[4]
        /// 2: might bonus - int
        /// 3: time min - string
        /// 4: time int - int
        /// </summary>
        WDOCtrl[UgrResWindow.UpgradeResearch].Load
            (type,
            need,
            row?.MightBonus,
            row?.TimeMin,
            row?.TimeInt
            );
    }

    public override void Load(params object[] data)
    {
        ElementTypeInfo armyType = ArmyTypes[Toggle.ActiveIndex];
        ListUpgrade type = armyType.BaseType;

        // sv data
        int mainbaseLv = SyncData.BaseUpgrade[ListUpgrade.MainBase].Level;
        int selTypeLv = SyncData.BaseUpgrade[type].Level;

        for (int i = 0,level = 0; i < armyType.Types.Length; i++)
        {
            level = SyncData.BaseUpgrade[armyType.Types[i]].Level;
            elements[i].Icon.InteractableChange(level > 0);
            elements[i].LevelBar.Value = level;
        }
        
        // check active or not for upgrade btn
        upgradeBtn.InteractableChange(mainbaseLv > selTypeLv);

        // set level bar value and rename element btn
        levelBar.Value = selTypeLv;
        for (int i = 0; i < armyType.Types.Length; i++)
        {
            elements[i].Icon.Placeholder.text = armyType.Titles[i];
        }
    }
}

﻿using System;
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
            typeName.text = ArmyTypes[Toggle.ActiveIndex].BaseType.ToString().InsertSpace();
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
                    OnElementBtn(ArmyTypes[Toggle.ActiveIndex].Types[captureIndex]);
                };
        }

    }

    private void OnElementBtn(ListUpgrade type)
    {
        WDOCtrl.Open(UgrResWindow.UpgradeResearch);
        WDOCtrl[UgrResWindow.UpgradeResearch].Load(type);
    }

    private void OnUpgradeBtn()
    {
        ElementTypeInfo armyType = ArmyTypes[Toggle.ActiveIndex];
        ListUpgrade type = armyType.BaseType;

        // open
        WDOCtrl.Open(UgrResWindow.UpgradeResearch);
        WDOCtrl[UgrResWindow.UpgradeResearch].Load(type);
    }

    public override void Load(params object[] data)
    {
        ElementTypeInfo armyType = ArmyTypes[Toggle.ActiveIndex];
        ListUpgrade type = armyType.BaseType;

        // sv data
        int mainbaseLv = SyncData.BaseUpgrade[ListUpgrade.MainBase].Level;
        int selTypeLv = SyncData.BaseUpgrade[type].Level;

        for (int i = 0, level = 0; i < armyType.Types.Length; i++)
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

    public override void Open()
    {
        base.Open();
        Toggle.ActiveToggle(0);
    }
}
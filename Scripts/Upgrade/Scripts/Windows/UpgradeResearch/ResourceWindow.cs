using DB;
using EnumCollect;
using ManualTable;
using ManualTable.Row;
using System.Linq;
using UI.Widget;
using UnityEngine;
using static WindowManager;

public class ResourceWindow : BaseWindow,IWindowGroup
{
    [Header("Constructs"), Space]
    public Transform[] Constructs;
    private ArmyWindow.Element[] constructElements;

    [Header("Research")]
    public Transform[] Researchs;
    private ArmyWindow.Element[] researchElements;

    public ListUpgrade[] ResearchTypes;
    public ListUpgrade[] ConstructTypes;

    public WindowGroup Group
    {
        get { return WDOCtrl[GroupType]; }
    }

    public WindowGroupType GroupType
    {
        get { return WindowGroupType.UpgradeResearchGroup; }
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
                    Group.Open(WindowType.UpgradeResearch);
                    OnBtnElement(ConstructTypes[captureIndex]);
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
                   Group.Open(WindowType.UpgradeResearch);
                   OnBtnElement(ResearchTypes[captureIndex]);
               };
        }
    }

    protected override void Init()
    {
        SetupConstructElement();
        SetupResearchElements();
    }

    public override void Load(params object[] input)
    {
        throw new System.NotImplementedException();
    }

    private void OnBtnElement(ListUpgrade type)
    {
        MainBaseTable table = DBReference.Instance[type] as MainBaseTable;
        if (table == null) return;

        int[] need;
        MainBaseRow row = table.Rows.FirstOrDefault(x => x.Level == SyncData.BaseUpgrade[type].Level);

        if (row != null)
            need = new int[] { row.FoodCost, row.WoodCost, row.StoneCost, row.MetalCost };
        else need = new int[4];

        Group[WindowType.UpgradeResearch].Load(
            type,
            need,
            row?.MightBonus,
            row?.TimeMin,
            row?.TimeInt
            );
    }
}

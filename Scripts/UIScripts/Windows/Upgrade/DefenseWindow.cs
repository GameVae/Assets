using UnityEngine;
using UI.Widget;
using static WindowManager;
using EnumCollect;
using ManualTable;
using DB;
using ManualTable.Row;
using System.Linq;
using Generic.Singleton;

public class DefenseWindow : BaseWindow,IWindowGroup
{
    [Header("Constructs"), Space]
    public Transform[] Constructs;
    public ListUpgrade[] Types;

    public WindowGroup Group
    {
        get { return WDOCtrl[GroupType]; }
    }

    public WindowGroupType GroupType
    {
        get { return WindowGroupType.UpgradeResearchGroup; }
    }

    private ArmyWindow.Element[] constructElements;

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
                    OnBtnElement(Types[captureIndex]);
                };
        }
    }

    protected override void Init()
    {
        SetupConstructElement();
    }

    public override void Load(params object[] input)
    {
        throw new System.NotImplementedException();
    }

    private void OnBtnElement(ListUpgrade type)
    {
        SQLiteTable_MainBase table = Singleton.Instance<DBReference>()[type] as SQLiteTable_MainBase;
        if (table == null) return;

        int[] need;
        MainBaseRow row = table.Rows.FirstOrDefault(x => x.Level == SyncData.CurrentBaseUpgrade[type].Level);

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

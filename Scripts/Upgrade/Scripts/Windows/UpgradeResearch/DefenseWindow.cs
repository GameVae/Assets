using UnityEngine;
using UI.Widget;
using static UpgResWdoCtrl;
using EnumCollect;
using ManualTable;
using DB;
using ManualTable.Row;
using System.Linq;

public class DefenseWindow : BaseWindow
{
    [Header("Constructs"), Space]
    public Transform[] Constructs;
    public ListUpgrade[] Types;

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
                    Controller.Open(UgrResWindow.UpgradeResearch);
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
        MainBaseTable table = DBReference.Instance[type] as MainBaseTable;
        if (table == null) return;

        int[] need;
        MainBaseRow row = table.Rows.FirstOrDefault(x => x.Level == SyncData.BaseUpgrade[type].Level);

        if (row != null)
            need = new int[] { row.FoodCost, row.WoodCost, row.StoneCost, row.MetalCost };
        else need = new int[4];

        Controller[UgrResWindow.UpgradeResearch].Load(
            type,
            need,
            row?.MightBonus,
            row?.TimeMin,
            row?.TimeInt
            );
    }
}

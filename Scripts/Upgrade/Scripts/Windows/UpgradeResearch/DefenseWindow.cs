using UnityEngine;
using UI.Widget;
using static UpgResWdoCtrl;
using EnumCollect;

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
                    Controller[UgrResWindow.UpgradeResearch].Load(Types[captureIndex]);
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

}

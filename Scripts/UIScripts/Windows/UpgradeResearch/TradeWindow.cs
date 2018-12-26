using UnityEngine;
using UI.Widget;


public class TradeWindow : MonoBehaviour, IWindow
{
    private bool inited;
    private UpgradeResearchManager manager;

    [Header("Constructs"), Space]
    public ArmyWindow.Element Market;
    public ArmyWindow.Element PhoHienShip;

    [Header("Research"), Space]
    public Transform[] MarketResearchs;
    public Transform[] PHShiptResearchs;

    private ArmyWindow.Element[] marketResearchElements;
    private ArmyWindow.Element[] PHShipResearchElements;

    private void Awake()
    {
        if (!inited)
        {
            manager = GetComponentInParent<UpgradeResearchManager>();
            SetupMarketResearch();
            SetupHPShipResearch();
            inited = true;
        }
    }

    private void SetupMarketResearch()
    {
        int count = MarketResearchs.Length;
        marketResearchElements = new ArmyWindow.Element[count];
        for (int i = 0; i < count; i++)
        {
            marketResearchElements[i] = new ArmyWindow.Element()
            {
                Icon = MarketResearchs[i].GetComponentInChildren<GUIInteractableIcon>(),
                LevelBar = MarketResearchs[i].GetComponentInChildren<GUIProgressSlider>(),
            };
            marketResearchElements[i].Icon.OnClickEvents
                += delegate { manager.Open(UpgradeResearchManager.Window.UpgradeResearch); };
        }
    }

    private void SetupHPShipResearch()
    {
        int count = PHShiptResearchs.Length;
        PHShipResearchElements = new ArmyWindow.Element[count];
        for (int i = 0; i < count; i++)
        {
            PHShipResearchElements[i] = new ArmyWindow.Element()
            {
                Icon = PHShiptResearchs[i].GetComponentInChildren<GUIInteractableIcon>(),
                LevelBar = PHShiptResearchs[i].GetComponentInChildren<GUIProgressSlider>(),
            };
            PHShipResearchElements[i].Icon.OnClickEvents
                += delegate { manager.Open(UpgradeResearchManager.Window.UpgradeResearch); };
        }
    }

    public void LoadData(params object[] input)
    {
        throw new System.NotImplementedException();
    }

}

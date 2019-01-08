using UnityEngine;
using UI.Widget;
using static UpgradeResearchManager;
using EnumCollect;

public class TradeWindow : MonoBehaviour, IWindow
{
    private UpgradeResearchManager manager;

    [Header("Constructs"), Space]
    public ArmyWindow.Element Market;
    public ArmyWindow.Element PhoHienShip;
    public ListUpgrade[] UpgradeTypes;

    [Header("Research"), Space]
    public Transform[] MarketResearchs;
    public Transform[] PHShiptResearchs;

    public ListUpgrade[] MarketResearchTypes;
    public ListUpgrade[] PHShiptResearchTypes;

    private ArmyWindow.Element[] marketResearchElements;
    private ArmyWindow.Element[] PHShipResearchElements;

    private void Awake()
    {
        manager = GetComponentInParent<UpgradeResearchManager>();
        SetupMarketResearch();
        SetupHPShipResearch();
    }

    private void SetupMarketResearch()
    {
        int count = MarketResearchs.Length;
        marketResearchElements = new ArmyWindow.Element[count];
        for (int i = 0; i < count; i++)
        {
            int captureIndex = i;
            marketResearchElements[i] = new ArmyWindow.Element()
            {
                Icon = MarketResearchs[i].GetComponentInChildren<GUIInteractableIcon>(),
                LevelBar = MarketResearchs[i].GetComponentInChildren<GUIProgressSlider>(),
            };
            marketResearchElements[i].Icon.OnClickEvents
                += delegate 
                {
                    manager.Open(Window.UpgradeResearch);
                    manager[Window.UpgradeResearch].Load(MarketResearchTypes[captureIndex]);
                };
        }
    }

    private void SetupHPShipResearch()
    {
        int count = PHShiptResearchs.Length;
        PHShipResearchElements = new ArmyWindow.Element[count];
        for (int i = 0; i < count; i++)
        {
            int captureIndex = i;
            PHShipResearchElements[i] = new ArmyWindow.Element()
            {
                Icon = PHShiptResearchs[i].GetComponentInChildren<GUIInteractableIcon>(),
                LevelBar = PHShiptResearchs[i].GetComponentInChildren<GUIProgressSlider>(),
            };
            PHShipResearchElements[i].Icon.OnClickEvents
                += delegate 
                {
                    manager.Open(Window.UpgradeResearch);
                    manager[Window.UpgradeResearch].Load(PHShiptResearchTypes[captureIndex]);
                };
        }
    }

    public void Load(params object[] input)
    {
        throw new System.NotImplementedException();
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

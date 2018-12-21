using ManualTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeResearchManager : MonoBehaviour
{
    public enum Window
    {
        MainWindow,
        UpgradeAndResearch,
        Army,
        Resource,
        Defense,
        Trade
    }

    public class ArmyWindowData
    {
        public string[] ElementName;
        public Image[] ElementImage;
        public int[] ElementLevel;
    }

    private ArmyWindowData armyData;

    public Transform ActiveWindow { get; private set; }

    [Header("Window Ref")]
    public Transform MainWindow;
    public Transform UpgradeResourceWindow;
    public Transform ResourceWindow;
    public Transform ArmyWindow;
    public Transform DefenseWindow;
    public Transform TradeWindow;
    public ManualAction BackButton;

    public MainBaseTable InfantryData;

    private void Awake()
    {
        int size = 4;
        armyData = new ArmyWindowData()
        {
            ElementName = new string[size],
            ElementImage = new Image[size],
            ElementLevel = new int[size],
        };

        BackButton.ClickAction += delegate
        {
            if (ActiveWindow != MainWindow)
                ActiveMainWindow();
            else
            {
                ActiveWindow.gameObject.SetActive(false);
                ActiveWindow = null;
                gameObject.SetActive(false);
            }
        };
    }

    private void Start()
    {
        Open();
    }

    public ArmyWindowData GetArmyData(ArmyWindow.ArmyType type)
    {
        switch (type)
        {
            case global::ArmyWindow.ArmyType.Infantry:
                SetInfantryData();
                break;
            case global::ArmyWindow.ArmyType.Ranged:
                break;
            case global::ArmyWindow.ArmyType.Mounted:
                break;
            case global::ArmyWindow.ArmyType.SeigeEngine:
                break;
        }
        return armyData;
    }

    private void SetInfantryData()
    {
        armyData.ElementName[0] = "Soldier";
        armyData.ElementName[1] = "Trained";
        armyData.ElementName[2] = "Forbidden";
        armyData.ElementName[3] = "Heroic";
    }

    public void Active(Window type)
    {
        switch (type)
        {
            case Window.MainWindow:
                if (ActiveWindow != MainWindow)
                {
                    ActiveWindow?.gameObject.SetActive(false);
                    ActiveWindow = MainWindow;
                }
                break;
            case Window.UpgradeAndResearch:
                if (ActiveWindow != UpgradeResourceWindow)
                {
                    ActiveWindow?.gameObject.SetActive(false);
                    ActiveWindow = UpgradeResourceWindow;
                }
                break;
            case Window.Army:
                if (ActiveWindow != ArmyWindow)
                {
                    ActiveWindow?.gameObject.SetActive(false);
                    ActiveWindow = ArmyWindow;
                }
                break;
            case Window.Resource:
                if (ActiveWindow != ResourceWindow)
                {
                    ActiveWindow?.gameObject.SetActive(false);
                    ActiveWindow = ResourceWindow;
                }
                break;
            case Window.Defense:
                if (ActiveWindow != DefenseWindow)
                {
                    ActiveWindow?.gameObject.SetActive(false);
                    ActiveWindow = DefenseWindow;
                }
                break;
            case Window.Trade:
                if (ActiveWindow != TradeWindow)
                {
                    ActiveWindow?.gameObject.SetActive(false);
                    ActiveWindow = TradeWindow;
                }
                break;
        }
        if (!ActiveWindow.gameObject.activeInHierarchy)
        {
            ActiveWindow.gameObject.SetActive(true);
        }
    }

    public void ActiveMainWindow()
    {
        Active(Window.MainWindow);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        ActiveMainWindow();
    }
}

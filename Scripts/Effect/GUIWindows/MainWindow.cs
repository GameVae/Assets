using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainWindow : MonoBehaviour
{
    public class LevelBar
    {
        public Slider Slider;
        public Text Placeholder;
    }

    private UpgradeResearchManager manager;
    private LevelBar level;

    public Transform MainBaseLevelBar;
    [Header("Button Group")]
    public ManualAction MainBase;
    public ManualAction Resource;
    public ManualAction Army;    
    public ManualAction Defense;
    public ManualAction Trade;

    private void Awake()
    {
        manager = GetComponentInParent<UpgradeResearchManager>();
        level = new LevelBar()
        {
            Slider = MainBaseLevelBar.GetComponentInChildren<Slider>(),
            Placeholder = MainBaseLevelBar.GetComponentInChildren<Text>(),
        };


        #region Setup button switch window
        MainBase.ClickAction += delegate 
        {
            manager.Active(UpgradeResearchManager.Window.UpgradeAndResearch);
        };

        Resource.ClickAction += delegate
        {
            manager.Active(UpgradeResearchManager.Window.Resource);
        };

        Army.ClickAction += delegate
        {
            manager.Active(UpgradeResearchManager.Window.Army);
        };

        Defense.ClickAction += delegate
        {
            manager.Active(UpgradeResearchManager.Window.Defense);
        };

        Trade.ClickAction += delegate
        {
            manager.Active(UpgradeResearchManager.Window.Trade);
        };
        #endregion
    }
}

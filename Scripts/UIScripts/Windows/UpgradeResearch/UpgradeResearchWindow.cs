using EnumCollect;
using Network.Sync;
using System;
using System.Text.RegularExpressions;
using TMPro;
using UI.Widget;
using UnityEngine;

public class UpgradeResearchWindow : MonoBehaviour, IWindow
{
    private ListUpgrade type;
    private int[] curMaterials;
    private int[] needMaterials;
    private int mightBonus;
    private string timeMin;
    private int timeInt;

    private UpgradeResearchManager manager;

    public TextMeshProUGUI Title;

    [Header("Progess Bar"), Space]
    public GUISliderWithBtn ProgressSlider;

    [Header("Requirement"), Space]
    public TextMeshProUGUI BuildingLevel;
    public TextMeshProUGUI ResearchLevel;

    [Header("Order Materials"), Space]
    public GUIHorizontalInfo[] OrderMaterialElements;
    public TextMeshProUGUI DurationText;

    [Header("Info Group"), Space]
    public TextMeshProUGUI IllusName;
    public GUIInteractableIcon IllustrateImage;

    [Header("Result Group"), Space]
    public TextMeshProUGUI NumberName;
    public TextMeshProUGUI Amount;

    [Header("Button Group"), Space]
    public GUIInteractableIcon InstantBtn;
    public GUIInteractableIcon LevelUpBtn;


    private void Awake()
    {
        manager = GetComponentInParent<UpgradeResearchManager>();
        curMaterials = new int[4];
    }

    private void Start()
    {
    }

    private void Update()
    {
        ProgressBarCount();
    }
    /// <summary>
    /// 0: name - string
    /// 1: need material - int[4]
    /// 2: might bonus - int
    /// 3: time min - string
    /// 4: time int - int
    /// </summary>
    /// <param name="data">Params object</param>
    public void Load(params object[] data)
    {
        type = data.TryGet<ListUpgrade>(0);
        needMaterials = data.TryGet<int[]>(1);
        mightBonus = data.TryGet<int>(2);
        timeMin = data.TryGet<string>(3);
        timeInt = data.TryGet<int>(4);

        ProgressSlider.Slider.MaxValue = timeInt;
        bool activeProgressBar = type.IsUpgrade() ? type == manager.Sync.BaseInfo.UpgradeWait_ID
                                                              : type == manager.Sync.BaseInfo.ResearchWait_ID;
        ActiveButtonGroup(!activeProgressBar);

        curMaterials[0] = manager.Sync.BaseInfo.Farm;
        curMaterials[1] = manager.Sync.BaseInfo.Wood;
        curMaterials[2] = manager.Sync.BaseInfo.Stone;
        curMaterials[3] = manager.Sync.BaseInfo.Metal;
        // 1 - name - title
        Title.text = type.ToString().InsertSpace();


        NumberName.text = "Might Bonus";
        Amount.text = string.Format("{0}", mightBonus);
        DurationText.text = "Duration: " + timeMin;


        if (curMaterials != null && needMaterials != null)
        {
            for (int i = 0; i < 4; i++)
            {
                int captureInt = i;
                OrderMaterialElements[i].Button.OnClickEvents += delegate
                {
                    SetMaterialRequirement(captureInt, ++curMaterials[captureInt], needMaterials[captureInt]);
                };
                SetMaterialRequirement(i, curMaterials[i], needMaterials[i]);
            }
        }

        if (manager.Sync.Levels.CurrentUpgradeLv < manager.Sync.Levels.UpgradeRequire)
        {
            BuildingLevel.transform.parent.gameObject.SetActive(true);
            BuildingLevel.text = manager.Sync.Levels.UpgradeRequire.ToString();
            BuildingLevel.color = Color.red;
        }
        else BuildingLevel.transform.parent.gameObject.SetActive(false);

        if (manager.Sync.Levels.CurrentUpgradeLv < manager.Sync.Levels.ResearchRequire)
        {
            ResearchLevel.transform.parent.gameObject.SetActive(true);
            ResearchLevel.text = manager.Sync.Levels.ResearchRequire.ToString();
            ResearchLevel.color = Color.red;
        }
        else ResearchLevel.transform.parent.gameObject.SetActive(false);

    }

    private void SetMaterialRequirement(int index, int cur, int need)
    {
        GUIHorizontalInfo material = OrderMaterialElements[index];
        material.InteractableChange(cur < need);
        if (cur >= need)
            material.Placeholder.text = string.Format("{0}/{1}", cur, need);
        else
            material.Placeholder.text = string.Format("<color=red>{0}</color>/{1}", cur, need);
    }

    private void ActiveButtonGroup(bool value)
    {
        InstantBtn.InteractableChange(value);
        LevelUpBtn.InteractableChange(value);
        ProgressSlider.gameObject.SetActive(!value);
    }


    private void ProgressBarCount()
    {
        if (ProgressSlider.gameObject.activeInHierarchy)
        {
            if (type.IsUpgrade())
            {
                ProgressSlider.Slider.Placeholder.text = type.ToString().InsertSpace() + " " +
                   TimeSpan.FromSeconds(manager.Sync.BaseInfo.UpgradeRemainingInt).ToString();
                if (manager.Sync.BaseInfo.UpgradeRemainingInt == 0)
                    ActiveButtonGroup(true);
            }
            else
            {
                ProgressSlider.Slider.Placeholder.text = type.ToString().InsertSpace() + " " +
                   TimeSpan.FromSeconds(manager.Sync.BaseInfo.ResearchRemainingInt).ToString();
                if (manager.Sync.BaseInfo.ResearchRemainingInt == 0)
                    ActiveButtonGroup(true);
            }
        }
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

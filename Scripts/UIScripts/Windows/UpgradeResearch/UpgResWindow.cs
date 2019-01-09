using EnumCollect;
using System;
using TMPro;
using UI.Widget;
using UnityEngine;

public class UpgResWindow : BaseWindow
{
    private ListUpgrade type;
    private int[] curMaterials;
    private int[] needMaterials;
    private int timeInt;
    private int mightBonus;
    private string timeMin;
    private bool isUpgradeType;

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

    protected override void Update()
    {
        base.Update();
        SetTextProgCoundown();
    }

    protected override void Init()
    {
        curMaterials = new int[4];
    }

    /// <summary>
    /// 0: name - string
    /// 1: need material - int[4]
    /// 2: might bonus - int
    /// 3: time min - string
    /// 4: time int - int
    /// </summary>
    /// <param name="data">Params object</param>
    public override void Load(params object[] data)
    {
        type = data.TryGet<ListUpgrade>(0);
        needMaterials = data.TryGet<int[]>(1);
        mightBonus = data.TryGet<int>(2);
        timeMin = data.TryGet<string>(3);
        timeInt = data.TryGet<int>(4);
        isUpgradeType = type.IsUpgrade();

        ProgressSlider.Slider.MaxValue = timeInt;
        bool activeProgressBar = isUpgradeType ? type == Controller.Sync.BaseInfo.UpgradeWait_ID
                                                              : type == Controller.Sync.BaseInfo.ResearchWait_ID;
        ActiveButtonGroup(!activeProgressBar);

        curMaterials[0] = Controller.Sync.BaseInfo.Farm;
        curMaterials[1] = Controller.Sync.BaseInfo.Wood;
        curMaterials[2] = Controller.Sync.BaseInfo.Stone;
        curMaterials[3] = Controller.Sync.BaseInfo.Metal;
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

        if (Controller.Sync.Levels.CurrentUpgradeLv < Controller.Sync.Levels.UpgradeRequire)
        {
            BuildingLevel.transform.parent.gameObject.SetActive(true);
            BuildingLevel.text = Controller.Sync.Levels.UpgradeRequire.ToString();
            BuildingLevel.color = Color.red;
        }
        else BuildingLevel.transform.parent.gameObject.SetActive(false);

        if (Controller.Sync.Levels.CurrentResearchLv < Controller.Sync.Levels.ResearchRequire)
        {
            ResearchLevel.transform.parent.gameObject.SetActive(true);
            ResearchLevel.text = Controller.Sync.Levels.ResearchRequire.ToString();
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

    private void SetTextProgCoundown()
    {
        if (ProgressSlider.gameObject.activeInHierarchy)
        {
            if (isUpgradeType)
            {
                ProgressSlider.Slider.Placeholder.text = Controller.Sync.BaseInfo.GetUpgTimeString();
                if (Controller.Sync.BaseInfo.UpgIsDone())
                {
                    ActiveButtonGroup(true);
                }
            }
            else
            {
                ProgressSlider.Slider.Placeholder.text = Controller.Sync.BaseInfo.GetResTimeString();
                if (Controller.Sync.BaseInfo.ResIsDone())
                {
                    ActiveButtonGroup(true);
                }
            }
        }
    }
}

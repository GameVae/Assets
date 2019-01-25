using DB;
using EnumCollect;
using Json.Interface;
using ManualTable;
using ManualTable.Interface;
using ManualTable.Row;
using Network.Data;
using System;
using System.Collections.Generic;
using TMPro;
using UI.Widget;
using UnityEngine;
using static WindowManager;

public class UpgResWindow : BaseWindow,IWindowGroup
{
    private int[] curMaterials;
    private bool isUpgradeType;
    private BaseUpgradeRow refType;

    

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

    public WindowGroup Group
    {
        get { return WDOCtrl[GroupType]; }
    }

    public WindowGroupType GroupType
    {
        get { return WindowGroupType.UpgradeResearchGroup; }
    }

    protected override void Awake()
    {
        base.Awake();
        EventListenersController.Instance.AddEmiter("S_UPGRADE", CreateUpgData);
    }

    protected override void Update()
    {
        base.Update();
        SetTextProgCountdown();
    }

    protected override void Init()
    {
        curMaterials = new int[4];
        LevelUpBtn.OnClickEvents += OnUpgradeBtn;
    }

    /// <summary>
    /// 0: type - ListUpgrade
    /// 1: need material - int[4]
    /// 2: might bonus - int
    /// 3: time min - string
    /// 4: time int - int
    /// </summary>
    /// <param name="data">Params object</param>
    public override void Load(params object[] data)
    {
        ListUpgrade type = data.TryGet<ListUpgrade>(0);
        string title = type.ToString().InsertSpace();
        ITable table = DBReference.Instance[type];
        int level = SyncData.BaseUpgrade[type].Level;
        string jsonData = table[level - 1].ToJSON();
        GenericUpgradeInfo needInfo = Json.JSONBase.FromJSON<GenericUpgradeInfo>(jsonData);

        int[] needMaterials = new int[] {
            needInfo.FoodCost,
            needInfo.WoodCost,
            needInfo.StoneCost,
            needInfo.MetalCost };
        int mightBonus = needInfo.MightBonus;

        curMaterials[0] = SyncData.CurrentMainBase.Farm;
        curMaterials[1] = SyncData.CurrentMainBase.Wood;
        curMaterials[2] = SyncData.CurrentMainBase.Stone;
        curMaterials[3] = SyncData.CurrentMainBase.Metal;

        isUpgradeType = type.IsUpgrade();

        bool activeProgressBar = isUpgradeType ? SyncData.CurrentMainBase.UpgradeWait_ID.IsDefined()
                                                            : SyncData.CurrentMainBase.ResearchWait_ID.IsDefined();

        bool activeBtnGroup = !activeProgressBar;
        activeBtnGroup = activeBtnGroup && IsEnoughtMeterial(needMaterials);
        ActiveBtnGroup(activeBtnGroup);


        bool isSimilarType = true;
        if (isUpgradeType)
        {
            if (SyncData.CurrentMainBase.UpgradeWait_ID.IsDefined() &&
                type != SyncData.CurrentMainBase.UpgradeWait_ID)
            {
                type = SyncData.CurrentMainBase.UpgradeWait_ID;
                isSimilarType = false;
            }
        }
        else
        {
            if (SyncData.CurrentMainBase.ResearchWait_ID.IsDefined() &&
                type != SyncData.CurrentMainBase.ResearchWait_ID)
            {
                type = SyncData.CurrentMainBase.ResearchWait_ID;
                isSimilarType = false;
            }
        }

        if (isSimilarType)
        {            
            table = DBReference.Instance[type];
            level = SyncData.BaseUpgrade[type].Level;
            jsonData = table[level - 1].ToJSON();
            needInfo = Json.JSONBase.FromJSON<GenericUpgradeInfo>(jsonData);
        }
        refType = SyncData.BaseUpgrade[type];

        string timeMin = needInfo.TimeMin;
        int timeInt = needInfo.TimeInt;

        ActiveProgressBar(activeProgressBar && timeInt > 0);
        ProgressSlider.Slider.MaxValue = timeInt;


        bool isBuildingRequire = false;
        bool isResearchRequire = false;
        BuildingLevel.transform.parent.gameObject.SetActive(isBuildingRequire);
        ResearchLevel.transform.parent.gameObject.SetActive(isResearchRequire);

        #region display info
        Title.text = title;

        if (curMaterials != null && needMaterials != null)
        {
            for (int i = 0; i < 4; i++)
            {
                int captureInt = i;
                OrderMaterialElements[i].Button.OnClickEvents += delegate
                {
                    SetMaterialRequirement(captureInt, ++curMaterials[captureInt], needMaterials[captureInt]);
                    if (IsEnoughtMeterial(needMaterials))
                    {
                        ActiveBtnGroup(true);
                    }
                };
                SetMaterialRequirement(i, curMaterials[i], needMaterials[i]);
            }
        }

        // ============================ //
        NumberName.text = "Might Bonus";
        Amount.text = string.Format("{0}", mightBonus);

        DurationText.text = "Duration: " + timeMin;
        #endregion
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

    private void ActiveProgressBar(bool value)
    {
        ProgressSlider.gameObject.SetActive(value);
    }

    private void ActiveBtnGroup(bool value)
    {
        InstantBtn.InteractableChange(value);
        LevelUpBtn.InteractableChange(value);
    }

    private void SetTextProgCountdown()
    {
        if (ProgressSlider.gameObject.activeInHierarchy)
        {
            if (isUpgradeType)
            {
                ProgressSlider.Slider.Value = ProgressSlider.Slider.MaxValue - SyncData.CurrentMainBase.UpgradeTime;
                ProgressSlider.Slider.Placeholder.text = SyncData.CurrentMainBase.GetUpgTimeString();

                if (SyncData.CurrentMainBase.UpgIsDone())
                {
                    ActiveProgressBar(false);
                    ActiveBtnGroup(true);
                }
            }
            else
            {
                ProgressSlider.Slider.Value = ProgressSlider.Slider.MaxValue - SyncData.CurrentMainBase.ResearchTime;
                ProgressSlider.Slider.Placeholder.text = SyncData.CurrentMainBase.GetResTimeString();
                if (SyncData.CurrentMainBase.ResIsDone())
                {
                    ActiveProgressBar(false);
                    ActiveBtnGroup(true);
                }
            }
        }
    }

    private JSONObject CreateUpgData()
    {
        UserInfoRow userInfo = (UserInfoRow)SyncData.UserInfo[0];
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"ID_Server"   ,userInfo.Server_ID },
            {"ID_User"     ,userInfo.ID_User.ToString()},
            {"BaseNumber"  ,SyncData.CurrentMainBase.BaseNumber.ToString() },
            {"ID_Upgrade"  ,((int)refType.ID).ToString()},
            {"UpgradeType" ,refType.UpgradeType.ToString() },
            {"Level"       ,refType.Level.ToString()}
        };
        return new JSONObject(data);
    }

    private void OnUpgradeBtn()
    {

        ITable table = DBReference.Instance[refType.ID];
        string jsonData = table[refType.Level - 1].ToJSON();
        GenericUpgradeInfo needInfo = Json.JSONBase.FromJSON<GenericUpgradeInfo>(jsonData);

        SyncData.CurrentMainBase.Farm -= needInfo.FoodCost;
        SyncData.CurrentMainBase.Wood -= needInfo.WoodCost;
        SyncData.CurrentMainBase.Metal -= needInfo.MetalCost;
        SyncData.CurrentMainBase.Stone -= needInfo.StoneCost;

        SyncData.CurrentMainBase.UpgradeWait_ID = refType.ID;
        SyncData.CurrentMainBase.UpgradeTime = needInfo.TimeInt;

        EventListenersController.Instance.Emit("S_UPGRADE");
        Group.Close();
    }

    private bool IsEnoughtMeterial(int[] needMaterials)
    {
        for (int i = 0; i < 4; i++)
        {
            if (curMaterials[i] < needMaterials[i]) return false;
        }
        return true;
    }
}

using DB;
using EnumCollect;
using Generic.Singleton;
using Json.Interface;
using ManualTable;
using ManualTable.Interface;
using ManualTable.Row;
using Network.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UI.Widget;
using UnityEngine;
using static WindowManager;

public class UpgResWindow : BaseWindow, IWindowGroup
{
    /// singleton
    private DBReference dbReference;
    private FieldReflection fieldReflection;
    private EventListenersController listenersController;
    ///

    private int[] curMaterials;
    private bool isUpgradeType;
    private BaseUpgradeRow refUpgType;
    private BaseInfoRow currentMainBase;

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

        dbReference = Singleton.Instance<DBReference>();
        fieldReflection = Singleton.Instance<FieldReflection>();
        listenersController = Singleton.Instance<EventListenersController>();
        listenersController.AddEmiter("S_UPGRADE", S_UPGRADE);

        LevelUpBtn.OnClickEvents += OnLevelBtn;
    }

    protected override void Update()
    {
        base.Update();
        SetTextProgCountdown();
    }

    protected override void Init()
    {
        curMaterials = new int[4];
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
        currentMainBase = SyncData.CurrentMainBase;

        ListUpgrade loadType = data.TryGet<ListUpgrade>(0);
        ITable table = dbReference[loadType];

        string title = loadType.ToString().InsertSpace();
        int level = SyncData.CurrentBaseUpgrade[loadType].Level;


        IJSON needInfo = table[level - 1];

        int foodCost = GetPublicValue<int>(needInfo, "FoodCost");
        int woodCost = GetPublicValue<int>(needInfo, "WoodCost");
        int metalCost = GetPublicValue<int>(needInfo, "MetalCost");
        int stoneCost = GetPublicValue<int>(needInfo, "StoneCost");
        int mightBonus = GetPublicValue<int>(needInfo, "MightBonus");

        int[] needMaterials = new int[] {
            foodCost,
            woodCost,
            stoneCost,
            metalCost };

        curMaterials[0] = currentMainBase.Farm;
        curMaterials[1] = currentMainBase.Wood;
        curMaterials[2] = currentMainBase.Stone;
        curMaterials[3] = currentMainBase.Metal;

        isUpgradeType = loadType.IsUpgrade();

        bool activeProgressBar = isUpgradeType ? currentMainBase.UpgradeWait_ID.IsDefined()
                                                            : currentMainBase.ResearchWait_ID.IsDefined();

        bool activeBtnGroup = !activeProgressBar;
        activeBtnGroup = activeBtnGroup && IsEnoughtMeterial(needMaterials);
        ActiveBtnGroup(activeBtnGroup);


        bool isSimilarCurUpgResType = true;
        if (isUpgradeType)
        {
            if (currentMainBase.UpgradeWait_ID.IsDefined() &&
                loadType != currentMainBase.UpgradeWait_ID)
            {
                loadType = currentMainBase.UpgradeWait_ID;
                isSimilarCurUpgResType = false;
            }
        }
        else
        {
            if (currentMainBase.ResearchWait_ID.IsDefined() &&
                loadType != currentMainBase.ResearchWait_ID)
            {
                loadType = currentMainBase.ResearchWait_ID;
                isSimilarCurUpgResType = false;
            }
        }

        refUpgType = SyncData.CurrentBaseUpgrade[loadType];
        if (!isSimilarCurUpgResType)
        {
            table = dbReference[loadType];
            level = refUpgType.Level;
            needInfo = table[level - 1];
        }


        string timeMin = GetPublicValue<string>(needInfo, "TimeMin");// needInfo.TimeMin;
        int timeInt = GetPublicValue<int>(needInfo, "TimeInt"); //needInfo.TimeInt;

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
                ProgressSlider.Slider.Value = ProgressSlider.Slider.MaxValue - (float)currentMainBase.UpgradeTime;
                ProgressSlider.Slider.Placeholder.text = UpgradeText(currentMainBase);

                if (currentMainBase.IsUpgradeDone)
                {
                    ActiveProgressBar(false);
                    ActiveBtnGroup(true);
                }
            }
            else
            {
                ProgressSlider.Slider.Value = ProgressSlider.Slider.MaxValue - (float)currentMainBase.ResearchTime;
                ProgressSlider.Slider.Placeholder.text = ResearchText(currentMainBase);

                if (currentMainBase.IsResearchDone)
                {
                    ActiveProgressBar(false);
                    ActiveBtnGroup(true);
                }
            }
        }
    }

    private string ResearchText(BaseInfoRow baseInfo)
    {
        string type = baseInfo.ResearchWait_ID.ToString().InsertSpace();
        string remainTime = System.TimeSpan.FromSeconds(Mathf.RoundToInt((float)baseInfo.ResearchTime)).ToString().Replace(".", "d ");

        return type + " " + remainTime;
    }

    private string UpgradeText(BaseInfoRow baseInfo)
    {
        string type = baseInfo.UpgradeWait_ID.ToString().InsertSpace();
        string remainTime = System.TimeSpan.FromSeconds(Mathf.RoundToInt((float)baseInfo.UpgradeTime)).ToString().Replace(".", "d ");

        return type + " " + remainTime;
    }

    private JSONObject S_UPGRADE()
    {
        UserInfoRow userInfo = SyncData.MainUser;
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"ID_Server"   ,userInfo.Server_ID },
            {"ID_User"     ,userInfo.ID_User.ToString()},
            {"BaseNumber"  ,currentMainBase.BaseNumber.ToString() },
            {"ID_Upgrade"  ,((int)refUpgType.ID).ToString()},
            {"UpgradeType" ,refUpgType.UpgradeType.ToString() },
            {"Level"       ,refUpgType.Level.ToString()}
        };
        return new JSONObject(data);
    }

    private void OnLevelBtn()
    {
        ITable table = dbReference[refUpgType.ID];
        IJSON needInfo = table[refUpgType.Level - 1];

        #region old
        //string jsonData = table[refUpgType.Level - 1].ToJSON();
        //// GenericUpgradeInfo needInfo = JsonUtility.FromJson<GenericUpgradeInfo>(jsonData);
        #endregion

        int foodCost = GetPublicValue<int>(needInfo, "FoodCost");
        int woodCost = GetPublicValue<int>(needInfo, "WoodCost");
        int metalCost = GetPublicValue<int>(needInfo, "MetalCost");
        int stoneCost = GetPublicValue<int>(needInfo, "StoneCost");
        int timeInt = GetPublicValue<int>(needInfo, "TimeInt");

        currentMainBase.Farm -= foodCost;  // needInfo.FoodCost; 
        currentMainBase.Wood -= woodCost;  //needInfo.WoodCost;
        currentMainBase.Metal -= metalCost;// needInfo.MetalCost;
        currentMainBase.Stone -= stoneCost;// needInfo.StoneCost;

        currentMainBase.UpgradeWait_ID = refUpgType.ID;
        currentMainBase.SetUpgradeTime(timeInt); // needInfo.TimeInt;

        listenersController.Emit("S_UPGRADE");
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

    private T GetPublicValue<T>(object obj, string name)
    {
        return fieldReflection.GetPublicField<T>(obj, name);
    }
}

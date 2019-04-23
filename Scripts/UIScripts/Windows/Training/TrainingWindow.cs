using DB;
using EnumCollect;
using Generic.Singleton;
using DataTable;
using DataTable.Row;
using Network.Data;
using System;
using System.Collections.Generic;
using TMPro;
using UI.Widget;
using UnityEngine;
using UnityEngine.UI;
using UI.Composites;

public class TrainingWindow : BaseWindow
{
    [SerializeField] private GUIHorizontalGrid rowLayoutPrefab;

    // Singleton
    private DBReference dBReference;
    private FieldReflection fieldReflection;
    private EventListenersController listenersController;

    private List<GUIHorizontalGrid> rows;
    private GUIHorizontalGrid curRow;
    private int elementCount;

    private TrainningCostRow refCostInfo;
    private ITableData refTypeTraining;

    private BaseUpgradeRow refType;
    private ListUpgrade selectedType;

    public GUIScrollView ScrollView;
    public GUIInteractableIcon Element;
    public int ColumnNum;


    [Header("Cost Infomation")]
    public TextMeshProUGUI QualityNum;
    public TextMeshProUGUI FoodInfo;
    public TextMeshProUGUI WoodInfo;
    public TextMeshProUGUI StoneInfo;
    public TextMeshProUGUI MetalInfo;

    [Header("Selected group")]
    public GUIInteractableIcon CurrentSelect;
    public Slider QualitySlider;
    public SelectableComp AcceptBtn;
    public CustomInputField QualityInput;

    [Header("Main group")]
    public GUISliderWithBtn TranningProgress;
    public GUIInteractableIcon OpenButton;

    [Range(0f, 1f)]
    public float ElementSize;
    private int quality;

    private void Awake()
    {
        QualitySlider.onValueChanged.AddListener((float value) => OnSliderValueChanged(value));

        OpenButton.OnClickEvents += Open;

        AcceptBtn.OnClickEvents += OnAccept;
        QualityInput.OnValueChanged += OnInputFieldhanged;
    }

    protected override void Start()
    {
        base.Start();

        dBReference = Singleton.Instance<DBReference>();
        fieldReflection = Singleton.Instance<FieldReflection>();
        listenersController = Singleton.Instance<EventListenersController>();

        listenersController.AddEmiter("S_TRAINING", S_TRAINING);
    }

    public override void Load(params object[] input)
    {
        BaseInfoRow baseInfo = SyncData.CurrentMainBase;

        ListUpgrade tranningType = baseInfo.TrainingUnit_ID;
        if (tranningType.IsDefined())
        {
            ITable table = dBReference[tranningType];
            int level = SyncData.CurrentBaseUpgrade[tranningType].Level;

            ITableData typeInfo = table[level - 1];

            TranningProgress.Slider.MaxValue =
                fieldReflection.GetPublicField<int>(typeInfo, "TrainingTime") * baseInfo.TrainingQuality;

            AcceptBtn.Interactable = false;
            TranningProgress.gameObject.SetActive(true);
        }
        else
        {
            AcceptBtn.Interactable = true;
            TranningProgress.gameObject.SetActive(false);
        }
    }

    protected override void Init()
    {
        if (rows == null)
            rows = new List<GUIHorizontalGrid>();
        if (curRow == null)
        {
            curRow = Instantiate(rowLayoutPrefab, ScrollView.Content);
            curRow.ElementSize = ElementSize;
            rows.Add(curRow);
        }

        elementCount = 0;
        List<ListUpgrade> types = new List<ListUpgrade>()
        {
            ListUpgrade.Soldier,
            ListUpgrade.TraninedSolider,
            ListUpgrade.ForbiddenGuard,
            ListUpgrade.Heroic
        };
        AddElement(types);
    }

    private void AddElement(List<ListUpgrade> types)
    {
        List<RectTransform> rectList = new List<RectTransform>();
        for (int i = 0; i < types.Count; i++)
        {
            int capture = i;
            GUIInteractableIcon e = Instantiate(Element);
            rectList.Add(e.transform as RectTransform);
            e.Placeholder.text = types[i].ToString().InsertSpace();
            e.InteractableChange(SyncData.CurrentBaseUpgrade[types[capture]].Level > 0);
            e.OnClickEvents += delegate
            {
                OnSelected(types[capture]);
            };

            elementCount = (elementCount + 1) % ColumnNum;
            if (elementCount == 0 || types.Count - 1 == i)
            {
                curRow.Add(rectList);
                rectList.Clear();

                curRow = Instantiate(rowLayoutPrefab, ScrollView.Content);
                curRow.ElementSize = ElementSize;
                rows.Add(curRow);
            }
        }
    }

    private bool CheckEnoughtResource()
    {
        try
        {
            return (
                SyncData.CurrentMainBase.IsEnoughtResource
                (refCostInfo.FoodCost * quality,
                refCostInfo.WoodCost * quality,
                refCostInfo.StoneCost * quality,
                refCostInfo.MetalCost * quality));
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            return false;
        }
    }

    public override void Open()
    {
        base.Open();
        Load();
    }

    private JSONObject S_TRAINING()
    {
        UserInfoRow user = SyncData.MainUser;
        BaseInfoRow baseInfo = SyncData.CurrentMainBase;

        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"ID_User"      ,user.ID_User.ToString()},
            {"Server_ID"    ,user.Server_ID.ToString()},
            {"BaseNumber"   ,baseInfo.BaseNumber.ToString()},
            {"ID_Unit"      ,((int)refType.ID).ToString()},
            {"Level"        ,refType.Level.ToString()},
            {"Quality"      ,quality.ToString()},
        };
        JSONObject result = new JSONObject(data);
        Debug.Log(result.ToString());
        return result;
    }

    protected override void Update()
    {
        SetText();
    }

    private void SetText()
    {
        if (TranningProgress.gameObject.activeInHierarchy)
        {
            //Debug.Log(SyncData.CurrentMainBase.TrainingTime);
            if (SyncData.CurrentMainBase.TrainingTime > 0)
            {
                TranningProgress.Slider.Value = TranningProgress.Slider.MaxValue - (int)SyncData.CurrentMainBase.TrainingTime;
                TranningProgress.Placeholder.text =
                    TimeSpan.FromSeconds((int)SyncData.CurrentMainBase.TrainingTime).ToString().Replace(".", "d ");
            }
            else
            {
                TranningProgress.gameObject.SetActive(false);
            }
        }
        //Debug.Log("update");
    }

   

    private void SetCostInfo()
    {
        string norFormat = "{0}/{1}";
        string warnFormat = "<color=red>{0}</color>/{1}";

        bool enoughtFood = refCostInfo.FoodCost * quality <= SyncData.CurrentMainBase.Farm;
        bool enoughtWood = refCostInfo.WoodCost * quality <= SyncData.CurrentMainBase.Wood;
        bool enoughtStone = refCostInfo.StoneCost * quality <= SyncData.CurrentMainBase.Stone;
        bool enoughtMetal = refCostInfo.MetalCost * quality <= SyncData.CurrentMainBase.Metal;

        FoodInfo.text = string.Format(enoughtFood ? norFormat : warnFormat, refCostInfo.FoodCost * quality,
            SyncData.CurrentMainBase.Farm);
        WoodInfo.text = string.Format(enoughtWood ? norFormat : warnFormat, refCostInfo.WoodCost * quality,
            SyncData.CurrentMainBase.Wood);
        StoneInfo.text = string.Format(enoughtStone ? norFormat : warnFormat, refCostInfo.StoneCost * quality,
            SyncData.CurrentMainBase.Stone);
        MetalInfo.text = string.Format(enoughtMetal ? norFormat : warnFormat, refCostInfo.MetalCost * quality,
            SyncData.CurrentMainBase.Metal);
    }

    private void OnSelected(ListUpgrade type)
    {
        selectedType = type;
        refType = SyncData.CurrentBaseUpgrade[type];
        ITable dbTable = dBReference[selectedType];

        SQLiteTable_TrainningCost costTable = dBReference[DBType.TrainningCost] as SQLiteTable_TrainningCost;


        refTypeTraining = dbTable[SyncData.CurrentBaseUpgrade[selectedType].Level - 1];
        refCostInfo = costTable[selectedType];

        CurrentSelect.Placeholder.text = selectedType.ToString().InsertSpace();
    }

    private void OnAccept()
    {
        if (CheckEnoughtResource())
        {
            SyncData.CurrentMainBase.Farm -= refCostInfo.FoodCost * quality;
            SyncData.CurrentMainBase.Wood -= refCostInfo.WoodCost * quality;
            SyncData.CurrentMainBase.Stone -= refCostInfo.StoneCost * quality;
            SyncData.CurrentMainBase.Metal -= refCostInfo.MetalCost * quality;

            int trainingMight = fieldReflection.GetPublicField<int>(refTypeTraining, "MightBonus") * quality;
            int trainingTime = fieldReflection.GetPublicField<int>(refTypeTraining, "TrainingTime") * quality;

            SyncData.CurrentMainBase.SetTrainingTime(trainingTime);
            SyncData.CurrentMainBase.Training_Might = trainingMight;
            SyncData.CurrentMainBase.TrainingUnit_ID = selectedType;
            SyncData.CurrentMainBase.TrainingQuality = quality;

            Close();
            listenersController.Emit("S_TRAINING");
        }
    }

    private void OnSliderValueChanged(float value)
    {
        if (!selectedType.IsDefined())
        {
            quality = 0;
            QualitySlider.value = 0;
            return;
        }

        quality = (int)value;
        bool isEnoughResource = CheckEnoughtResource();
        if (isEnoughResource)
        {
            AcceptBtn.Interactable = true;
            QualityNum.text = quality + "/" + QualitySlider.maxValue;
        }
        else
        {
            AcceptBtn.Interactable = false;
            QualityNum.text = string.Format("<color=red>{0}</color>/{1}", quality, QualitySlider.maxValue);
        }
        SetCostInfo();

        if (SyncData.CurrentMainBase.TrainingUnit_ID.IsDefined())
        {
            AcceptBtn.Interactable = false;
        }

        QualityInput.SetContent(quality.ToString());
    }

    private void OnInputFieldhanged(string str)
    {
        if (!selectedType.IsDefined())
        {
            QualityInput.SetContent("0");
            return;
        }

        string parseValue = string.IsNullOrEmpty(str) ? "0" : str;

        int value = int.Parse(parseValue);
        int clampValue = Mathf.Clamp(value, 0, (int)QualitySlider.maxValue);

        QualityInput.SetContent(clampValue.ToString()); // 

        QualitySlider.value = clampValue;
    }
}

using DB;
using EnumCollect;
using ManualTable;
using ManualTable.Interface;
using ManualTable.Row;
using Network.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI.Widget;
using UnityEngine;
using UnityEngine.UI;

public class TranningWindow : BaseWindow
{
    [SerializeField] private GUIHorizontalGrid rowLayoutPrefab;

    private List<GUIHorizontalGrid> rows;
    private GUIHorizontalGrid curRow;
    private int elementCount;

    private TrainningCostRow refCostInfo;
    private MilitaryRow refTrainInfo;

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
    public GUIInteractableIcon AcceptBtn;
    public InputField QualityInput;

    [Header("Main group")]
    public GUISliderWithBtn TranningProgress;
    public GUIOnOffSwitch OpenBtn;


    [Range(0f, 1f)]
    public float ElementSize;
    private int quality;

    protected override void Awake()
    {
        base.Awake();
        QualitySlider.onValueChanged.AddListener((float value) => OnQualitySliderChanged(value));

        OpenBtn.InteractableChange(true);
        OpenBtn.OnClick.AddListener(OpenWindow);

        AcceptBtn.OnClickEvents += OnAccept;
        QualityInput.onValueChanged.AddListener((string value) => OnQualityInputChanged());
    }

    protected override void Start()
    {
        base.Start();
        EventListenersController.Instance.AddEmiter("S_TRAINING", S_TRAINING);
    }

    public override void Load(params object[] input)
    {
        BaseInfoRow baseInfo = SyncData.CurrentMainBase as BaseInfoRow;
        ListUpgrade tranningType = baseInfo.TrainingUnit_ID;
        if (tranningType.IsDefined())
        {
            ITable table = DBReference.Instance[tranningType];
            int level = SyncData.CurrentBaseUpgrade[tranningType].Level;
            string jsonData = table[level - 1].ToJSON();
            MilitaryRow typeInfo = JsonUtility.FromJson<MilitaryRow>(jsonData);
            TranningProgress.Slider.MaxValue = typeInfo.TrainingTime * baseInfo.TrainingQuality;

            AcceptBtn.InteractableChange(false);
            TranningProgress.gameObject.SetActive(true);
        }
        else
        {
            AcceptBtn.InteractableChange(true);
            TranningProgress.gameObject.SetActive(false);
            Debug.Log("false");
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
            ListUpgrade.Solider,
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
            return (SyncData.CurrentMainBase.IsEnoughtResource
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

    private void OpenWindow()
    {
        Open();
        Load();
    }

    private JSONObject S_TRAINING()
    {
        UserInfoRow user = (UserInfoRow)SyncData.UserInfo[0];
        BaseInfoRow baseInfo = (BaseInfoRow)SyncData.BaseInfo[0];

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

    private void OnQualitySliderChanged(float value)
    {
        if (!selectedType.IsDefined())
        {
            quality = 0;
            QualitySlider.value = 0;
            return;
        }

        quality = (int)value;
        QualityInput.text = quality.ToString();

        bool isEnoughResource = CheckEnoughtResource();
        if (isEnoughResource)
        {
            AcceptBtn.InteractableChange(true);
            QualityNum.text = quality + "/" + QualitySlider.maxValue;
        }
        else
        {
            AcceptBtn.InteractableChange(false);
            QualityNum.text = string.Format("<color=red>{0}</color>/{1}", quality, QualitySlider.maxValue);
        }
        SetCostInfo();

        if (SyncData.CurrentMainBase.TrainingUnit_ID.IsDefined())
        {
            AcceptBtn.InteractableChange(false);
        }

    }

    private void SetCostInfo()
    {
        string norFormat = "{0}/{1}";
        string warFormat = "<color=red>{0}</color>/{1}";

        bool enoughtFood = refCostInfo.FoodCost * quality <= SyncData.CurrentMainBase.Farm;
        bool enoughtWood = refCostInfo.WoodCost * quality <= SyncData.CurrentMainBase.Wood;
        bool enoughtStone = refCostInfo.StoneCost * quality <= SyncData.CurrentMainBase.Stone;
        bool enoughtMetal = refCostInfo.MetalCost * quality <= SyncData.CurrentMainBase.Metal;

        FoodInfo.text = string.Format(enoughtFood ? norFormat : warFormat, refCostInfo.FoodCost * quality,
            SyncData.CurrentMainBase.Farm);
        WoodInfo.text = string.Format(enoughtWood ? norFormat : warFormat, refCostInfo.WoodCost * quality,
            SyncData.CurrentMainBase.Wood);
        StoneInfo.text = string.Format(enoughtStone ? norFormat : warFormat, refCostInfo.StoneCost * quality,
            SyncData.CurrentMainBase.Stone);
        MetalInfo.text = string.Format(enoughtMetal ? norFormat : warFormat, refCostInfo.MetalCost * quality,
            SyncData.CurrentMainBase.Metal);
    }

    private void OnSelected(ListUpgrade type)
    {
        selectedType = type;
        refType = SyncData.CurrentBaseUpgrade[type];
        ITable dbTable = DBReference.Instance[selectedType];
        string data = dbTable[SyncData.CurrentBaseUpgrade[selectedType].Level - 1].ToJSON();
        TrainningCostTable table = DBReference.Instance[DBType.TrainningCost] as TrainningCostTable;
        refTrainInfo = JsonUtility.FromJson<MilitaryRow>(data);
        refCostInfo = table.Rows.FirstOrDefault(r => r.ID_Unit == selectedType);

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

            SyncData.CurrentMainBase.TrainingTime = refTrainInfo.TrainingTime * quality;
            SyncData.CurrentMainBase.Training_Might = refTrainInfo.MightBonus * quality;
            SyncData.CurrentMainBase.TrainingUnit_ID = selectedType;
            SyncData.CurrentMainBase.TrainingQuality = quality;

            Close();
            EventListenersController.Instance.Emit("S_TRAINING");
        }
    }

    private void OnQualityInputChanged()
    {
        if (!selectedType.IsDefined())
        {
            QualityInput.text = "0";
            return;
        }

        int value = int.Parse(QualityInput.text);
        int clampValue = (int)Mathf.Clamp(value, 0, QualitySlider.maxValue);
        QualityInput.text = clampValue.ToString();
        QualitySlider.value = clampValue;
        OnQualitySliderChanged(clampValue);
    }
}

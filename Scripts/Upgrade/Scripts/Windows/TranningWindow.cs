using DB;
using EnumCollect;
using Json;
using ManualTable.Interface;
using ManualTable.Row;
using Network.Data;
using System;
using System.Collections.Generic;
using UI.Widget;
using UnityEngine;

public class TranningWindow : BaseWindow
{
    [SerializeField] private GUIHorizontalGrid rowLayoutPrefab;

    private List<GUIHorizontalGrid> rows;
    private GUIHorizontalGrid curRow;
    private int elementCount;
    private BaseUpgradeRow refType;
    private int quality;

    public GUIOnOffSwitch OpentBtn;
    public GUIScrollView ScrollView;
    public GUIInteractableIcon Element;
    public int ColumnNum;
    [Range(0f, 1f)]
    public float ElementSize;

    protected override void Awake()
    {
        base.Awake();
        OpentBtn.On += On;
        OpentBtn.Off += Off;
        OpentBtn.InteractableChange(true);
    }

    protected override void Start()
    {
        base.Start();
        EventListenersController.Instance.AddEmiter("S_TRAINING", S_TRAINING);
    }

    public override void Load(params object[] input)
    {

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
            ListUpgrade.ForbiddenGuard,
            ListUpgrade.TraninedSolider,
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
            e.InteractableChange(true);
            GUIProgressSlider slider = e.GetComponentInChildren<GUIProgressSlider>();
            e.OnClickEvents += delegate
            {
                if (OnElementChoose(types[capture]))
                {
                    EventListenersController.Instance.Emit("S_TRAINING");
                }
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

    //private void AddElement(ListUpgrade type)
    //{
    //    GUIInteractableIcon e = Instantiate(Element);
    //    curRow.Add(e.transform as RectTransform);
    //    e.Placeholder.text = type.ToString().InsertSpace();
    //    e.InteractableChange(true);
    //    e.OnClickEvents += delegate
    //    {
    //        OnElementChoose(type);
    //        tranningDataListner.Emit("S_TRANNING");
    //    };

    //    elementCount = (elementCount + 1) % ColumnNum;
    //    if (elementCount == 0)
    //    {
    //        curRow = Instantiate(rowLayoutPrefab, ScrollView.Content);
    //        curRow.ElementSize = ElementSize;
    //        rows.Add(curRow);
    //    }
    //}

    private bool OnElementChoose(ListUpgrade type)
    {
        try
        {
            refType = SyncData.BaseUpgrade[type];
            ITable table = DBReference.Instance[type];
            GenericUpgradeInfo info = JSONBase.FromJSON<GenericUpgradeInfo>(table[refType.Level - 1].ToJSON());

            if (SyncData.BaseInfo.Rows[0].IsEnoughtResource
                (info.FoodCost,
                info.WoodCost,
                info.StoneCost,
                info.MetalCost))
            {
                SyncData.BaseInfo.Rows[0].Farm -= info.FoodCost;
                SyncData.BaseInfo.Rows[0].Wood -= info.WoodCost;
                SyncData.BaseInfo.Rows[0].Stone -= info.StoneCost;
                SyncData.BaseInfo.Rows[0].Metal -= info.MetalCost;
            }
            else return false;

            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            return false;
        }
    }

    private void On(GUIOnOffSwitch onOff)
    {
        Open();
        onOff.Placeholder.text = "Off";
    }

    private void Off(GUIOnOffSwitch onOff)
    {
        Close();
        onOff.Placeholder.text = "On";
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
            {"Quality"      ,"1"},
        };
        JSONObject result = new JSONObject(data);
        Debug.Log(result.ToString());
        return result;
    }
}

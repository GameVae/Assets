using EnumCollect;
using System.Collections.Generic;
using UI.Widget;
using UnityEngine;

public class TranningWindow : BaseWindow
{
    [SerializeField] private GUIHorizontalGrid rowLayoutPrefab;

    private List<GUIHorizontalGrid> rows;
    private GUIHorizontalGrid curRow;
    private int elementCount;

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

        List<ListUpgrade> types = new List<ListUpgrade>();
        for (int i = 1; i < 48; i++)
        {
            types.Add((ListUpgrade)i);
        }
        AddElement(types);
    }

    private void AddElement(List<ListUpgrade> types)
    {
        List<RectTransform> rectList = new List<RectTransform>();
        for (int i = 0; i < types.Count; i++)
        {
            GUIInteractableIcon e = Instantiate(Element);
            rectList.Add(e.transform as RectTransform);
            e.Placeholder.text = types[i].ToString().InsertSpace();
            e.InteractableChange(true);

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

    private void AddElement(ListUpgrade type)
    {
        GUIInteractableIcon e = Instantiate(Element);
        curRow.Add(e.transform as RectTransform);
        e.Placeholder.text = type.ToString().InsertSpace();
        e.InteractableChange(true);

        elementCount = (elementCount + 1) % ColumnNum;
        if (elementCount == 0)
        {
            curRow = Instantiate(rowLayoutPrefab, ScrollView.Content);
            curRow.ElementSize = ElementSize;
            rows.Add(curRow);
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
}

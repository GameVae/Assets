using ManualTable.Row;
using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class UpgradeResearchWindow : MonoBehaviour, IWindow
{
    [Serializable]
    public struct MaterialElement
    {
        public GUIInteractableIcon Icon;
        public TextMeshProUGUI Current;
        public TextMeshProUGUI Need;
        public GUIInteractableIcon GetBtn;
    }

    private UpgradeResearchManager manager;

    public TextMeshProUGUI Title;

    [Header("Progess Bar"), Space]
    public GUIProgressSlider ProgressSlider;
    public GUIInteractableIcon InstantProgress;

    [Header("Requirement"), Space]
    public TextMeshProUGUI BuildingLevel;
    public TextMeshProUGUI ResearchLevel;

    [Header("Order Materials"), Space]
    [HideInInspector] public MaterialElement[] OrderMaterialElements;
    public Transform[] OrderMaterials;
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
        SetupOrderMateralElements();
    }

    private void Start()
    {
        LoadData(new object[] { 2 });
    }
    private void SetupOrderMateralElements()
    {
        int count = OrderMaterials.Length;
        OrderMaterialElements = new MaterialElement[count];

        for (int i = 0; i < count; i++)
        {
            OrderMaterialElements[i] = new MaterialElement()
            {
                Icon = OrderMaterials[i].GetChild(1).GetComponent<GUIInteractableIcon>(),
                Current = OrderMaterials[i].GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>(),
                Need = OrderMaterials[i].GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>(),
                GetBtn = OrderMaterials[i].GetChild(2).GetComponent<GUIInteractableIcon>()
            };
        }
    }

    public void LoadData(params object[] data)
    {
        int curLevel = (int)data[0];
        MainBaseRow row = manager.MainbaseData.rows.FirstOrDefault(x => x.Level == curLevel);
        if (row != null)
        {
            ProgressSlider.Value = row.Level;
            NumberName.text = "Might Bonus" +
                                "\nFood Cost" +
                                "\nWood Cost" +
                                "\nStone Cost" +
                                "\nMetal Cost";
            Amount.text = string.Format("{0} \n{1} \n{2} \n{3} \n{4}",
                row.MightBonus, row.FoodCost, row.WoodCost, row.StoneCost, row.MetalCost);
            DurationText.text = "Duration: " + row.TimeMin;
        }
    }
}

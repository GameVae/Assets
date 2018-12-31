using ManualTable.Row;
using Network.Sync;
using System.Linq;
using TMPro;
using UI.Widget;
using UnityEngine;

public class UpgradeResearchWindow : MonoBehaviour, IWindow
{
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
    }

    private void Start()
    {
    }


    /// <summary>
    /// 0: name - string
    /// 1: current level - int
    /// 2: current material - int[4]
    /// 3: need material - int[4]
    /// 4: might bonus - int
    /// 5: time min - string
    /// 6: time int - int
    /// 7: building require level - int
    /// 8: research require level - int
    /// </summary>
    /// <param name="data">Params object</param>
    public void Load(params object[] data)
    {
        string name = data.TryGet<string>(0);
        int curLevel = data.TryGet<int>(1);
        int[] curMaterials = data.TryGet<int[]>(2);
        int[] needMaterials = data.TryGet<int[]>(3);
        int mightBonus = data.TryGet<int>(4);
        string timeMin = data.TryGet<string>(5);
        int timeInt = data.TryGet<int>(6);
        int buildingRequire = data.TryGet<int>(7);
        int researchRequire = data.TryGet<int>(8);

        // 1
        Title.text = name;

        if (name != "Main Base")
            ActiveButtonGroup((curLevel < Sync.Instance.MainBaseLevel && 
                Sync.Instance.ResearchRemainTime == 0));
        else ActiveButtonGroup(Sync.Instance.ResearchRemainTime == 0);

        NumberName.text = "Might Bonus";
        Amount.text = string.Format("{0}", mightBonus);
        DurationText.text = "Duration: " + timeMin;

        ProgressSlider.Slider.MaxValue = timeInt;
        ProgressSlider.Slider.Value = (float)(timeInt - Sync.Instance.ResearchRemainTime);

        if (curMaterials != null)
        {
            for (int i = 0; i < 4; i++)
            {
                SetMaterialRequirement(i, curMaterials[i], needMaterials[i]);
            }
        }

        // test 
        int testLevel = 5;
        BuildingLevel.transform.parent.gameObject.SetActive(testLevel <= buildingRequire);
        BuildingLevel.text = buildingRequire.ToString();

        ResearchLevel.transform.parent.gameObject.SetActive(testLevel <= researchRequire);
        ResearchLevel.text = researchRequire.ToString();
    }

    private void SetMaterialRequirement(int index, int cur, int need)
    {
        GUIHorizontalInfo material = OrderMaterialElements[index];
        material.InteractableChange(cur < need);
        material.Placeholder.text = string.Format("{0}/{1}", cur, need);
    }

    private void ActiveButtonGroup(bool value)
    {
        InstantBtn.InteractableChange(value);
        LevelUpBtn.InteractableChange(value);
        ProgressSlider.Button.InteractableChange(!value);
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

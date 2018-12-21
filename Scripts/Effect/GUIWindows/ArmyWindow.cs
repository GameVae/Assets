using UnityEngine;
using UnityEngine.UI;

public class ArmyWindow : MonoBehaviour
{
    public enum ArmyType
    {
        Infantry = 0,
        Ranged = 1,
        Mounted = 2,
        SeigeEngine = 3
    }

    [System.Serializable]
    public class SwitchElement
    {
        public Slider LevelBar;
        public ManualAction Button;
        public Text Name;
    }

    private UpgradeResearchManager manager;
    private ManualAction active;
    [SerializeField]
    private SwitchElement[] switchElements;
    [SerializeField]
    private ManualAction[] toggles;

    [Header("Info Group")]
    public ManualAction UpgradeBtn;
    public Slider LevelBar;
    public Text IllusName;
    public Image IllusImage;

    [Header("Switch Group")]
    public Transform SwitchGroup;
    public Transform ToggleGroup;

    private void Awake()
    {
        manager = GetComponentInParent<UpgradeResearchManager>();
        SetUpSwitchGroup();
        SetUpToggleGroup();
    }

    private void Start()
    {
        // defautl
        Active(toggles[(int)ArmyType.Infantry]);
        SwitchInfo(manager.GetArmyData(ArmyType.Infantry));
    }

    private void SetUpSwitchGroup()
    {
        int size = 4;
        switchElements = new SwitchElement[size];
        Transform child;
        for (int i = 0; i < size; i++)
        {
            child = SwitchGroup.GetChild(i);
            switchElements[i] = new SwitchElement()
            {
                LevelBar = child.GetComponentInChildren<Slider>(),
                Button = child.GetComponentInChildren<ManualAction>(),
                Name = child.GetComponentInChildren<Text>(),
            };
        }
    }

    private void SetUpToggleGroup()
    {
        int size = 4;
        toggles = new ManualAction[size];
        for (int i = 0; i < size; i++)
        {
            toggles[i] = ToggleGroup.GetChild(i).GetComponent<ManualAction>();
        }
        toggles[(int)ArmyType.Infantry].ClickAction += delegate
        {
            if(Active(toggles[(int)ArmyType.Infantry]))
            {
                SwitchInfo(manager.GetArmyData(ArmyType.Infantry));
            }
        };
        toggles[(int)ArmyType.Ranged].ClickAction += delegate
        {
            if(Active(toggles[(int)ArmyType.Ranged]))
            {

            }
        };
        toggles[(int)ArmyType.Mounted].ClickAction += delegate
        {
            if(Active(toggles[(int)ArmyType.Mounted]))
            {

            }
        };
        toggles[(int)ArmyType.SeigeEngine].ClickAction += delegate
        {
            if(Active(toggles[(int)ArmyType.SeigeEngine]))
            {

            }
        };       
    }

    private bool Active(ManualAction button)
    {
        if (active == button) return false;
        Deactive();
        active = button;
        active.GetComponent<Image>().color = Color.cyan;
        return true;
    }

    private void Deactive()
    {
        if (active != null)
            active.GetComponent<Image>().color = Color.white;
    }

    private void SwitchInfo(UpgradeResearchManager.ArmyWindowData data)
    {
        for (int i = 0; i < data.ElementName.Length; i++)
        {
            switchElements[i].Name.text = data.ElementName[i];
        }
    }
}

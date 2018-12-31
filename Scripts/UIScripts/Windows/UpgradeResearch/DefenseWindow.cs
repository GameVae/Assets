using UnityEngine;
using UI.Widget;
using static UpgradeResearchManager;

public class DefenseWindow : MonoBehaviour, IWindow
{
    private UpgradeResearchManager manager;

    [Header("Constructs"), Space]
    public Transform[] Constructs;
    private ArmyWindow.Element[] constructElements;

    private void Awake()
    {
        manager = GetComponentInParent<UpgradeResearchManager>();
        SetupConstructElement();
    }

    private void SetupConstructElement()
    {
        int count = Constructs.Length;
        constructElements = new ArmyWindow.Element[count];
        for (int i = 0; i < count; i++)
        {
            int captureIndex = i;
            constructElements[i] = new ArmyWindow.Element()
            {
                Icon = Constructs[i].GetComponentInChildren<GUIInteractableIcon>(),
                LevelBar = Constructs[i].GetComponentInChildren<GUIProgressSlider>(),
            };
            constructElements[i].Icon.OnClickEvents +=
                delegate 
                {
                    manager.Open(Window.UpgradeResearch);
                    manager[Window.UpgradeResearch].Load(constructElements[captureIndex].Icon.Placeholder.text);
                };
        }
    }

    public void Load(params object[] input)
    {
        throw new System.NotImplementedException();
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

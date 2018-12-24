using ManualTable;
using UnityEngine;

public class UpgradeResearchManager : MonoBehaviour
{
    public MainBaseTable MainbaseData;

    public ArmyWindow ArmyWindow;
    public UpgradeResearchWindow UpgradeResearchWindow;

    public void Open()
    {
        gameObject.SetActive(true);
        UpgradeResearchWindow.gameObject.SetActive(true);
    }
}

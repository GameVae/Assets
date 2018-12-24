using System;
using TMPro;
using UnityEngine;

public class ArmyWindow : MonoBehaviour
{
    [Serializable]
    public struct Element
    {
        public GUIInteractableIcon Icon;
        public GUIProgressSlider LevelBar;
    }

    [Header("Toggle Group")]
    public GUIToggle Toggle;

    [Header("Illustration Group")]
    private TextMeshProUGUI typeName;
    private GUIInteractableIcon illusIcon;
    private GUIInteractableIcon upgradeIcon;
    private GUIProgressSlider levelBar;
    public Transform IllustrationGroup;

    [Header("Element Group")]
    private Element[] elements;

    public Transform[] OrderElements;

    private void Awake()
    {
        SetupIllustrationGroup();
        SetupOrderElements();
    }

    private void SetupIllustrationGroup()
    {
        typeName = IllustrationGroup.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        illusIcon = IllustrationGroup.GetChild(0).GetComponentInChildren<GUIInteractableIcon>();

        upgradeIcon = IllustrationGroup.GetChild(1).GetComponent<GUIInteractableIcon>();

        levelBar = IllustrationGroup.GetChild(2).GetComponent<GUIProgressSlider>();

    }

    private void SetupOrderElements()
    {
        int elementCount = OrderElements.Length;
        elements = new Element[elementCount];

        for (int i = 0; i < elementCount; i++)
        {
            elements[i] = new Element()
            {
                Icon = OrderElements[i].GetComponentInChildren<GUIInteractableIcon>(),
                LevelBar = OrderElements[i].GetComponentInChildren<GUIProgressSlider>(),
            };
        }
    }
}

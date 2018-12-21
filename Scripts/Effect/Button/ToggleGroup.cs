using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGroup : MonoBehaviour
{
    public ManualAction[] Toggles;

    private ManualAction active;

    private void Awake()
    {
        for (int i = 0; i < Toggles.Length; i++)
        {
            Toggles[i].ClickAction += delegate
            {
                active = Toggles[i];
            };
        }
    }
}
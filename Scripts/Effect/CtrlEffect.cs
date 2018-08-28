using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CtrlEffect : MonoBehaviour {
    public Button CtrlBtn;
    private bool effectBool;
    [SerializeField]
    private GameObject Shield;
    private void Awake()
    {
        Shield = transform.GetChild(1).gameObject;
        effectBool = true;
        CtrlBtn.onClick.AddListener(()=> click());
    }

    private void click()
    {
        effectBool = !effectBool;
        Shield.SetActive(effectBool);
    }
    
	
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour {
    public static LoadingUI instance;
    public Slider LoadingSlider;
    private Text loadingTxt;

    private void Awake()
    {
        loadingTxt = LoadingSlider.GetComponentInChildren<Text>();
        instance = this;
    }
    void Start () {
		
	}
    public void SetLoadingText(string content)
    {
        loadingTxt.text = content;
    }
    public void SetSliderValue (int value)
    {
        LoadingSlider.value = value;
    }
}

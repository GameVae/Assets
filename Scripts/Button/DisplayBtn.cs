using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumCollect;
public class DisplayBtn : MonoBehaviour {

    public ButtonDes buttonDes;
    public Button BtnVn;
    public Button BtnEnglish;

    private void Awake()
    {
        BtnVn.onClick.AddListener(() => {
            PlayerPrefs.SetInt("enumLanguage", (int)EnumLanguage.Vietnamese);
            buttonDes.ChangeLanguage();
        });
        BtnEnglish.onClick.AddListener(() => {
            PlayerPrefs.SetInt("enumLanguage", (int)EnumLanguage.English);
            buttonDes.ChangeLanguage();
            buttonDes.name = "here";
        });
    }
}

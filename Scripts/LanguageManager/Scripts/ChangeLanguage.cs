using UnityEngine;
using UnityEngine.UI;
using EnumCollect;

public class ChangeLanguage : MonoBehaviour
{
    private Text gameObjectTxt;

    public Language Language;
    [Header("Change Language Button")]
    public Button BtnVietnamese;
    public Button BtnEnglish;

    void Awake()
    {
        gameObjectTxt = GetComponent<Text>();
        gameObjectTxt.text = Language.ChangeLanguage();
        BtnVietnamese.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("enumLanguage", (int)EnumLanguage.Vietnamese);
            gameObjectTxt.text = Language.ChangeLanguage();
        });
        BtnEnglish.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("enumLanguage", (int)EnumLanguage.English);
            gameObjectTxt.text = Language.ChangeLanguage();
        });
    }

 
}

using UnityEngine;
using UnityEngine.UI;
using EnumCollect;


public class CombineChangeLanguage : MonoBehaviour
{
    private Text gameObjectTxt;
    private string content;
    public Language Language1;
    [Space]
    public Language Language2;
    [Header("Change Language Button")]
    public Button BtnVietnamese;
    public Button BtnEnglish;

    public string Content { get => content; set => content = value; }

    void Awake()
    {
        gameObjectTxt = GetComponent<Text>();
        SetContent();
        BtnVietnamese.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("enumLanguage", (int)EnumLanguage.Vietnamese);
            SetContent();
        });
        BtnEnglish.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("enumLanguage", (int)EnumLanguage.English);
            SetContent();
        });

    }
    public void SetContent (){
        gameObjectTxt.text = Language1.ChangeLanguage() + content + Language2.ChangeLanguage();
    }
    public void SetContent(string contentString)
    {
        Content = contentString;
        gameObjectTxt.text = Language1.ChangeLanguage() + content + Language2.ChangeLanguage();
    }
   
    
}

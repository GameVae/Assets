using UnityEngine;
using UnityEngine.UI;
using Assets.LoginEnums;

public class LanguageUI : MonoBehaviour
{
    [SerializeField]
    private MultiLangManager MultiLangManager;

    [Space]
    public Button EnglishBtn;
    public Button VietnameseBtn;

    private void Start()
    {
        setClickListener();
    }

    private void setClickListener()
    {
        EnglishBtn.onClick.AddListener(() => english());
        VietnameseBtn.onClick.AddListener(() => vietnamese());

    }

    private void english()
    {
        MultiLangManager.LangEnums = LangEnum.English;
        MultiLangManager.SetTextUI();


    }
    private void vietnamese()
    {
        MultiLangManager.LangEnums = LangEnum.Vietnamese;
        MultiLangManager.SetTextUI();

    }

}

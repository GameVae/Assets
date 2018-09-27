using UnityEngine;
using EnumCollect;

[CreateAssetMenu(fileName = "Language", menuName = "Language")]
public class Language : ScriptableObject {
    public new string Name;
    public string Vietnamese;
    public string English;

    public string ChangeLanguage()
    {
        string retString = "";
        EnumLanguage enumLanguage = PlayerPrefs.HasKey("enumLanguage") ? (EnumLanguage)PlayerPrefs.GetInt("enumLanguage") : EnumLanguage.Vietnamese;
        switch (enumLanguage)
        {
            case EnumLanguage.Vietnamese:
                retString = Vietnamese;
                break;
            case EnumLanguage.English:
                retString = English;
                break;
            default:
                break;
        }
        return retString;
    }
}
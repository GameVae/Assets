using UnityEngine;
using EnumCollect;

public class ButtonDes : ScriptableObject {
    public new string name;
    public string Vietnamese;
    public string English;

   

    public string ChangeLanguage()
    {
        string retString="";
        EnumLanguage enumLanguage = PlayerPrefs.HasKey("enumLanguage")? (EnumLanguage)PlayerPrefs.GetInt("enumLanguage"): EnumLanguage.Vietnamese;
        Debug.Log("enumLanguage: "+ enumLanguage);
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

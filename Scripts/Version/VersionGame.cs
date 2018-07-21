using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionGame : MonoBehaviour
{
    public static VersionGame instance;
    [SerializeField]
    private int versionInt;
    public GameObject WaitPanel;

    public int VersionInt
    {
        get
        {
            versionInt = 1;
            if (PlayerPrefs.HasKey("Version"))
            {
                versionInt = PlayerPrefs.GetInt("Version");
            }
            return versionInt;
        }
        
        set
        {
            
            if (PlayerPrefs.GetInt("Version") != value)
            {
                versionInt = value;
                PlayerPrefs.SetInt("Version", value);

                WaitPanel.SetActive(false);
            }            
        }       
    }

    void Awake()
    {
      //  instance = this;
        versionInt = VersionInt;
    }

}

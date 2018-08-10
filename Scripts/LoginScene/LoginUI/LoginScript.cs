using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

[Serializable]
public class LoginUI
{
    public InputField InputUser;
    public InputField InputPassword;
    public Toggle ToggleRememberAccount;
    public Button LoginBtn;
    public Dropdown ListServerDropdown;

}
public class LoginScript : MonoBehaviour {
   [SerializeField]
    private LoginUI loginUI;
    [SerializeField]
    private SocketIOComponent socketIO;

    private void Awake()
    {
        loginUI.LoginBtn.onClick.AddListener(()=> testLoginServer());
    }
    
    private void testLoginServer()
    {
        int serverValue = loginUI.ListServerDropdown.value;
        Debug.Log(serverValue);
        //send login with user 
        //Dictionary<string, string> data = new Dictionary<string, string>();


        //datacheck["idUnitInLocationsOnline"] = idOnl;
        //datacheck["UserNameOnline"] = usernameOnl;
        //datacheck["UnitOrderOnline"] = unitOrderOnl;

        //datacheck["idUnitInLocationsOffline"] = idOff;
        //datacheck["UserNameOffline"] = usernameOff;
        //datacheck["UnitOrderOffline"] = unitOderOff;

        //datacheck["idUnitInLocationsTemp"] = idTemp;
        //datacheck["UserNameTemp"] = usernameTemp;
        //datacheck["UnitOrderTemp"] = unitOderTemp;

        //socketIO.Emit("SENDPOSITIONFIGHTOFFLINEvsONLINE", new JSONObject(datacheck));
        //Debug.Log(new JSONObject(datacheck));
        
      //  socketIO.Emit();

    }
   
	
}

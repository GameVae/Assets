using Generic.Singleton;
using SocketIO;
using System.Collections.Generic;
using UnityEngine;

public class SIO_LoginListener : Listener
{
    private string userName;
    private string password;
    private bool isWaiting;

    public GameOnStarted GameOnStarted;

    public override void RegisterCallback()
    {
        On("R_LOGIN", R_LOGIN);
        AddEmiter("S_LOGIN", S_LOGIN);
    }

    public void Login(string UserName, string Password)
    {
        if (!isWaiting)
        {
            userName = UserName;
            password = Password;
            Emit("S_LOGIN");

            isWaiting = true;
        }
    }

    private void R_LOGIN(SocketIOEvent obj)
    {
        //Debug.Log("R_LOGIN: " + obj);
        int successBool = int.Parse(obj.data["LoginBool"].ToString());
        isWaiting = false;
        switch (successBool)
        {
            case 0:
                Debugger.Log("Login fail");
                Singleton.Instance<MessagePopup>().OpenMessage("Password or User was wrong !");
                break;
            case 1:
                Debugger.Log("Login success");
                GameOnStarted.LoginTask();
                break;
        }
    }

    public JSONObject S_LOGIN()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["UserName"] = userName;
        data["Password"] = md5String(password);
        data["Model_Device"] = SystemInfo.deviceModel;
        data["Ram_Device"] = SystemInfo.systemMemorySize.ToString();
        return new JSONObject(data);
    }

    #region Encrypt Password
    private string md5String(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
    #endregion

}

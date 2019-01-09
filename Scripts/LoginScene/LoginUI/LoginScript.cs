using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

[Serializable]
public class ConnectRegion
{
    public GameObject ConnectLoginRegion;
    [Space]
    public Button SignOutBtn;
    public Button LoginBtn;
}

[Serializable]
public class FirstConnect
{
    public GameObject FirstConnectRegion;
    [Space]
    public InputField InputUser;
    public InputField InputPassword;
    public Toggle ToggleRememberAccount;
    public Button LoginBtn;
    [Header("ForgotPass")]
    public Button ForgotPasswordBtn;
    public GameObject ForgotPanel;
    [Header("CreateAccount")]
    public Button CreateAccountBtn;
    public GameObject CreateAccountPanel;
}
public class LoginScript : MonoBehaviour
{
    public static LoginScript instances;
    [SerializeField]
    private FirstConnect firstConnect;
    [SerializeField]
    private ConnectRegion connectRegion;
    [Space]
    private SocketIOComponent socketIO;

    private void Awake()
    {
        if (instances == null) { instances = this; }
        else { Destroy(gameObject); }

        firstConnect.LoginBtn.onClick.AddListener(() => login());
        firstConnect.ForgotPasswordBtn.onClick.AddListener(() => firstConnect.ForgotPanel.SetActive(true));
        firstConnect.CreateAccountBtn.onClick.AddListener(() => firstConnect.CreateAccountPanel.SetActive(true));

        connectRegion.SignOutBtn.onClick.AddListener(() => signOutClick());
        connectRegion.LoginBtn.onClick.AddListener(() => loginClick());

        bool rememberUserBool = PlayerPrefs.HasKey("UserName");
        connectRegion.ConnectLoginRegion.SetActive(rememberUserBool);
        firstConnect.FirstConnectRegion.SetActive(!rememberUserBool);
    }

    private void Start()
    {
        socketIO = Connection.Instance.Socket;
        socketIO.On("R_LOGIN", R_LOGIN);

        socketIO.On("R_GET_RSS", GetRSSData.instance.R_GET_RSS);
        socketIO.On("R_BASE_INFO", GetRSSData.instance.R_BASE_INFO);
        socketIO.On("R_USER_INFO", GetRSSData.instance.R_USER_INFO);
        socketIO.On("R_GET_POSITION", GetRSSData.instance.R_GET_POSITION);
    }

    private void R_LOGIN(SocketIOEvent obj)
    {
        //Debug.Log("R_LOGIN: " + obj);
        int successBool = int.Parse(obj.data["LoginBool"].ToString());
        switch (successBool)
        {
            case 0:
                Debug.Log("Login fail");
                break;
            case 1:
                Debug.Log("Login success");
                break;
        }
    }

    private void login()
    {
        string UserName = firstConnect.InputUser.text.ToString();
        string Password = firstConnect.InputPassword.text.ToString();
        if (UserName.Length >= 6 && Password.Length >= 6)
        {
            S_LOGIN(UserName, Password);
            if (firstConnect.ToggleRememberAccount.isOn == true)
            {
                PlayerPrefs.SetString("UserName", UserName);
                PlayerPrefs.SetString("Password", Password);
            }
        }
    }

    private void signOutClick()
    {
        PlayerPrefs.DeleteKey("UserName");
        PlayerPrefs.DeleteKey("Password");
        connectRegion.ConnectLoginRegion.SetActive(false);
        firstConnect.FirstConnectRegion.SetActive(true);
    }
    private void loginClick()
    {
        string UserName = PlayerPrefs.GetString("UserName");
        string Password = PlayerPrefs.GetString("Password");
        S_LOGIN(UserName, Password);
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
    public void S_LOGIN(string UserName, string Password)
    {
        GameProgress.Instance.AddTask("get user info", CheckGetDataDone);
        GameProgress.Instance.AddTask("get base info", CheckGetDataDone);
        GameProgress.Instance.AddTask("get position", CheckGetDataDone);


        Dictionary<string, string> data = new Dictionary<string, string>();
        data["UserName"] = UserName;
        data["Password"] = md5String(Password);
        data["Model_Device"] = SystemInfo.deviceModel;
        data["Ram_Device"] = SystemInfo.systemMemorySize.ToString();
        socketIO.Emit("S_LOGIN", new JSONObject(data));

    }

    public void CheckGetDataDone()
    {
        if (GameProgress.Instance.IsEmpty)
        {
            LoadingUICtrl.Instance.Done(() =>
            {
                LoadingUICtrl.Instance.LoadScene(1);
            });
        }
        else
        {
            LoadingUICtrl.Instance.StartProgress(1);

        }
    }
}

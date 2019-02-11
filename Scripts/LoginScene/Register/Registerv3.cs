using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SocketIO;
using System;
using Generic.Singleton;
//[Serializable]
//public class RegisterUI
//{
//    public GameObject RegisterPanel;
//    [Space]
//    public InputField UserName;
//    public InputField Password;
//    public InputField PasswordConfirm;
//    public InputField Email;
//    [Space]
//    public Text WarningText;
//    [Header("Warning")]
//    public GameObject WarningUserName;
//    public GameObject WarningPassword;
//    public GameObject WarningPasswordConfirm;
//    public GameObject WarningEmail;
//    [Header("Button")]
//    public Button RegisterBtn;
//    public Button CloseBtn;

//    public void ClearInfo()
//    {
//        UserName.text = "";
//        Password.text = "";
//        PasswordConfirm.text = "";
//        Email.text = "";
//        WarningText.text = "";

//        WarningUserName.SetActive(false);
//        WarningPassword.SetActive(false);
//        WarningPasswordConfirm.SetActive(false);
//        WarningEmail.SetActive(false);
//    }
//}
[Serializable]
public class RegisterUI
{
    public GameObject RegisterPanel;
    [Space]
    public InputField UserName;
    public InputField Password;
    public InputField PasswordConfirm;
    public InputField Email;
    [Space]
    public Text WarningText;
    [Header("Warning")]
    public GameObject WarningUserName;
    public GameObject WarningPassword;
    public GameObject WarningPasswordConfirm;
    public GameObject WarningEmail;
    [Header("Button")]
    public Button RegisterBtn;
    public Button CloseBtn;

    public void ClearInfo()
    {
        UserName.text = "";
        Password.text = "";
        PasswordConfirm.text = "";
        Email.text = "";
        WarningText.text = "";

        WarningUserName.SetActive(false);
        WarningPassword.SetActive(false);
        WarningPasswordConfirm.SetActive(false);
        WarningEmail.SetActive(false);
    }
}
public class Registerv3 : MonoBehaviour
{
    [SerializeField]
    private RegisterUI registerUI;
    [Space]
    private bool checkUserName = false;
    private bool checkPassword = false;
    private bool checkEmail = false;

    string UserName;
    string Password;

    private SocketIOComponent socketIO;

    //public Connection Connection;

    private void Awake()
    {
        registerUI.UserName.onEndEdit.AddListener(delegate { checkUserNameInput(registerUI.UserName.text); });
        registerUI.PasswordConfirm.onEndEdit.AddListener(delegate { checkPasswordConfirmInput(registerUI.PasswordConfirm.text); });
        registerUI.Email.onEndEdit.AddListener(delegate { checkEmailInput(registerUI.Email.text); });

        socketIO = Singleton.Instance<Connection>().Socket;
    }

    private void Start()
    {

        registerUI.RegisterBtn.onClick.AddListener(() => setRegisterClick());
        registerUI.CloseBtn.onClick.AddListener(() => registerUI.ClearInfo());
        socketIO.On("R_REGISTER", R_REGISTER);
    }

    private void R_REGISTER(SocketIOEvent obj)
    {
        Debug.Log("R_REGISTER: " + obj.data);
        int successBool = int.Parse(obj.data["R_REGISTER"].ToString());
        Debug.Log("successBool: " + successBool);
        switch (successBool)
        {
            case 0:
                registerUI.RegisterBtn.interactable = true;
                StartCoroutine("showWarningText");
                break;
            case 1:
                //LoginScript.instances.S_LOGIN(UserName, Password);
                Debug.Log("Load user data to map scene");
                break;
        }
    }
    private IEnumerator showWarningText()
    {
        registerUI.WarningText.gameObject.SetActive(true);
        registerUI.WarningText.text = registerUI.WarningText.GetComponent<ChangeLanguage>().Language.ChangeLanguage();
        Debug.Log("Warning Txt: "+registerUI.WarningText.text);
        registerUI.RegisterBtn.interactable = true;
        yield return new WaitForSeconds(3);
        registerUI.WarningText.gameObject.SetActive(false);
    }

    private void checkUserNameInput(string input)
    {
        if (input.Length <= 5 || input.Length > 30)
        {
            checkUserName = false;
        }
        else
        {
            checkUserName = true;
        }
        registerUI.WarningUserName.SetActive(!checkUserName);
    }
    private void checkEmailInput(string input)
    {
        checkEmail = false;
        if (input.Length > 0 && input.Contains("@"))
        {
            checkEmail = true;
        }
        registerUI.WarningEmail.SetActive(!checkEmail);
    }
    private void checkPasswordInput(string input)
    {
        if (registerUI.PasswordConfirm.text.Length > 5 && registerUI.PasswordConfirm.text.Equals(input))
        {
            checkPassword = true;
        }
        registerUI.WarningPassword.SetActive(!checkPassword);
        if (registerUI.PasswordConfirm.text.Length == 0)
        {
            registerUI.WarningPassword.SetActive(false);
        }
    }
    private void checkPasswordConfirmInput(string input)
    {
        if (input.Length >= 6)
        {
            if (registerUI.Password.text.Equals(input))
            {
                checkPassword = true;
            }
            else
            {
                checkPassword = false;
            }
        }
        else
        {
            checkPassword = false;
        }
        registerUI.WarningPassword.SetActive(!checkPassword);
        registerUI.WarningPasswordConfirm.SetActive(!checkPassword);
    }

    private void setRegisterClick()
    {
        if (checkUserAccount() == true)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["UserName"] = registerUI.UserName.text;
            data["Password"] = md5String(registerUI.Password.text);
            data["Email"] = registerUI.Email.text;
            UserName = registerUI.UserName.text;
            Password = md5String(registerUI.Password.text);
            registerUI.RegisterBtn.interactable = false;
            S_REGISTER(data);
        }
        registerUI.RegisterBtn.interactable = !checkUserAccount();
    }

    public void S_REGISTER(Dictionary<string,string> data)
    {
        socketIO.Emit("S_REGISTER", new JSONObject(data));

    }

    private bool checkUserAccount()
    {
        bool checkBool = false;
        if (checkUserName == true && checkPassword == true && checkEmail == true)
        {
            checkBool = true;
        }
        registerUI.WarningUserName.SetActive(!checkUserName);
        registerUI.WarningPassword.SetActive(!checkPassword);
        registerUI.WarningEmail.SetActive(!checkEmail);

        return checkBool;
    }

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
}

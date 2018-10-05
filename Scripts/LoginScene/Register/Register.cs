using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SocketIO;
using System;
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

        RegisterPanel.SetActive(false);
    }
}

public class Register : MonoBehaviour
{
    public SocketIOComponent SocketIO;
    public TextLoginRegion TextLoginRegion;

    [SerializeField]
    private RegisterUI registerUI;
    [Space]
    [SerializeField]
    private MultiLangManager multiLangManager;
    [Space]
    private bool checkUserName = false;
    private bool checkPassword = false;
    private bool checkEmail = false;

    private void Awake()
    {
        registerUI.UserName.onEndEdit.AddListener(delegate { checkUserNameInput(registerUI.UserName.text); });
        registerUI.PasswordConfirm.onEndEdit.AddListener(delegate { checkPasswordConfirmInput(registerUI.PasswordConfirm.text); });
        registerUI.Email.onEndEdit.AddListener(delegate { checkEmailInput(registerUI.Email.text); });

        registerUI.RegisterBtn.onClick.AddListener(() => setRegisterClick());
        registerUI.CloseBtn.onClick.AddListener(() => registerUI.ClearInfo());
    }

    private void Start()
    {
        SocketIO.On("R_REGISTER", R_REGISTER);
    }
    private void R_REGISTER(SocketIOEvent obj)
    {
        Debug.Log("R_REGISTER: " + obj.data);
        int successBool = int.Parse(obj.data["Message"].ToString());
        Debug.Log("successBool: " + successBool);
        switch (successBool)
        {
            case 0:
                StartCoroutine("showWarningText", multiLangManager.GetString(Assets.LoginStringEnums.LoginLangEnum.UsernameOrEmailExisted));
                break;
            case 1:
                Debug.Log("Load user data to map scene");
                break;
        }
    }
    private IEnumerator showWarningText(string stringContent)
    {
        registerUI.WarningText.text = stringContent;
        registerUI.RegisterBtn.interactable = true;
        yield return new WaitForSeconds(3);
        registerUI.WarningText.text = "";
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
            data["UserName"] = TextLoginRegion.UserName_RegisterRegion.text;
            data["Password"] = md5String(TextLoginRegion.Password_RegisterRegion.text);
            data["Email"] = TextLoginRegion.Email_RegisterRegion.text;
            SocketIO.Emit("S_REGISTER", new JSONObject(data));
        }
        registerUI.RegisterBtn.interactable = !checkUserAccount();
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

    //   public InputField username;
    //   public InputField password;
    //   public InputField passwordconfirm;
    //   public InputField email;
    //   public Text text;
    //   public SocketIOComponent socketIO;
    //   private string MessLogin = " ";

    //   // Use this for initialization
    //   public GameObject iconfalseUser, iconfalsePass, iconfalsePassconfirm, iconfalseEmail;
    //   private bool checkuser, checkpassword, checkemail ;

    //   public Text PlaceholderUser, PlaceholderPassword, PlaceholderPassConfirm
    //               , PlaceholderEmail, PlaceholderbtnRegister;
    //   public static Register instance;
    //void Awake(){
    //	instance = this;	
    //}
    //   void Start () {

    //      // StartCoroutine(CalltoServer());
    //      // socketIO.On("USER_CONNECTED", OnUserConnected);
    //       //socketIO.On("REGISTER", ORegisted);
    //socketIO.On("R_REGISTER_SUCCESS", R_REGISTER_SUCCESS);  
    //       //socketIO.On("RegisterUnsuccess", RegisterUnsuccess);

    //   }

    //   //private void ORegisted(SocketIOEvent obj)
    //   //{
    //   //    Debug.Log("Send message to the server" + obj.data+ "ORegisted");
    //   //}

    //   void R_REGISTER_SUCCESS(SocketIOEvent e)
    //   {
    //     //  text.text = "Bạn đã đăng kí thành công";
    //       MessLogin = e.data[0].ToString();
    //       Login login = new Login();
    //       if (login.jsontoString(MessLogin, "\"") == "0")
    //       {
    //          // text.text = "Username hoặc email đăng kí đã tồn tại";
    //           text.text = XmlLoader.instance.node13;
    //       }
    //       else if (login.jsontoString(MessLogin, "\"") == "2")
    //       {
    //           //text.text = "Bạn đã đăng kí thành công";
    //           text.text = XmlLoader.instance.node14;
    //       }
    //       else {
    //           //text.text = "Đăng kí bị lỗi. Vui lòng thử lại";
    //           text.text = XmlLoader.instance.node15;
    //       }
    //   }
    //   private void OnUserConnected(SocketIOEvent obj)
    //   {

    //       Debug.Log("Send message to the server"+obj.data+ "OnUserConnected");
    //   }

    //  /* IEnumerator CalltoServer()
    //   {
    //       yield return new WaitForSeconds(0.5f);
    //       socketIO.Emit("USER_CONNECT");       
    //   }*/

    //   public void ButtonLogin()
    //   {
    //       // Debug.Log(Md5Sum(password.text).ToString());
    //       CompareUser(username.text);

    //       ComparePass(password.text, passwordconfirm.text);

    //       LoginSound.instance.SoundUI(LoginSound.instance.LoginClip.RegisterClip,SoundManager.instance.CurrentUIVolume,SoundManager.instance.UiSoundStatus);
    //       if (IsValidEmailAddress(email.text)){
    //                   checkemail = true;
    //                   iconfalseEmail.SetActive(false);                   
    //               }
    //               else{
    //                   checkemail = false;
    //                   iconfalseEmail.SetActive(true);                   
    //               }                
    //        if(checkuser && checkpassword && checkemail){          
    //           Dictionary<string, string> data = new Dictionary<string, string>();
    //           data["name"] = username.text;
    //           data["password"] = Md5Sum(password.text).ToString();
    //           data["email"] = email.text;            
    //           socketIO.Emit("REGISTER", new JSONObject(data));           
    //       }
    //       else{
    //           //text.text = "Nhập đầy đủ thông tin";
    //           text.text = XmlLoader.instance.node16;
    //       }

    //   }






    //   private void CompareUser(string user)
    //   {
    //       //string[] fail = { " ", "`", "~", "!", "@", "#" };
    //       //for (int i = 0; i < user.Length; i++)
    //       //{

    //       //    for (int j = 0; j < fail.Length; j++)
    //       //    {
    //       //        if (user[i].ToString() == fail[j].ToString())
    //       //        {
    //       //            Debug.Log("F U");
    //       //        }
    //       //    }
    //       //}
    //       if(user.Length <= 6 || user.Length >= 50)
    //       {
    //           checkuser = false;
    //           iconfalseUser.SetActive(true);

    //       }
    //       else
    //       {
    //           checkuser = true;
    //           iconfalseUser.SetActive(false);
    //       }


    //   }
    //   public void StringUser()
    //   {
    //       if(username.text.Length <= 0)
    //       {
    //           //text.text = "tài khoản còn trống";
    //           text.text = XmlLoader.instance.node8;
    //       }
    //       else
    //       {
    //           //text.text = "tài khoản từ 7 - 50 kí tự";
    //           text.text = XmlLoader.instance.node17;
    //       }

    //   }

    //   public void StringPassword()
    //   {
    //       if (password.text.Length <= 0)
    //       {
    //           //text.text = "mật khẩu còn trống";
    //           text.text = XmlLoader.instance.node9;
    //       }
    //       else
    //       {
    //           //text.text = "mật khẩu từ 6 - 50 kí tự";
    //           text.text = XmlLoader.instance.node18;
    //       }

    //   }

    //   public void StringPassconfirm()
    //   {
    //       if (passwordconfirm.text.Length <= 0)
    //       {
    //           //text.text = "mật khẩu xác nhận còn trống";
    //           text.text = XmlLoader.instance.node10;
    //       }
    //       else
    //       {
    //           //text.text = "xác nhận mật khẩu ko khớp";
    //           text.text = XmlLoader.instance.node19;
    //       }

    //   }

    //   public void StringEmail()
    //   {
    //       if (email.text.Length <= 0)
    //       {
    //           //text.text = "email còn trống";
    //           text.text = XmlLoader.instance.node11;
    //       }
    //       else
    //       {
    //           //text.text = "Email không hợp lệ";
    //           text.text = XmlLoader.instance.node20;
    //       }

    //   }
    //   private void ComparePass(string pass,string passconfirm)
    //   {
    //       if (pass.Length <= 5 || pass.Length >= 50 )
    //       {
    //           checkpassword = false;
    //           iconfalsePass.SetActive(true);


    //       }
    //       else
    //       {
    //           iconfalsePass.SetActive(false);
    //           if (pass == passconfirm)
    //           {
    //               checkpassword = true;
    //               iconfalsePassconfirm.SetActive(false);
    //           }
    //           else
    //           {
    //               checkpassword = false;
    //               iconfalsePassconfirm.SetActive(true);

    //           }
    //       }

    //       if (passconfirm.Length <= 5 || passconfirm.Length >= 50)
    //       {
    //           checkpassword = false;
    //           iconfalsePassconfirm.SetActive(true);

    //       }
    //   }


    //   public bool IsValidEmailAddress(string s)
    //   {
    //       var regex = new System.Text.RegularExpressions.Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
    //               @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");
    //       return regex.IsMatch(s);
    //   }


}

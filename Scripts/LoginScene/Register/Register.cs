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
    public InputField UserName;
    public InputField Password;
    public InputField PasswordConfirm;
    public InputField Email;
    public Text WarningText;
    [Header("Warning")]
    public GameObject WarningUserName;
    public GameObject WarningPassword;
    public GameObject WarningPasswordConfirm;
    public GameObject WarningEmail;
}

public class Register : MonoBehaviour
{
    public SocketIOComponent SocketIO;
    public TextLoginRegion TextLoginRegion;
    [SerializeField]
    private Button RegisterBtn;
    private RegisterUI registerUI;
    [Space]
    private bool checkUserName = false;
    private bool checkPassword = false;
    private bool checkEmail = false;

    private void Awake()
    {
        RegisterBtn.onClick.AddListener(() => setRegisterClick());
        registerUI.UserName.onEndEdit.AddListener(delegate { checkUserNameInput(registerUI.UserName.text); });
        registerUI.PasswordConfirm.onEndEdit.AddListener(delegate { checkPasswordInput(registerUI.Password.text); });
        registerUI.Email.onEndEdit.AddListener(delegate { checkEmailInput(registerUI.Email.text); });

    }
    private void Start()
    {      
        SocketIO.On("R_REGISTER_UNSUCCESS", R_REGISTER_UNSUCCESS);
        SocketIO.On("R_REGISTER_SUCCESS", R_REGISTER_SUCCESS);
    }
    private void R_REGISTER_UNSUCCESS(SocketIOEvent obj)
    {
        Debug.Log("R_REGISTER_UNSUCCESS: "+obj.data);

    }
    private void R_REGISTER_SUCCESS(SocketIOEvent obj)
    {
        Debug.Log("R_REGISTER_UNSUCCESS: " + obj.data);

    }
    private void checkUserNameInput(string input)
    {
        if (input.Length<=6||input.Length>50)
        {
            checkUserName = false;
            registerUI.WarningUserName.SetActive(true);
        }
        else
        {
            checkUserName = true;
            registerUI.WarningUserName.SetActive(false);
        }
    }
    private void checkEmailInput(string input)
    {
        if (input.Length==0)
        {
            checkEmail = false;
            registerUI.WarningEmail.SetActive(true);
        }
        else
        {
            checkEmail = true;
            registerUI.WarningEmail.SetActive(false);
        }
    }
    private void checkPasswordInput(string input)
    {
        if (input.Length<=5)
        {
            if (registerUI.PasswordConfirm.text.Equals(input))
            {
                checkPassword = true;
                registerUI.WarningPassword.SetActive(false);
                registerUI.WarningPasswordConfirm.SetActive(false);
            }
            else
            {
                checkPassword = false;
                registerUI.WarningPasswordConfirm.SetActive(true);
            }
        }
        else
        {
            checkPassword = false;
            registerUI.WarningPassword.SetActive(true);

        }
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
        RegisterBtn.interactable = false;
    }
    private bool checkUserAccount()
    {
        bool checkBool = false;
        if (checkUserName == true && checkPassword == true && checkEmail == true)
        {
            checkBool = true;
        }
        return checkBool;
    }
    private bool checkCondition()
    {
        bool checkBool = false;

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

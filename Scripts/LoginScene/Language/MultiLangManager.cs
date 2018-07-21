using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.LoginEnums;
using Assets.LoginStringEnums;

public class MultiLangManager : MonoBehaviour {
    [SerializeField]
    private TextLoginRegion TextLoginRegions;
    [SerializeField]
    private LangEnum langEnums;
    [Space]
    private string[] engLang = new string[(int)(LoginLangEnum.Z_FinalItem + 1)];
    private string[] vieLang = new string[(int)(LoginLangEnum.Z_FinalItem + 1)];


    public LangEnum LangEnums
    {
        get
        {
            return langEnums;
        }

        set
        {
            switch (value)
            {
                case LangEnum.English:
                    PlayerPrefs.SetInt("Language", (int)LangEnum.English);
                    break;
                case LangEnum.Vietnamese:
                    PlayerPrefs.SetInt("Language", (int)LangEnum.Vietnamese);
                    break;
                default:
                    break;
            }
            langEnums = value;
        }
    }

    void Awake()
    {
        if (PlayerPrefs.HasKey("Language"))
        {
            LangEnums = (LangEnum)PlayerPrefs.GetInt("Language");
        }
        else
        {
            LangEnums = LangEnum.English;
            PlayerPrefs.SetInt("Language", (int)LangEnum.English);
        }
        setVieString();
        setEngString();

    }


    private string getString(LoginLangEnum loginLangEnums)
    {
        string stringRet = returnLangSelect(langEnums, loginLangEnums);
        return stringRet;
    }

    private string returnLangSelect(LangEnum langEnums, LoginLangEnum loginLangEnum)
    {
        string retString = "";
        switch (langEnums)
        {
            case LangEnum.English:
                retString = engLang[(int)loginLangEnum];
                break;
            case LangEnum.Vietnamese:
                retString = vieLang[(int)loginLangEnum];
                break;
        }
        return retString;
    }

    private void setEngString()
    {
        engLang[(int)LoginLangEnum.AccountName] = "Account Name";
        engLang[(int)LoginLangEnum.AccountNameChangePass] = "Account Name";
        engLang[(int)LoginLangEnum.AccountNameForgotPass] = "Account Name";
        engLang[(int)LoginLangEnum.Change] = "Change";
        engLang[(int)LoginLangEnum.Cancel] = "Cancel";
        engLang[(int)LoginLangEnum.ChangePassSuccess] = "Change Password Success";
        engLang[(int)LoginLangEnum.CheckMail] = "Please Check Your Email To Receive Code";
        engLang[(int)LoginLangEnum.CloseExplain] = "Auto Close After 5 Seconds";
        engLang[(int)LoginLangEnum.CodeChangePass] = "Code Change Pass";
        engLang[(int)LoginLangEnum.Content] = "Content";
        engLang[(int)LoginLangEnum.CreateAccount] = "Create" + "\n" + "Account";
        engLang[(int)LoginLangEnum.Email] = "Email";
        engLang[(int)LoginLangEnum.EmailNotMatch] = "Email Not Match";
        engLang[(int)LoginLangEnum.EmailSupport] = "Email Support: @demanvi.com";
        engLang[(int)LoginLangEnum.EmptyAccount] = "Account Name Need To Fill";
        engLang[(int)LoginLangEnum.EmptyPassword] = "Password Need To Fill";
        engLang[(int)LoginLangEnum.English] = "English";
        engLang[(int)LoginLangEnum.EnterContent] = "Enter Content…";
        engLang[(int)LoginLangEnum.ErrorRegistered] = "Error Registered";
        engLang[(int)LoginLangEnum.Exit] = "Exit";
        engLang[(int)LoginLangEnum.FillInfomation] = "Please Fill Full Information";
        engLang[(int)LoginLangEnum.FinishedChangePass] = "Finished Change Password";
        engLang[(int)LoginLangEnum.ForgotPassword] = "Forgot" + "\n" + "Password";
        engLang[(int)LoginLangEnum.Language] = "Language: ";
        engLang[(int)LoginLangEnum.Loading] = "Loading";
        engLang[(int)LoginLangEnum.Login] = "Login";
        engLang[(int)LoginLangEnum.NotConnectServer] = "Not Connected Server, Please Try Again";
        engLang[(int)LoginLangEnum.Number] = "Number";
        engLang[(int)LoginLangEnum.Password] = "Password";
        engLang[(int)LoginLangEnum.PasswordChangePass] = "New Password";
        engLang[(int)LoginLangEnum.PasswordConfirmPass] = "Confirm Password";
        engLang[(int)LoginLangEnum.RememberAccount] = "Remember Account";
        engLang[(int)LoginLangEnum.Resend] = "Resend To Get Code";
        engLang[(int)LoginLangEnum.ResendForgetPassword] = "Resend Request Code";
        engLang[(int)LoginLangEnum.Send] = "Send";
        engLang[(int)LoginLangEnum.SendSupport] = "Send";
        engLang[(int)LoginLangEnum.ServerReset] = "Server is maintaining," + "/n" + "Please come back after:";
        engLang[(int)LoginLangEnum.SuccessfullyRegisted] = "Successfully Registed";
        engLang[(int)LoginLangEnum.SignOut] = "Sign Out";
        engLang[(int)LoginLangEnum.Support] = "Support";
        engLang[(int)LoginLangEnum.ThankYouSendSupport] = "Thank You For Sending Support";
        engLang[(int)LoginLangEnum.ToDemoGame] = "To Demo Game.";
        engLang[(int)LoginLangEnum.Username] = "User 's name";
        engLang[(int)LoginLangEnum.Vietnamese] = "Vietnamese";
        engLang[(int)LoginLangEnum.UsernameOrEmailExisted] = "Username or Email Existed";
        engLang[(int)LoginLangEnum.Warning] = "Warning";
        engLang[(int)LoginLangEnum.Welcome] = "Welcome ";
        engLang[(int)LoginLangEnum.WrongAccPass] = "Wrong Account or Password";
        engLang[(int)LoginLangEnum.WrongCode] = "Wrong Reset Code";
        engLang[(int)LoginLangEnum.YourCaseNumber] = "Your Case's Number:";
        engLang[(int)LoginLangEnum.Z_FinalItem] = "Z";
    }

    private void setVieString()
    {
        vieLang[(int)LoginLangEnum.AccountName] = "Tên Tài Khoản";
        vieLang[(int)LoginLangEnum.AccountNameChangePass] = "Tên Tài Khoản";
        vieLang[(int)LoginLangEnum.AccountNameForgotPass] = "Tên Tài Khoản";
        vieLang[(int)LoginLangEnum.Change] = "Thay đổi";
        vieLang[(int)LoginLangEnum.Cancel] = "Hủy";
        vieLang[(int)LoginLangEnum.ChangePassSuccess] = "Thay đổi Mật Khẩu Thành Công";
        vieLang[(int)LoginLangEnum.CheckMail] = "Hãy Kiểm Tra Email Của Bạn Để Nhận Code";
        vieLang[(int)LoginLangEnum.CloseExplain] = "Tự động đóng sau 5 giây";
        vieLang[(int)LoginLangEnum.CodeChangePass] = "Code Đổi Mật Khẩu";
        vieLang[(int)LoginLangEnum.Content] = "Nội Dung";
        vieLang[(int)LoginLangEnum.CreateAccount] = "Tạo Tài Khoản";
        vieLang[(int)LoginLangEnum.Email] = "Email";
        vieLang[(int)LoginLangEnum.EmailNotMatch] = "Email Không Phù Hợp";
        vieLang[(int)LoginLangEnum.EmailSupport] = "Email Hỗ trợ: @demanvi.com";
        vieLang[(int)LoginLangEnum.EmptyAccount] = "Tài Khoản Cần Điền Thông Tin";
        vieLang[(int)LoginLangEnum.EmptyPassword] = "Mật Khẩu Cần Điền Thông Tin";
        vieLang[(int)LoginLangEnum.English] = "Tiếng Anh";
        vieLang[(int)LoginLangEnum.EnterContent] = "Nhập Nội Dung...";
        vieLang[(int)LoginLangEnum.ErrorRegistered] = "Đăng Ký Thất Bại. Vui Lòng Thử Lại ";
        vieLang[(int)LoginLangEnum.Exit] = "Thoát";
        vieLang[(int)LoginLangEnum.FillInfomation] = " Vui Lòng Điền Đầy Đủ Thông Tin";
        vieLang[(int)LoginLangEnum.FinishedChangePass] = "Hoàn Tất Thay Đổi Mật Khẩu";
        vieLang[(int)LoginLangEnum.ForgotPassword] = "Quên Mật Khẩu";
        vieLang[(int)LoginLangEnum.Language] = "Ngôn ngữ: ";
        vieLang[(int)LoginLangEnum.Loading] = "Đang Tải";
        vieLang[(int)LoginLangEnum.Login] = "Đăng Nhập";
        vieLang[(int)LoginLangEnum.NotConnectServer] = "Không Có Kết Nối Với Server, Xin Vui Lòng Thử Lại";
        vieLang[(int)LoginLangEnum.Number] = "Number";
        vieLang[(int)LoginLangEnum.Password] = "Mật Khẩu";
        vieLang[(int)LoginLangEnum.PasswordChangePass] = "Mật Khẩu Mới";
        vieLang[(int)LoginLangEnum.PasswordConfirmPass] = "Nhập Lại Mật Khẩu";
        vieLang[(int)LoginLangEnum.RememberAccount] = "Nhớ Tài Khoản Đăng Nhập";
        vieLang[(int)LoginLangEnum.Resend] = "Gửi Lại Để Lấy Code";
        vieLang[(int)LoginLangEnum.ResendForgetPassword] = "Đã Gửi Lấy Code Tạo Lại Mật Khẩu";
        vieLang[(int)LoginLangEnum.Send] = "Gửi";
        vieLang[(int)LoginLangEnum.SendSupport] = "Gửi";
        vieLang[(int)LoginLangEnum.ServerReset] = "Server Đang Bảo Trì" + "/n" + "Vui lòng quay lại sau:";
        vieLang[(int)LoginLangEnum.SuccessfullyRegisted] = "Đăng Ký Thành Công";
        vieLang[(int)LoginLangEnum.SignOut] = "Đăng Xuất ";
        vieLang[(int)LoginLangEnum.Support] = "Hỗ Trợ";
        vieLang[(int)LoginLangEnum.ThankYouSendSupport] = "Cảm Ơn Bạn Đã Gửi Đóng Góp";
        vieLang[(int)LoginLangEnum.ToDemoGame] = "Đến Với Demo Game.";
        vieLang[(int)LoginLangEnum.Username] = "Tên Người Dùng";
        vieLang[(int)LoginLangEnum.Vietnamese] = "Tiếng Việt";
        vieLang[(int)LoginLangEnum.UsernameOrEmailExisted] = "Tên Tài Khoản Hoặc Email đã tồn tại";
        vieLang[(int)LoginLangEnum.Warning] = "Thông Báo";
        vieLang[(int)LoginLangEnum.Welcome] = "Chào mừng ";
        vieLang[(int)LoginLangEnum.WrongAccPass] = "Tài Khoản hoặc Mật Khẩu Chưa Đúng";
        vieLang[(int)LoginLangEnum.WrongCode] = "Code Đổi Mật Khẩu Chưa Đúng";

        vieLang[(int)LoginLangEnum.YourCaseNumber] = "Trường Hợp Của Bạn Là Số:";
        vieLang[(int)LoginLangEnum.Z_FinalItem] = "Z";
    }

    public void SetTextUI()
    {
        //TextLoginRegions.User_FirstConnect.text = getString(LoginLangEnum.AccountName);
        //TextLoginRegions.Password_FirstConnect.text = getString(LoginLangEnum.Password);
        //TextLoginRegions.RememberAcc_FirstConnect.text = getString(LoginLangEnum.RememberAccount);
        //TextLoginRegions.LoginBtn_FirstConnect.text = getString(LoginLangEnum.Login);
        //TextLoginRegions.CreateAccount_FirstConnect.text = getString(LoginLangEnum.CreateAccount);
        //TextLoginRegions.ForgotPassBtn_FirstConnect.text = getString(LoginLangEnum.ForgotPassword);
        //TextLoginRegions.LoginBtn_ConnectRegion.text = getString(LoginLangEnum.Login);
        //TextLoginRegions.SignOutBtn_ConnectRegion.text = getString(LoginLangEnum.SignOut);
        //TextLoginRegions.User_RegisterRegion.text = getString(LoginLangEnum.AccountName);
        //TextLoginRegions.Password_RegisterRegion.text = getString(LoginLangEnum.Password);
        //TextLoginRegions.PassConfirm_RegisterRegion.text = getString(LoginLangEnum.PasswordConfirmPass);
        //TextLoginRegions.Email_RegisterRegion.text = getString(LoginLangEnum.Email);
        //TextLoginRegions.RegisterBtn_RegisterRegion.text = getString(LoginLangEnum.CreateAccount);
        //TextLoginRegions.User_SendMail.text = getString(LoginLangEnum.AccountName);
        //TextLoginRegions.Email_SendMail.text = getString(LoginLangEnum.Email);
        //TextLoginRegions.SendBtn_SendMail.text = getString(LoginLangEnum.Send);
        //TextLoginRegions.Code_ChangePass.text = getString(LoginLangEnum.CodeChangePass);
        //TextLoginRegions.Password_ChangePass.text = getString(LoginLangEnum.Password);
        //TextLoginRegions.PassConfirm_ChangePass.text = getString(LoginLangEnum.PasswordConfirmPass);
        //TextLoginRegions.ChangeBtn_ChangePass.text = getString(LoginLangEnum.Change);
        //TextLoginRegions.ResendBtn_ChangePass.text = getString(LoginLangEnum.Resend);
        //TextLoginRegions.SupportBtn.text = getString(LoginLangEnum.Support);
        //TextLoginRegions.Username_SupportPanel.text = getString(LoginLangEnum.Username) + ": " + PlayerPrefs.GetString("UserName");
        //TextLoginRegions.InputContent_SupportPanel.text = getString(LoginLangEnum.EnterContent);
        //TextLoginRegions.SendBtn_SupportPanel.text = getString(LoginLangEnum.Send);
        //TextLoginRegions.ThankSupportTxt_SuccessSendPanel.text = getString(LoginLangEnum.ThankYouSendSupport);
        //TextLoginRegions.TitleTxt_SuccessSendPanel.text = getString(LoginLangEnum.YourCaseNumber);
        //TextLoginRegions.CloseExplain_SuccessSendPanel.text = getString(LoginLangEnum.CloseExplain);
        //TextLoginRegions.ServerTimeOutTxt.text = getString(LoginLangEnum.ServerReset);
        //TextLoginRegions.LoadingTxt_LoadingPanel.text = getString(LoginLangEnum.Loading);
        //TextLoginRegions.ExitBtn_ExitPanel.text = getString(LoginLangEnum.Exit);
        //TextLoginRegions.CancelBtn_ExitPanel.text = getString(LoginLangEnum.Cancel);
        //TextLoginRegions.Text_DisconnectPanel.text = getString(LoginLangEnum.NotConnectServer);
        TextLoginRegions.LanguageTxt.text = getString(LoginLangEnum.Language);
    }


}

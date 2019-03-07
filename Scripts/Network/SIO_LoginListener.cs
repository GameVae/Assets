using Generic.Singleton;
using SocketIO;
using System.Collections.Generic;
using UnityEngine;

public class SIO_LoginListener : Listener
{
    private string userName;
    private string password;

    private LoadingPanel gameTask;

    protected override void Start()
    {
        base.Start();
        gameTask = Singleton.Instance<LoadingPanel>();
    }

    public override void RegisterCallback()
    {
        On("R_LOGIN", R_LOGIN);
        On("R_BASE_INFO", delegate { getBaseInfoDone = true; });
        On("R_USER_INFO", delegate { getUserInfoDone = true; });
        On("R_GET_POSITION", delegate { getPositionDone = true; });


        AddEmiter("S_LOGIN", S_LOGIN);
    }

    public void Login(string UserName, string Password)
    {
        userName = UserName;
        password = Password;
        Emit("S_LOGIN");
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

    public JSONObject S_LOGIN()
    {

        AddProgress();

        Dictionary<string, string> data = new Dictionary<string, string>();
        data["UserName"] = userName;
        data["Password"] = md5String(password);
        data["Model_Device"] = SystemInfo.deviceModel;
        data["Ram_Device"] = SystemInfo.systemMemorySize.ToString();
        //socketIO.Emit("S_LOGIN", new JSONObject(data));
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


    private bool getUserInfoDone = false;
    private bool getBaseInfoDone = false;
    private bool getPositionDone = false;

    private void AddProgress()
    {
        GameProgress.Task task1 = new GameProgress.Task()
        {
            Name = "get user info",
            GetProgress = delegate { return 1; },
            IsDone = delegate { return getUserInfoDone; },
            Start = null,
            Title = "Getting user info ..."
        };
        GameProgress.Task task2 = new GameProgress.Task()
        {
            Name = "get base info",
            GetProgress = delegate { return 1; },
            IsDone = delegate { return getBaseInfoDone; },
            Start = null,
            Title = "Getting base info ..."
        };
        GameProgress.Task task3 = new GameProgress.Task()
        {
            Name = "get position",
            GetProgress = delegate { return 1; },
            IsDone = delegate { return getPositionDone; },
            Start = null,
            Title = "Getting rss position ..."
        };

        GameProgress prog = new GameProgress(
            doneAct: GetDataProgressDoneAct,
            t: new GameProgress.Task[] { task1, task2,/* task3*/ });

        gameTask.AddTask(prog);

    }

    private void GetDataProgressDoneAct()
    {
        gameTask.LoadScene(1);
    }
}

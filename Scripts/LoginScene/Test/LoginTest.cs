using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginTest : MonoBehaviour {
    public SocketIOComponent SocketIO;
    public VersionGame versionGame;
   
    private void Start()
    {
        
        StartCoroutine("doCheck");
        //checkVersion(data);
        
    }
    IEnumerator doCheck()
    {
        yield return new WaitForSeconds(1);
        Dictionary<string, string> data = new Dictionary<string, string>();
       // data["Version"] = VersionGame.instance.VersionInt.ToString();
        data["Version"] = versionGame.VersionInt.ToString();
        SocketIO.Emit("S_CHECK_VERSION", new JSONObject(data));
        SocketIO.On("R_CHECK_VERSION", R_CHECK_VERSION);
    }
   
    //private void checkVersion(Dictionary<string, int> data)
    //{
    //    Debug.Log(data);
    //    SocketIO.Emit("S_CHECK_VERSION", new JSONObject(data.ToString()));
    //    SocketIO.On("R_CHECK_VERSION", R_CHECK_VERSION);
    //}
    private void R_CHECK_VERSION(SocketIOEvent obj)
    {
        Debug.Log("R_CHECK_VERSION: " + obj);
        //VersionGame.instance.VersionInt = int.Parse(obj.data["Version"].ToString());
        versionGame.VersionInt = int.Parse(obj.data["Version"].ToString());
    }
}

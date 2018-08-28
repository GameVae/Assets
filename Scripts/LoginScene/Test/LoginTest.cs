using SocketIO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoginTest : MonoBehaviour {
    //public SocketIOComponent SocketIO;
    //public VersionGame versionGame;

    //private void Start()
    //{

    //    StartCoroutine("doCheck");
    //    //checkVersion(data);
    //}
    //IEnumerator doCheck()
    //{
    //    yield return new WaitForSeconds(1);
    //    Dictionary<string, string> data = new Dictionary<string, string>();
    //   // data["Version"] = VersionGame.instance.VersionInt.ToString();
    //    data["Version"] = versionGame.VersionInt.ToString();
    //    SocketIO.Emit("S_CHECK_VERSION", new JSONObject(data));
    //    SocketIO.On("R_CHECK_VERSION", R_CHECK_VERSION);
    //}

    ////private void checkVersion(Dictionary<string, int> data)
    ////{
    ////    Debug.Log(data);
    ////    SocketIO.Emit("S_CHECK_VERSION", new JSONObject(data.ToString()));
    ////    SocketIO.On("R_CHECK_VERSION", R_CHECK_VERSION);
    ////}
    //private void R_CHECK_VERSION(SocketIOEvent obj)
    //{
    //    Debug.Log("R_CHECK_VERSION: " + obj);
    //    //VersionGame.instance.VersionInt = int.Parse(obj.data["Version"].ToString());
    //    versionGame.VersionInt = int.Parse(obj.data["Version"].ToString());
    //}

    private void Start()
    {
        Dictionary<int, MonoClass> thisTest = new Dictionary<int, MonoClass>();
        MonoClass monoClass1 = new MonoClass(1, "Man", 0.25f);
        MonoClass monoClass2 = new MonoClass(2, "Boy", 0.5f);
        MonoClass monoClass3 = new MonoClass(1, "Girl", 0.75f);
        thisTest.Add(1, monoClass1);
        thisTest.Add(2, monoClass2);
        thisTest.Add(3, monoClass3);

        //foreach (var item in thisTest)
        //{

        //    Debug.Log(item.Value);
        //}
        //List<MonoClass> findClass = new List<MonoClass>();
       // Dictionary<int, MonoClass> thisTest2 =  thisTest.Where(x => x.Value.ID == 1).;
        var found = thisTest.Where(x => x.Value.ID == 1).ToList();
       
        Debug.Log(found.GetType());
        // List<MonoClass> fo = found.ToList();
        for (int i = 0; i < found.Count; i++)
        {
            Debug.Log(found[i].Value.ID);
            Debug.Log(found[i].Value.Name);
            Debug.Log(found[i].Value.Time);
        }
        //Debug.Log(thisTest.Select(x => x.Value.ID == 1));


        //if (findValue==null)
        //{
        //    Debug.Log("not found");
        //}
        //Debug.Log(findValue.ID);
        //Debug.Log(findValue.Name);
        //Debug.Log(findValue.Time);
    }
}

public class MonoClass
{
    public int ID;
    public string Name;
    public float Time;

    public MonoClass(int iD, string name, float time)
    {
        ID = iD;
        Name = name;
        Time = time;
    }
}

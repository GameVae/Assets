using Entities.Navigation;
using Generic.Singleton;
using DataTable;
using DataTable.Row;
using DataTable.SQL;
using Network.Data;
using Json;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using MultiThread;
using Generic.Pooling;
using Generic.BinarySearch;
using System.Xml.Linq;
using System.Xml;
using System;
using UnityEngine.Events;

public class TestActionNode : ActionNode
{
    private string action;
    public TestActionNode(string actionName)
        : base(null)
    {
        action = actionName;
    }
}

public class TestDecisionNode : DecisionNode
{
    private bool branch;

    public TestDecisionNode(DecisionTreeNode t, DecisionTreeNode f, bool branch)
        : base(t, f)
    {
        this.branch = branch;
    }

    public override DecisionTreeNode GetBranch()
    {
        return branch == true ? trueNode : falseNode;
    }
}


public class TestObj : MonoBehaviour
{
    Ray ray;

    public NavAgent AgentTest;
    public Connection moveEvent;
    public EventSystem eventSystem;

    [Header("Test SQL")]
    public SQLiteManualConnection DBConn;
    public SQLiteTable_MainBase mainbase;
    public SQLiteTable_Military military;
    public SQLiteTable_TrainningCost trainningCost;

    [Header("Test Ser Event")]
    public EventListenersController events;
    public SIO_LoginListener loginEvent;
    public JSONTable_Position rss_table1;
    public JSONTable_Position rss_table2;
    public JSONTable_Position rss_table3;

    public MultiThreadHelper ThreadHelper;

    [Header("Test on screen label")]
    public LightweightLabel prefabLabelv2;
    public RectTransform canvas;
    private Pooling<LightweightLabel> labelPooling;
    public Transform headPoint;
    private LightweightLabel lable;

    private bool inited = false;
    private void Update()
    {
        //Vector3 position = Camera.main.WorldToScreenPoint(headPoint.position);
        //lable.GetComponent<RectTransform>().
        //        SetPositionAndRotation(position, Quaternion.identity);
        //if (inited)
        //{
        //    Debug.Log(
        //        "Thread 1" + ": " + info1.Operation.Progress + "% " +
        //        " Thread 2" + ": " + info2.Operation.Progress + "% " +
        //        " Thread 3" + ": " + info3.Operation.Progress + "% ");
        //}
        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    StartCoroutine(LockMainThread());
        //}
    }

    private LightweightLabel CreatLabelv2(int id)
    {
        LightweightLabel item = Instantiate(prefabLabelv2, canvas);
        item.FirstSetup(id);
        return item;
    }

    private void Awake()
    {
        TrainingCostRow cost = new TrainingCostRow()
        {
            ID = -1,
        };

        trainningCost.LoadTable();
        trainningCost.Rows.BinarySort_R();

        TrainingCostRow rs = trainningCost.Rows.FirstOrDefault_R(cost);

        Debug.Log("JSON: " + JsonUtility.ToJson(rs));
        Debug.Log("index: " + trainningCost.Rows.BinarySearch_R(cost));

        Debug.Log("removed: " + trainningCost.Rows.Remove_R(cost));
        trainningCost.Rows.UpdateOrInsert_R(cost);
        Debug.Log("removed: " + trainningCost.Rows.Remove_R(cost));
    }

    private void Start()
    {
        //mainbase.LoadTable();
        //trainningCost.LoadTable();
        //military.LoadTable();

        labelPooling = new Pooling<LightweightLabel>(CreatLabelv2);

        string path = Application.dataPath + @"/Z_Temp/DecisionMaking/AgentDecisionTree.xml";
        //XElement xElement = XElement.Load(path);
        XmlDocument doc = new XmlDocument();
        doc.Load(path);

        string methodName = doc.FirstChild.Attributes.Item(0).Value;

        string property = doc.FirstChild.FirstChild.Attributes[0].Value;

        string text = doc.ChildNodes[0].InnerText;

        string itemName = "Name";
        XmlNode node = doc.FirstChild.Attributes.GetNamedItem(itemName);
        Debug.Log(itemName + " " + node);
        //AgentDecisionTree decisionTree = new AgentDecisionTree();

        //DecisionTreeNode root = decisionTree.Root(path);
        //Debug.Log(root);
        //Debug.Log(methodName);

        //Type classType = typeof(MyClass);
        //MyClass myclass = new MyClass() { Interger = 10, };
        ////MethodInfo info = thisType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        //// info.Invoke(this, new object[] { text });
        ////MyDelegate method = (MyDelegate)Delegate.CreateDelegate(typeof(MyDelegate), this, info.Name);
        ////method.Invoke(text);
        //Debug.Log("Before: " + myclass.Interger);
        //PropertyInfo propertyInfo = classType.GetProperty(property);
        //Debug.Log("Property: " + propertyInfo + " type: " + propertyInfo.PropertyType);
        //var interger = Convert.ChangeType(text, propertyInfo.PropertyType);
        //Debug.Log("After convert: " + interger.GetType());
        //propertyInfo.SetValue(myclass,interger);
        //Debug.Log("After: " + myclass.Interger);


        //Type type = Type.GetType("System.Int32");
        //var reflection = Activator.CreateInstance(type);
        ////reflection = text;
        //Debug.Log("Create by Activator.CreateInstance - value: " + reflection + " type: " + reflection.GetType());

        //InvokeDynamic(method as UnityAction<string>, text);

        //Debug.Log(xElement);
        //LogXml(doc.ChildNodes);

        #region
        //for (int i = 0; i < 100; i++)
        //{
        //    LightweightLabel item = labelPooling.GetItem();
        //    Vector2 pos = new Vector2(UnityEngine.Random.Range(0, 1920), UnityEngine.Random.Range(0, 1080));
        //    item.GetComponent<RectTransform>().SetPositionAndRotation(pos,Quaternion.identity);

        //    item.gameObject.SetActive(true);
        //}

        //for (int i = 0; i < 100; i++)
        //{
        //    GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //    Vector3 pos =
        //        new Vector3
        //        (UnityEngine.Random.Range(-10, 20f),
        //        UnityEngine.Random.Range(-10, 20f),
        //        UnityEngine.Random.Range(-2f, 0));

        //    obj.transform.position = pos;
        //    labelPooling.GetItem().GetComponent<RectTransform>().
        //        SetPositionAndRotation(Camera.main.WorldToScreenPoint(pos),Quaternion.identity);
        //}

        //Vector3 position = Camera.main.WorldToScreenPoint(headPoint.position);
        //lable = labelPooling.GetItem();
        //lable.GetComponent<RectTransform>().
        //        SetPositionAndRotation(position,Quaternion.identity);

        // moveEvent = Singleton.Instance<Connection>();
        // moveEvent.On("R_MOVE", R_MOVE);
        //Debugger.Log("This is a log from Debugger");

        //VersionRow row = new VersionRow()
        //{
        //    Comment = "comment",
        //    Content = "content",
        //    Id = 1,
        //    Task = "for test"

        //};
        // RefectionTest(row);

        // TestDBConnection();
        // events.On("R_GET_POSITION", R_GET_RSS);

        //rss_table1.Rows?.Clear();
        //rss_table2.Rows?.Clear();
        //rss_table3.Rows?.Clear();

        //events.On("R_GET_POSITION", R_GET_RSS_Thread);
        //StartCoroutine(TestLogin());

        //SafeThreadTest();

        //th1 = new Thread(AnotherThreadExecute);
        //th1.Start();

        //TestActionNode move = new TestActionNode("Move");
        //TestActionNode attack = new TestActionNode("Attack");

        //TestDecisionNode selectOnEnemy = new TestDecisionNode(attack, move, true);

        //selectOnEnemy.MakeDecision();
        #endregion
    }
    delegate void MyDelegate(string v);
    class MyClass
    {
        public int Interger { get; set; }
    }
    private void InvokeDynamic(UnityAction<string> action, string v)
    {
        action.Invoke(v);
    }
    private void LogXml(XmlNodeList list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(list[i].LocalName);
            LogAttributes(list[i].Attributes);
            LogXml(list[i].ChildNodes);
        }
    }

    private void LogAttributes(XmlAttributeCollection attrs)
    {
        if (attrs == null) return;
        for (int i = 0; i < attrs.Count; i++)
        {
            Debug.Log("attri - name: " + attrs[i].Name + " value: " + attrs[i].Value);
            LogAttributes(attrs[i].Attributes);
        }
    }

    private void XmlMethod(string v)
    {
        Debug.Log("Reflection from xml file: " + v);
    }

    private void AnotherThreadExecute()
    {
        th2 = new Thread(
            () =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    Debugger.Log("another thread running " + i);
                }
            }
            );
        th2.Start();
        ThreadHelper.MainThreadInvoke(() =>
        {
            for (int i = 0; i < 1000; i++)
            {
                Debugger.Log("Main thread from another thread " + i);
            }
        }
      );

    }

    private IEnumerator LockMainThread()
    {
        bool exit = false;
        while (!exit)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) exit = true;
        }
        Debug.Log("Exited");
        yield break;
    }


    private IEnumerator TestLogin()
    {
        yield return new WaitForSeconds(2);
        loginEvent.Login("ndinhtoan45", "12345678");
        yield break;
    }

    private void TestDBConnection()
    {
        DBConn.LoadTable(mainbase);
        //table.Rows[0] = new MainBaseRow(); 
        MainBaseRow r = new MainBaseRow()
        {
            StoneCost = 10000,
        };
        mainbase.SQLInsert(r);
    }

    private void RefectionTest(ITableData manualRow)
    {
        Debug.Log(Singleton.Instance<SQLiteHelper>().CreateUpdateValuesFrom(manualRow));
    }

    private void BinarySearchTest()
    {
        List<int> li = new List<int>()
        {0,1,10,2,3,300,891,10,2,3,300,25};

        // ebug.Log("index: " + li.BinarySearch(0,li.Count - 1, 101));
        li.BinarySort_R();
        Debug.Log("sorted list: ");
        li.Log(" - ");
        Debug.Log("index: " + li.BinarySearch_R(891));
    }

    private void R_MOVE(SocketIO.SocketIOEvent ev)
    {
        Debugger.Log(ev.data["R_MOVE"]);

    }

    public void Log()
    {
        Debug.Log("This is a message");
    }

    ///// Parse json by thread

    private Thread parseJson;

    private void R_GET_RSS(SocketIO.SocketIOEvent evt)
    {
        JSONObject data = evt.data["R_GET_POSITION"];
        string json = data.ToString();
        Debug.Log(data);
        StartThread(data);
        Debug.Log("parse thread started");
    }

    AsyncTableLoader<PositionRow> parser;
    AsyncTableLoader<PositionRow>.ParseInfo info1;
    AsyncTableLoader<PositionRow>.ParseInfo info2;
    AsyncTableLoader<PositionRow>.ParseInfo info3;

    private void R_GET_RSS_Thread(SocketIO.SocketIOEvent evt)
    {
        JSONObject data = evt.data["R_GET_POSITION"];
        string json = data.ToString();
        //        Debug.Log(json);

        parser = Singleton.Instance<AsyncTableLoader<PositionRow>>();

        info1 = new AsyncTableLoader<PositionRow>.ParseInfo()
        {
            Obj = data,
            ResultHandler = delegate (PositionRow r)
            {
                lock (locker)
                {
                    LoadRowForTable(rss_table1, r);
                }
            },
            Operation = new AJPHelper.Operation(),
        };
        info2 = new AsyncTableLoader<PositionRow>.ParseInfo()
        {
            Obj = data,
            ResultHandler = delegate (PositionRow r)
            {
                lock (locker)
                {
                    LoadRowForTable(rss_table2, r);
                }
            },
            Operation = new AJPHelper.Operation(),

        };
        info3 = new AsyncTableLoader<PositionRow>.ParseInfo()
        {
            Obj = data,
            ResultHandler = delegate (PositionRow r)
            {
                lock (locker)
                {
                    LoadRowForTable(rss_table3, r);
                }
            },
            Operation = new AJPHelper.Operation(),
        };

        parser.Start(info1);
        parser.Start(info2);
        parser.Start(info3);
        inited = true;
    }

    private void StartParse(JSONObject data)
    {
        rss_table1.Clear();
        int count = data.Count;
        int i = 0;
        while (i < count)
        {
            // rss_table1.LoadRow(data[i].ToString());
            i++;
            Thread.Sleep(200);
        }
    }

    private void LoadRowForTable<T>(JSONTable<T> table, T value)
        where T : ITableData
    {
        table.Insert(value);
    }

    private void StartThread(JSONObject data)
    {
        parseJson = new Thread
            (() => StartParse(data));
        parseJson.Start();
    }

    private void OnApplicationQuit()
    {
        int liCount = list.Count;
        parseJson?.Abort();
        th1?.Abort();
        th2?.Abort();
    }


    private int testValue;
    Thread th1;
    Thread th2;

    private List<int> list = new List<int>();
    private object locker = new object();
    private void SafeThreadTest()
    {

        th1 = new Thread(() =>
        {
            while (testValue < 1000)
            {
                lock (locker)
                {
                    Debug.Log("Th1 locked");
                    testValue++;
                    list.Add(testValue);
                    Debug.Log("Th1 added: " + testValue);
                }
            }
        }
        );
        th2 = new Thread(() =>
         {
             while (testValue < 1000)
             {
                 lock (locker)
                 {
                     Debug.Log("Th2 locked");

                     testValue++;
                     list.Add(testValue);
                     Debug.Log("Th2 added: " + testValue);
                 }
             }
         }
         );

        th1.Start();
        th2.Start();

    }

    private void OnBecameInvisible()
    {
        Debug.Log(gameObject.name + " Invisible");
    }

    private void OnBecameVisible()
    {
        Debug.Log(gameObject.name + " Visible");
    }
}
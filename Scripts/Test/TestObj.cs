﻿using Entities.Navigation;
using Generic.Singleton;
using DataTable;
using DataTable;
using DataTable.Row;
using DataTable.SQL;
using Network.Data;
using System;
using Json;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using MultiThread;
using Generic.Pooling;

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
    public SQLiteTable_MainBase table;

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
        Vector3 position = Camera.main.WorldToScreenPoint(headPoint.position);
        lable.GetComponent<RectTransform>().
                SetPositionAndRotation(position, Quaternion.identity);
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
    private void Start()
    {
        labelPooling = new Pooling<LightweightLabel>(CreatLabelv2);

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

        Vector3 position = Camera.main.WorldToScreenPoint(headPoint.position);
        lable = labelPooling.GetItem();
        lable.GetComponent<RectTransform>().
                SetPositionAndRotation(position,Quaternion.identity);

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

        TestActionNode move = new TestActionNode("Move");
        TestActionNode attack = new TestActionNode("Attack");

        TestDecisionNode selectOnEnemy = new TestDecisionNode(attack, move, true);

        selectOnEnemy.MakeDecision();
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
        ThreadHelper.Invoke(() =>
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
        DBConn.LoadTable(table);
        //table.Rows[0] = new MainBaseRow(); 
        MainBaseRow r = new MainBaseRow()
        {
            StoneCost = 10000,
        };
        table.SQLInsert(DBConn.DbConnection, r);
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
        Debug.Log("index: " + li.BinarySearch_L(0, li.Count - 1, 891));
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

    AsyncLoadTable<PositionRow> parser;
    AsyncLoadTable<PositionRow>.ParseInfo info1;
    AsyncLoadTable<PositionRow>.ParseInfo info2;
    AsyncLoadTable<PositionRow>.ParseInfo info3;

    private void R_GET_RSS_Thread(SocketIO.SocketIOEvent evt)
    {
        JSONObject data = evt.data["R_GET_POSITION"];
        string json = data.ToString();
        //        Debug.Log(json);

        parser = Singleton.Instance<AsyncLoadTable<PositionRow>>();

        info1 = new AsyncLoadTable<PositionRow>.ParseInfo()
        {
            Obj = data,
            ResultHandler = delegate (PositionRow r)
            {
                lock (locker)
                {
                    LoadRowForTable(rss_table1, r);
                }
            },
            Operation = new AsyncLoadTable<PositionRow>.ParseOperation(),
        };
        info2 = new AsyncLoadTable<PositionRow>.ParseInfo()
        {
            Obj = data,
            ResultHandler = delegate (PositionRow r)
            {
                lock (locker)
                {
                    LoadRowForTable(rss_table2, r);
                }
            },
            Operation = new AsyncLoadTable<PositionRow>.ParseOperation(),

        };
        info3 = new AsyncLoadTable<PositionRow>.ParseInfo()
        {
            Obj = data,
            ResultHandler = delegate (PositionRow r)
            {
                lock (locker)
                {
                    LoadRowForTable(rss_table3, r);
                }
            },
            Operation = new AsyncLoadTable<PositionRow>.ParseOperation(),
        };

        parser.Start(info1);
        parser.Start(info2);
        parser.Start(info3);
        inited = true;
    }

    private void StartParse(JSONObject data)
    {
        rss_table1.Rows.Clear();
        int count = data.Count;
        int i = 0;
        while (i < count)
        {
            rss_table1.LoadRow(data[i].ToString());
            i++;
            Thread.Sleep(200);
        }
    }

    private void LoadRowForTable<T>(JSONTable<T> table, T value)
        where T : ITableData
    {
        table.Rows.Add(value);
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
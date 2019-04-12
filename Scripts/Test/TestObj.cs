using Entities.Navigation;
using Generic.Singleton;
using ManualTable;
using ManualTable.Interface;
using ManualTable.Row;
using ManualTable.SQL;
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
    public PositionJSONTable rss_table1;
    public PositionJSONTable rss_table2;
    public PositionJSONTable rss_table3;

    public MultiThreadHelper ThreadHelper;
    //void Update()
    //{
    //    RaycastHit hit;
    //    if (Input.touchCount>0)
    //    {
    //        ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            string name = hit.transform.gameObject.name;
    //            if (name == "Cube")
    //                Debug.Log("hit Cube");
    //        }

    //        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //        Debug.Log("mouse: " + Input.mousePosition);
    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            string name = hit.transform.gameObject.name;
    //            if (name == "Cube")
    //                Debug.Log("hit Cube");
    //        }
    //    }
    //    if (Input.GetButtonDown("Fire1"))
    //    {
    //        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //        Debug.Log("mouse: " + Input.mousePosition);
    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            string name = hit.transform.gameObject.name;
    //            if (name == "Cube")
    //                Debug.Log("hit Cube");
    //        }
    //    }
    //}

    private bool inited = false;
    private void Update()
    {
        //if (inited)
        //{
        //    Debug.Log(
        //        "Thread 1" + ": " + info1.Operation.Progress + "% " +
        //        " Thread 2" + ": " + info2.Operation.Progress + "% " +
        //        " Thread 3" + ": " + info3.Operation.Progress + "% ");
        //}
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(LockMainThread());
        }
    }
    private void Start()
    {
        // moveEvent = Singleton.Instance<Connection>();
        // moveEvent.On("R_MOVE", R_MOVE);
        //Debugger.Log("This is a log from Debugger");

        VersionRow row = new VersionRow()
        {
            Comment = "comment",
            Content = "content",
            Id = 1,
            Task = "for test"

        };
        // RefectionTest(row);

        // TestDBConnection();
        // events.On("R_GET_POSITION", R_GET_RSS);

        //rss_table1.Rows?.Clear();
        //rss_table2.Rows?.Clear();
        //rss_table3.Rows?.Clear();

        //events.On("R_GET_POSITION", R_GET_RSS_Thread);
        //StartCoroutine(TestLogin());

        //SafeThreadTest();

        th1 = new Thread(AnotherThreadExecute);
        th1.Start();
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

    private void RefectionTest(IManualRow manualRow)
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

    AsyncJsonParser<PositionRow> parser;
    AsyncJsonParser<PositionRow>.ParseInfo info1;
    AsyncJsonParser<PositionRow>.ParseInfo info2;
    AsyncJsonParser<PositionRow>.ParseInfo info3;

    private void R_GET_RSS_Thread(SocketIO.SocketIOEvent evt)
    {
        JSONObject data = evt.data["R_GET_POSITION"];
        string json = data.ToString();
        //        Debug.Log(json);

        parser = Singleton.Instance<AsyncJsonParser<PositionRow>>();

        info1 = new AsyncJsonParser<PositionRow>.ParseInfo()
        {
            Obj = data,
            ResultHandler = delegate (PositionRow r)
            {
                lock (locker)
                {
                    LoadRowForTable(rss_table1, r);
                }
            },
            Operation = new AsyncJsonParser<PositionRow>.ParseOperation(),
        };
        info2 = new AsyncJsonParser<PositionRow>.ParseInfo()
        {
            Obj = data,
            ResultHandler = delegate (PositionRow r)
            {
                lock (locker)
                {
                    LoadRowForTable(rss_table2, r);
                }
            },
            Operation = new AsyncJsonParser<PositionRow>.ParseOperation(),

        };
        info3 = new AsyncJsonParser<PositionRow>.ParseInfo()
        {
            Obj = data,
            ResultHandler = delegate (PositionRow r)
            {
                lock (locker)
                {
                    LoadRowForTable(rss_table3, r);
                }
            },
            Operation = new AsyncJsonParser<PositionRow>.ParseOperation(),
        };

        parser.Start(info1);
        parser.Start(info2);
        parser.Start(info3);
        inited = true;
    }

    private void StartParse(JSONObject data)
    {
        rss_table1.Rows?.Clear();
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
        where T : IManualRow
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
}
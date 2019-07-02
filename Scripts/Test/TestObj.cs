#define CUSTOM_PREPROCESSOR
//#undef CUSTOM_PREPROCESSOR

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
using Extensions.BinarySearch;
using System.Xml.Linq;
using System.Xml;
using System;
using UnityEngine.Events;
using Extensions.Xml;
using DataConvert;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class TestObj : MonoBehaviour
{
    public void Awake()
    {
        System.Diagnostics.Debug.Write(" this is a log");
        MilitaryRow row = new MilitaryRow()
        {
            Attack = 5,
            Defend = 10,
        };

        Serializer ser = new Serializer(new BinaryFormatter());
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        byte[] arr = ser.Serialize("This is message");

        arr.Log();

        stopwatch.Stop();
        Debug.Log(stopwatch.ElapsedMilliseconds);
        FileStream fs = new FileStream(UnityPath.Combinate("serfile.ser", UnityPath.AssetPath.Persistent), FileMode.OpenOrCreate);
        fs.Write(arr, 0, arr.Length);
    }
}
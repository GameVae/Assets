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



public class TestObj : MonoBehaviour
{
    public void Awake()
    {
#if UNITY_DEBUG
        Debug.Log("test");
#endif
    }
}
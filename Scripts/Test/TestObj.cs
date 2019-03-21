using Entities.Navigation;
using Generic.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObj : MonoBehaviour {
    Ray ray;

    public NavAgent AgentTest;
    public Connection moveEvent;
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

    private void Update()
    {
        
    }
    private void Start()
    {
        moveEvent = Singleton.Instance<Connection>();
        // moveEvent.On("R_MOVE", R_MOVE);
        //Debugger.Log("This is a log from Debugger");
    }

    private void R_MOVE(SocketIO.SocketIOEvent ev)
    {
        Debugger.Log(ev.data["R_MOVE"]);
      
    }

    public void Log()
    {
        Debug.Log("This is a message");
    }
}
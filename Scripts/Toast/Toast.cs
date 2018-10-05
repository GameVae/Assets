using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour {
    private Text thisText;

	
	void Awake () {
        thisText = GetComponent<Text>();
	}

    private void Start()
    {
        InvokeRepeating("clearToast", 1,2);    
    }
    private void clearToast()
    {
        if (thisText.text.Length>0)
        {
            Debug.Log("here");
            StartCoroutine("clearTxt");
           
        }
    }
    private IEnumerator clearTxt()
    {
        yield return new WaitForSeconds(2);
        thisText.text = "";
    }
    public void ShowToast(string txt)
    {
        thisText.text = txt;
    }
}

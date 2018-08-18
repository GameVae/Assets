using UnityEngine;
using UnityEngine.UI;


public class CloseBtn : MonoBehaviour {	
	void Awake () {
        GameObject parentGO = transform.parent.gameObject;
        Button closeBtn = gameObject.GetComponent<Button>();
        closeBtn.onClick.AddListener(() =>
        {
            parentGO.SetActive(false);
        });
	}

}

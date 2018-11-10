using UnityEngine;
using UnityEngine.UI;

public class EnableAgent : MonoBehaviour
{ 
    public NavAgent agent;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(Enable);
    }

    private void Enable()
    {
        agent.enabled = true;
    }
}

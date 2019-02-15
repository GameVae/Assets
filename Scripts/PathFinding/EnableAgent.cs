using Generic.Singleton;
using UnityEngine;
using UnityEngine.UI;

public class EnableAgent : MonoBehaviour
{
    public int Index;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => Singleton.Instance<AgentController>().SwitchToAgent(Index));
    }
}

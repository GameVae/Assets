using TMPro;
using UnityEngine;

public class PingInfo : MonoBehaviour
{
    public Connection Conn;
    public TextMeshProUGUI PingText;

    private void Update()
    {
        if (Conn.IsServerConnected)
        {
            PingText.text = "Ping: " + (float)System.Math.Round(Conn.Ping, 3) * 1000.0f + " ms";
            PingText.text += "\n FPS: " + Mathf.RoundToInt(1.0f / Time.deltaTime);
        }
    }
}

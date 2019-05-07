using TMPro;
using UnityEngine;

public class PingInfo : MonoBehaviour
{
    public Connection Conn;
    public TextMeshProUGUI PingText;

    private int fpsSmoothFrame = 0;
    private int fps = 0;
    private float ping = 0.0f;

    private void Update()
    {
        if ((fpsSmoothFrame += 1) % 15 == 0)
        {
            fps = Mathf.RoundToInt(1.0f / Time.deltaTime);
            ping = (float)System.Math.Round(Conn.Ping, 3) * 1000.0f;
        }

        if (Conn.IsServerConnected)
        {
            PingText.text = "Ping: " + ping + " ms";
            PingText.text += "\n FPS: " + fps;
        }
        else
        {
            PingText.text = "Ping: ";
            PingText.text += "\n FPS: " + fps;
        }
    }
}

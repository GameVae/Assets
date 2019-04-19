using TMPro;
using UnityEngine;

public class PingInfo : MonoBehaviour
{
    public Connection Conn;
    public TextMeshProUGUI PingText;

    private int fpsSmoothFrame = 0;
    private int fps = 0;

    private void Update()
    {
        if ((fpsSmoothFrame += 1) % 10 == 0)
            fps = Mathf.RoundToInt(1.0f / Time.deltaTime);

        if (Conn.IsServerConnected)
        {
            PingText.text = "Ping: " + (float)System.Math.Round(Conn.Ping, 3) * 1000.0f + " ms";
            PingText.text += "\n FPS: " + fps;
        }
        else
        {
            PingText.text = "Ping: ";
            PingText.text += "\n FPS: " + fps;
        }
    }
}

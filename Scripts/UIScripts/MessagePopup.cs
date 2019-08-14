using Generic.Singleton;
using UI.Composites;
using UnityEngine;

public sealed class MessagePopup : MonoSingle<MessagePopup>
{
    private float timer;
    private bool isOpen = false;

    public float LifeTime;
    public PlaceholderComp Message;

    private void Update()
    {
        
        if(isOpen)
        {
            timer += Time.deltaTime;
            if(timer >= LifeTime)
            {
                CloseMessage();
            }
        }
    }

    public void CloseMessage()
    {
        Message.gameObject.SetActive(false);
        isOpen = false;
        timer = 0.0f;
    }

    public void OpenMessage(string msg)
    {
        isOpen = true;
        Message.Text = msg;
        Message.gameObject.SetActive(true);
    }
    
    public static void Open(string msg)
    {
        ((MessagePopup)Instance).OpenMessage(msg);
    }
}

using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public Dialogue CurrentConversation;
    public DialogueManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    public void StartConversation()
    {
        CurrentConversation.StartConversation();
    }

    public void NextDialogue()
    {
        string sentence = CurrentConversation?.NextDialogue;
        if (sentence == null) EndConversation();
        else
        {

        }
    }

    private void EndConversation()
    {

    }
}

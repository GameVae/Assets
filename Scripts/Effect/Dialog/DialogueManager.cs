using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour, IPointerDownHandler
{
    public Dialogue CurrentConversation;
    public DialogueManager Instance { get; private set; }

    [Header("UI")]
    public Text DisplayText;
    public Animator Animator;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    private void EndConversation()
    {
        Animator.SetBool("IsOpen", false);
    }

    private IEnumerator TypeText(string sentence)
    {
        int size = sentence.Length;
        DisplayText.text = "";
        for (int i = 0; i < size; i++)
        {
            DisplayText.text += sentence[i];
            yield return null;
        }
        yield break;
    }

    public void StartConversation()
    {
        Animator.SetBool("IsOpen", true);
        CurrentConversation.StartConversation();
        NextDialogue();
    }

    public void NextDialogue()
    {
        string sentence = CurrentConversation?.NextDialogue;
        if (sentence == null) EndConversation();
        else
        {
            StopAllCoroutines();
            StartCoroutine(TypeText(sentence));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        NextDialogue();
    }   
}

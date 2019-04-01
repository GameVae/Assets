using Generic.Singleton;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Dialogue
{
    public class DialogueManager : MonoSingle<DialogueManager>, IPointerDownHandler
    {
        public Dialogue CurrentConversation;

        [Header("UI")]
        public Text DisplayText;
        public Animator Animator;

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
}

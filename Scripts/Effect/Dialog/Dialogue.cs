using EnumCollect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "UI/Dialogue")]
public class Dialogue : ScriptableObject
{
    public Language[] Contents;

    private Queue<string> sentences;

    public void StartConversation()
    {
        if (sentences == null) sentences = new Queue<string>();
        else sentences.Clear();

        EnumLanguage enumLanguage = PlayerPrefs.HasKey("enumLanguage") ? 
            (EnumLanguage)PlayerPrefs.GetInt("enumLanguage") : EnumLanguage.Vietnamese;
        switch (enumLanguage)
        {
            case EnumLanguage.Vietnamese:
                for (int i = 0; i < Contents.Length; i++)
                {
                    sentences.Enqueue(Contents[i].Vietnamese);
                }
                break;
            case EnumLanguage.English:
                for (int i = 0; i < Contents.Length; i++)
                {
                    sentences.Enqueue(Contents[i].English);
                }
                break;
        }
    }

    public string NextDialogue
    {
        get
        {
            if(sentences.Count == 0)
                return null;
            return sentences.Dequeue();
        }
    }
}

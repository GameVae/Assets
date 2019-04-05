using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Widget
{
    [RequireComponent(typeof(Button))]
    public class GUIInputField : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI placeholder;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button button;

        private Action<string> onValueChange;

        public event UnityAction OnSelectField
        {
            add { button.onClick.AddListener(value); }
            remove { button.onClick.RemoveListener(value); }
        }

        public event Action<string> OnValueChange
        {
            add { onValueChange += value; }
            remove { onValueChange -= value; }
        }

        public string Text
        {
            get { return text.text; }
        }

        private void Awake()
        {
            OnValueChange += TextChange;
        }

        private void TextChange(string value)
        {
            if (placeholder.gameObject.activeInHierarchy)
            {
                if (text.text.Length > 0)
                    placeholder.gameObject.SetActive(false);
            }
            else
            {
                if (text.text.Length == 0)
                    placeholder.gameObject.SetActive(true);
            }
        }

        public void AddLetter(char letter)
        {
            text.text += letter;
            onValueChange?.Invoke(letter.ToString());
        }

        public void SetText(string value)
        {
            text.text = value;
            onValueChange?.Invoke(value);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MultiLang
{
    public sealed class MultiLangManager : MonoBehaviour
    {
        public static MultiLangManager Instance { get; private set; }
        private event UnityAction LangChangeEvts;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else Destroy(gameObject);
        }

        public void Add(UnityAction act)
        {
            LangChangeEvts += act;
        }

        public void Invoke()
        {
            LangChangeEvts?.Invoke();
        }
    }
}

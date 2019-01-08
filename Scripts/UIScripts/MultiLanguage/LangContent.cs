using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MultiLang
{
    public class LangContent : MonoBehaviour
    {
        public Language Language;

        private TextMeshProUGUI meshText;

        public string MeshText
        {
            get
            {
                return (meshText?.text ?? (meshText = GetComponent<TextMeshProUGUI>())?.text);
            }

            set
            {
                if (meshText != null)
                    meshText.text = value;
            }
        }
        private void Awake()
        {
            meshText = GetComponent<TextMeshProUGUI>();

            MultiLangManager.Instance.Add(() =>
            {
                meshText.text = Language.ChangeLanguage();
            });
        }
    }
}
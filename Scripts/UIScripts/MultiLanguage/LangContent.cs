using Generic.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MultiLang
{
    public class LangContent : MonoBehaviour
    {
        public Language Language;

        private TextMeshProUGUI meshText;

        public TextMeshProUGUI MeshText
        {
            get
            {
                return (meshText ?? (meshText = GetComponent<TextMeshProUGUI>()));
            }

            private set
            {
                meshText = value;
            }
        }

        private void Awake()
        {
            MeshText.text = Language.ChangeLanguage();
            Singleton.Instance<MultiLangManager>().Add(() =>
            {
                MeshText.text = Language.ChangeLanguage();
            });
        }
    }
}
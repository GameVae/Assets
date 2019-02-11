using Generic.Singleton;
using UnityEngine.Events;

namespace MultiLang
{
    public sealed class MultiLangManager : MonoSingle<MultiLangManager>
    {
        private event UnityAction LangChangeEvts;

        public void Add(UnityAction act)
        {
            LangChangeEvts += act;
        }

        public void ChangeLanguage()
        {
            LangChangeEvts?.Invoke();
        }
    }
}

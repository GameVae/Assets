using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Tooltip
{
    public class Tooltip : MonoBehaviour, IPointerDownHandler
    {
        private string content;
        private TooltipType1 type1Instance;
        private TooltipType2 type2Instance;


        public Language Language;
        private void Start()
        {
            type2Instance = TooltipType2.Instance;
            type1Instance = TooltipType1.Instance;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            content = Language.ChangeLanguage();
            if (type2Instance)
            {
                //content = Language.ChangeLanguage();
                type2Instance.DisplayTooltip(transform.localPosition, content);
            }
            else
            {
                //content = Language.ChangeLanguage();
                type1Instance.DisplayTooltip(transform.localPosition, content);
            }
        }

        public void SetFontSize(int size)
        {
            if (type2Instance)
            {
                type2Instance.ResiseFont(size);
            }
            else
            {
                type1Instance.ResiseFont(size);
            }
        }

    }
}
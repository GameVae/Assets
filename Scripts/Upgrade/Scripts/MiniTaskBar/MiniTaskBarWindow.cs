using DB;
using EnumCollect;
using ManualTable;
using ManualTable.Row;
using System.Collections.Generic;
using System.Linq;
using UI.Animation;
using UI.Widget;
using UnityEngine;

namespace UI
{
    public class MiniTaskBarWindow : BaseWindow, IWindow
    {
        private GUIVerticalList verticalList;

        public GUISliderWithBtn HasUpgrade;
        public GUITextWithIcon DontHasUpgrade;

        public GUIOnOffSwitch OpenTaskBtn;
        private List<ListUpgrade> curUpgTypes;

        protected override void Awake()
        {
            base.Awake();
            inited = true;
            Init();
        }
        protected override void Start()
        {
            OpenTaskBtn.SwitchOff();
        }
        public override void Close()
        {
            DontHasUpgrade.gameObject.SetActive(false);
            verticalList.gameObject.SetActive(false);
        }

        public override void Open()
        {
            verticalList.gameObject.SetActive(true);
            Load();
        }

        private void On(GUIOnOffSwitch onOff)
        {
            Open();
            onOff.Placeholder.text = "Off";
        }

        private void Off(GUIOnOffSwitch onOff)
        {
            Close();
            onOff.Placeholder.text = "On";
        }

        protected override void Init()
        {
            curUpgTypes = new List<ListUpgrade>();
            HasUpgrade.Slider.Placeholder = HasUpgrade.GetComponentInChildren<GUITextWithIcon>().Placeholder;
            HasUpgrade.Placeholder = HasUpgrade.Slider.Placeholder;
            verticalList = Window.GetComponentInChildren<GUIVerticalList>();

            OpenTaskBtn.SetIsOn(true);
            OpenTaskBtn.On += On;
            OpenTaskBtn.Off += Off;
            OpenTaskBtn.InteractableChange(true);
        }

        public override void Load(params object[] input)
        {
            ListUpgrade upgType = SyncData.CurrentMainBase.UpgradeWait_ID;
            ListUpgrade resType = SyncData.CurrentMainBase.ResearchWait_ID;
            bool hasUpgRes = upgType.IsDefined() || resType.IsDefined();
            if (!hasUpgRes)
            {
                DontHasUpgrade.gameObject.SetActive(true);
            }
            else
            {
                Add(SyncData.CurrentMainBase.UpgradeWait_ID);
                Add(SyncData.CurrentMainBase.ResearchWait_ID);
            }
        }

        private void SwitchText(AFadeInOut inOut, GUISliderWithBtn infoBar, ListUpgrade type, int id)
        {
            float time = type.IsUpgrade() ? SyncData.CurrentMainBase.UpgradeTime : SyncData.CurrentMainBase.ResearchTime;
            infoBar.Slider.Value = infoBar.Slider.MaxValue - time;

            if (inOut.LoopCounter % 2 == 0)
            {
                infoBar.Placeholder.text = type.ToString().InsertSpace() + " ...";
            }
            else
            {
                infoBar.Placeholder.text = System.TimeSpan.FromSeconds(time).ToString(@"h\:m\:s");

                if (time <= 0)
                {
                    Destroy(inOut.gameObject);
                    verticalList.RemoveElement(id);
                    curUpgTypes.Remove(type);
                }
            }
        }

        private void Add(ListUpgrade type)
        {
            if (type.IsDefined())
            {
                if (curUpgTypes.Contains(type)) return;

                GUISliderWithBtn element = verticalList.AddElement<GUISliderWithBtn>(HasUpgrade.transform as RectTransform, out int id);
                element.InteractableChange(true);

                MainBaseTable table = DBReference.Instance[type] as MainBaseTable;
                MainBaseRow r = table.Rows.FirstOrDefault(x => x.Level == SyncData.BaseUpgrade[type].Level);
                element.Slider.MaxValue = r.TimeInt;


                AFadeInOut fader = element.GetComponent<AFadeInOut>();
                fader.StartFadingAction += delegate { SwitchText(fader, element, type, id); };
                element.gameObject.SetActive(true);

                curUpgTypes.Add(type);
            }
        }
    }
}